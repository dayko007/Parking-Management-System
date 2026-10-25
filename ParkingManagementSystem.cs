using System;

public class Program
{
    public static void Main(string[] args)
    {
        ParkingFeeCalculator calculator =
            new ParkingFeeCalculator(3.00m);

        ParkingLot parkingLot = new ParkingLot(2, calculator);

        // Sample vehicles
        Vehicle vehicle1 = new Vehicle("ABC1234", "Car");
        Vehicle vehicle2 = new Vehicle("XYZ5678", "Motorcycle");
        Vehicle vehicle3 = new Vehicle("DEF9012", "Car");

        // Display vehicle information
        vehicle1.DisplayInfo();
        vehicle2.DisplayInfo();

        // Park vehicles
        ParkingTicket ticket1 = parkingLot.ParkVehicle(vehicle1);
        ParkingTicket ticket2 = parkingLot.ParkVehicle(vehicle2);

        parkingLot.DisplayParkingSpaces();

        // Test full parking lot and duplicate vehicle
        parkingLot.ParkVehicle(vehicle3);
        parkingLot.ParkVehicle(vehicle1);

        // Checkout first vehicle
        if (ticket1 != null)
        {
            parkingLot.Checkout(ticket1.TicketId);
        }

        parkingLot.DisplayParkingSpaces();

        // Test invalid ticket
        parkingLot.Checkout("INVALID1");

        // Park another vehicle
        ParkingTicket ticket3 = parkingLot.ParkVehicle(vehicle3);

        // Checkout remaining vehicles
        if (ticket2 != null)
        {
            parkingLot.Checkout(ticket2.TicketId);
        }

        if (ticket3 != null)
        {
            parkingLot.Checkout(ticket3.TicketId);
        }

        // Display final parking space status
        parkingLot.DisplayParkingSpaces();

        Console.WriteLine(
            "Parking management program completed.");
    }
}