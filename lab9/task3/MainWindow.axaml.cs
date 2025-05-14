using System;
using System.IO;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace lab9
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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Виберіть файл з математичним виразом",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
            });

            if (files.Count > 0 && files[0].TryGetLocalPath() is string filePath)
            {
                if (this.FindControl<TextBox>("txtFilePath") is { } txtFilePath)
                    txtFilePath.Text = filePath;
                CheckParentheses(filePath);
            }
        }

        private void CheckParentheses(string filePath)
        {
            try
            {
                string expression = File.ReadAllText(filePath);
                var (isBalanced, pairs) = CheckParenthesesBalance(expression);

                if (this.FindControl<TextBlock>("txtResult") is { } txtResult)
                {
                    txtResult.Text = isBalanced ? "Дужки збалансовані" : "Дужки НЕ збалансовані!";
                }

                if (this.FindControl<TextBlock>("txtPairs") is { } txtPairs)
                {
                    txtPairs.Text = isBalanced ? "Пари дужок:\n" + string.Join("\n", pairs) : "";
                }
            }
            catch (Exception ex)
            {
                if (this.FindControl<TextBlock>("txtResult") is { } txtResult)
                    txtResult.Text = $"Помилка: {ex.Message}";

                if (this.FindControl<TextBlock>("txtPairs") is { } txtPairs)
                    txtPairs.Text = "";
            }
        }

        private (bool isBalanced, List<string> pairs) CheckParenthesesBalance(string expression)
        {
            Stack<int> stack = new Stack<int>();
            List<(int open, int close)> pairsList = new List<(int, int)>();
            bool isBalanced = true;
            List<string> pairs = new List<string>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    stack.Push(i);
                }
                else if (expression[i] == ')')
                {
                    if (stack.Count == 0)
                    {
                        isBalanced = false;
                        break;
                    }
                    int openPos = stack.Pop();
                    pairsList.Add((openPos, i));
                }
            }

            if (stack.Count > 0)
            {
                isBalanced = false;
            }

            // Сортуємо пари за позицією закриваючої дужки
            pairsList.Sort((a, b) => a.close.CompareTo(b.close));

            foreach (var pair in pairsList)
            {
                pairs.Add($"({pair.open}, {pair.close})");
            }

            return (isBalanced, pairs);
        }
    }
}