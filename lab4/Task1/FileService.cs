using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileService
{
    public static List<Patient> ReadPatientsFromFile(string filePath)
    {
        List<Patient> patients = new List<Patient>();
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] data = line.Split('|');
                Patient patient = new Patient
                {
                    LastName = data[0],
                    FirstName = data[1],
                    Patronymic = data[2],
                    Gender = data[3],
                    Nationality = data[4],
                    Height = double.Parse(data[5]),
                    Weight = double.Parse(data[6]),
                    BirthDate = DateTime.Parse(data[7]),
                    PhoneNumber = data[8],
                    HomeAddress = new Address
                    {
                        PostalCode = data[9],
                        Country = data[10],
                        Region = data[11],
                        District = data[12],
                        City = data[13],
                        Street = data[14],
                        House = data[15],
                        Apartment = data[16]
                    },
                    HospitalNumber = int.Parse(data[17]),
                    Department = int.Parse(data[18]),
                    MedicalCardNumber = data[19],
                    Diagnosis = data[20],
                    BloodType = data[21]
                };
                patients.Add(patient);
            }
        }
        return patients;
    }

    public static void WritePatientsToFile(string filePath, List<Patient> patients)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var patient in patients.Where(p => p.Department == 18))
            {
                writer.WriteLine($"Прізвище: {patient.LastName}");
                writer.WriteLine($"Ім'я: {patient.FirstName}");
                writer.WriteLine($"По-батькові: {patient.Patronymic}");
                writer.WriteLine($"Стать: {patient.Gender}");
                writer.WriteLine($"Національність: {patient.Nationality}");
                writer.WriteLine($"Зріст: {patient.Height}");
                writer.WriteLine($"Вага: {patient.Weight}");
                writer.WriteLine($"Дата народження: {patient.BirthDate}");
                writer.WriteLine($"Номер телефону: {patient.PhoneNumber}");
                writer.WriteLine($"Адреса: {patient.HomeAddress.PostalCode}, {patient.HomeAddress.Country}, {patient.HomeAddress.Region}, {patient.HomeAddress.District}, {patient.HomeAddress.City}, {patient.HomeAddress.Street}, {patient.HomeAddress.House}, {patient.HomeAddress.Apartment}");
                writer.WriteLine($"Номер лікарні: {patient.HospitalNumber}");
                writer.WriteLine($"Відділення: {patient.Department}");
                writer.WriteLine($"Номер медичної картки: {patient.MedicalCardNumber}");
                writer.WriteLine($"Діагноз: {patient.Diagnosis}");
                writer.WriteLine($"Група крові: {patient.BloodType}");
                writer.WriteLine(new string('-', 50));
            }
        }
    }
}