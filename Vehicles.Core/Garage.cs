
using System;
using System.Collections.ObjectModel;

namespace Vehicles.Core;

public class Garage
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    public event Action<string>? Message;

    public void Add(Vehicle vehicle)
    {
        foreach (Vehicle other in Vehicles)
        {
            Message?.Invoke($"{other} annab signaali!");
        }

        Vehicles.Add(vehicle);
        Message?.Invoke($"{vehicle} lisati garaaži.");
    }

    public void Remove(Vehicle vehicle)
    {
        if (!Vehicles.Remove(vehicle))
        {
            return;
        }

        Message?.Invoke($"{vehicle} eemaldati garaažist.");

        foreach (Vehicle other in Vehicles)
        {
            Message?.Invoke($"{other} vilgutab tulesid!");
        }
    }
}
