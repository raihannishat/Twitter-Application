namespace Application.Comments.Queries.GetComments;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Result<List<CommentViewModel>>>
{
    private readonly ICommentService _commentService;
    private readonly ILogger<GetCommentQueryHandler> _logger;

    public GetCommentQueryHandler(ICommentService commentService,
        ILogger<GetCommentQueryHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<Result<List<CommentViewModel>>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _commentService.GetCommentsAsync(request.TweetId, request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetCommentQuery>.QueryExecutedSuccessfully);
                
                return Result<List<CommentViewModel>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetCommentQuery>.FailedToExecuteQuery);
            
            return Result<List<CommentViewModel>>.Failure(QueryMessages<GetCommentQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetCommentQuery>.FailedToExecuteQueryException, ex.Message);
            
            return Result<List<CommentViewModel>>.Failure(QueryMessages<GetCommentQuery>.FailedToExecuteQueryException);
        }
    }
}
