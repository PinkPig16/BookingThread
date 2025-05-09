using System.Collections.Concurrent;

namespace BookingThread.Console;

public class LoggingService
{
    private static readonly ConcurrentQueue<string> Logs = new();
    private const string LogFile = "log.txt";

    private void Log(string message)
    {
        File.AppendAllText(LogFile, message + Environment.NewLine);
    }

    public void EnqueueLog(string message)
    {
        Logs.Enqueue(message);
    }

    public bool TryDequeueLog(out string? message) => Logs.TryDequeue(out message);
    
}