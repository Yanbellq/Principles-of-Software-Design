using System;

namespace Task1;

public class Factory
{
    // Поля класу
    private string name;
    private string location;
    private int employeesCount;
    private double productionCapacity;
    private string owner;
    private int yearEstablished;
    private string productType;

    // Властивості для доступу до полів
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Location
    {
        get { return location; }
        set { location = value; }
    }

    public int EmployeesCount
    {
        get { return employeesCount; }
        set { employeesCount = value; }
    }

    public double ProductionCapacity
    {
        get { return productionCapacity; }
        set { productionCapacity = value; }
    }

    public string Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public int YearEstablished
    {
        get { return yearEstablished; }
        set { yearEstablished = value; }
    }

    public string ProductType
    {
        get { return productType; }
        set { productType = value; }
    }

    // Конструктор без параметрів
    public Factory()
    {
        name = "Unknown";
        location = "Unknown";
        employeesCount = 0;
        productionCapacity = 0.0;
        owner = "Unknown";
        yearEstablished = 0;
        productType = "Unknown";
    }

    // Методи класу
    public string DisplayInfo()
    {
        return $"Factory Name: {name}\n" +
               $"Location: {location}\n" +
               $"Employees Count: {employeesCount}\n" +
               $"Production Capacity: {productionCapacity}\n" +
               $"Owner: {owner}\n" +
               $"Year Established: {yearEstablished}\n" +
               $"Product Type: {productType}";
    }

    public double CalculateAnnualProduction()
    {
        return productionCapacity * 365;
    }

    public bool IsOldFactory()
    {
        return yearEstablished > 1995;
    }
}