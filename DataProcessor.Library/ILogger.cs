namespace DataProcessor.Library;

public interface ILogger
{
    void LogMessage(string message, string data);
    Task LogMessageAsync(string message, string datat);
}