namespace Application.IdentityManagement.User.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
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
