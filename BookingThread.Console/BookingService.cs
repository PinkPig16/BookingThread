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
    private readonly object lockObject = new();
    public async Task<Room?> BookRoom(int roomId)
    {
        System.Console.WriteLine($"Room Id: {roomId} before globalSemaphore Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        
        await GlobalSemaphore.WaitAsync();
        System.Console.WriteLine($"Room Id: {roomId} AFTER globalSemaphore Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        try
        {
            System.Console.WriteLine($"Room Id: {roomId} before roomLock Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            var roomLocker = _semaphoresRoom.GetOrAdd(roomId, _ => new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1))).Value;
                                    
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                System.Console.WriteLine($"Room Id: {roomId} isn't found Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                return null;
            }
            
            await roomLocker.WaitAsync();
            System.Console.WriteLine($"Room Id: {roomId} after roomLock Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            try
            {
                if (!room.IsAvailable)
                {
                    System.Console.WriteLine($"Room Id: {roomId} isn't Available Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                    return null;
                }
                room.IsAvailable = false;
            }
            finally
            {
                roomLocker.Release();
                System.Console.WriteLine($"Room Id: {roomId} roomLock release Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            }

            try
            {
                await _paymentService.Payment(room);
                System.Console.WriteLine($"Room Id: {roomId} after Payment Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                _loggingQueue.EnqueueLog($"Room {roomId} has been booked Thread: {Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception ex)
            {
                await roomLocker.WaitAsync();
                room.IsAvailable = true;
                roomLocker.Release();
            }

            return room;
        }
        finally
        {
            GlobalSemaphore.Release();
            System.Console.WriteLine($"Room Id: {roomId} GlobalSemaphore release Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}