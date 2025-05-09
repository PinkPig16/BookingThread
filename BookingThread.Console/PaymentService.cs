namespace BookingThread.Console;

public class PaymentService
{
    public async Task Payment(int roomId)
    {
        try
        {
            System.Console.WriteLine($"Enter amount of payment RoomId: {roomId} Thread: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(2000);
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Payment failed RoomId: {roomId} Thread: {Thread.CurrentThread.ManagedThreadId}, Error: {e.Message}");
        }
    }
}