namespace TicTacToe
{
    internal class Program
    {
        static readonly Random rnd = new();
        static string playerChar1 = "";
        static string playerChar2 = "";
        static int winningPlayer = 0;
        static bool gameWon = false;
        static readonly string[,] ticTacToeField =
        {
            {"/", "/", "/"},
            {"/", "/", "/"},
            {"/", "/", "/"}
        };

        static void Main()
        {
            Console.WriteLine("Welcome to good-old Tic Tac Toe! Are you a two-player Team (1) or is it you against the computer (2)?");
            string? input = Console.ReadLine();

            while (true)
            {
                if (input == "1") // Player vs. Player
                {
                    if (playerChar1 == "" && playerChar2 == "")
                    {
                        Console.Write("Player 1, pick your symbol (anything goes): ");
                        playerChar1 = Console.ReadLine();
                        Console.Write("Same thing for player 2: ");
                        playerChar2 = Console.ReadLine();
                    }
                    while (CheckEmptyFields() && !gameWon)
                    {
                        PrintField();
                        PlayerTurn(playerChar1);
                        PrintField();
                        PlayerTurn(playerChar2);
                    }
                }
                else if (input == "2") // Player vs. CPU
                {
                    playerChar1 = "X";
                    playerChar2 = "O";
                    while (CheckEmptyFields() && !gameWon)
                    {
                        PrintField();
                        PlayerTurn(playerChar1);
                        CpuTurn(playerChar2);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input :( Session terminated");
                    return;
                }

                if (winningPlayer != 0)
                {
                    PrintField();
                    Console.WriteLine($"\nThe game is over! Player {winningPlayer} has won!");
                }
                else
                {
                    PrintField();
                    Console.WriteLine("\nIt's a draw!");
                }

                Console.WriteLine("\nYou wanna go again? (y or n)");
                if (Console.ReadLine() == "y")
                {
                    // resetting the board
                    winningPlayer = 0;
                    gameWon = false;
                    for (int i = 0; i < ticTacToeField.GetLength(0); i++)
                    {
                        for (int j = 0; j < ticTacToeField.GetLength(1); j++)
                        {
                            ticTacToeField[i, j] = "/";
                        }
                    }

                    Console.WriteLine("Two players (1) or against the CPU (2)?");
                    input = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nThank you for playing!");
                    break;
                }
            }
        }
        static bool CheckForWin()
        {
            if (CheckHorizontal(playerChar1) || CheckVertical(playerChar1) || CheckDiagonally(playerChar1))
            {
                winningPlayer = 1;
                gameWon = true;
                return true;
            }
            else if (CheckHorizontal(playerChar2) || CheckVertical(playerChar2) || CheckDiagonally(playerChar2))
            {
                winningPlayer = 2;
                gameWon = true;
                return true;
            }
            else
            {
                return false;
            }

            bool CheckHorizontal(string playerChar)
            {
                List<bool> sameSymbols = [];
                bool horizontalWin = false;
                for (int row = 0; row < ticTacToeField.GetLength(0); row++)
                {
                    for (int field = 0; field < ticTacToeField.GetLength(1); field++)
                    {
                        sameSymbols.Add(ticTacToeField[row, field] == playerChar);
                    }
                    if (CheckList(sameSymbols))
                    {
                        horizontalWin = true;
                        break;
                    }
                    else
                    {
                        sameSymbols.Clear();
                    }
                }
                return horizontalWin;
            }
            bool CheckVertical(string playerChar)
            {
                bool verticalWin = false;
                List<bool> sameSymbols = [];

                for (int field = 0; field < ticTacToeField.GetLength(1); field++)
                {
                    for (int row = 0; row < ticTacToeField.GetLength(0); row++)
                    {
                        sameSymbols.Add(ticTacToeField[row, field] == playerChar);
                    }
                    if (CheckList(sameSymbols))
                    {
                        verticalWin = true;
                        break;
                    }
                    else
                    {
                        sameSymbols.Clear();
                    }
                }
                return verticalWin;
            }
            bool CheckDiagonally(string playerChar)
            {
                List<bool> sameSymbols = [];

                // check from left to right
                for (int i = 0; i < ticTacToeField.GetLength(0); i++)
                {
                    sameSymbols.Add(ticTacToeField[i, i] == playerChar);
                }
                if (CheckList(sameSymbols))
                {
                    return true;
                }

                // check from right to left
                sameSymbols.Clear();
                for (int i = 0; i < ticTacToeField.GetLength(0); i++)
                {
                    sameSymbols.Add(ticTacToeField[i, 2 - i] == playerChar);
                }
                if (CheckList(sameSymbols))
                {
                    return true;
                }
                return false;
            }
            bool CheckList(List<bool> sameSymbols)
            {
                if (sameSymbols.TrueForAll(x => x == true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        static bool CheckEmptyFields()
        {
            bool emptyFields = false;
            for (int i = 0; i < ticTacToeField.GetLength(0); i++)
            {
                for (int j = 0; j < ticTacToeField.GetLength(1); j++)
                {
                    if (ticTacToeField[i, j] == "/")
                    {
                        emptyFields = true;
                    }
                }
            }
            return emptyFields;
        }
        static void CpuTurn(string cpuCharacter)
        {
            // TODO: improve Enemy "AI"?
            if (CheckEmptyFields() && !gameWon)
            {
                while (true)
                {
                    int row = rnd.Next(0, 3);
                    int field = rnd.Next(0, 3);
                    if (ticTacToeField[row, field] == "/")
                    {
                        ticTacToeField[row, field] = cpuCharacter;
                        break;
                    }
                }
                CheckForWin();
            }
        }
        static void PlayerTurn(string playerCharacter)
        {
            if (CheckEmptyFields() && !gameWon)
            {
                Console.WriteLine($"\nWhere do you want to set your {playerCharacter}? Enter row number and field number (1-3)");
                try
                {
                    while (true)
                    {
                        Console.Write("Row: ");
                        int row = int.Parse(Console.ReadLine()) - 1;
                        Console.Write("Field: ");
                        int field = int.Parse(Console.ReadLine()) - 1;
                        if (ticTacToeField[row, field] != "/")
                        {
                            Console.WriteLine("Field is already taken, pick a different one.");
                        }
                        else
                        {
                            ticTacToeField[row, field] = playerCharacter;
                            break;
                        }
                    }
                    CheckForWin();
                }
                catch
                {
                    Console.WriteLine("Invalid Inputs. Turn skipped");
                    return;
                }
            }
        }
        static void PrintField()
        {
            Console.Clear();
            for (int i = 0; i < ticTacToeField.GetLength(0); i++)
            {
                for (int j = 0; j < ticTacToeField.GetLength(1); j++)
                {
                    if (j == 2)
                    {
                        Console.Write($"{ticTacToeField[i, j]}\n");
                    }
                    else
                    {
                        Console.Write($"{ticTacToeField[i, j]} | ");
                    }
                }
            }
        }
    }
}