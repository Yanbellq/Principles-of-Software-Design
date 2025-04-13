using Avalonia.Controls;
using Task2.Models;

namespace Task2;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnAddBookClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string title = TitleInput.Text ?? string.Empty;
        string author = AuthorInput.Text ?? string.Empty;
        string subject = SubjectInput.Text ?? string.Empty;
        int duration = int.TryParse(DurationInput.Text, out var d) ? d : 0;

        var book = new Book
        {
            Title = title,
            Author = author,
            Subject = subject,
            Duration = duration
        };

        ResultText.Text = $"Додано книгу: {book.Title}, Автор: {book.Author}";
    }

    private void OnAddNotesClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string title = TitleInput.Text ?? string.Empty;
        string author = AuthorInput.Text ?? string.Empty;
        string subject = SubjectInput.Text ?? string.Empty;
        int duration = int.TryParse(DurationInput.Text, out var d) ? d : 0;

        var notes = new Notes
        {
            Title = title,
            Author = author,
            Subject = subject,
            Duration = duration
        };

        ResultText.Text = $"Додано конспект: {notes.Title}, Автор: {notes.Author}";
    }
}