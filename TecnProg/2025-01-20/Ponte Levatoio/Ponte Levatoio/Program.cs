using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Ponte_Levatoio
{
    class Program
    {
        const int MAX_AUTO_SUL_PONTE = 4;
        const int CONSOLE_LENGTH = 115;
        const int CONSOLE_HEIGHT = 29;

        static SemaphoreSlim _sem = new SemaphoreSlim(MAX_AUTO_SUL_PONTE);
        static List<Thread> Waiting_Threads = new List<Thread>();
        static Thread[] Active_Threads = new Thread[MAX_AUTO_SUL_PONTE];
        static int auto_counter = 0;

        static Object _lock = new Object();
        static Random rnd = new Random();

        static void StampaMappa()
        {
            lock (_lock)
            {
                ForegroundColor = ConsoleColor.Cyan;
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░▓▓▓▓░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░▓▓▓░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░▓▓▓░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░▓▓░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░▓▓▓▓░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░▓░▓░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░▓▓░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░▓▓▓░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░▓░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                WriteLine("                                           ║░░░░░░░░░░░░░░░░░░░░░░░░░░░║                                           ");
                ForegroundColor = ConsoleColor.White;
            }
        }

        static void StampaPonteAperto()
        {
            lock (_lock)
            {
                SetCursorPosition(0, 12);
                Write("═════════════════════════════════════════╗                               ╔═════════════════════════════════════════");
                SetCursorPosition(0, 13);
                Write("                                         ║                               ║");
                SetCursorPosition(0, 14);
                Write("                                         ║                               ║");
                SetCursorPosition(0, 15);
                Write("                                         ║                               ║");
                SetCursorPosition(0, 16);
                Write("                                         ║                               ║");
                SetCursorPosition(0, 17);
                Write("                                         ║                               ║");
                SetCursorPosition(0, 18);
                Write("═════════════════════════════════════════╝                               ╚═════════════════════════════════════════");

                ForegroundColor = ConsoleColor.Cyan;
                for (int i = 0; i < 7; i++)
                {
                    SetCursorPosition(43 , 12+i);
                    Write("║░░░░░░░░░░░░░░░░░░░░░░░░░░░║");
                }
                ForegroundColor = ConsoleColor.White;
            }
        }

        static void StampaPonteChiuso()
        {
            lock (_lock)
            {
                SetCursorPosition(0, 12);
                Write("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                SetCursorPosition(0, 13);
                Write("                                                                          ");
                SetCursorPosition(0, 14);
                Write("                                                                          ");
                SetCursorPosition(0, 15);
                Write("                                                                          ");
                SetCursorPosition(0, 16);
                Write("                                                                          ");
                SetCursorPosition(0, 17);
                Write("                                                                          ");
                SetCursorPosition(0, 18);
                Write("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
            }
        }

        static void AggiungiAuto()
        {
            Thread new_thread = new Thread(Auto);
            new_thread.Name = $"Auto {auto_counter++}";
            Waiting_Threads.Add(new_thread);
            UpdateThreadList();
            new_thread.Start();
        }

        static void UpdateThreadList()
        {
            lock (_lock)
            {
                for (int i = 0; i < Waiting_Threads.Count; i++)
                {
                    SetCursorPosition(5, i);
                    Write(Waiting_Threads[i].Name);
                }
                SetCursorPosition(5, Waiting_Threads.Count);
                Write("            ");
            }
        }

        static void Auto()
        {
            _sem.Wait();
            Thread.Sleep(50);

            Waiting_Threads.RemoveAt(Waiting_Threads.IndexOf(Thread.CurrentThread));
            UpdateThreadList();

            int x = 0;
            int y = 13;
            for (int i = 0; i < MAX_AUTO_SUL_PONTE; i++)
            {
                if (Active_Threads[i] is null || Active_Threads[i].ThreadState == ThreadState.Stopped) //find first free spot in active threads
                {
                    y += i;
                    Active_Threads[i] = Thread.CurrentThread;
                    break;
                }
            }
                

            int speed = rnd.Next(50, 500);
            for(int i = 0; i < CONSOLE_LENGTH-Thread.CurrentThread.Name.Length; i++)
            {
                lock (_lock)
                {
                    Thread.Sleep(speed);
                    SetCursorPosition(x + i, y);
                    Write(' ' + Thread.CurrentThread.Name + ' ');
                }
            }

            _sem.Release();
        }

        static void Main(string[] args)
        {
            OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            StampaMappa();
            StampaPonteAperto();

            while (true)
            {
                if (KeyAvailable)
                {
                    //lettura comando utente
                    char c = char.ToUpper(ReadKey(true).KeyChar);
                    switch(c)
                    {
                        case 'A':
                            AggiungiAuto();
                            break;
                    }
                }
            }
            ReadKey(true);
        }
    }
}
