using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace lab13
{
    public partial class MainWindow : Window
    {
        private Thread? serverCheckThread;
        private bool isChecking = false;
        private readonly Random random = new();

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            if (isChecking) return;

            isChecking = true;
            serverCheckThread = new Thread(CheckServerStatus);
            serverCheckThread.Start();
            AddLogMessage("Start...");
        }

        private void StopButton_Click(object? sender, RoutedEventArgs e)
        {
            if (!isChecking) return;

            isChecking = false;
            if (serverCheckThread != null && serverCheckThread.IsAlive)
                serverCheckThread.Join();
            AddLogMessage("Stop");
        }

        private void CheckServerStatus()
        {
            while (isChecking)
            {
                // Імітація пінгу сервера (50% шанс успіху)
                bool isSuccess = random.Next(2) == 0;
                string message = isSuccess
                    ? $"Success [{DateTime.Now:T}]"
                    : $"Fail [{DateTime.Now:T}]";

                Dispatcher.UIThread.Post(() => AddLogMessage(message));

                Thread.Sleep(5000); // Перевірка кожні 5 секунд
            }
        }

        private void AddLogMessage(string message)
        {
            MessageListBox.Items.Add(message);
            if (MessageListBox.Items.Count > 100)
                MessageListBox.Items.RemoveAt(0);

            // Автоматична прокрутка до останнього повідомлення
            MessageListBox.ScrollIntoView(MessageListBox.Items.Count - 1);
        }
    }
}