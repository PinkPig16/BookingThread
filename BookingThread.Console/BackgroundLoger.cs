namespace BookingThread.Console;

public class BackgroundLog
{
    private readonly Mutex _fileMutex = new Mutex(false, @"Global\BackgroundLoger");
    private readonly string fileName = "log.txt";
    private readonly LoggingService _loggingService = new LoggingService();
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _fileMutex.WaitOne();
            try
            {
                while(_loggingService.TryDequeueLog(out string? message))
                {
                    File.AppendAllText(fileName,message);
                }
            }
            finally
            {
                _fileMutex.ReleaseMutex();
            }

            await Task.Delay(1000, cancellationToken);
        }
    }
}