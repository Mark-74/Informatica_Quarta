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
        static Object _lockConsole = new Object();
        static bool Gate1Closed = false;
        static bool Gate2Closed = false;
        static bool Finished = false;
        static int ominoIsCrossing = -1;

        static Random r = new Random();

        static Thread thOmino, thTrain1, thTrain2;

        #region personaggi
        static string[] omino = { @"  [] ", @" /▓▓\", @"  ┘└ " };
        static string[] treno =
        {
            "╔═══╗",
            "║   ║",
            "║   ║",
            "╚═╦═╝",
            "╔═╩═╗",
            "║   ║",
            "║   ║",
            "╚═╦═╝",
            "╔═╩═╗",
            "║   ║",
            "║   ║",
            "╚═╦═╝",
            "╔═╩═╗",
            "║   ║",
            "║   ║",
            "╚═╦═╝",
            "╔═╩═╗",
            "║   ║",
            "╚╗ ╔╝",
            " ╚═╝ ",
        };

        #endregion

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
            WriteLine("▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨         ▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨         ▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("                                                                                                                        ");
            WriteLine("▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨         ▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨         ▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨▨");
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

        static void Stato()
        {
            lock (_lockConsole)
            {
                SetCursorPosition(0, 5);
                Write("Treno 1: ");
                SetCursorPosition(0, 6);
                Write($"IsAlive = {thTrain1.IsAlive}    ");
                SetCursorPosition(0, 7);
                Write($"State = {thTrain1.ThreadState}       ");

                SetCursorPosition(40, 5);
                Write("Treno 2: ");
                SetCursorPosition(40, 6);
                Write($"IsAlive = {thTrain2.IsAlive}    ");
                SetCursorPosition(40, 7);
                Write($"State = {thTrain2.ThreadState}       ");

                SetCursorPosition(85, 5);
                Write("Omino: ");
                SetCursorPosition(85, 6);
                Write($"IsAlive = {thOmino.IsAlive}    ");
                SetCursorPosition(85, 7);
                Write($"State = {thOmino.ThreadState}                            ");

            }
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
            
            lock (_lockConsole)
            {
                ForegroundColor = color;
                SetCursorPosition(x, y);
                Write("□     □");
                SetCursorPosition(x, y + 6);
                Write("□     □");
                ForegroundColor = ConsoleColor.White;
            }
        }

        static void CloseGate(int gate)
        {
            int x, y = 12;
            switch (gate)
            {
                case 1:
                    x = 31;
                    Gate1Closed = true;
                    break;

                case 2:
                    x = 76;
                    Gate2Closed = true;
                    break;

                default: 
                    return;
            }

            TurnGate(gate, ConsoleColor.Red);
            for (int i = 0; i < 5; i++)
            {
                lock (_lockConsole)
                {
                    SetCursorPosition(x, y + i);
                    Write("┋     ┋");
                }
            }

        }

        static void OpenGate(int gate)
        {
            int x, y = 12;
            switch (gate)
            {
                case 1:
                    x = 31;
                    Gate1Closed = false;
                    break;

                case 2:
                    x = 76;
                    Gate2Closed = false;
                    break;

                default:
                    return;
            }

            TurnGate(gate, ConsoleColor.Green);
            for (int i = 0; i < 5; i++)
            {
                lock (_lockConsole)
                {
                    SetCursorPosition(x, y + i);
                    Write("       ");
                }
            }
        }

        #endregion

        #region gestione treno

        static void Passa(int gate)
        {
            if(ominoIsCrossing == gate)
            {
                while (!Finished && ominoIsCrossing == gate)
                    Thread.Sleep(50);

                if (Finished) return;
            }

            CloseGate(gate);
            int x;
            switch (gate)
            {
                case 1:
                    x = 32;
                    break;

                case 2:
                    x = 77;
                    break;

                default:
                    return;
            }

            //stampa graduale treno per ogni posizione
            for(int j = 0; j < 57; j++)
            {
                for(int i = 0; i <= j; i++)
                {
                    lock (_lockConsole)
                    {
                        if (j - i > 28) continue;
                        SetCursorPosition(x, j - i);
                        if (treno.Length - 1 - i < 0) Write("     ");
                        else Write(treno[treno.Length - 1 - i]);
                    }
                }
                Thread.Sleep(50);
            }
            OpenGate(gate);
            
        }

        static void TrenoManager(object gate)
        {
            while (!Finished)
            {
                int waitTime = r.Next(500, 2000);
                Thread.Sleep(waitTime);

                Passa((int)gate);
            }
            
        }

        #endregion

        #region gestione omino

        static void Cammina()
        {
            for (int pos = 0; pos < 114; pos++) 
            {
                Stato();
                if (pos > 27 && pos < 38) ominoIsCrossing = 1;
                else if (pos > 72 && pos < 83) ominoIsCrossing = 2;
                else ominoIsCrossing = -1;

                if (Gate1Closed && pos == 27)
                {
                    while (Gate1Closed) {
                        Stato();
                        Thread.Sleep(50);
                    }
                        
                }

                if(Gate2Closed && pos == 72)
                {
                    while (Gate2Closed)
                    {
                        Stato();
                        Thread.Sleep(50);
                    }
                }

                lock (_lockConsole)
                {
                    SetCursorPosition(pos, 15);
                    Write(omino[2]);
                    
                }
                Thread.Sleep(20);
                lock (_lockConsole)
                {
                    SetCursorPosition(pos, 14);
                    Write(omino[1]);
                }
                Thread.Sleep(20);
                lock (_lockConsole)
                {
                    SetCursorPosition(pos, 13);
                    Write(omino[0]);
                }
                Thread.Sleep(20);
            }

            Finished = true;
        }

        #endregion

        static void Main(string[] args)
        {
            //impostazioni per la console
            OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            StampaMappa();
            StampaOmino();

            //thOmino = new Thread(Cammina);
            thTrain1 = new Thread(TrenoManager);
            thTrain2 = new Thread(TrenoManager);
            thOmino = new Thread(Cammina);

            //thOmino.Start();
            thTrain1.Start(1);
            thTrain2.Start(2);
            thOmino.Start();

            SetCursorPosition(1, 20);
            Write("MENU:");
            SetCursorPosition(1, 21);
            Write("A: Abort");
            SetCursorPosition(1, 22);
            Write("S: Suspend");
            SetCursorPosition(1, 23);
            Write("R: Resume");

            while (thOmino.IsAlive)
            {
                if (KeyAvailable)
                {
                    char c = char.ToUpper(ReadKey(true).KeyChar);
                    switch (c)
                    {
                        case 'A':
                            thOmino.Abort();
                            Finished = true;
                            break;

                        case 'S':
                            if(thOmino.ThreadState.Equals(ThreadState.Running) || thOmino.ThreadState.Equals(ThreadState.WaitSleepJoin))thOmino.Suspend();
                            Stato();
                            break;

                        case 'R':
                            if (thOmino.ThreadState.Equals(ThreadState.Suspended) || thOmino.ThreadState.Equals(ThreadState.SuspendRequested)) thOmino.Resume();
                            Stato();
                            break;

                        default:
                            break;
                    }
                }
            }

            thTrain1.Join();
            thTrain2.Join();

            Stato();
            SetCursorPosition(46, 20);
            Write("  Premi un tasto per");
            SetCursorPosition(46, 21);
            Write("terminare il programma");
            ReadLine();


        }
    }
}
