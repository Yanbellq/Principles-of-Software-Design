using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace Task1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        string inputPath = "db/Input Data.txt";
        var patients = FileService.ReadPatientsFromFile(inputPath);

        var wrapPanel = this.FindControl<WrapPanel>("PatientsWrapPanel");

        var filteredPatients = patients.Where(p => p.Department == 18).ToList();

        for (int i = 0; i < filteredPatients.Count; i++)
        {
            var patient = filteredPatients[i];

            // Створюємо ListBox для кожного пацієнта
            var listBox = new ListBox
            {
                Width = 300,
                Height = 400,
                Margin = new Avalonia.Thickness(10),
                // Background = Avalonia.Media.Brushes.Black,  // Темний фон
                Foreground = Avalonia.Media.Brushes.White   // Білий текст
            };

            // Додаємо інформацію про пацієнта
            var patientHeader = new TextBlock
            {
                Text = $"Пацієнт {i + 1}",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                FontSize = 20,
                FontWeight = Avalonia.Media.FontWeight.Bold
            };
            listBox.Items.Add(patientHeader);
            listBox.Items.Add($"Прізвище: {patient.LastName}");
            listBox.Items.Add($"Ім'я: {patient.FirstName}");
            listBox.Items.Add($"По-батькові: {patient.Patronymic}");
            listBox.Items.Add($"Стать: {patient.Gender}");
            listBox.Items.Add($"Національність: {patient.Nationality}");
            listBox.Items.Add($"Зріст: {patient.Height}");
            listBox.Items.Add($"Вага: {patient.Weight}");
            listBox.Items.Add($"Дата народження: {patient.BirthDate}");
            listBox.Items.Add($"Телефон: {patient.PhoneNumber}");
            listBox.Items.Add($"Адреса: {patient.HomeAddress.City}, {patient.HomeAddress.Street}, {patient.HomeAddress.House}");
            listBox.Items.Add($"Номер лікарні: {patient.HospitalNumber}");
            listBox.Items.Add($"Відділення: {patient.Department}");
            listBox.Items.Add($"Діагноз: {patient.Diagnosis}");
            listBox.Items.Add($"Група крові: {patient.BloodType}");

            // Додаємо ListBox в WrapPanel
            if (wrapPanel != null)
            {
                wrapPanel.Children.Add(listBox);
            }
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
