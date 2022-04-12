using System;
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

        //State
        static int Score = 0;
        static void Main(string[] args)
        {
            Console.Title = "Tetris v1.0";
            Console.CursorVisible = false;
            Console.SetWindowSize(ConsoleCols, ConsoleRows);
            Console.SetBufferSize(ConsoleCols, ConsoleRows);
            DrawBorder();
            DrawInfo();
            while (true)
            {
                Score++;
                if (Console.KeyAvailable)
                {
                  var key = Console.ReadKey();
                  if(key.Key == ConsoleKey.Escape)
                  {
                      return;
                  }
                }
                // user input
                // change state

                // Redraw UI
                DrawBorder();
                DrawInfo();

                Thread.Sleep(40);
            }
        }

        static void DrawInfo()
        {
            Write("Score:", 1, 3 + TetrisCols);
            Write(Score.ToString(), 2, 3 + TetrisCols);
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
