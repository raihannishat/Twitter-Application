namespace Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<CommentResponse>>
{
    private readonly ICommentService _commentService;
    private readonly ILogger<DeleteCommentCommandHandler> _logger;

    public DeleteCommentCommandHandler(ICommentService commentService,
        ILogger<DeleteCommentCommandHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<Result<CommentResponse>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var commentResult = await _commentService.DeleteCommentAsync(request.TweetId, request.CommentId);

            if (commentResult != null)
            {
                var response = new CommentResponse()
                {
                    TotalComments = commentResult
                };
                _logger.LogInformation(CommandMessages<DeleteCommentCommand>.CommandExecutedSuccessfully);

                return Result<CommentResponse>.Success(response);
            }
            _logger.LogError(CommandMessages<DeleteCommentCommand>.FailedToExecuteCommand);

            return Result<CommentResponse>.Failure(CommandMessages<DeleteCommentCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<DeleteCommentCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<CommentResponse>.Failure(CommandMessages<DeleteCommentCommand>.FailedToExecuteCommandException);
        }
    }
}
