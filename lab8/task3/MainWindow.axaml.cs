using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace lab8
{
    public partial class MainWindow : Window
    {
        private TextBox monthTextBox;
        private TextBlock resultText;
        private Button showButton;
        private Button continueButton;
        private Button exitButton;
        private StackPanel mainPanel;
        private StackPanel dialogPanel;

        public enum Month
        {
            Січень = 1,
            Лютий,
            Березень,
            Квітень,
            Травень,
            Червень,
            Липень,
            Серпень,
            Вересень,
            Жовтень,
            Листопад,
            Грудень
        }

        public Dictionary<Month, string> monthAccessories = new Dictionary<Month, string>
        {
            { Month.Січень, "Пуховик" },
            { Month.Лютий, "Шапка" },
            { Month.Березень, "Пальто" },
            { Month.Квітень, "Легка куртка" },
            { Month.Травень, "Сонцезахисні окуляри" },
            { Month.Червень, "Кепка" },
            { Month.Липень, "Шорти" },
            { Month.Серпень, "Футболка" },
            { Month.Вересень, "Куртка" },
            { Month.Жовтень, "Светр" },
            { Month.Листопад, "Шарф" },
            { Month.Грудень, "Рукавиці" }
        };

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            // Get references to controls
            monthTextBox = this.FindControl<TextBox>("MonthTextBox");
            resultText = this.FindControl<TextBlock>("ResultText");
            showButton = this.FindControl<Button>("ShowButton");
            continueButton = this.FindControl<Button>("ContinueButton");
            exitButton = this.FindControl<Button>("ExitButton");
            mainPanel = this.FindControl<StackPanel>("MainPanel");
            dialogPanel = this.FindControl<StackPanel>("DialogPanel");

            // Initially hide the dialog buttons
            dialogPanel.IsVisible = false;
        }

        // Method to display month and accessory
        public void ShowMonthAndAccessory(int monthNumber)
        {
            if (Enum.IsDefined(typeof(Month), monthNumber))
            {
                Month month = (Month)monthNumber;
                string accessory = monthAccessories[month];

                // Display result in TextBlock
                resultText.Text = $"Місяць: {month}\nАксесуар: {accessory}";
            }
            else
            {
                resultText.Text = "Невірний номер місяця!";
            }
        }

        private void OnShowButtonClick(object sender, RoutedEventArgs e)
        {
            int monthNumber;

            // Check if the field is empty
            if (string.IsNullOrEmpty(monthTextBox.Text))
            {
                resultText.Text = "Будь ласка, введіть номер місяця.";
                return;
            }

            // Validate user input
            if (!int.TryParse(monthTextBox.Text, out monthNumber) || monthNumber < 1 || monthNumber > 12)
            {
                resultText.Text = "Будь ласка, введіть коректний номер місяця (від 1 до 12).";
                return;
            }

            // Call method to show month and accessory
            ShowMonthAndAccessory(monthNumber);

            // Show the dialog buttons
            dialogPanel.IsVisible = true;
            showButton.IsVisible = false;
        }

        private void OnContinueButtonClick(object sender, RoutedEventArgs e)
        {
            // Clear the input field for new input
            monthTextBox.Text = string.Empty;
            resultText.Text = string.Empty;

            // Hide dialog buttons and show main button
            dialogPanel.IsVisible = false;
            showButton.IsVisible = true;
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }
    }
}