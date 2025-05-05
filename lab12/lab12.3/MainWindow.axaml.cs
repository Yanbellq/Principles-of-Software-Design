using System;
using System.IO;
using System.Text;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;

namespace lab12
{
    public partial class MainWindow : Window
    {
        private string inputText = string.Empty;
        private string inputFilePath = string.Empty;
        private const string Culture = "en-us"; // Changed to constant string

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel == null) return;

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Open Text File",
                    AllowMultiple = false,
                    FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
                });

                if (files.Count > 0 && files[0] != null)
                {
                    inputFilePath = files[0].Path.AbsolutePath;
                    await using var stream = await files[0].OpenReadAsync();
                    using var streamReader = new StreamReader(stream);
                    inputText = await streamReader.ReadToEndAsync();
                    txtInput.Text = inputText;

                    var msgBox = new Window
                    {
                        Title = "Інформація",
                        Content = new TextBlock { Text = "Файл успішно завантажено!" },
                        SizeToContent = SizeToContent.WidthAndHeight
                    };
                    await msgBox.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                var msgBox = new Window
                {
                    Title = "Помилка",
                    Content = new TextBlock { Text = $"Помилка при завантаженні файлу: {ex.Message}" },
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                await msgBox.ShowDialog(this);
            }
        }

        private async void btnRecognize_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                var msgBox = new Window
                {
                    Title = "Попередження",
                    Content = new TextBlock { Text = "Будь ласка, завантажте файл або введіть текст для розпізнавання." },
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                await msgBox.ShowDialog(this);
                return;
            }

            var resultBuilder = new StringBuilder();
            inputText = txtInput.Text;

            // Number recognition (using constant culture)
            var numberResults = NumberRecognizer.RecognizeNumber(inputText, Culture);
            foreach (var result in numberResults)
            {
                resultBuilder.AppendLine($"Number: {result.Text} (Value: {result.Resolution["value"]})");
            }

            // Number with unit recognition
            var numberWithUnitResults = NumberWithUnitRecognizer.RecognizeCurrency(inputText, Culture);
            foreach (var result in numberWithUnitResults)
            {
                resultBuilder.AppendLine($"Currency: {result.Text} (Value: {result.Resolution["value"]} {result.Resolution["unit"]})");
            }

            // Date and time recognition
            var dateTimeResults = DateTimeRecognizer.RecognizeDateTime(inputText, Culture);
            foreach (var result in dateTimeResults)
            {
                resultBuilder.AppendLine($"DateTime: {result.Text} (Value: {result.Resolution["values"]})");
            }

            // Phone number recognition
            var phoneResults = SequenceRecognizer.RecognizePhoneNumber(inputText, Culture);
            foreach (var result in phoneResults)
            {
                resultBuilder.AppendLine($"Phone: {result.Text}");
            }

            txtOutput.Text = resultBuilder.ToString();
        }

        private async void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                var msgBox = new Window
                {
                    Title = "Попередження",
                    Content = new TextBlock { Text = "Немає результатів для збереження." },
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                await msgBox.ShowDialog(this);
                return;
            }

            try
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel == null) return;

                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Save Results",
                    DefaultExtension = ".txt",
                    FileTypeChoices = new[] { FilePickerFileTypes.TextPlain }
                });

                if (file != null)
                {
                    await using var stream = await file.OpenWriteAsync();
                    using var streamWriter = new StreamWriter(stream);
                    await streamWriter.WriteAsync(txtOutput.Text);

                    var msgBox = new Window
                    {
                        Title = "Інформація",
                        Content = new TextBlock { Text = "Результати успішно збережено!" },
                        SizeToContent = SizeToContent.WidthAndHeight
                    };
                    await msgBox.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                var msgBox = new Window
                {
                    Title = "Помилка",
                    Content = new TextBlock { Text = $"Помилка при збереженні файлу: {ex.Message}" },
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                await msgBox.ShowDialog(this);
            }
        }
    }
}