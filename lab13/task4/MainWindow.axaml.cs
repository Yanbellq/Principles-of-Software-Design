using System;
using System.Net.NetworkInformation;
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
        private TextBox IpInput;
        private TextBox OutputBox;
        private Button ScanNetworkButton;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            Width = 600;
            Height = 500;
            Title = "Мережевий сканер";

            // Створення елементів інтерфейсу
            IpInput = new TextBox
            {
                Watermark = "Введіть IP-адресу (наприклад, 192.168.1.1)",
                Margin = new Thickness(10),
                Width = 300
            };

            OutputBox = new TextBox
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10),
                Height = 350
            };

            ScanNetworkButton = new Button
            {
                Content = "Сканувати мережу",
                Margin = new Thickness(10),
                Width = 150
            };
            ScanNetworkButton.Click += ScanNetwork_Click;

            // Розміщення елементів
            var mainPanel = new StackPanel
            {
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        Margin = new Thickness(10),
                        Children =
                        {
                            IpInput,
                            ScanNetworkButton
                        }
                    },
                    OutputBox
                }
            };

            Content = mainPanel;
        }

        private async void ScanNetwork_Click(object sender, RoutedEventArgs e)
        {
            OutputBox.Text = string.Empty;
            ScanNetworkButton.IsEnabled = false;

            string userIp = IpInput.Text.Trim();
            string[] parts = userIp.Split('.');

            if (parts.Length != 4)
            {
                OutputBox.Text = "Невірний формат IP-адреси.";
                ScanNetworkButton.IsEnabled = true;
                return;
            }

            OutputBox.Text = "Сканування...\n";

            Task[] tasks = new Task[254];

            for (int i = 1; i < 255; i++)
            {
                string ipAddress = $"{parts[0]}.{parts[1]}.{parts[2]}.{i}";
                int index = i - 1;

                tasks[index] = Task.Run(async () =>
                {
                    using (Ping ping = new Ping())
                    {
                        try
                        {
                            PingReply reply = await ping.SendPingAsync(ipAddress, 100);
                            string result = reply.Status == IPStatus.Success
                                ? $"{ipAddress} - доступна"
                                : $"{ipAddress} - не доступна";

                            await Dispatcher.UIThread.InvokeAsync(() =>
                                OutputBox.Text += result + "\n");
                        }
                        catch
                        {
                            await Dispatcher.UIThread.InvokeAsync(() =>
                                OutputBox.Text += $"{ipAddress} - не доступна\n, error");
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);

            OutputBox.Text += "Сканування завершено.\n";
            ScanNetworkButton.IsEnabled = true;
        }
    }
}