using System;

class Fractions : GenericEnum<Fractions, double>
{
    public static readonly double Sixth = 1.0 / 6.0;
    public static readonly double Fifth = 0.2;
    public static readonly double Quarter = 0.25;
    public static readonly double Third = 1.0 / 3.0;
    public static readonly double Half = 0.5;

    public double FractionOf(double amount)
    {
        return this.Value * amount;
    }
}

class Seasons : GenericEnum<Seasons, DateTime>
{
    public static readonly DateTime Spring = new DateTime(2011, 3, 1);
    public static readonly DateTime Summer = new DateTime(2011, 6, 1);
    public static readonly DateTime Autumn = new DateTime(2011, 9, 1);
    public static readonly DateTime Winter = new DateTime(2011, 12, 1);
}

public class Planets : GenericEnum<Planets, Planet>
{
    public static readonly Planet Mercury = new Planet(3.303e+23, 2.4397e6);
    public static readonly Planet Venus = new Planet(4.869e+24, 6.0518e6);
    public static readonly Planet Earth = new Planet(5.976e+24, 6.37814e6);
    public static readonly Planet Mars = new Planet(6.421e+23, 3.3972e6);
    public static readonly Planet Jupiter = new Planet(1.9e+27, 7.1492e7);
    public static readonly Planet Saturn = new Planet(5.688e+26, 6.0268e7);
    public static readonly Planet Uranus = new Planet(8.686e+25, 2.5559e7);
    public static readonly Planet Neptune = new Planet(1.024e+26, 2.4746e7);

    public bool IsCloserToSunThan(Planets p)
    {
        if (this.Index < p.Index) return true;
        return false;
    }
}

public class Planet
{
    public double Mass { get; private set; }  // in kilograms
    public double Radius { get; private set; } // in meters

    public Planet(double mass, double radius)
    {
        Mass = mass;
        Radius = radius;
    }

    // universal gravitational constant  (m^3 kg^-1 s^-2)
    public static double G = 6.67300E-11;

    public double SurfaceGravity()
    {
        return G * Mass / (Radius * Radius);
    }

    public double SurfaceWeight(double otherMass)
    {
        return otherMass * SurfaceGravity();
    }
}