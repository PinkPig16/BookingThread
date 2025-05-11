

using System.Collections.Frozen;
using System.Security.Cryptography;
using BookingThread.Console;

var cancellationToken = new CancellationTokenSource().Token;
var bookingService = new BookingService();
var backgroundLog = new BackgroundLog();
var tasks = new List<Task>();
RandomNumberGenerator.Create();

/*for (int i = 0; i < 4; i++)
{
    Task.Run(() =>
    {
        tasks.Add(bookingService.BookRoom(RandomNumberGenerator.GetInt32(1, 30)));
    });
}*/

tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(3)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(3)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(3)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(3)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(2)));
tasks.Add(Task.Run(() => bookingService.BookRoom(3)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));
tasks.Add(Task.Run(() => bookingService.BookRoom(1)));

await Task.WhenAll(tasks);

Task.Run(() => backgroundLog.RunAsync(cancellationToken));


Console.ReadLine();
