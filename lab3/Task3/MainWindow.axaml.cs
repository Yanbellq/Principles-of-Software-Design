using Avalonia.Controls;

namespace Task3;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnReplaceButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string inputText = InputTextBox.Text;
        string wordToReplace = WordToReplaceTextBox.Text;
        string newWord = NewWordTextBox.Text;

        if (!string.IsNullOrEmpty(inputText) && !string.IsNullOrEmpty(wordToReplace) && !string.IsNullOrEmpty(newWord))
        {
            string resultText = inputText.Replace(wordToReplace, newWord);
            ResultTextBox.Text = resultText;
        }
        else
        {
            ResultTextBox.Text = "Будь ласка, введіть текст";
            return;
        }
    }
}