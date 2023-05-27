namespace Application.IdentityManagement.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
	public ResetPasswordCommandValidator()
	{
		RuleFor(x => x.Password)
			.Matches(@"^(?=.*[A-Za-z])(?=.*?[0-9])(?=.*?[!@#$&*~]).{6,}$")
			.WithMessage("Password must contain 1 alphabet, 1 letter and 1 special character")
			.MinimumLength(6)
			.WithMessage("Password length must be minimum 6 characters");
	}
}

