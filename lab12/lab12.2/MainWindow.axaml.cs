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
using Avalonia.Data;
using Avalonia.Media;

namespace lab12
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // Initialize controls safely
            btnOpenFile = this.FindControl<Button>("btnOpenFile");
            txtInput = this.FindControl<TextBox>("txtInput");
            resultsListView = this.FindControl<ListBox>("resultsListView");
            lblCount = this.FindControl<TextBlock>("lblCount");
        }

        private async void BtnOpenFile_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var storageProvider = GetStorageProvider();
                if (storageProvider is null) return;

                var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Select text file",
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Text files") { Patterns = new[] { "*.txt" } },
                        new FilePickerFileType("All files") { Patterns = new[] { "*" } }
                    }
                });

                if (files.Count > 0 && files[0] is { } file)
                {
                    await using var stream = await file.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    txtInput.Text = await reader.ReadToEndAsync();

                    var results = RecognizeOrdinals(txtInput.Text ?? string.Empty);
                    DisplayResults(results);
                    await SaveResultsAsync(results, file.Name);
                }
            }
            catch (Exception ex)
            {
                await ShowSimpleDialog("Error", $"Error: {ex.Message}");
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
            lblCount.Text = $"Found ordinals: {results.Count}";

            var items = new List<string>();
            foreach (var result in results)
            {
                items.Add($"{result.Text} - {result.Resolution["value"]}");
            }
            resultsListView.ItemsSource = items;
        }

        private async Task SaveResultsAsync(List<ModelResult> results, string originalFileName)
        {
            var storageProvider = GetStorageProvider();
            if (storageProvider is null) return;

            var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save results",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Text files") { Patterns = new[] { "*.txt" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*" } }
                },
                SuggestedFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_results.txt"
            });

            if (file is not null)
            {
                await using var stream = await file.OpenWriteAsync();
                await using var writer = new StreamWriter(stream);

                await writer.WriteLineAsync($"Found ordinals: {results.Count}");
                foreach (var result in results)
                {
                    await writer.WriteLineAsync($"{result.Text} - {result.Resolution["value"]}");
                }

                await ShowSimpleDialog("Success", "Results saved successfully!");
            }
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