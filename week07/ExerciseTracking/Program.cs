using System;
using System.Collections.Generic;

abstract class Activity
{
    private DateTime _date;
    private int _minutes;

    protected Activity(DateTime date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    public DateTime Date => _date;
    public int Minutes => _minutes;

    public abstract double GetDistance(); // km
    public abstract double GetSpeed();    // kph
    public abstract double GetPace();     // min per km

    public virtual string GetSummary()
    {
        return $"{Date:dd MMM yyyy} {GetType().Name} ({Minutes} min): " +
               $"Distance {GetDistance():0.0} km, " +
               $"Speed {GetSpeed():0.0} kph, " +
               $"Pace: {GetPace():0.00} min per km";
    }
}

class Running : Activity
{
    private double _distanceKm;

    public Running(DateTime date, int minutes, double distanceKm)
        : base(date, minutes)
    {
        _distanceKm = distanceKm;
    }

    public override double GetDistance() => _distanceKm;

    public override double GetSpeed() => (GetDistance() / Minutes) * 60;

    public override double GetPace() => Minutes / GetDistance();
}

class Cycling : Activity
{
    private double _speedKph;

    public Cycling(DateTime date, int minutes, double speedKph)
        : base(date, minutes)
    {
        _speedKph = speedKph;
    }

    public override double GetDistance() => (_speedKph * Minutes) / 60;

    public override double GetSpeed() => _speedKph;

    public override double GetPace() => 60 / _speedKph;
}

class Swimming : Activity
{
    private int _laps;

    public Swimming(DateTime date, int minutes, int laps)
        : base(date, minutes)
    {
        _laps = laps;
    }

    public override double GetDistance()
    {
        // 50 meters per lap → convert to km
        return (_laps * 50) / 1000.0;
    }

    public override double GetSpeed() => (GetDistance() / Minutes) * 60;

    public override double GetPace() => Minutes / GetDistance();
}

class Program
{
    static void Main()
    {
        List<Activity> activities = new List<Activity>
        {
            new Running(new DateTime(2022, 11, 3), 30, 4.8),   // 4.8 km run
            new Cycling(new DateTime(2022, 11, 3), 40, 20.0), // 20 kph cycling
            new Swimming(new DateTime(2022, 11, 3), 25, 30)   // 30 laps swimming
        };

        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
