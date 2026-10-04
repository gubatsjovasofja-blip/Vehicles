namespace Vehicles.Core;

public abstract class Vehicle
{
    public string Make { get; }
    public string Model { get; }
    public double Odometer { get; private set; }

    protected Vehicle(string make, string model)
    {
        Make = make;
        Model = model;
    }

    public abstract string Move(double km);

    protected void AddKm(double km)
    {
        if (km <= 0)
        {
            throw new ArgumentException();
        }

        Odometer += km;
    }

    public override string ToString()
    {
        return $"{Make} {Model}";
    }
}