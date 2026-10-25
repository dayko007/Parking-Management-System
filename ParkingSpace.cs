public class ParkingSpace
{
    public int SpaceNumber { get; set; }
    public bool IsOccupied { get; private set; }
    public Vehicle ParkedVehicle { get; private set; }

    public ParkingSpace(int number)
    {
        SpaceNumber = number;
        IsOccupied = false;
    }

    public void Occupy(Vehicle vehicle)
    {
        ParkedVehicle = vehicle;
        IsOccupied = true;
    }

    public void Release()
    {
        ParkedVehicle = null;
        IsOccupied = false;
    }
}