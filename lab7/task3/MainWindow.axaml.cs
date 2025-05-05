using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace lab7
{
    public partial class MainWindow : Window
    {
        private readonly List<ConstructionProject> projects = new();

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // Initialize combo box
            var comboBox = this.Find<ComboBox>("comboBox1");
            if (comboBox != null)
            {
                comboBox.ItemsSource = new List<string> { "будується", "зданий", "заморожено" };
                comboBox.SelectedIndex = 0;
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var textBox1 = this.Find<TextBox>("textBox1");
                var textBox2 = this.Find<TextBox>("textBox2");
                var textBox3 = this.Find<TextBox>("textBox3");
                var textBox4 = this.Find<TextBox>("textBox4");
                var textBox5 = this.Find<TextBox>("textBox5");
                var comboBox1 = this.Find<ComboBox>("comboBox1");

                if (textBox1 == null || textBox2 == null || textBox3 == null ||
                    textBox4 == null || textBox5 == null || comboBox1 == null)
                {
                    return;
                }

                var project = new ConstructionProject
                {
                    CompanyName = textBox1.Text ?? string.Empty,
                    ObjectName = textBox2.Text ?? string.Empty,
                    Area = double.Parse(textBox3.Text ?? "0"),
                    StartDate = DateTime.ParseExact(textBox4.Text ?? string.Empty, "ddMMyyyy", CultureInfo.InvariantCulture),
                    PlannedEndDate = DateTime.ParseExact(textBox5.Text ?? string.Empty, "ddMMyyyy", CultureInfo.InvariantCulture),
                    Status = comboBox1.SelectedItem as string ?? string.Empty
                };

                projects.Add(project);
                UpdateList();
                ShowStatusMessage("Об'єкт додано!");
            }
            catch (Exception ex)
            {
                ShowStatusMessage("Помилка: " + ex.Message);
            }
        }

        private void UpdateList()
        {
            var listBox = this.Find<ListBox>("listBox1");
            if (listBox == null) return;

            var items = projects.Select(p => $"{p.ObjectName} ({p.CompanyName}) - {p.Status}").ToList();
            listBox.ItemsSource = items;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.Find<ListBox>("listBox2");
            if (listBox == null) return;

            var items = projects
                .Select(p => $"{p.ObjectName}: {(p.PlannedEndDate - p.StartDate).Days} днів")
                .ToList();
            listBox.ItemsSource = items;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.Find<ListBox>("listBox2");
            if (listBox == null) return;

            var items = projects
                .Where(p => string.Equals(p.Status, "будується", StringComparison.OrdinalIgnoreCase) &&
                            p.PlannedEndDate.Year == DateTime.Now.Year &&
                            p.PlannedEndDate.Month >= 10)
                .Select(p => p.ObjectName)
                .ToList();
            listBox.ItemsSource = items;
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.Find<ListBox>("listBox2");
            if (listBox == null) return;

            var thisYear = DateTime.Now.Year;
            var filtered = projects.Where(p => p.PlannedEndDate.Year == thisYear).ToList();
            double totalArea = filtered.Sum(p => p.Area);

            var items = filtered
                .Select(p => $"{p.ObjectName}: {p.Area} м²")
                .Concat(new[] { $"Загальна кількість: {filtered.Count}, площа: {totalArea} м²" })
                .ToList();

            listBox.ItemsSource = items;
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.Find<ListBox>("listBox2");
            if (listBox == null || projects.Count == 0) return;

            var min = projects.OrderBy(p => (p.PlannedEndDate - p.StartDate).Days).First();
            listBox.ItemsSource = new[] { $"Найшвидший об'єкт: {min.ObjectName}, {min.PlannedDurationDays} днів" };
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            var listBox = this.Find<ListBox>("listBox2");
            if (listBox == null) return;

            var items = projects
                .Where(p => string.Equals(p.Status, "будується", StringComparison.OrdinalIgnoreCase) &&
                          (DateTime.Now - p.PlannedEndDate).TotalDays > 180)
                .Select(p => $"{p.CompanyName} - {p.ObjectName} (прострочено)")
                .ToList();
            listBox.ItemsSource = items;
        }

        private void ShowStatusMessage(string message)
        {
            var statusBlock = this.Find<TextBlock>("statusTextBlock");
            if (statusBlock != null)
            {
                statusBlock.Text = message;
            }
        }
    }

    public struct ConstructionProject
    {
        public string CompanyName;
        public string ObjectName;
        public double Area;
        public DateTime StartDate;
        public DateTime PlannedEndDate;
        public string Status;

        public int PlannedDurationDays => (PlannedEndDate - StartDate).Days;
    }
}