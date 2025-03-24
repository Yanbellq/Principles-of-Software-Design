using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;

namespace Task1;

public partial class MainWindow : Window
{
    private Factory factory;

    public MainWindow()
    {
        InitializeComponent();
        factory = new Factory();

        var saveButton = this.FindControl<Button>("SaveButton");
        var displayButton = this.FindControl<Button>("DisplayButton");

        if (saveButton != null)
            saveButton.Click += SaveButton_Click;
        if (displayButton != null)
            displayButton.Click += DisplayButton_Click;
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        factory.Name = this.FindControl<TextBox>("NameTextBox")?.Text ?? "Unknown";
        factory.Location = this.FindControl<TextBox>("LocationTextBox")?.Text ?? "Unknown";
        factory.EmployeesCount = int.TryParse(this.FindControl<TextBox>("EmployeesTextBox")?.Text, out var employees) ? employees : 0;
        factory.ProductionCapacity = double.TryParse(this.FindControl<TextBox>("CapacityTextBox")?.Text, out var capacity) ? capacity : 0.0;
        factory.Owner = this.FindControl<TextBox>("OwnerTextBox")?.Text ?? "Unknown";
        factory.YearEstablished = int.TryParse(this.FindControl<TextBox>("YearTextBox")?.Text, out var year) ? year : 0;
        factory.ProductType = this.FindControl<TextBox>("ProductTextBox")?.Text ?? "Unknown";

        SaveToFile(factory);
        ShowMessage("Дані збережено у файл!", 100);

    }

    private void DisplayButton_Click(object? sender, RoutedEventArgs e)
    {
        string info = factory.DisplayInfo();
        double annualProduction = factory.CalculateAnnualProduction();
        bool isOld = factory.IsOldFactory();

        ShowMessage(info + $"\n\nРічна продуктивність: {annualProduction}\n" +
                    (isOld ? "Це новий завод." : "Це старий завод."), 250);
    }

    private void SaveToFile(Factory factory)
    {
        // using (StreamWriter writer = new StreamWriter("factory_data.txt" , append: true))
        using (StreamWriter writer = new StreamWriter("factory_data.txt"))
        {
            writer.WriteLine($"Name: {factory.Name}");
            writer.WriteLine($"Location: {factory.Location}");
            writer.WriteLine($"Employees Count: {factory.EmployeesCount}");
            writer.WriteLine($"Production Capacity: {factory.ProductionCapacity}");
            writer.WriteLine($"Owner: {factory.Owner}");
            writer.WriteLine($"Year Established: {factory.YearEstablished}");
            writer.WriteLine($"Product Type: {factory.ProductType}");
            writer.WriteLine(new string('-', 50));
        }
    }

    private async void ShowMessage(string message, int height)
    {
        var messageWindow = new Window
        {
            Width = 300,
            Height = height,
            Content = new StackPanel
            {
                Margin = new Thickness(10),
                Children =
                {
                    new TextBlock { Text = message, Margin = new Thickness(10) },
                    new Button { Content = "OK", Margin = new Thickness(10), HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center }
                }
            }
        };

        var button = (Button)((StackPanel)messageWindow.Content).Children[1];
        button.Click += (_, _) => messageWindow.Close();

        await messageWindow.ShowDialog(this);
    }
}
