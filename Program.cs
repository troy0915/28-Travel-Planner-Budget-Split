using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FareRule
{
    private decimal baseRate;
    private decimal peakMultiplier;
    private decimal offPeakMultiplier;
    private Dictionary<string, decimal> passengerMultipliers;

    public FareRule(decimal baseRate, decimal peakMultiplier, decimal offPeakMultiplier)
    {
        this.baseRate = baseRate;
        this.peakMultiplier = peakMultiplier;
        this.offPeakMultiplier = offPeakMultiplier;
        passengerMultipliers = new Dictionary<string, decimal>
        {
            { "ADULT", 1.0m },
            { "STUDENT", 0.8m },
            { "SENIOR", 0.7m }
        };
    }

    public decimal CalculateFare(decimal distance, string timeSlot, string passengerType)
    {
        if (distance <= 0)
        {
            throw new ArgumentException("Distance must be greater than 0.");
        }

        decimal multiplier = timeSlot.ToUpper() == "PEAK" ? peakMultiplier : offPeakMultiplier;

        if (!passengerMultipliers.TryGetValue(passengerType.ToUpper(), out decimal passengerMultiplier))
        {
            throw new ArgumentException("Unknown passenger category.");
        }

        return baseRate * distance * multiplier * passengerMultiplier;
    }

    public decimal ApplyBestCoupon(decimal fare, string couponCode)
    {
        couponCode = couponCode.ToUpper();
        decimal discount = 0;

        switch (couponCode)
        {
            case "SAVE10":
                discount = 0.10m;
                break;
            case "SAVE20":
                discount = 0.20m; 
                break;
            case "SAVE5":
                discount = 5.00m; 
                break;
            default:
                Console.WriteLine("Invalid coupon code.");
                break;
        }

        if (discount > 0)
        {
            if (discount < 1) 
            {
                return fare * (1 - discount);
            }
            else
            {
                return fare - discount;
            }
        }

        return fare; 
    }
}

namespace _28__Travel_Planner_Budget_Split
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FareRule fareRule = new FareRule(10.0m, 1.5m, 1.0m);
            while (true)
            {
                try
                {
                    Console.Write("Enter trip distance (km): ");
                    decimal distance = Convert.ToDecimal(Console.ReadLine());

                    Console.Write("Enter time slot (peak/off-peak): ");
                    string timeSlot = Console.ReadLine();

                    Console.Write("Enter passenger type (adult/student/senior): ");
                    string passengerType = Console.ReadLine();

                    Console.Write("Enter coupon code (or leave blank): ");
                    string couponCode = Console.ReadLine();

                    decimal fare = fareRule.CalculateFare(distance, timeSlot, passengerType);
                    fare = fareRule.ApplyBestCoupon(fare, couponCode);

                    Console.WriteLine($"Total fare: {fare:C}");
                    break; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}. Please try again.");
                }
            }
        }
    }
}




