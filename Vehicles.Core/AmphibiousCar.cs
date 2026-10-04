namespace Vehicles.Core;

public class AmphibiousCar : Vehicle, IDriveable, ISwimmable
{
    public AmphibiousCar(string make, string model)
        : base(make, model)
    {
    }

    public string Drive(double km)
    {
        AddKm(km);
        return $"{Make} sõitis {km} km. Läbisõit: {Odometer} km.";
    }

    public string Swim(double km)
    {
        AddKm(km);
        return $"{Make} ujus {km} km. Läbisõit: {Odometer} km.";
    }

    public override string Move(double km)
    {
        return Drive(km);
    }
}