namespace Application.Tweets.Commands.UpdateTweet;

public class UpdateTweetCommandValidator : AbstractValidator<UpdateTweetCommand>
{
    public UpdateTweetCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .NotNull()
            .WithMessage("post can not be empty");
    }
}
