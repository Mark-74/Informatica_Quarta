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
        //Variablili Globali
        static Object _lockConsole = new Object();
        static bool Gate1Closed = false;
        static bool Gate2Closed = false;
        static bool Finished = false;
        static int ominoIsCrossing = -1;

        static Random r = new Random();

        //Thread personaggi
        static Thread thOmino, thTrain1, thTrain2;

        #region personaggi

        //dichiarazione stringhe dei personaggi
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
        /// <summary>
        /// Stampa la mappa, da chiamare solo all'inizio per inizializzare la console
        /// </summary>
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

            //apri i gate
            TurnGate(1, ConsoleColor.Green);
            TurnGate(2, ConsoleColor.Green);
        }

        /// <summary>
        /// Stampa l'omino, da chiamare all'inizio del programma
        /// </summary>
        static void StampaOmino()
        {
            SetCursorPosition(0, 13);
            Write(omino[0]);
            SetCursorPosition(0, 14);
            Write(omino[1]);
            SetCursorPosition(0, 15);
            Write(omino[2]);
        }

        /// <summary>
        /// Stampa il ThreadState e lo stato Alive dei Thread in gioco
        /// </summary>
        static void Stato()
        {
            lock (_lockConsole)
            {
                //stato Treno 1
                SetCursorPosition(0, 5);
                Write("Treno 1: ");
                SetCursorPosition(0, 6);
                Write($"IsAlive = {thTrain1.IsAlive}    ");
                SetCursorPosition(0, 7);
                Write($"State = {thTrain1.ThreadState}       ");

                //stato Treno 2
                SetCursorPosition(40, 5);
                Write("Treno 2: ");
                SetCursorPosition(40, 6);
                Write($"IsAlive = {thTrain2.IsAlive}    ");
                SetCursorPosition(40, 7);
                Write($"State = {thTrain2.ThreadState}       ");

                //stato Omino
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
        /// <summary>
        /// cambia colore al gate
        /// </summary>
        /// <param name="gate">il gate selezionato</param>
        /// <param name="color">il colore che il gate deve diventare</param>
        static void TurnGate(int gate, ConsoleColor color)
        {
            //posizione del gate
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
                //modifico il colore del gate
                ForegroundColor = color;
                SetCursorPosition(x, y);
                Write("□     □");
                SetCursorPosition(x, y + 6);
                Write("□     □");
                ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Chiude il gate selezionato
        /// </summary>
        /// <param name="gate">il gate da chiudere</param>
        static void CloseGate(int gate)
        {
            //posizione gate
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

            //rendi il gate rosso
            TurnGate(gate, ConsoleColor.Red);

            //stampa barriere
            for (int i = 0; i < 5; i++)
            {
                lock (_lockConsole)
                {
                    SetCursorPosition(x, y + i);
                    Write("┋     ┋");
                }
            }

        }

        /// <summary>
        /// Apre il gate selezionato
        /// </summary>
        /// <param name="gate">il gate selezionato</param>
        static void OpenGate(int gate)
        {
            //posizione gate
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

            //rendi il gate verde
            TurnGate(gate, ConsoleColor.Green);

            //rimuovi barriere
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

        /// <summary>
        /// Fa passare il treno lungo il inario del gate selezionato
        /// </summary>
        /// <param name="gate">il gate selezionato</param>
        static void PassaTreno(int gate)
        {
            //controllo che l'omino non stia attraversando i binari prima che i gate si chiudano
            if(ominoIsCrossing == gate)
            {
                //se l'omino sta attraversando, aspetto che passi per partire
                while (!Finished && ominoIsCrossing == gate)
                    Thread.Sleep(50);

                if (Finished) return;
            }

            //chiudo il gate
            CloseGate(gate);

            //posizione treno
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
                        if (treno.Length - 1 - i < 0) Write("     "); //pulisci coda
                        else Write(treno[treno.Length - 1 - i]);      //stampa treno
                    }
                }
                Thread.Sleep(50);
            }

            //apri gate
            OpenGate(gate);
            
        }

        /// <summary>
        /// Gestisce il passaggio dei treni, richiamando la funzione PassaTreno ad intervalli casuali
        /// </summary>
        /// <param name="rail">il binario lungo cui il treno deve passare</param>
        static void TrenoManager(object rail)
        {
            while (!Finished)
            {
                //aspetta intervallo casuale
                int waitTime = r.Next(500, 2000);
                Thread.Sleep(waitTime);

                //fai passare il treno
                PassaTreno((int)rail);
            }
            
        }

        #endregion

        #region gestione omino

        /// <summary>
        /// Gestisce il movimento dell'omino
        /// </summary>
        static void Cammina()
        {
            //cammina fino a che non arrivo alla fine della console (115)
            for (int pos = 0; pos < 114; pos++) 
            {
                Stato();

                //controllo se l'omino sta attraversando un binario, importante per la funzione PassaTreno
                if (pos > 27 && pos < 38) ominoIsCrossing = 1;
                else if (pos > 72 && pos < 83) ominoIsCrossing = 2;
                else ominoIsCrossing = -1;

                //Se il gate 1 è chiuso, aspetta
                if (Gate1Closed && pos == 27)
                {
                    while (Gate1Closed) {
                        Stato();
                        Thread.Sleep(50);
                    }       
                }

                //Se il gate 2 è chiuso, aspetta
                if(Gate2Closed && pos == 72)
                {
                    while (Gate2Closed)
                    {
                        Stato();
                        Thread.Sleep(50);
                    }
                }

                //Stampa graduale omino
                lock (_lockConsole)
                {
                    SetCursorPosition(pos, 15);
                    Write(omino[2]); //gambe
                    
                }
                Thread.Sleep(20);
                lock (_lockConsole)
                {
                    SetCursorPosition(pos, 14);
                    Write(omino[1]); //busto
                }
                Thread.Sleep(20);
                lock (_lockConsole)
                {
                    SetCursorPosition(pos, 13);
                    Write(omino[0]); //testa
                }
                Thread.Sleep(20);
            }

            //Thread completato, importante per la funzione PassaTreno
            Finished = true;
        }

        #endregion

        /// <summary>
        /// Programma CrossTrain
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //impostazioni per la console
            Title = "Marco Balducci 4H";
            OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            //inizializzazione mappa
            StampaMappa();
            StampaOmino();

            //richiesta input per avviare il programma
            SetCursorPosition(46, 20);
            Write("Premi un tasto per");
            SetCursorPosition(46, 21);
            Write("avviare il programma.");
            ReadKey(true);
            SetCursorPosition(46, 20);
            Write("                  ");
            SetCursorPosition(46, 21);
            Write("                     ");

            //creazione thread dei personaggi
            thTrain1 = new Thread(TrenoManager);
            thTrain2 = new Thread(TrenoManager);
            thOmino = new Thread(Cammina);

            //partenza thread
            thTrain1.Start(1);
            thTrain2.Start(2);
            thOmino.Start();

            //stampa menu
            SetCursorPosition(1, 20);
            Write("MENU:");
            SetCursorPosition(1, 21);
            Write("A: Abort");
            SetCursorPosition(1, 22);
            Write("S: Suspend");
            SetCursorPosition(1, 23);
            Write("R: Resume");

            //gestione comandi utente
            while (!Finished)
            {
                if (KeyAvailable)
                {
                    //lettura comando utente
                    char c = char.ToUpper(ReadKey(true).KeyChar);
                    switch (c)
                    {
                        //caso Abort
                        case 'A':
                            if (thOmino.ThreadState.Equals(ThreadState.Suspended)) thOmino.Resume();
                            thOmino.Abort();
                            Stato();
                            Finished = true; //importante per informare i treni che il thread omino è concluso
                            
                            break;

                        //caso Suspend
                        case 'S':
                            if(thOmino.ThreadState.Equals(ThreadState.Running) || thOmino.ThreadState.Equals(ThreadState.WaitSleepJoin)) thOmino.Suspend();
                            Stato();
                            break;

                        //caso Resume
                        case 'R':
                            if (thOmino.ThreadState.Equals(ThreadState.Suspended) || thOmino.ThreadState.Equals(ThreadState.SuspendRequested) || thOmino.ThreadState.Equals(ThreadState.WaitSleepJoin)) thOmino.Resume();
                            Stato();
                            break;

                        default:
                            //comando non valido
                            break;
                    }
                }
            }

            //quando il thread omino si è concluso, attendo che gli ultimi treni finiscano di passare
            thTrain1.Join();
            thTrain2.Join();

            Stato();

            //stampa messaggio finale
            SetCursorPosition(46, 20);
            Write("  Premi un tasto per");
            SetCursorPosition(46, 21);
            Write("terminare il programma");
            ReadLine();


        }
    }
}
