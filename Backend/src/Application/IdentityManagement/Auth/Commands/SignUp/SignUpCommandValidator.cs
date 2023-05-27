namespace Application.IdentityManagement.Auth.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
	public SignUpCommandValidator()
	{
		RuleFor(x => x.Name)
			.Matches(@"^[A-Za-z\s]*$")
			.WithMessage("Name can not contain number or special character")
			.MinimumLength(3)
			.WithMessage("User name length must be minimum 3 characters");

		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress()
			.WithMessage("Please enter a valid email address");

		RuleFor(x => x.Password)
			.Matches(@"^(?=.*[A-Za-z])(?=.*?[0-9])(?=.*?[!@#$&*~]).{6,}$")
			.WithMessage("Password must contain 1 alphabet, 1 letter and 1 special character")
			.MinimumLength(6)
			.WithMessage("Password length must be minimum 6 characters");
	}
}
