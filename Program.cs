namespace TicTacToe
{
    internal class Program
    {
        private static readonly Random _rnd = new();
        private static string _playerChar1 = "";
        private static string _playerChar2 = "";
        private static int _winningPlayer = 0;
        private static bool _gameWon = false;
        private static readonly string[,] _ticTacToeField =
        {
            {"/", "/", "/"},
            {"/", "/", "/"},
            {"/", "/", "/"}
        };

        private static void Main()
        {
            Console.WriteLine("Welcome to good-old Tic Tac Toe! Are you a two-player Team (1) or is it you against the computer (2)?");
            string? input = Console.ReadLine();

            while (true)
            {
                switch (input)
                {
                    case "1": // Player vs. Player
                    {
                        if (_playerChar1 == "" && _playerChar2 == "")
                        {
                            Console.Write("Player 1, pick your symbol (anything goes): ");
                            _playerChar1 = Console.ReadLine().Trim();
                            Console.Write("Same thing for player 2: ");
                            while (true)
                            {
                                _playerChar2 = Console.ReadLine().Trim();
                                if (_playerChar2 != _playerChar1)
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("No two players can have the same character; choose a different one");
                                }
                            }
                        }
                        while (CheckEmptyFields() && !_gameWon)
                        {
                            PrintField();
                            PlayerTurn(_playerChar1);
                            PrintField();
                            PlayerTurn(_playerChar2);
                        }

                        break;
                    }
                    case "2": // Player vs. CPU
                    {
                        _playerChar1 = "X";
                        _playerChar2 = "O";
                        while (CheckEmptyFields() && !_gameWon)
                        {
                            PrintField();
                            PlayerTurn(_playerChar1);
                            CpuTurn(_playerChar2);
                        }

                        break;
                    }
                    default:
                        Console.WriteLine("Invalid input :( Session terminated");
                        return;
                }

                if (_winningPlayer != 0)
                {
                    PrintField();
                    Console.WriteLine($"\nThe game is over! Player {_winningPlayer} has won!");
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
                    _winningPlayer = 0;
                    _gameWon = false;
                    for (int i = 0; i < _ticTacToeField.GetLength(0); i++)
                    {
                        for (int j = 0; j < _ticTacToeField.GetLength(1); j++)
                        {
                            _ticTacToeField[i, j] = "/";
                        }
                    }

                    Console.WriteLine("Two players (1) or against the CPU (2)?");
                    input = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nThank you for playing! (Press any key to close)");
                    Console.ReadKey();
                    break;
                }
            }
        }
        private static bool CheckForWin()
        {
            if (CheckHorizontal(_playerChar1) || CheckVertical(_playerChar1) || CheckDiagonally(_playerChar1))
            {
                _winningPlayer = 1;
                _gameWon = true;
                return true;
            }
            else if (CheckHorizontal(_playerChar2) || CheckVertical(_playerChar2) || CheckDiagonally(_playerChar2))
            {
                _winningPlayer = 2;
                _gameWon = true;
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
                for (int row = 0; row < _ticTacToeField.GetLength(0); row++)
                {
                    for (int field = 0; field < _ticTacToeField.GetLength(1); field++)
                    {
                        sameSymbols.Add(_ticTacToeField[row, field] == playerChar);
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

                for (int field = 0; field < _ticTacToeField.GetLength(1); field++)
                {
                    for (int row = 0; row < _ticTacToeField.GetLength(0); row++)
                    {
                        sameSymbols.Add(_ticTacToeField[row, field] == playerChar);
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
                for (int i = 0; i < _ticTacToeField.GetLength(0); i++)
                {
                    sameSymbols.Add(_ticTacToeField[i, i] == playerChar);
                }
                if (CheckList(sameSymbols))
                {
                    return true;
                }

                // check from right to left
                sameSymbols.Clear();
                for (int i = 0; i < _ticTacToeField.GetLength(0); i++)
                {
                    sameSymbols.Add(_ticTacToeField[i, 2 - i] == playerChar);
                }
                return CheckList(sameSymbols);
            }
            bool CheckList(List<bool> sameSymbols)
            {
                return sameSymbols.TrueForAll(x => x == true);
            }
        }
        private static bool CheckEmptyFields()
        {
            bool emptyFields = false;
            for (int i = 0; i < _ticTacToeField.GetLength(0); i++)
            {
                for (int j = 0; j < _ticTacToeField.GetLength(1); j++)
                {
                    if (_ticTacToeField[i, j] == "/")
                    {
                        emptyFields = true;
                    }
                }
            }
            return emptyFields;
        }
        private static void CpuTurn(string cpuCharacter)
        {
            // TODO: improve Enemy "AI"?
            if (!CheckEmptyFields() || _gameWon) return;
            while (true)
            {
                int row = _rnd.Next(0, 3);
                int field = _rnd.Next(0, 3);
                if (_ticTacToeField[row, field] == "/")
                {
                    _ticTacToeField[row, field] = cpuCharacter;
                    break;
                }
            }
            CheckForWin();
        }
        private static void PlayerTurn(string playerCharacter)
        {
            if (!CheckEmptyFields() || _gameWon) return;
            Console.WriteLine($"\nWhere do you want to set your {playerCharacter}? Enter row number and field number (1-3)");
            try
            {
                while (true)
                {
                    Console.Write("Row: ");
                    int row = int.Parse(Console.ReadLine()) - 1;
                    Console.Write("Field: ");
                    int field = int.Parse(Console.ReadLine()) - 1;
                    if (_ticTacToeField[row, field] != "/")
                    {
                        Console.WriteLine("Field is already taken, pick a different one.");
                    }
                    else
                    {
                        _ticTacToeField[row, field] = playerCharacter;
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
        private static void PrintField()
        {
            Console.Clear();
            for (int i = 0; i < _ticTacToeField.GetLength(0); i++)
            {
                for (int j = 0; j < _ticTacToeField.GetLength(1); j++)
                {
                    Console.Write(_ticTacToeField[i, j]);
                    Console.Write(j == 2 ? "\n" : " | ");
                }
            }
        }
    }
}