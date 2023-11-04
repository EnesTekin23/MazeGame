using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static string[,] maze;
    static int playerX;
    static int playerY;
    static int exitX;
    static int exitY;
    static bool mazeVisible = true;
    static bool dotsVisible = true;
    static bool exitVisible = true;
    static List<Point> route = new List<Point>();
    static int level = 1;
    static int regionX = -1;
    static int regionY = -1;

    static void Main()
    {
        while (true)
        {
            int matrixSize = 5 + level;
            maze = GenerateRandomMaze(matrixSize);
            InitializePlayerAndExit(matrixSize);

            while (true)
            {
                Console.Clear();
                ShowMaze();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char movementDirection = keyInfo.KeyChar;

                if (movementDirection == 'q')
                {
                    Console.WriteLine("Game over. Total completed levels: " + (level - 1));
                    Console.ReadKey();
                    return;
                }
                else if (movementDirection == 'l')
                {
                    mazeVisible = !mazeVisible;
                    dotsVisible = !dotsVisible;
                    exitVisible = !exitVisible;
                }
                else if (CheckMovement(movementDirection))
                {
                    if (playerX == exitX && playerY == exitY)
                    {
                        level++;
                        Console.WriteLine("Level: " + level + " Completed. Total completed levels: " + (level - 1));
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }
    }

    static string[,] GenerateRandomMaze(int matrixSize)
    {
        string[,] newMaze = new string[matrixSize, matrixSize];
        Random random = new Random();

        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                int randomNumber = random.Next(4);
                if (randomNumber == 0)
                {
                    newMaze[i, j] = "■";
                }
                else
                {
                    newMaze[i, j] = ".";
                }
            }
        }

        return newMaze;
    }

    static void InitializePlayerAndExit(int matrixSize)
    {
        Random random = new Random();
        do
        {
            playerX = random.Next(matrixSize);
            playerY = random.Next(matrixSize);
            exitX = random.Next(matrixSize);
            exitY = random.Next(matrixSize);
        } while (maze[playerY, playerX] == "■" || maze[exitY, exitX] == "■");
    }

    static void ShowMaze()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (i == playerY && j == playerX)
                {
                    Console.Write('♦');
                }
                else if (i == exitY && j == exitX)
                {
                    if (exitVisible)
                    {
                        Console.Write('▲');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                else if (route.Exists(p => p.X == j && p.Y == i))
                {
                    if (dotsVisible)
                    {
                        Console.Write('.');
                        maze[i, j] = ".";
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                else if (mazeVisible)
                {
                    Console.Write(maze[i, j]);
                }
                else if (maze[i, j] == "█")
                {
                    Console.Write('█');
                }
                else
                {
                    Console.Write(' ');
                }
            }
            Console.WriteLine();
        }


        if (playerX >= regionX && playerX < regionX + 5 && playerY >= regionY && playerY < regionY + 5)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Warm");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Cold");
            Console.ResetColor();
        }

        // Seviyeye göre arka plan rengini ayarla
        Console.BackgroundColor = GetBackgroundColor(level);

        Console.WriteLine("Level: " + level);
        Console.WriteLine("When the difficulty increases, the background color will change");
        Console.WriteLine("Press 'Q' to exit. Press 'L' to make the maze invisible.");
    }

    static bool CheckMovement(char direction)
    {
        int targetX = playerX;
        int targetY = playerY;

        switch (direction)
        {
            case 'w':
                targetY--;
                break;
            case 's':
                targetY++;
                break;
            case 'a':
                targetX--;
                break;
            case 'd':
                targetX++;
                break;
        }

        if (targetX >= 0 && targetX < maze.GetLength(1) && targetY >= 0 && targetY < maze.GetLength(0) && maze[targetY, targetX] != "■")
        {
            playerX = targetX;
            playerY = targetY;
            route.Add(new Point(playerX, playerY));


            regionX = Math.Max(0, exitX - 2);
            regionY = Math.Max(0, exitY - 2);

            return true;
        }

        return false;
    }

    static ConsoleColor GetBackgroundColor(int level)
    {
        if (level >= 0 && level <= 30)
        {
            return ConsoleColor.Green;
        }
        else if (level > 30 && level <= 70)
        {
            return ConsoleColor.Magenta;
        }
        else
        {
            return ConsoleColor.DarkRed;
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
