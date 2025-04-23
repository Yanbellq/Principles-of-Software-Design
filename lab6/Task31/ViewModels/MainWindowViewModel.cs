// filepath: /home/yanbellq/Документы/vscode/C#/lab6/Task31/ViewModels/MainWindowViewModel.cs
using System.Collections.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Task31.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();
    }
}



public class City : IComparable<City>, IComparer<City>, IEnumerable<City>
{
    
    public string Name { get; set; }
    public double Area { get; set; } // Територія
    public int Population { get; set; } // Населення

    public City(string name, double area, int population)
    {
        Name = name;
        Area = area;
        Population = population;

        // ShowMassage($"Додано місто: {name}, Площа: {area}, Населення: {population}");
    }

    // Реалізація IComparable для порівняння за територією
    public int CompareTo(City? other)
    {
        if (other == null) return 1;
        return Area.CompareTo(other.Area);
    }

    // Реалізація IComparer для порівняння за територією і населенням
    public int Compare(City? x, City? y)
    {
        if (x == null || y == null) return 0;

        int areaComparison = x.Area.CompareTo(y.Area);
        if (areaComparison != 0)
            return areaComparison;

        return x.Population.CompareTo(y.Population);
    }

    // Реалізація IEnumerable
    public IEnumerator<City> GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return $"{Name} - Площа: {Area}, Населення: {Population}";
    }


}