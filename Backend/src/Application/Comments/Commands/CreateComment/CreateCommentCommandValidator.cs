namespace Application.Comments.Commands.CreateComment;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
	public CreateCommentCommandValidator()
	{
		RuleFor(x => x.Content).NotEmpty();
	}
}
