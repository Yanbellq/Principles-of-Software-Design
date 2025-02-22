using System;

class Program
{
    static string CheckPoint(double x, double y, double R)
    {
        // Межі квадрату: він знаходиться у другому і четвертому квадрантах координатної площини
        bool insideSquare = (x >= -R && x <= 0) && (y >= 0 && y <= R);

        // Межі кіл: перевіряємо, чи точка всередині будь-якого з двох кіл
        bool insideTopCircle = (Math.Pow(x, 2) + Math.Pow(y - R, 2) <= R * R);
        bool insideBottomCircle = (Math.Pow(x, 2) + Math.Pow(y + R, 2) <= R * R);

        // Перевірка потрапляння точки
        if (insideSquare)
            return "Так"; // Точка всередині заштрихованої області
        if (insideTopCircle || insideBottomCircle)
            return "Ні"; // Точка поза заштрихованою областю
        return "На межі"; // Точка на межі фігури
    }

    static void Main()
    {
        Console.Write("Введіть координату x: ");
        double x = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть координату y: ");
        double y = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть радіус R: ");
        double R = Convert.ToDouble(Console.ReadLine());

        string result = CheckPoint(x, y, R);
        Console.WriteLine($"Результат: {result}");
    }
}
