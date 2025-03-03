using System;

class Program
{
    static void Main()
    {
        // Chessboard size
        const int boardSize = 8;

        // Input coordinates for White King, White Bishop, and Black Queen
        Console.WriteLine("Enter coordinates for White King (x y):");
        var whiteKing = ReadCoordinates();
        Console.WriteLine("Enter coordinates for White Bishop (x y):");
        var whiteBishop = ReadCoordinates();
        Console.WriteLine("Enter coordinates for Black Queen (x y):");
        var blackQueen = ReadCoordinates();

        // Validate coordinates
        if (!IsValidCoordinate(whiteKing, boardSize) || !IsValidCoordinate(whiteBishop, boardSize) || !IsValidCoordinate(blackQueen, boardSize) ||
            AreSameCoordinates(whiteKing, whiteBishop) || AreSameCoordinates(whiteKing, blackQueen) || AreSameCoordinates(whiteBishop, blackQueen))
        {
            Console.WriteLine("Invalid coordinates. Please ensure all pieces are within the board and not on the same square.");
            return;
        }

        // Determine the result based on the first move
        Console.WriteLine("Enter the first move (White King, White Bishop, Black Queen):");
        string firstMove = Console.ReadLine() ?? string.Empty;

        switch (firstMove.ToLower())
        {
            case "white king":
                HandleWhiteKingMove(whiteKing, whiteBishop, blackQueen);
                break;
            case "white bishop":
                HandleWhiteBishopMove(whiteKing, whiteBishop, blackQueen);
                break;
            case "black queen":
                HandleBlackQueenMove(whiteKing, whiteBishop, blackQueen);
                break;
            default:
                Console.WriteLine("Invalid move. Please enter 'White King', 'White Bishop', or 'Black Queen'.");
                break;
        }
    }

    static (int x, int y) ReadCoordinates()
    {
        var input = Console.ReadLine()?.Split() ?? Array.Empty<string>();
        return (int.Parse(input[0]), int.Parse(input[1]));
    }

    static bool IsValidCoordinate((int x, int y) coord, int boardSize)
    {
        return coord.x >= 1 && coord.x <= boardSize && coord.y >= 1 && coord.y <= boardSize;
    }

    static bool AreSameCoordinates((int x, int y) coord1, (int x, int y) coord2)
    {
        return coord1.x == coord2.x && coord1.y == coord2.y;
    }

    static void HandleWhiteKingMove((int x, int y) whiteKing, (int x, int y) whiteBishop, (int x, int y) blackQueen)
    {
        if (IsWithinQueenRange(whiteKing, blackQueen))
        {
            Console.WriteLine("White King attacks Black Queen");
        }
        else if (IsWithinKingRange(whiteKing, whiteBishop) && IsWithinQueenRange(blackQueen, whiteBishop))
        {
            Console.WriteLine("White King defends White Bishop");
        }
        else
        {
            Console.WriteLine("White King makes a simple move");
        }
    }

    static void HandleWhiteBishopMove((int x, int y) whiteKing, (int x, int y) whiteBishop, (int x, int y) blackQueen)
    {
        if (IsWithinBishopRange(whiteBishop, blackQueen))
        {
            Console.WriteLine("White Bishop attacks Black Queen");
        }
        else if (IsWithinBishopRange(whiteBishop, whiteKing) && IsWithinQueenRange(blackQueen, whiteKing))
        {
            Console.WriteLine("White Bishop defends White King");
        }
        else
        {
            Console.WriteLine("White Bishop makes a simple move");
        }
    }

    static void HandleBlackQueenMove((int x, int y) whiteKing, (int x, int y) whiteBishop, (int x, int y) blackQueen)
    {
        if (IsWithinQueenRange(blackQueen, whiteBishop))
        {
            Console.WriteLine("Black Queen attacks White Bishop");
        }
        else if (IsWithinQueenRange(blackQueen, whiteKing))
        {
            Console.WriteLine("Black Queen attacks White King");
        }
        else
        {
            Console.WriteLine("Black Queen makes a simple move");
        }
    }

    static bool IsWithinKingRange((int x, int y) king, (int x, int y) target)
    {
        return Math.Abs(king.x - target.x) <= 1 && Math.Abs(king.y - target.y) <= 1;
    }

    static bool IsWithinBishopRange((int x, int y) bishop, (int x, int y) target)
    {
        return Math.Abs(bishop.x - target.x) == Math.Abs(bishop.y - target.y);
    }

    static bool IsWithinQueenRange((int x, int y) queen, (int x, int y) target)
    {
        return queen.x == target.x || queen.y == target.y || Math.Abs(queen.x - target.x) == Math.Abs(queen.y - target.y);
    }
}