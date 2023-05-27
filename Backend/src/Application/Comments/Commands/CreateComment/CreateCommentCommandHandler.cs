using Application.Common.Constants;

namespace Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentResponse>>
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CreateCommentCommandHandler> _logger;

    public CreateCommentCommandHandler(ICommentService commentService,
        ILogger<CreateCommentCommandHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<Result<CommentResponse>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var commentResult = await _commentService.CreateCommentAsync(request.TweetId, request.Content);

            if (commentResult != null)
            {
                var response = new CommentResponse
                {
                    TotalComments = commentResult
                };

                _logger.LogInformation(CommandMessages<CreateCommentCommand>.CommandExecutedSuccessfully);
                
                return Result<CommentResponse>.Success(response);
            }
            _logger.LogError(CommandMessages<CreateCommentCommand>.FailedToExecuteCommand);
            
            return Result<CommentResponse>.Failure(CommandMessages<CreateCommentCommand>.FailedToExecuteCommand);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<CreateCommentCommand>.FailedToExecuteCommandException, ex.Message);
            
            return Result<CommentResponse>.Failure(CommandMessages<CreateCommentCommand>.FailedToExecuteCommandException);
        }
    }

}
