namespace BookingThread.Console;

public class PaymentService
{
    public async Task Payment(Room room)
    {
        try
        {
            System.Console.WriteLine($"Enter amount of payment RoomId: {room.Id} Thread: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(2000);
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Payment failed RoomId: {room.Id} Thread: {Thread.CurrentThread.ManagedThreadId}, Error: {e.Message}");
        }
    }
}