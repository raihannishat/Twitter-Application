using Infrastructure.Common;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRedisTokenService _redisTokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<IdentityService> _logger;
    private readonly IMapper _mapper;
    private readonly string _secretKey;

    public IdentityService(IUserRepository userRepository,
        IAuthRepository authRepository,
        IMapper mapper,
        IConfiguration configuration,
        IJwtTokenService jwtTokenService,
        IEmailService emailService,
        ILogger<IdentityService> logger,
        IRedisTokenService redisTokenService)
    {
        _userRepository = userRepository;
        _authRepository = authRepository;
        _mapper = mapper;
        _secretKey = configuration.GetSection("JwtSettings:Secret").Value;
        _jwtTokenService = jwtTokenService;
        _emailService = emailService;
        _logger = logger;
        _redisTokenService = redisTokenService;
    }

    public async Task<Result<Unit>> Registration(SignUpCommand request)
    {
        var userEmailExist = await _userRepository.FindOneByMatchAsync(x => x.Email == request.Email);

        if (userEmailExist != null)
        {
            return Result<Unit>.Failure(AuthMessage.EmailUsed);
        }

        request.Password = SHA_256.ComputeHash(request.Password);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            VerificationToken = CreateRandomString(8),
            Role = "User"
        };

        await _userRepository.InsertOneAsync(user);

        var userDto = _mapper.Map<UserDto>(user);

        var name = new List<string> { request.Name };

        var emailDto = _emailService.CreateMailForAccountVerification(userDto);

        _emailService.SendMail(request.Email, emailDto.Subject, emailDto.Message);

        _logger.LogInformation(AuthMessage.RegistrationSuccessful);

        return Result<Unit>.Success(Unit.Value);

    }

    public async Task<Result<AuthResult>> Login(SignInCommand request)
    {
        var password = SHA_256.ComputeHash(request.Password);

        var existingUser = await _userRepository.FindOneByMatchAsync(x =>
            x.Email == request.Email && x.Password == password);

        if (existingUser == null)
        {
            return Result<AuthResult>.Failure(AuthMessage.InvalidUser);
        }

        if (existingUser.VerifiedAt == null)
        {
            return Result<AuthResult>.Failure(AuthMessage.VerificationFailed);
        }

        if (existingUser.IsBlockedByAdmin)
        {
            return Result<AuthResult>.Failure(AuthMessage.AdminBlocked);
        }

        var tokenResult = _jwtTokenService.GenerateToken(existingUser, _secretKey);

        //await _authRepository.InsertOneAsync(_mapper.Map<RefreshToken>(tokenResult.RefreshToken));

        await _redisTokenService.SetKeywithTTL(tokenResult.RefreshToken.Token, tokenResult.AccessToken, 1);

        return Result<AuthResult>.Success(new AuthResult
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken.Token,
            User = _mapper.Map<UserProfileViewModel>(existingUser)
        });

    }

    public async Task<Result<AuthResult>> RefreshToken(RefreshTokenCommand request)
    {
        var jwtToken = _jwtTokenService.GetPrincipalFromToken(request.AccessToken, _secretKey);

        var storedRefreshToken = await _redisTokenService.GetKey(request.RefreshToken);

        if (storedRefreshToken.IsNullOrEmpty())
        {
            return Result<AuthResult>.Failure(AuthMessage.RefreshTokenInvalid);
        }

        var id = jwtToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var user = await _userRepository.FindByIdAsync(id);

        var tokenResult = _jwtTokenService.GenerateToken(user, _secretKey);

        //await _authRepository.InsertOneAsync(_mapper.Map<RefreshToken>(tokenResult.RefreshToken));

        await _redisTokenService.UpdateKey(request.RefreshToken, tokenResult.AccessToken);

        return Result<AuthResult>.Success(new AuthResult
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = request.RefreshToken,
            User = _mapper.Map<UserProfileViewModel>(user)
        });
    }

    public async Task<Result<Unit>> VerifyAccount(string token)
    {
        var user = await _userRepository.FindOneAsync(x => x.VerificationToken == token);

        if (user == null)
        {
            return Result<Unit>.Failure(AuthMessage.InvalidUser);
        }

        user.VerifiedAt = DateTime.Now;

        await _userRepository.ReplaceOneAsync(user);

        return Result<Unit>.Success(Unit.Value);
    }

    public async Task<Result<Unit>> ForgetPassword(string email)
    {
        var user = await _userRepository.FindOneByMatchAsync(x => x.Email == email);

        if (user == null)
        {
            return Result<Unit>.Failure(AuthMessage.MailNotFound);
        }

        if (user.VerifiedAt == null)
        {
            return Result<Unit>.Failure(AuthMessage.AccountNotVerified) ;
        }

        user.PasswordResetToken = CreateRandomString(8);
        user.ResetTokenExpires = DateTime.Now.AddDays(1);

        await _userRepository.ReplaceOneAsync(user);
        var userDto = _mapper.Map<UserDto>(user);

        var emailDto = _emailService.CreateMailForResetPassword(userDto);
        
        _emailService.SendMail(user.Email, emailDto.Subject, emailDto.Message);

        return Result<Unit>.Success(Unit.Value);
    }

    public async Task<Result<Unit>> ResetPassword(ResetPasswordDto request)
    {
        var user = await _userRepository
            .FindOneByMatchAsync(x => x.PasswordResetToken == request.Token);

        if (user == null || user.ResetTokenExpires < DateTime.Now)
        {
            return Result<Unit>.Failure(AuthMessage.InvalidUser);
        }

        user.Password = SHA_256.ComputeHash(request.Password);
        user.PasswordResetToken = null;
        user.ResetTokenExpires = null;

        await _userRepository.ReplaceOneAsync(user);

        return Result<Unit>.Success(Unit.Value);

    }

    private static string CreateRandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
}
