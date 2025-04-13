namespace Task2.Models;

// Интерфейс Навчальний матеріал
public interface ILearningMaterial
{
    string Title { get; set; }
    string Author { get; set; }

    void DisplayInfo();
    void ReadMaterial();
}

// Интерфейс Испит
public interface IExam
{
    string Subject { get; set; }
    int Duration { get; set; }

    void StartExam();
    void EndExam();
}

// Класс Книга
public class Book : ILearningMaterial, IExam
{
    public string Title { get; set; } = string.Empty; // Инициализация по умолчанию
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public int Duration { get; set; }

    public void DisplayInfo()
    {
        // Логика отображения информации будет реализована в GUI
    }

    public void ReadMaterial()
    {
        // Логика чтения материала будет реализована в GUI
    }

    public void StartExam()
    {
        // Логика начала экзамена будет реализована в GUI
    }

    public void EndExam()
    {
        // Логика завершения экзамена будет реализована в GUI
    }
}

// Класс Конспект
public class Notes : ILearningMaterial, IExam
{
    public string Title { get; set; } = string.Empty; // Инициализация по умолчанию
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public int Duration { get; set; }

    public void DisplayInfo()
    {
        // Логика отображения информации будет реализована в GUI
    }

    public void ReadMaterial()
    {
        // Логика чтения материала будет реализована в GUI
    }

    public void StartExam()
    {
        // Логика начала экзамена будет реализована в GUI
    }

    public void EndExam()
    {
        // Логика завершения экзамена будет реализована в GUI
    }
}