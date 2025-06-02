using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace lab13
{
    public partial class MainWindow : Window
    {
        private bool isRunning = false;
        private readonly Random random = new();
        private int requestCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private async void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            if (isRunning) return;

            isRunning = true;
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            AddLogMessage("Початок роботи з базою даних...");

            while (isRunning)
            {
                requestCounter++;
                try
                {
                    // Імітація асинхронного запиту до БД
                    var result = await SimulateDatabaseQueryAsync(requestCounter);
                    Dispatcher.UIThread.Post(() => AddLogMessage(result));
                }
                catch (TaskCanceledException)
                {
                    Dispatcher.UIThread.Post(() => AddLogMessage("Запит скасовано"));
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.Post(() => AddLogMessage($"Помилка: {ex.Message}"));
                }
            }
        }

        private void StopButton_Click(object? sender, RoutedEventArgs e)
        {
            isRunning = false;
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            AddLogMessage("Роботу з базою даних зупинено");
        }

        private async Task<string> SimulateDatabaseQueryAsync(int requestId)
        {
            AddLogMessage($"Запит #{requestId}: Відправка запиту до БД...");

            // Імітація затримки запиту до БД (4 секунди)
            await Task.Delay(4000);

            // Генерація випадкових даних
            var tables = new List<string> { "Користувачі", "Замовлення", "Товари", "Транзакції" };
            var operations = new List<string> { "SELECT", "UPDATE", "INSERT", "DELETE" };
            var statuses = new List<string> { "успішно", "з помилкою" };

            string table = tables[random.Next(tables.Count)];
            string operation = operations[random.Next(operations.Count)];
            string status = statuses[random.Next(statuses.Count)];
            int rowsAffected = random.Next(1, 100);

            return $"Запит #{requestId}: {operation} на таблиці {table} виконано {status}. " +
                   $"Змінено рядків: {rowsAffected}. Час: {DateTime.Now:T}";
        }

        private void AddLogMessage(string message)
        {
            MessageListBox.Items.Add(message);
            if (MessageListBox.Items.Count > 100)
                MessageListBox.Items.RemoveAt(0);

            MessageListBox.ScrollIntoView(MessageListBox.Items.Count - 1);
        }
    }
}