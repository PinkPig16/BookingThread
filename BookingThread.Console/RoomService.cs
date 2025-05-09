using System.Collections.Concurrent;

namespace BookingThread.Console;

public class RoomService
{
    public static Dictionary<int, Room> GetRooms() => new Dictionary<int, Room>()
        {
            { 1, new Room(1, "Room #1", true) },
            { 2, new Room(2, "Room #2", false) },
            { 3, new Room(3, "Room #3", true) },
            { 4, new Room(4, "Room #4", true) },
            { 5, new Room(5, "Room #5", false) },
            { 6, new Room(6, "Room #6", true) },
            { 7, new Room(7, "Room #7", true) },
            { 8, new Room(8, "Room #8", true) },
            { 9, new Room(9, "Room #9", true) },
            { 10, new Room(10, "Room #10", true) },
            { 11, new Room(11, "Room #11", true) },
            { 12, new Room(12, "Room #12", false) },
            { 13, new Room(13, "Room #13", true) },
            { 14, new Room(14, "Room #14", true) }
        };
}