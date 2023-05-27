namespace Application.Tweets.Commands.CreateTweet;

public class CreateTweetCommandValidator : AbstractValidator<CreateTweetCommand>
{
    public CreateTweetCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .NotNull()
            .WithMessage("post can not be empty");
    }
}
