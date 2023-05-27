namespace Application.Common.Constants;
public static class CommandMessages<T>
{
    public static string FailedToExecuteCommand = $"Failed to execute {typeof(T).Name}.";
    public static string FailedToExecuteCommandException = $"An error occurred while processing the {typeof(T).Name}.";
    public static string CommandExecutedSuccessfully = $"{typeof(T).Name} executed successfully.";
}


