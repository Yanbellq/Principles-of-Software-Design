using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;
using Avalonia.Input;
using System.Windows.Input;
using Avalonia.Media;

namespace lab12
{
    public partial class MainWindow : Window
    {
        private List<ModelResult> _currentResults = new List<ModelResult>();
        private string? _currentFilePath;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            // Ініціалізація кнопок
            btnExtract = this.FindControl<Button>("btnExtract");
            btnSave = this.FindControl<Button>("btnSave");
            txtInput = this.FindControl<TextBox>("txtInput");
            resultsListView = this.FindControl<ListBox>("resultsListView");
            lblCount = this.FindControl<TextBlock>("lblCount");

            btnExtract.Click += BtnExtract_Click;
            btnSave.Click += BtnSave_Click;

            // Спочатку кнопка "Зберегти" неактивна
            btnSave.IsEnabled = false;
        }

        private async void BtnExtract_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var storageProvider = GetStorageProvider();
                if (storageProvider is null) return;

                var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Виберіть текстовий файл",
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Текстові файли") { Patterns = new[] { "*.txt" } },
                        new FilePickerFileType("Всі файли") { Patterns = new[] { "*" } }
                    }
                });

                if (files.Count > 0 && files[0] is { } file)
                {
                    _currentFilePath = file.Path.AbsolutePath;
                    await using var stream = await file.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    txtInput.Text = await reader.ReadToEndAsync();

                    _currentResults = RecognizeOrdinals(txtInput.Text ?? string.Empty);
                    DisplayResults(_currentResults);

                    // Активуємо кнопку "Зберегти" після успішного витягування
                    btnSave.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await ShowSimpleDialog("Помилка", $"Помилка: {ex.Message}");
            }
        }

        private async void BtnSave_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentResults.Count == 0)
            {
                await ShowSimpleDialog("Попередження", "Немає результатів для збереження");
                return;
            }

            try
            {
                var storageProvider = GetStorageProvider();
                if (storageProvider is null) return;

                var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Зберегти результати",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("Текстові файли") { Patterns = new[] { "*.txt" } },
                        new FilePickerFileType("Всі файли") { Patterns = new[] { "*" } }
                    },
                    SuggestedFileName = Path.GetFileNameWithoutExtension(_currentFilePath ?? "results") + "_results.txt"
                });

                if (file is not null)
                {
                    await using var stream = await file.OpenWriteAsync();
                    await using var writer = new StreamWriter(stream);

                    await writer.WriteLineAsync($"Знайдено порядкових числівників: {_currentResults.Count}");
                    foreach (var result in _currentResults)
                    {
                        await writer.WriteLineAsync($"{result.Text} - {result.Resolution["value"]}");
                    }

                    await ShowSimpleDialog("Успіх", "Результати успішно збережено!");
                }
            }
            catch (Exception ex)
            {
                await ShowSimpleDialog("Помилка", $"Помилка при збереженні: {ex.Message}");
            }
        }

        private async Task ShowSimpleDialog(string title, string message)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = message,
                            Margin = new Thickness(10),
                            TextWrapping = TextWrapping.Wrap
                        },
                        new Button
                        {
                            Content = "OK",
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Margin = new Thickness(10),
                            Command = new CloseWindowCommand(this)
                        }
                    }
                }
            };

            await dialog.ShowDialog(this);
        }

        private IStorageProvider? GetStorageProvider()
        {
            return this.StorageProvider;
        }

        private List<ModelResult> RecognizeOrdinals(string text)
        {
            return new List<ModelResult>(NumberRecognizer.RecognizeOrdinal(text, Culture.English));
        }

        private void DisplayResults(List<ModelResult> results)
        {
            lblCount.Text = $"Знайдено порядкових числівників: {results.Count}";

            var items = new List<string>();
            foreach (var result in results)
            {
                items.Add($"{result.Text} - {result.Resolution["value"]}");
            }
            resultsListView.ItemsSource = items;
        }
    }

    public class CloseWindowCommand : ICommand
    {
        private readonly Window _window;

        public CloseWindowCommand(Window window)
        {
            _window = window;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _window.Close();
        }

        public event EventHandler? CanExecuteChanged;
    }
}