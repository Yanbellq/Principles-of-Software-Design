using System;
using System.Threading;
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
        private CancellationTokenSource? cts;
        private readonly Random random = new();
        private int activeTasks = 0;
        private const int MaxThreads = 3; // Кількість потоків для перевірки

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            if (cts != null) return;

            cts = new CancellationTokenSource();
            activeTasks = MaxThreads;

            for (int i = 0; i < MaxThreads; i++)
            {
                int threadNumber = i + 1;
                int delay = (i + 1) * 1000; // Різні затримки для кожного потоку (1s, 2s, 3s)

                Task.Run(() => CheckServerStatus(threadNumber, delay, cts.Token), cts.Token);
            }

            AddLogMessage($"Запущено {MaxThreads} потоків перевірки сервера");
        }

        private void StopButton_Click(object? sender, RoutedEventArgs e)
        {
            if (cts == null) return;

            cts.Cancel();
            AddLogMessage("Запит на зупинку потоків...");
        }

        private async Task CheckServerStatus(int threadNumber, int delay, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // Імітація пінгу сервера (70% шанс успіху)
                    bool isSuccess = random.Next(10) < 7;
                    string message = isSuccess
                        ? $"Потік {threadNumber}: Успішний пінг (затримка {delay}ms) [{DateTime.Now:T}]"
                        : $"Потік {threadNumber}: Помилка пінгу (затримка {delay}ms) [{DateTime.Now:T}]";

                    Dispatcher.UIThread.Post(() => AddLogMessage(message));

                    await Task.Delay(delay, token); // Різні затримки для кожного потоку
                }
            }
            catch (TaskCanceledException)
            {
                // Очікувана помилка при скасуванні
            }
            finally
            {
                Interlocked.Decrement(ref activeTasks);
                if (activeTasks == 0)
                {
                    Dispatcher.UIThread.Post(() => AddLogMessage("Всі потоки зупинено"));
                    cts?.Dispose();
                    cts = null;
                }
            }
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