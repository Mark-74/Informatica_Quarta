/*
 * Marco Balducci 4H 2024-11-25 
 * Programma a console che simula l'attraversamento di binari tramite thread
*/

using System;
using System.Text;
using System.Threading;
using static System.Console;

namespace CrossTrain
{
    internal class Program
    {
        static string[] omino = { @" [] ", @"/▓▓\", @" ┘└ " };

        #region metodi video
        static void StampaMappa()
        {
            WriteLine("###############################|     |######################################|     |#####################################");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("                               |     |                                      |     |                                     ");
            WriteLine("###############################|     |######################################|     |#####################################");

            TurnGate(1, ConsoleColor.Green);
            TurnGate(2, ConsoleColor.Green);
        }

        static void StampaOmino()
        {
            SetCursorPosition(0, 13);
            Write(omino[0]);
            SetCursorPosition(0, 14);
            Write(omino[1]);
            SetCursorPosition(0, 15);
            Write(omino[2]);
        }

        #endregion

        #region gestione gate
        static void TurnGate(int gate, ConsoleColor color)
        {
            int x, y = 11;
            switch (gate)
            {
                case 1:
                    x = 31;
                    break;

                case 2:
                    x = 76;
                    break;

                default:
                    return;
            }

            ForegroundColor = color;
            SetCursorPosition(x, y);
            Write("□     □");
            SetCursorPosition(x, y+5);
            Write("□     □");
            ForegroundColor = ConsoleColor.White;
        }

        static void CloseGate(int gate)
        {
            int x, y = 12;
            switch (gate)
            {
                case 1:
                    x = 31;
                    break;

                case 2:
                    x = 76;
                    break;

                default: 
                    return;
            }

            for (int i = 0; i < 4; i++)
            {
                SetCursorPosition(x, y+i);
                Write("|     |");
            }

        }

        static void OpenGate(int gate)
        {
            int x, y = 12;
            switch (gate)
            {
                case 1:
                    x = 31;
                    break;

                case 2:
                    x = 76;
                    break;

                default:
                    return;
            }

            for (int i = 0; i < 4; i++)
            {
                SetCursorPosition(x, y + i);
                Write("       ");
            }
        }

        #endregion

        static void Main(string[] args)
        {
            //impostazioni per la console
            OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            StampaMappa();
            StampaOmino();
            ReadLine();
        }
    }
}
