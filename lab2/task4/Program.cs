using System;

class Program
{
    static void Main()
    {
        Random rand = new Random();

        int[][] jaggedArray = new int[5][];
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            jaggedArray[i] = new int[rand.Next(1, 6)];
            for (int j = 0; j < jaggedArray[i].Length; j++)
            {
                jaggedArray[i][j] = rand.Next(-10, 11);
            }
        };

        int maxCols = 0;
        foreach (var row in jaggedArray)
        {
            if (row.Length > maxCols)
            {
                maxCols = row.Length;
            }
        }

        Console.WriteLine("Елементи кожного стовпця:");
        for (int col = 0; col < maxCols; col++)
        {
            Console.Write($"Стовпець {col + 1}: ");
            foreach (var row in jaggedArray)
            {
                if (col < row.Length)
                {
                    Console.Write(row[col] + "   ");
                }
                else
                {
                    Console.Write("   ");
                }
            }
            Console.WriteLine();
        }

        int[] negativeSums = new int[maxCols];

        for (int col = 0; col < maxCols; col++)
        {
            int sum = 0;
            foreach (var row in jaggedArray)
            {
                if (col < row.Length && row[col] < 0)
                {
                    sum += row[col];
                }
            }
            negativeSums[col] = sum;
        }

        Console.WriteLine("\nСуми від'ємних елементів для кожного стовпця:");
        for (int i = 0; i < negativeSums.Length; i++)
        {
            Console.WriteLine($"Стовпець {i + 1}: {negativeSums[i]}");
        }
    }
}