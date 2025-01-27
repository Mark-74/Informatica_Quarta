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
        static int[] positions = new int[MAX_AUTO_SUL_PONTE];
        static int auto_counter = 0;

        static Object _lock = new Object();
        static Random rnd = new Random();

        static bool ponteChiuso = false;

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

        static void ApriPonte()
        {
            ponteChiuso = false;

            lock (_lock)
            {
                SetCursorPosition(0, 12);
                Write("═════════════════════════════════════════╗                               ╔═════════════════════════════════════════");
                SetCursorPosition(41, 13);
                Write("║                               ║");
                SetCursorPosition(41, 14);
                Write("║                               ║");
                SetCursorPosition(41, 15);
                Write("║                               ║");
                SetCursorPosition(41, 16);
                Write("║                               ║");
                SetCursorPosition(0, 17);
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

        static void ChiudiPonte()
        {
            ponteChiuso = true;

            lock (_lock)
            {
                SetCursorPosition(0, 12);
                Write("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
                SetCursorPosition(41, 13);
                Write("                                 ");
                SetCursorPosition(41, 14);
                Write("                                 ");
                SetCursorPosition(41, 15);
                Write("                                 ");
                SetCursorPosition(41, 16);
                Write("                                 ");
                SetCursorPosition(0, 17);
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

        static bool checkAutoSulPonte()
        {
            bool auto_sul_ponte = false;
            for(int i = 0; i < Active_Threads.Length; i++)
            {
                if (Active_Threads[i] is null || Active_Threads[i].ThreadState == ThreadState.Stopped) continue;

                int current_Thread_Name_Length = Active_Threads[i].Name.Length;
                if(positions[i] + current_Thread_Name_Length > 40 || positions[i] + current_Thread_Name_Length < 64) auto_sul_ponte = true;
            }

            return auto_sul_ponte;
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
                

            int speed = rnd.Next(50, 200);
            for(int i = 0; i < CONSOLE_LENGTH-Thread.CurrentThread.Name.Length; i++)
            {
                Thread.Sleep(speed);
                positions[y - 13] = i;

                while (i == 40 - Thread.CurrentThread.Name.Length && !ponteChiuso)
                    Thread.Sleep(500);

                lock (_lock)
                {
                    SetCursorPosition(x + i, y);
                    Write(' ' + Thread.CurrentThread.Name + ' ');
                }
            }

            _sem.Release();
        }

        static void Main(string[] args)
        {
            Title = "Marco Balducci 4H";
            OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            StampaMappa();
            ApriPonte();

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

                        case 'C':
                            ChiudiPonte();
                            break;

                        case 'O':
                            if (!checkAutoSulPonte()) ApriPonte();
                            break;

                        default:
                            break;
                    }
                }
            }
            ReadKey(true);
        }
    }
}
