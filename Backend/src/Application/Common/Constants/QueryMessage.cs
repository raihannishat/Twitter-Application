namespace Application.Common.Constants;
public static class QueryMessages<T>
{
    public static string FailedToExecuteQuery = $"Failed to execute {typeof(T).Name} query.";
    public static string FailedToExecuteQueryException = $"An error occurred while processing the {typeof(T).Name} query.";
    public static string QueryExecutedSuccessfully = $"{typeof(T).Name} query executed successfully.";
}

