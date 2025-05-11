using System.Collections.Concurrent;

namespace BookingThread.Console;

public class BookingService
{
    private const int _count = 10;
    private readonly ConcurrentDictionary<int, Lazy<SemaphoreSlim>> _semaphoresRoom = new ConcurrentDictionary<int, Lazy<SemaphoreSlim>>();
    private readonly Dictionary<int, Room> _rooms = RoomService.GetRooms();
    private readonly PaymentService _paymentService = new();
    private readonly LoggingService _loggingQueue = new();
    private static readonly SemaphoreSlim GlobalSemaphore = new(_count, _count);
    public async Task<Room?> BookRoom(int roomId)
    {
        System.Console.WriteLine($"Room Id: {roomId} before globalSemaphore Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        
        await GlobalSemaphore.WaitAsync();
        System.Console.WriteLine($"Room Id: {roomId} AFTER globalSemaphore Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        try
        {
            System.Console.WriteLine($"Room Id: {roomId} before roomLock Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            var roomLocker = _semaphoresRoom.GetOrAdd(roomId, _ => new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1))).Value;
            
            await roomLocker.WaitAsync();
            System.Console.WriteLine($"Room Id: {roomId} after roomLock Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            try
            {
                        
                if (!_rooms.TryGetValue(roomId, out var room) || !room.IsAvailable)
                {
                    System.Console.WriteLine($"Room Id: {roomId} isn't available or null Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                    return null;
                }
                
                await _paymentService.Payment(roomId);
                room.IsAvailable = false;
                System.Console.WriteLine($"Room Id: {roomId} after Payment Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                _loggingQueue.EnqueueLog($"Room {roomId} has been booked Thread: {Thread.CurrentThread.ManagedThreadId}");
                return room;
            }
            finally
            {
                roomLocker.Release();
                System.Console.WriteLine($"Room Id: {roomId} roomLock release Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            }
        }
        finally
        {
            GlobalSemaphore.Release();
            System.Console.WriteLine($"Room Id: {roomId} GlobalSemaphore release Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}