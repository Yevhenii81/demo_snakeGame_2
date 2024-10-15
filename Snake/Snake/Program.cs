using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace SnakeGame
{
    internal class Program
    {
        //private const int MapWidth = 60;
        //private const int MapHeight = 30;

        //private const int WindowWidth = MapWidth + 1;  // Учет рамок
        //private const int WindowHeight = MapHeight + 1;

        //private const int ScreenWidth = MapWidth * 3;
        //private const int ScreenHeight = MapHeight * 3;

        private const int FrameMs = 200;

        private const ConsoleColor BorderColor = ConsoleColor.DarkRed;

        private const ConsoleColor HeadColor = ConsoleColor.DarkGreen;
        private const ConsoleColor BodyColor = ConsoleColor.Green;

        private const ConsoleColor FoodColor = ConsoleColor.Yellow;

        private const ConsoleColor BombColor = ConsoleColor.Red;

        private static readonly Random Random = new Random();

        static void Main()
        {
            Title = "Snake Game";
            //SetWindowSize(WindowWidth, WindowHeight);
            //SetBufferSize(WindowWidth, WindowHeight);

            //SetWindowSize(MapWidth, MapHeight);
            //SetBufferSize(MapWidth, MapHeight);
            CursorVisible = false;

            var consoleHelper = new ConsoleHelper();
            consoleHelper.CenterConsoleWindow();

            while (true)
            {
                ShowLogo();
                ShowMenu();
                ReadKey();
            }
        }

        static void ShowLogo()
        {
            Clear();
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 4);
            WriteLine("  SSSSS  N   N   AAAAA   K   K  EEEEE ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 3);
            WriteLine(" S       NN  N   A   A   K  K   E     ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 2);
            WriteLine("  SSS    N N N   AAAAA   KKK    EEEEE ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 1);
            WriteLine("     S   N  NN   A   A   K  K   E     ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2);
            WriteLine(" SSSSS   N   N   A   A   K   K  EEEEE ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 + 2);
            WriteLine("Press Enter to start...");

            while (true)
            {
                if (KeyAvailable && ReadKey(true).Key == ConsoleKey.Enter)
                    break;
            }
        }

        static void ShowMenu()
        {
            Clear();
            string[] options = {
                "1. Правила",
                "2. Играть (Легкий уровень)",
                "3. Выбор уровня сложности",
                "4. Выйти"
            };

            int selectedOption = 0;

            while (true)
            {
                Clear();

                int menuWidth = options.Max(option => option.Length);
                int startX = (WindowWidth - menuWidth) / 2;
                int startY = (WindowHeight - options.Length) / 2;

                // Отрисовка меню с подсветкой
                for (int i = 0; i < options.Length; i++)
                {
                    SetCursorPosition(startX, startY + i);
                    if (i == selectedOption)
                    {
                        ForegroundColor = ConsoleColor.Blue; // Подсветка
                        WriteLine(" " + options[i]);
                        ResetColor();
                    }
                    else
                    {
                        WriteLine(options[i]);
                    }
                }

                ConsoleKey key = ReadKey(true).Key;

                // Обработка нажатия клавиш
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                        break;
                    case ConsoleKey.Enter:
                        ExecuteMenuChoice(selectedOption);
                        return;
                }
            }
        }

        static void ExecuteMenuChoice(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    ShowRules();
                    break;
                case 1:
                    StartGame(40, 20, withBomb: false);
                    break;
                case 2:
                    ChooseDifficulty();
                    break;
                case 3:
                    CloseConsole();
                    break;
            }
        }

        static void ChooseDifficulty()
        {
            Clear();
            string[] difficultyOptions = {
                "1. Легкий (без бомб, поле 40x20)",
                "2. Средний (с бомбами, поле 60x30)",
                "3. Сложный (с бомбами, поле 80x40)",
                "4. Назад"
            };

            int selectedDifficulty = 0;

            while (true)
            {
                Clear();

                int menuWidth = difficultyOptions.Max(option => option.Length);
                int startX = (WindowWidth - menuWidth) / 2;
                int startY = (WindowHeight - difficultyOptions.Length) / 2;

                // Отрисовка выбора уровня сложности
                for (int i = 0; i < difficultyOptions.Length; i++)
                {
                    SetCursorPosition(startX, startY + i);
                    if (i == selectedDifficulty)
                    {
                        ForegroundColor = ConsoleColor.Blue; // Подсветка выбора
                        WriteLine(" " + difficultyOptions[i]);
                        ResetColor();
                    }
                    else
                    {
                        WriteLine(difficultyOptions[i]);
                    }
                }

                ConsoleKey key = ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedDifficulty = (selectedDifficulty == 0) ? difficultyOptions.Length - 1 : selectedDifficulty - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedDifficulty = (selectedDifficulty == difficultyOptions.Length - 1) ? 0 : selectedDifficulty + 1;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedDifficulty)
                        {
                            case 0:
                                StartGame(40, 20, withBomb: false);
                                break;
                            case 1:
                                StartGame(60, 30, withBomb: true);
                                break;
                            case 2:
                                StartGame(80, 40, withBomb: true);
                                break;
                            case 3:
                                ShowMenu(); // Возврат в меню
                                return;
                        }
                        return;
                }
            }
        }

        /////меню с подсветкой
        //static void ShowMenu()
        //{
        //    Clear();
        //    string[] options = {
        //        "1. Правила",
        //        "2. Играть (Легкий уровень)",
        //        "3. Выбор уровня сложности",
        //        "4. Выйти"
        //    };

        //    int selectedOption = 0;  // Индекс выбранного пункта меню

        //    while (true)
        //    {
        //        Clear();

        //        // Отрисовка меню
        //        for (int i = 0; i < options.Length; i++)
        //        {
        //            if (i == selectedOption)
        //            {
        //                ForegroundColor = ConsoleColor.Yellow; // Выделение выбранного пункта
        //                WriteLine("  " + options[i]);
        //                ResetColor();
        //            }
        //            else
        //            {
        //                WriteLine("   " + options[i]);
        //            }
        //        }

        //        ConsoleKey key = ReadKey(true).Key;

        //        // Обработка нажатия клавиш
        //        switch (key)
        //        {
        //            case ConsoleKey.UpArrow:
        //                selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
        //                break;
        //            case ConsoleKey.DownArrow:
        //                selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
        //                break;
        //            case ConsoleKey.Enter:
        //                ExecuteMenuChoice(selectedOption);
        //                return;
        //        }
        //    }
        //}

        //static void ExecuteMenuChoice(int selectedOption)
        //{
        //    switch (selectedOption)
        //    {
        //        case 0:
        //            ShowRules();
        //            break;
        //        case 1:
        //            StartGame(40, 20, withBomb: false);
        //            break;
        //        case 2:
        //            ChooseDifficulty();
        //            break;
        //        case 3:
        //            CloseConsole(); // Закрыть консоль
        //            break;
        //    }
        //}

        /////основной вариант меню
        //static void ShowMenu()
        //{
        //    Clear();
        //    string[] options = {

        //        "1. Правила",
        //        "2. Играть (Легкий уровень)",
        //        "3. Выбор уровня сложности",
        //        "4. Выйти"
        //    };

        //    int menuWidth = options.Max(option => option.Length);
        //    int startX = (WindowWidth - menuWidth) / 2;
        //    int startY = (WindowHeight - options.Length) / 2;

        //    for (int i = 0; i < options.Length; i++)
        //    {
        //        SetCursorPosition(startX, startY + i);
        //        WriteLine(options[i]);
        //    }

        //    SetCursorPosition(startX, startY + options.Length + 1);
        //    WriteLine("Выберите пункт меню:");

        //    ConsoleKeyInfo choice = ReadKey(true);
        //    switch (choice.KeyChar)
        //    {
        //        case '1':
        //            ShowRules();
        //            break;
        //        case '2':
        //            StartGame(40, 20, withBomb: false);
        //            break;
        //        case '3':
        //            ChooseDifficulty();
        //            break;
        //        case '4':
        //            CloseConsole(); // Закрыть консоль
        //            break;
        //    }
        //}

        static void CloseConsole()
        {
            Clear(); // Очистка экрана перед закрытием
            WriteLine("Игра завершена. Нажмите любую клавишу, чтобы закрыть.");
            ReadKey(true); // Ожидание нажатия клавиши
            Environment.Exit(0); // Закрытие программы и консоли
        }

        static void ShowRules()
        {
            Clear();
            WriteLine("Правила игры:");
            WriteLine("1. Управляйте змейкой с помощью стрелок.");
            WriteLine("2. Съедайте еду, чтобы расти и набирать очки.");
            WriteLine("3. Если съедите бомбу (в сложных уровнях), игра завершится.");
            WriteLine("\nНажмите любую клавишу для возврата в меню...");
            ReadKey();
            ShowMenu();
        }

        /////основной вариант выбора уровня сложности 
        //static void ChooseDifficulty()
        //{
        //    Clear();
        //    WriteLine("Выберите уровень сложности:");
        //    WriteLine("1. Легкий (без бомб, поле 40x20)");
        //    WriteLine("2. Средний (с бомбами, поле 60x30)");
        //    WriteLine("3. Сложный (с бомбами, поле 80x40)");

        //    ConsoleKeyInfo choice = ReadKey(true);
        //    switch (choice.KeyChar)
        //    {
        //        case '1':
        //            StartGame(40, 20, withBomb: false);
        //            break;
        //        case '2':
        //            StartGame(60, 30, withBomb: true);
        //            break;
        //        case '3':
        //            StartGame(80, 40, withBomb: true);
        //            break;
        //    }
        //}

        static void StartGame(int mapWidth, int mapHeight, bool withBomb)
        {

            SetWindowSize(mapWidth + 1, mapHeight + 1);
            SetBufferSize(mapWidth + 1, mapHeight + 1);

            Clear();

            DrawBorder(mapWidth, mapHeight);

            Direction currentMovement = Direction.Right;

            var snake = new Snake(10, 5, HeadColor, BodyColor);

            Pixel food = GenFood(snake, mapWidth, mapHeight);
            food.Draw();

            Pixel? bomb = withBomb ? GenBomb(snake, food, mapWidth, mapHeight) : (Pixel?)null;
            bomb?.Draw();

            int score = 0;
            int maxScore = (mapWidth - 2) * (mapHeight - 2) - snake.Body.Count - 1;

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMs)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                if(snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);

                    food = GenFood(snake, mapWidth, mapHeight);
                    food.Draw();

                    if(withBomb)
                    {
                        bomb?.Clear(); // Очистить старую бомбу
                        bomb = GenBomb(snake, food, mapWidth, mapHeight); // Генерировать новую бомбу
                        bomb?.Draw(); // Отрисовать новую бомбу
                    }

                    score++;

                    Task.Run(() => Beep(1200, 200));

                    if(score == maxScore)
                    {
                        SetCursorPosition(WindowWidth / 3, WindowHeight / 2);
                        WriteLine($"You Win! Final Score: {score}");
                        break;
                    }
                }
                else if(withBomb && bomb.HasValue && snake.Head.X == bomb.Value.X && snake.Head.Y == bomb.Value.Y)
                {
                    break;
                }
                else
                {
                    snake.Move(currentMovement);
                }

                if (snake.Head.X == mapWidth - 1
                   || snake.Head.X == 0
                   || snake.Head.Y == mapHeight - 1
                   || snake.Head.Y == 0
                   || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;
            }

            snake.Clear();
            food.Clear();
            bomb?.Clear();

            SetCursorPosition(WindowWidth / 3, WindowHeight / 2);
            WriteLine($"Game over, score: {score}");

            Task.Run(() => Beep(200, 600));
        }

        static  Pixel GenFood(Snake snake, int mapWidth, int mapHeight)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, mapWidth - 2), Random.Next(1, mapHeight - 2), FoodColor, 'o');
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Pixel GenBomb(Snake snake, Pixel food, int mapWidth, int mapHeight)
        {
            Pixel bomb;
            do
            {
                bomb = new Pixel(Random.Next(1, mapWidth - 2), Random.Next(1, mapHeight - 2), BombColor, '*');
            } while (snake.Body.Any(b => b.X == bomb.X && b.Y == bomb.Y || (food.X == bomb.X && food.Y == bomb.Y)));
            return bomb;
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if(!KeyAvailable)
                return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection;
        }


        //пока что оставаляю этот ваиант отрисовки границ, так как вроде работает получше
        static void DrawBorder(int mapWidth, int mapHeight)
        {
            // Отрисовка верхней границы
            for (int i = 0; i <= mapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw(); // Верхний ряд
            }

            // Отрисовка боковых границ
            for (int i = 1; i <= mapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw(); // Левая граница
                new Pixel(1, i, BorderColor).Draw(); // Вторая левая граница
                new Pixel(mapWidth, i, BorderColor).Draw(); // Правая граница
                new Pixel(mapWidth - 1, i, BorderColor).Draw(); // Вторая правая граница
            }

            // Отрисовка нижней границы
            for (int i = 0; i <= mapWidth; i++)
            {
                new Pixel(i, mapHeight, BorderColor).Draw(); // Нижний ряд
            }
        }

        //это основной вариант отрисовки границ
        //static void DrawBorder(int mapWidth, int mapHeight)
        //{
        //    for (int i = 0; i <= mapWidth; i++) // Меняем < на <=
        //    {
        //        new Pixel(i, 0, BorderColor).Draw();
        //        new Pixel(i, mapHeight, BorderColor).Draw(); // Меняем MapHeight - 1 на MapHeight
        //    }

        //    for (int i = 0; i <= mapHeight; i++) // Меняем < на <=
        //    {
        //        new Pixel(0, i, BorderColor).Draw();
        //        new Pixel(mapWidth, i, BorderColor).Draw(); // Меняем MapWidth - 1 на MapWidth
        //    }
        //}


        //static void DrawBorder()
        //{
        //    for (int i = 0; i < MapWidth; i++)
        //    {
        //        new Pixel(i, 0, BorderColor).Draw();
        //        new Pixel(i, MapHeight - 1, BorderColor).Draw();
        //    }

        //    for (int i = 0; i < MapHeight; i++)
        //    {
        //        new Pixel(0, i, BorderColor).Draw();
        //        new Pixel(MapWidth - 1, i, BorderColor).Draw();
        //    }
        //}
    }
}