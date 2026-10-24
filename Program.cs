
using System;
using System.Collections.Generic;

// =====================================
// 1. VEHICLE CLASS
// =====================================
public class Vehicle
{
    public string PlateNumber { get; set; }
    public string VehicleType { get; set; }

    public Vehicle(string plateNumber, string vehicleType)
    {
        PlateNumber = plateNumber;
        VehicleType = vehicleType;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Vehicle: {PlateNumber} | Type: {VehicleType}");
    }
}

// =====================================
// 2. PARKING SPACE CLASS
// =====================================
public class ParkingSpace
{
    public int SpaceNumber { get; private set; }
    public bool IsOccupied { get; private set; }
    public Vehicle ParkedVehicle { get; private set; }

    public ParkingSpace(int spaceNumber)
    {
        SpaceNumber = spaceNumber;
        IsOccupied = false;
        ParkedVehicle = null;
    }

    public bool Occupy(Vehicle vehicle)
    {
        if (IsOccupied)
        {
            return false;
        }

        ParkedVehicle = vehicle;
        IsOccupied = true;
        return true;
    }

    public void Release()
    {
        ParkedVehicle = null;
        IsOccupied = false;
    }
}

// =====================================
// 3. PARKING TICKET CLASS
// =====================================
public class ParkingTicket
{
    public string TicketId { get; private set; }
    public Vehicle Vehicle { get; private set; }
    public ParkingSpace Space { get; private set; }
    public DateTime EntryTime { get; private set; }
    public DateTime? ExitTime { get; private set; }

    public ParkingTicket(Vehicle vehicle, ParkingSpace space)
    {
        TicketId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        Vehicle = vehicle;
        Space = space;
        EntryTime = DateTime.Now;
        ExitTime = null;
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

// =====================================
// 4. PARKING FEE CALCULATOR CLASS
// =====================================
public class ParkingFeeCalculator
{
    public decimal HourlyRate { get; set; }

    public ParkingFeeCalculator(decimal hourlyRate)
    {
        HourlyRate = hourlyRate;
    }

    public decimal CalculateFee(TimeSpan duration)
    {
        // Charge at least one hour.
        // Any partial hour is charged as a full hour.
        double hours = Math.Ceiling(duration.TotalHours);

        if (hours < 1)
        {
            hours = 1;
        }

        return (decimal)hours * HourlyRate;
    }
}

// =====================================
// 5. PARKING LOT CLASS
// =====================================
public class ParkingLot
{
    private List<ParkingSpace> spaces;
    private List<ParkingTicket> activeTickets;
    private ParkingFeeCalculator feeCalculator;

    public ParkingLot(int numberOfSpaces, ParkingFeeCalculator calculator)
    {
        spaces = new List<ParkingSpace>();
        activeTickets = new List<ParkingTicket>();
        feeCalculator = calculator;

        for (int i = 1; i <= numberOfSpaces; i++)
        {
            spaces.Add(new ParkingSpace(i));
        }
    }

    public ParkingTicket ParkVehicle(Vehicle vehicle)
    {
        // Prevent the same vehicle from entering twice.
        foreach (ParkingTicket ticket in activeTickets)
        {
            if (ticket.Vehicle.PlateNumber == vehicle.PlateNumber)
            {
                Console.WriteLine("Error: This vehicle is already parked.");
                return null;
            }
        }

        // Find an available parking space.
        foreach (ParkingSpace space in spaces)
        {
            if (!space.IsOccupied)
            {
                space.Occupy(vehicle);

                ParkingTicket ticket = new ParkingTicket(vehicle, space);
                activeTickets.Add(ticket);

                Console.WriteLine("\nVehicle parked successfully!");
                Console.WriteLine($"Parking space: {space.SpaceNumber}");
                Console.WriteLine($"Ticket ID: {ticket.TicketId}");
                Console.WriteLine($"Entry time: {ticket.EntryTime}");

                return ticket;
            }
        }

        Console.WriteLine("Error: Parking lot is full!");
        return null;
    }

    public void Checkout(string ticketId)
    {
        ParkingTicket ticket = activeTickets.Find(
            t => t.TicketId == ticketId
        );

        if (ticket == null)
        {
            Console.WriteLine("Error: Invalid ticket!");
            return;
        }

        ticket.CloseTicket();

        TimeSpan duration = ticket.CalculateDuration();
        decimal fee = feeCalculator.CalculateFee(duration);

        Console.WriteLine("\n========== PARKING RECEIPT ==========");
        Console.WriteLine($"Ticket ID: {ticket.TicketId}");
        Console.WriteLine($"Vehicle: {ticket.Vehicle.PlateNumber}");
        Console.WriteLine($"Parking space: {ticket.Space.SpaceNumber}");
        Console.WriteLine($"Entry time: {ticket.EntryTime}");
        Console.WriteLine($"Exit time: {ticket.ExitTime}");
        Console.WriteLine($"Duration: {duration.TotalMinutes:F1} minutes");
        Console.WriteLine($"Hourly rate: RM {feeCalculator.HourlyRate:F2}");
        Console.WriteLine($"Total fee: RM {fee:F2}");
        Console.WriteLine("=====================================");

        // Release the parking space.
        ticket.Space.Release();

        // Remove the ticket from the active list.
        activeTickets.Remove(ticket);

        Console.WriteLine("Parking space released successfully!");
    }

    public void DisplayParkingSpaces()
    {
        Console.WriteLine("\n========== PARKING SPACES ==========");

        foreach (ParkingSpace space in spaces)
        {
            string status = space.IsOccupied ? "Occupied" : "Available";

            Console.WriteLine(
                $"Space {space.SpaceNumber}: {status}"
            );
        }

        Console.WriteLine("====================================");
    }
}

// =====================================
// MAIN APPLICATION
// =====================================
public class Program
{
    public static void Main(string[] args)
    {
        // Create the fee calculator.
        ParkingFeeCalculator calculator =
            new ParkingFeeCalculator(3.00m);

        // Create a parking lot with 3 spaces.
        ParkingLot parkingLot =
            new ParkingLot(3, calculator);

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== PARKING MANAGEMENT SYSTEM =====");
            Console.WriteLine("1. Park a vehicle");
            Console.WriteLine("2. Checkout a vehicle");
            Console.WriteLine("3. Display parking spaces");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter vehicle plate number: ");
                    string plateNumber = Console.ReadLine();

                    Console.Write("Enter vehicle type (Car/Motorcycle): ");
                    string vehicleType = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(plateNumber) ||
                        string.IsNullOrWhiteSpace(vehicleType))
                    {
                        Console.WriteLine(
                            "Error: Vehicle details cannot be empty."
                        );
                        break;
                    }

                    Vehicle vehicle = new Vehicle(
                        plateNumber.Trim().ToUpper(),
                        vehicleType.Trim()
                    );

                    vehicle.DisplayInfo();

                    parkingLot.ParkVehicle(vehicle);
                    break;

                case "2":
                    Console.Write("Enter ticket ID: ");
                    string ticketId = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(ticketId))
                    {
                        Console.WriteLine(
                            "Error: Ticket ID cannot be empty."
                        );
                        break;
                    }

                    parkingLot.Checkout(ticketId.Trim().ToUpper());
                    break;

                case "3":
                    parkingLot.DisplayParkingSpaces();
                    break;

                case "4":
                    running = false;
                    Console.WriteLine("Thank you for using the system!");
                    break;

                default:
                    Console.WriteLine(
                        "Invalid option. Please choose 1, 2, 3, or 4."
                    );
                    break;
            }
        }
    }
}

