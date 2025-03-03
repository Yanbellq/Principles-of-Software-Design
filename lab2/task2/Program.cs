using System;

class Program
{
    static void Main()
    {
        int rows = 5;
        int cols = 7;
        int[,] matrix = new int[rows, cols];
        Random rand = new Random();

        // Заповнення матриці випадковими числами в діапазоні від -50 до 49
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = rand.Next(-50, 50);
            }
        }

        // Виведення матриці
        Console.WriteLine("Матриця:\n");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }

        Line(rows);

        // Знаходження максимального та мінімального елементів за модулем
        int maxElement = matrix[0, 0];
        int minElement = matrix[0, 0];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (Math.Abs(matrix[i, j]) > Math.Abs(maxElement))
                {
                    maxElement = matrix[i, j];
                }
                if (Math.Abs(matrix[i, j]) < Math.Abs(minElement))
                {
                    minElement = matrix[i, j];
                }
            }
        }


        // Визначення кількості та суми елементів, які не перебувають між максимальними і мінімальними по модулю
        int count = 0;
        int sum = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i, j] < Math.Min(maxElement, minElement) || matrix[i, j] > Math.Max(maxElement, minElement))
                {
                    Console.Write(matrix[i, j] + "\t");
                    count++;
                    sum += matrix[i, j];
                }
            }
        }

        Line(count);

        Console.WriteLine($"\nКількість елементів: {count}");
        Console.WriteLine($"Сума елементів: {sum}");
    }

    static void Line(int count)
    {
        int lineLength = count * 8; // Assuming each element takes up to 8 characters including tab space
        string line = new('-', lineLength);
        Console.WriteLine($"\n\n{line}\n");
    }
}