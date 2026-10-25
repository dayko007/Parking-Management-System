using System;

public class ParkingFeeCalculator
{
    public decimal HourlyRate { get; set; }

    public ParkingFeeCalculator(decimal hourlyRate)
    {
        HourlyRate = hourlyRate;
    }

    public decimal CalculateFee(TimeSpan duration)
    {
        double hours = Math.Max(1, Math.Ceiling(duration.TotalHours));
        return (decimal)hours * HourlyRate;
    }
}