using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;

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
        }

        private async void BtnOpenFile_Click(object? sender, RoutedEventArgs e)
        {
            var btnOpenFile = this.FindControl<Button>("btnOpenFile");
            var txtInput = this.FindControl<TextBox>("txtInput");
            var txtOutput = this.FindControl<TextBox>("txtOutput");
            var resultsListView = this.FindControl<ListBox>("resultsListView");

            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select a text file",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Text files") { Patterns = new[] { "*.txt" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*" } }
                }
            });

            if (files.Count > 0 && files[0] is IStorageFile file)
            {
                try
                {
                    await using var stream = await file.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    string inputText = await reader.ReadToEndAsync();

                    txtInput.Text = inputText;

                    var results = RecognizeNumbers(inputText);
                    DisplayResults(results, resultsListView);

                    string outputText = ReplaceNumbers(inputText, results);
                    txtOutput.Text = outputText;
                }
                catch (Exception ex)
                {
                    await ShowMessageBox("Error", $"Помилка: {ex.Message}");
                }
            }
        }

        private async void BtnSaveFile_Click(object? sender, RoutedEventArgs e)
        {
            var btnSaveFile = this.FindControl<Button>("btnSaveFile");
            var txtOutput = this.FindControl<TextBox>("txtOutput");

            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                await ShowMessageBox("Warning", "Немає даних для збереження");
                return;
            }

            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save result",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Text files") { Patterns = new[] { "*.txt" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*" } }
                }
            });

            if (file is IStorageFile saveFile)
            {
                try
                {
                    await using var stream = await saveFile.OpenWriteAsync();
                    using var writer = new StreamWriter(stream);
                    await writer.WriteAsync(txtOutput.Text);

                    await ShowMessageBox("Success", "Файл успішно збережено!");
                }
                catch (Exception ex)
                {
                    await ShowMessageBox("Error", $"Помилка: {ex.Message}");
                }
            }
        }

        private List<ModelResult> RecognizeNumbers(string text)
        {
            var results = NumberRecognizer.RecognizeNumber(text, Culture.English);
            return new List<ModelResult>(results);
        }

        private void DisplayResults(List<ModelResult> results, ListBox listBox)
        {
            var items = new List<ResultItem>();

            foreach (var result in results)
            {
                items.Add(new ResultItem
                {
                    Text = result.Text,
                    Start = result.Start.ToString(),
                    End = result.End.ToString(),
                    Value = result.Resolution["value"].ToString()
                });
            }

            listBox.ItemsSource = items;
        }

        private string ReplaceNumbers(string text, List<ModelResult> results)
        {
            for (int i = results.Count - 1; i >= 0; i--)
            {
                var result = results[i];
                string numericValue = result.Resolution["value"].ToString() ?? string.Empty;
                text = text.Remove(result.Start, result.End - result.Start + 1)
                           .Insert(result.Start, numericValue);
            }
            return text;
        }

        private async Task ShowMessageBox(string title, string message)
        {
            var dialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, Margin = new Thickness(20) },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            await dialog.ShowDialog(this);
        }
    }

    public class ResultItem
    {
        public string? Text { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public string? Value { get; set; }
    }
}