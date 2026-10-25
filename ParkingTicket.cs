using System;

public class ParkingTicket
{
    public string TicketId { get; private set; }
    public Vehicle Vehicle { get; private set; }
    public ParkingSpace Space { get; private set; }
    public DateTime EntryTime { get; private set; }
    public DateTime? ExitTime { get; private set; }

    public ParkingTicket(Vehicle vehicle, ParkingSpace space)
    {
        TicketId = Guid.NewGuid().ToString("N").Substring(0, 8);
        Vehicle = vehicle;
        Space = space;
        EntryTime = DateTime.Now;
    }

    public TimeSpan CalculateDuration()
    {
        DateTime endTime = ExitTime ?? DateTime.Now;
        return endTime - EntryTime;
    }

    public void CloseTicket()
    {
        ExitTime = DateTime.Now;
    }
}