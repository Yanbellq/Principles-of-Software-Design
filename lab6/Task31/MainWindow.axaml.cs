using System.Collections.ObjectModel;
using System.Linq; // Добавлено для методов LINQ
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;


namespace Task31;

public partial class MainWindow : Window
{
    // private ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();
    private List<City> Cities;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        Cities = new List<City>();
    }


    private void Show()
    {
        CityListBox.Text = "";
        foreach (var city in Cities)
        {
            CityListBox.Text += $"{city.Name} - Площа: {city.Area}, Населення: {city.Population}\n";
        }

    }
    private void AddCity_Click(object sender, RoutedEventArgs e)
    {
        string? name = CityName.Text;
        if (!string.IsNullOrWhiteSpace(name) && double.TryParse(CityArea.Text, out double area) && int.TryParse(CityPopulation.Text, out int population))
        {
            Cities.Add(new City(name, area, population));
            // ShowMassage($"Додано місто");
            SortCitiesByPopulation();
            ClearInputs();

            Show();
            ShowMessage("Місто додано успішно!");
        }
        else
        {
            ShowMessage("Будь ласка, введіть коректні дані.");
        }
    }

    private void SortCitiesByPopulation()
    {
        var sortedCities = Cities.OrderBy(c => c.Population).ToList();
        Cities.Clear();
        foreach (var city in sortedCities)
        {
            Cities.Add(city);
        }
    }

    private void ClearInputs()
    {
        CityName.Text = string.Empty;
        CityArea.Text = string.Empty;
        CityPopulation.Text = string.Empty;
    }

    public async void ShowMessage(string message)
    {
        var dialog = new Window
        {
            Title = "Повідомлення",
            Width = 300,
            Height = 150,
            Content = new TextBlock
            {
                Text = message,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            }
        };

        await dialog.ShowDialog(this);
    }
}