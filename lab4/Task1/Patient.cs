using System;

public class Patient
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronymic { get; set; }
    public string Gender { get; set; }
    public string Nationality { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public string BirthDate { get; set; }
    public string PhoneNumber { get; set; }
    public Address HomeAddress { get; set; }
    public int HospitalNumber { get; set; }
    public int Department { get; set; }
    public string Diagnosis { get; set; }
    public string BloodType { get; set; }

    public Patient()
    {
        HomeAddress = new Address();
    }
}

public class Address
{
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string Apartment { get; set; }
}