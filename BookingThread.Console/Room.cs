namespace BookingThread.Console;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
    
    public Room(int id, string room, bool isAvailable)
    {
        Id = id;
        Name = room;
        IsAvailable = isAvailable;
    }
}