using System;
using System.Collections.Generic;

public class ParkingLot
{
    public List<ParkingSpace> Spaces { get; private set; }
    public List<ParkingTicket> ActiveTickets { get; private set; }

    private ParkingFeeCalculator calculator;

    public ParkingLot(int numberOfSpaces, ParkingFeeCalculator feeCalculator)
    {
        calculator = feeCalculator;
        Spaces = new List<ParkingSpace>();
        ActiveTickets = new List<ParkingTicket>();

        for (int i = 1; i <= numberOfSpaces; i++)
        {
            Spaces.Add(new ParkingSpace(i));
        }
    }

    public ParkingTicket ParkVehicle(Vehicle vehicle)
    {
        foreach (ParkingTicket ticket in ActiveTickets)
        {
            if (ticket.Vehicle.PlateNumber == vehicle.PlateNumber)
            {
                Console.WriteLine("Vehicle is already parked.");
                return null;
            }
        }

        foreach (ParkingSpace space in Spaces)
        {
            if (!space.IsOccupied)
            {
                space.Occupy(vehicle);

                ParkingTicket ticket = new ParkingTicket(vehicle, space);
                ActiveTickets.Add(ticket);

                Console.WriteLine(
                    vehicle.PlateNumber + " parked in space " +
                    space.SpaceNumber);

                return ticket;
            }
        }

        Console.WriteLine("Parking lot is full.");
        return null;
    }

    public void Checkout(string ticketId)
    {
        ParkingTicket ticket = ActiveTickets.Find(
            t => t.TicketId == ticketId);

        if (ticket == null)
        {
            Console.WriteLine("Invalid ticket ID.");
            return;
        }

        ticket.CloseTicket();

        TimeSpan duration = ticket.CalculateDuration();
        decimal fee = calculator.CalculateFee(duration);

        Console.WriteLine("\n=== PARKING RECEIPT ===");
        Console.WriteLine("Ticket ID: " + ticket.TicketId);
        Console.WriteLine("Vehicle: " + ticket.Vehicle.PlateNumber);
        Console.WriteLine("Duration: " +
                          duration.TotalMinutes.ToString("F1") + " minutes");
        Console.WriteLine("Fee: RM " + fee.ToString("F2"));

        ticket.Space.Release();
        ActiveTickets.Remove(ticket);
    }

    public void DisplayParkingSpaces()
    {
        Console.WriteLine("\n=== PARKING SPACES ===");

        foreach (ParkingSpace space in Spaces)
        {
            string status = space.IsOccupied ? "Occupied" : "Available";

            Console.WriteLine(
                "Space " + space.SpaceNumber + ": " + status);
        }
    }
}