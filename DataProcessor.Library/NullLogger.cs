namespace DataProcessor.Library;

public class NullLogger : ILogger
{
    public void LogMessage(string message, string data)
    {
        // does nothing
    }
    public async Task LogMessageAsync(string message, string data)
    {
        // does nothing
        await Task.CompletedTask;
    }
}
