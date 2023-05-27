namespace Application.IdentityManagement.Auth.Commands.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
	public SignInCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage("Email can not be empty")
			.EmailAddress()
			.WithMessage("Please enter a valid email address");

		RuleFor(x => x.Password)
			.NotNull()
			.NotEmpty()
			.WithMessage("Password can not be empty");
	}
}

