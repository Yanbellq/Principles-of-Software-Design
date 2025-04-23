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
        
    }

    public void ReadMaterial()
    {
        
    }

    public void StartExam()
    {
        
    }

    public void EndExam()
    {
        
    }
}

// Класс Конспект
public class Notes : ILearningMaterial, IExam
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public int Duration { get; set; }

    public void DisplayInfo()
    {
    
    }

    public void ReadMaterial()
    {
    
    }

    public void StartExam()
    {
    
    }

    public void EndExam()
    {
    
    }
}