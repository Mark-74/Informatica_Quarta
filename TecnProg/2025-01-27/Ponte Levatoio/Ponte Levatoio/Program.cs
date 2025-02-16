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
        const int MAX_WAITING_AUTO = 10;

        static SemaphoreSlim _sem = new SemaphoreSlim(MAX_AUTO_SUL_PONTE);
        static List<Thread> Waiting_Threads = new List<Thread>();
        static Thread[] Active_Threads = new Thread[MAX_AUTO_SUL_PONTE];
        static int[] positions = new int[MAX_AUTO_SUL_PONTE];
        static int auto_counter = 0;
        static int waiting_auto_counter = 0;

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

        static void StampaComandi()
        {
            lock (_lock)
            {
                SetCursorPosition(90, 1);
                Write("COMMANDS");
                SetCursorPosition(90, 2);
                Write("<A> Add car");
                SetCursorPosition(90, 3);
                Write("<O> Open bridge");
                SetCursorPosition(90, 4);
                Write("<C> Close bridge");
                SetCursorPosition(90, 5);
                Write("<Q> Quit");
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
            if (ponteChiuso) return;

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
            if(waiting_auto_counter >= MAX_WAITING_AUTO) return;

            waiting_auto_counter += 1;
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

        static bool CheckAutoSulPonte()
        {
            bool auto_sul_ponte = false;
            for(int i = 0; i < Active_Threads.Length; i++)
            {
                if (Active_Threads[i] is null || Active_Threads[i].ThreadState == ThreadState.Stopped) continue;

                int current_Thread_Name_Length = Active_Threads[i].Name.Length;
                if(positions[i] + current_Thread_Name_Length > 40 && positions[i] + current_Thread_Name_Length < 80) auto_sul_ponte = true;
            }

            return auto_sul_ponte;
        }

        static void Quit()
        {
            for (int i = 0; i < Waiting_Threads.Count; i++)
            {
                Waiting_Threads[i].Abort();
            }

            for (int i = 0; i < Active_Threads.Length; i++)
            {
                if (Active_Threads[i] is null || Active_Threads[i].ThreadState == ThreadState.Stopped) continue;
                Active_Threads[i].Abort();
            }
        }

        static void Auto()
        {
            _sem.Wait();
            Thread.Sleep(50);

            Waiting_Threads.RemoveAt(Waiting_Threads.IndexOf(Thread.CurrentThread));
            UpdateThreadList();
            waiting_auto_counter -= 1;

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
            StampaComandi();
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
                            if (!CheckAutoSulPonte()) ApriPonte();
                            break;

                        case 'Q':
                            Quit();
                            return;

                        default:
                            break;
                    }
                }
            }
            ReadKey(true);
        }
    }
}
