using System;
using System.Collections.Generic;
using System.Threading;

namespace Tetris
{
    class Program
    {
        //Settings
        static int TetrisRows = 20;
        static int TetrisCols = 10;
        static int InfoCols = 10;
        static int ConsoleRows = 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;
        static List<bool[,]> TetrisFigures = new List<bool[,]>()
            {
                new bool[,] // I
                {
                    { true, true, true, true }  // - - - - 
                },
                new bool[,] // O
                {
                    { true, true },    // - -
                    { true, true }     // - - 
                },
                new bool[,] // T
                {
                    { false, true, false }, //   -
                    { true, true, true }    // - - -
                },
                new bool[,] // S
                {
                    { false, true, true },  //   - -
                    { true, true, false }   // - - 
                },  
                new bool[,] // Z
                {
                    { true, true, false },  // - -   
                    { false, true, true }   //   - -
                },
                new bool[,] // J
                {
                    { true, false, false },  // -    
                    { true, true, true }     // - - -
                },
                new bool[,] // L
                {
                    { false, false, true }, //     -
                    { true, true, true }    // - - -
                }
            };

        //State
        static int Score = 0;
        static int Frame = 0;
        static int FramesToMoveFigure = 20;
        static int CurrentFigureRow = 0;
        static int CurrentFigureCol = 0;
        static bool[,] CurrentFigure = null; // TODO: Random
        static Random Random = new Random();
        static bool[,] TetrisField = new bool[TetrisRows, TetrisCols];

        static void Main(string[] args)
        {
            Console.Title = "Tetris v1.0";
            Console.CursorVisible = false;
            Console.SetWindowSize(ConsoleCols, ConsoleRows);
            Console.SetBufferSize(ConsoleCols, ConsoleRows);
            CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
            while (true)
            {
                Frame++;
                // Read user input
                if (Console.KeyAvailable)
                {
                      var key = Console.ReadKey();
                      if(key.Key == ConsoleKey.Escape)
                      {
                          return;
                      }
                      if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                      {
                        if (CurrentFigureCol >= 1)
                        {
                            CurrentFigureCol--;
                        }
                      }
                      if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                      {
                        if (CurrentFigureCol < TetrisCols - CurrentFigure.GetLength(1))
                        {
                            CurrentFigureCol++;
                        }
                      }
                      if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.W || key.Key == ConsoleKey.UpArrow)
                      {
                          // TODO: Implement 90-degree rotation of the current figure
                      }
                      if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                      {
                          Frame = 1;
                          Score++;
                          CurrentFigureRow++;
                          // TODO: Move current figure down
                      }                    
                }

                // Update the game state
                if(Frame % FramesToMoveFigure ==  0)
                {
                    CurrentFigureRow++;
                    Frame = 0;
                }
                if (Collision())
                {
                    AddCurrentFigureToTetrisField();
                    CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
                    CurrentFigureRow = 0;
                    CurrentFigureCol = 0;
                  //CheckForFullLines();
                }

                // Redraw UI
                DrawBorder();
                DrawInfo();
                DrawTetrisField();
                DrawCurrentFigure();
                
                Thread.Sleep(40);
            }
        }

        static void DrawTetrisField()
        {
            for (int row = 0; row < TetrisRows; row++)
            {
                for (int col = 0; col < TetrisCols; col++)
                {
                    if (TetrisField[row, col])
                    {
                        Write("*", row + 1, col + 1); 
                    }
                    
                }
            }
        }

        static void AddCurrentFigureToTetrisField()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        TetrisField[CurrentFigureRow + row, CurrentFigureCol + col] = true; 
                    }
                }
            }
        }

        static bool Collision()
        {
            // TODO: Collide with other figures
            if (CurrentFigureRow + CurrentFigure.GetLength(0) == TetrisRows)
            {
                return true;
            }

            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col] && TetrisField[CurrentFigureRow + row + 1, CurrentFigureCol + col])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static void DrawCurrentFigure()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        Write("*", row + 1 + CurrentFigureRow, col + 1 + CurrentFigureCol, ConsoleColor.Green);
                    }
                }
            }
        }

        static void DrawInfo()
        {
            Write("Score:", 1, 3 + TetrisCols);
            Write(Score.ToString(), 2, 3 + TetrisCols);
            Write("Frame:", 4, 3 + TetrisCols);
            Write(Frame.ToString(), 5, 3 + TetrisCols);
            Write("Position:", 7, 3 + TetrisCols);
            Write($"{CurrentFigureRow}, {CurrentFigureCol}", 8, 3 + TetrisCols);
            Write("Keys:", 10, 3 + TetrisCols);
            Write("    ^", 12, 3 + TetrisCols);
            Write("  < v >", 13, 3 + TetrisCols);
        }

        static void DrawBorder()
        {
            Console.SetCursorPosition(0, 0);
            
            //First line
            string line = "╔";
            line += new string('═', TetrisCols);
              //for (int i = 0; i < TetrisCols; i++)
              //{
              //    line += "═";
              //}
            line += "╦";
            line += new string('═', InfoCols);
            line += "╗";
            Console.WriteLine(line);
           
            //Middle line
            for (int i = 0; i < TetrisRows; i++)
            {
                string middleLine = "║";
                middleLine += new string(' ', TetrisCols);
                middleLine += "║";
                middleLine += new string(' ', InfoCols);
                middleLine += "║";
                Console.Write(middleLine);
            }
            
            //End line
            string endLine = "╚";
            endLine += new string('═', TetrisCols);
            endLine += "╩";
            endLine += new string('═', InfoCols);
            endLine += "╝";
            Console.Write(endLine);
        }

        static void Write(string text,int row, int col,ConsoleColor color = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(col, row);
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
