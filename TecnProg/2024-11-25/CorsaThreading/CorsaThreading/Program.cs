/*
 * 2024-11-04 Marco Balducci 4H 
 * Corsa di thread rappresentati da omini, l'esercizio punta a mostrare l'esecuzione di + thread e lock.
*/

//con questi using non è necessario chiamare sempre System.<...>
using System;
using System.Text;
using System.Threading;
using static System.Console;

namespace CorsaThreading
{
    class Program
    {

        #region area globale

        //dichiarazione posizioni di partenza
        static int posAndrea = 0;
        static int posBaldo = 0;
        static int posCarlo = 0;
        static int classifica = 0;

        //dichiarazione thread
        static Thread thAndrea;
        static Thread thBaldo;
        static Thread thCarlo;

        //"testimone", chi lo possiede può accedere alla console, necessario per risolvere i conflitti tra thread
        static Object _lock = new Object();

        //dichiarazione parti degli omini
        static string[] teste = { "  [] ", "  () ", "  <> " };
        static string[] corpi = { @" /▓▓\ ", @" ╔▓▓╗ ", @" ═▓▓═ " };
        static string[] gambe = { "  ┘└ ", @"  /\ ", "  ╝╚ " };

        static Random r = new Random();
        static string comando = "";

        #endregion

        //funzione di partenza per mostrare i corridori
        static void Pronti()
        {
            //stampa Andrea
            SetCursorPosition(posAndrea, 1);
            Write("Andrea");
            SetCursorPosition(posAndrea, 2);
            Write(teste[0]);
            SetCursorPosition(posAndrea, 3);
            Write(corpi[0]);
            SetCursorPosition(posAndrea, 4);
            Write(gambe[0]);

            //stampa Baldo
            SetCursorPosition(posBaldo, 5);
            Write("Baldo");
            SetCursorPosition(posBaldo, 6);
            Write(teste[1]);
            SetCursorPosition(posBaldo, 7);
            Write(corpi[1]);
            SetCursorPosition(posBaldo, 8);
            Write(gambe[1]);

            //stampa Carlo
            SetCursorPosition(posCarlo, 9);
            Write("Carlo");
            SetCursorPosition(posCarlo, 10);
            Write(teste[2]);
            SetCursorPosition(posCarlo, 11);
            Write(corpi[2]);
            SetCursorPosition(posCarlo, 12);
            Write(gambe[2]);

        }

        #region funzioni Corridori

        //funzione per il movimento di Andrea
        static void Andrea()
        {
            //velocità di Andrea
            int speed = 50;

            do
            {
                //gestione comando J (join)
                if(comando.Length == 3)
                    if (comando[1] == 'J' && comando[0] == 'A')
                        switch (comando[2])
                        {
                            case 'B':
                                thBaldo.Join();
                                break;

                            case 'C':
                                thCarlo.Join();
                                break;

                            default: 
                                //destinatario dell'aspetta non valido
                                break;
                        }

                //aggiornamento posizione omino, prina gambe, poi corpo e infine testa
                posAndrea++;
                Thread.Sleep(speed);
                lock (_lock) //se il testimone è libero: lo prende e va avanti; altrimenti si blocca prima di eseguire questo blocco di codice
                {
                    Stato();
                    //se mettessi Thread.Sleep dentro al blocco lock allora il thread terrebbe il testimone per troppo tempo ed il programma rallenta -> dentro al lock solo istruzioni per accedere a risorse, niente di più
                    SetCursorPosition(posAndrea, 4);
                    Write(gambe[0]);
                }
                Thread.Sleep(speed); //stato sleep
                lock (_lock)
                {
                    Stato();
                    //stato running
                    SetCursorPosition(posAndrea, 3);
                    Write(corpi[0]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posAndrea, 2);
                    Write(teste[0]);
                }
                speed = r.Next(0, 100);

            } while (posAndrea < 115);

            //il primo che arriva si prende la posizone 1, il secondo la 2 e così via. Dato che un thread può finire prima di un altro la classifica mostra l'ordine di fine.
            lock (_lock)
            {
                Stato();
                classifica++;
                SetCursorPosition(posAndrea, 1);
                Write(classifica);
            }
        }

        //funzione per il movimento di Baldo
        static void Baldo()
        {
            //celocità di Baldo
            int speed = 50;

            do
            {
                //gestione comando J (join)
                if (comando.Length == 3)
                    if (comando[1] == 'J' && comando[0] == 'B')
                        switch (comando[2])
                        {
                            case 'A':
                                thAndrea.Join();
                                break;

                            case 'C':
                                thCarlo.Join();
                                break;

                            default:
                                //destinatario dell'aspetta non valido
                                break;
                        }

                //aggiornamento posizione omino, prina gambe, poi corpo e infine testa
                posBaldo++;
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posBaldo, 8);
                    Write(gambe[1]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posBaldo, 7);
                    Write(corpi[1]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posBaldo, 6);
                    Write(teste[1]);
                }
                speed = r.Next(0, 100);

            } while (posBaldo < 115);

            //il primo che arriva si prende la posizone 1, il secondo la 2 e così via perchè la variabile classifica è nello spazio comune dei thread. Dato che un thread può finire prima di un altro la classifica mostra l'ordine di fine.
            lock (_lock)
            {
                Stato();
                classifica++;
                SetCursorPosition(posBaldo, 5);
                Write(classifica);
            }
        }

        //funzione per il movimento di Carlo
        static void Carlo()
        {
            //velocità di Carlo
            int speed = 50;

            do
            {
                //gestione comando J (join)
                if (comando.Length == 3)
                    if (comando[1] == 'J' && comando[0] == 'C')
                        switch (comando[2])
                        {
                            case 'A':
                                thAndrea.Join();
                                break;

                            case 'C':
                                thBaldo.Join();
                                break;

                            default:
                                //destinatario dell'aspetta non valido
                                break;
                        }

                //aggiornamento posizione omino, prina gambe, poi corpo e infine testa
                posCarlo++;
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posCarlo, 12);
                    Write(gambe[2]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posCarlo, 11);
                    Write(corpi[2]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    Stato();
                    SetCursorPosition(posCarlo, 10);
                    Write(teste[2]);
                }
                speed = r.Next(0, 100);

            } while (posCarlo < 115);

            //il primo che arriva si prende la posizone 1, il secondo la 2 e così via. Dato che un thread può finire prima di un altro la classifica mostra l'ordine di fine.
            lock (_lock)
            {
                Stato();
                classifica++;
                SetCursorPosition(posCarlo, 9);
                Write(classifica);
            }
        }

        #endregion

        #region funzioni azioni
        static void AccettaComandi()
        {
            Thread thAzione;
            comando = "";
            char choice = ' ';

            //Legge il comando utente
            choice = ReadKey(true).KeyChar;
            choice = char.ToUpper(choice);

            //salvo il thread scelto dall'utente in thAzione
            switch (choice)
            {
                case 'A':
                    thAzione = thAndrea;
                    break;

                case 'B':
                    thAzione = thBaldo;
                    break;

                case 'C':
                    thAzione = thCarlo;
                    break;

                default:
                    return;
            }

            comando += choice;

            //legge azione da interpretare
            choice = StampaMenuAzioni("Azione su " + thAzione.Name);
            switch (choice)
            {
                //per alcune azioni è necessario il lock perchè se il Thread viene sospeso o abortito mentre ha un lock, quest'ultimo non viene rilasciato e tutti gli altri thread sono bloccati.
                case 'S':
                    lock (_lock)
                        if (thAzione.ThreadState.Equals(ThreadState.Running) || thAzione.ThreadState.Equals(ThreadState.WaitSleepJoin)) thAzione.Suspend();
                    break;

                case 'R':
                    if (thAzione.ThreadState.Equals(ThreadState.Suspended) || thAzione.ThreadState.Equals(ThreadState.SuspendRequested)) thAzione.Resume();
                    break;

                case 'A':
                    lock (_lock)
                        if(thAzione.ThreadState.Equals(ThreadState.Running) || thAzione.ThreadState.Equals(ThreadState.WaitSleepJoin)) thAzione.Abort();
                    break;

                case 'J':
                    comando += choice;
                    StampaMenu(message: "CHI ASPETTA", offset: 60);
                    choice = char.ToUpper(ReadKey(true).KeyChar);
                    comando += choice;
                    break;

                default:
                    break;

            }

            //pulisco la console dal menù azioni stampato
            lock (_lock)
                ClearMenuAzioni();
        }

        static void ClearMenuAzioni()
        {
            SetCursorPosition(33, 20);
            Write("                                                                ");
            SetCursorPosition(33, 22);
            Write("                                                                ");
            SetCursorPosition(33, 23);
            Write("                                                                ");
            SetCursorPosition(33, 24);
            Write("                                                                ");
            SetCursorPosition(33, 25);
            Write("                                                                ");
        }

        static char StampaMenuAzioni(string titolo)
        {
            char c;
            lock (_lock)
            {
                SetCursorPosition(33, 20);
                Write(titolo);
                SetCursorPosition(33, 22);
                Write("Sospendere (S)");
                SetCursorPosition(33, 23);
                Write("Riprendere (R)");
                SetCursorPosition(33, 24);
                Write("Abort      (A)");
                SetCursorPosition(33, 25);
                Write("Aspetta    (J)");
            }

            //leggi input
            c = ReadKey(true).KeyChar;
            return char.ToUpper(c);
        }

        static void StampaMenu(string message = "MENU", int offset = 0)
        {
            //Stampa il menù sotto i corridori

            SetCursorPosition(offset + 0, 20);
            Write(message);
            SetCursorPosition(offset + 0, 22);
            Write("Andrea (A)");
            SetCursorPosition(offset + 0, 23);
            Write("Baldo  (B)");
            SetCursorPosition(offset + 0, 24);
            Write("Carlo  (C)");
        }

        #endregion

        static void Stato()
        {
            //scrive lo stato dei thread e se sono attivi

            //Andrea
            SetCursorPosition(0, 1);
            Write("Andrea -> " + thAndrea.ThreadState + "             ");
            SetCursorPosition(50, 1);
            Write("Is alive = " + thAndrea.IsAlive + "             ");

            //Baldo
            SetCursorPosition(0, 5);
            Write("Baldo -> " + thBaldo.ThreadState + "             ");
            SetCursorPosition(50, 5);
            Write("Is alive = " + thBaldo.IsAlive + "             ");

            //Carlo
            SetCursorPosition(0, 9);
            Write("Carlo -> " + thCarlo.ThreadState + "             ");
            SetCursorPosition(50, 9);
            Write("Is alive = " + thCarlo.IsAlive + "             ");
        }

        static void Main(string[] args)
        {
            //impostazioni per la console
            OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            //stampo griglia di partenza
            Pronti();

            //dichiaro e istanzio i thread
            thAndrea = new Thread(Andrea);
            thAndrea.Name = "Andrea";
            thBaldo = new Thread(Baldo);
            thBaldo.Name = "Baldo";
            thCarlo = new Thread(Carlo);
            thCarlo.Name = "Carlo";

            //Stampo lo stato prima di partire
            Stato();
            SetCursorPosition(0, 20);
            Write("premi invio per partire.");
            ReadLine();
            SetCursorPosition(0, 20);
            Write("                        ");
            //corsa partita

            //stampo il menù
            StampaMenu();

            //faccio partire i thread
            thAndrea.Start();
            thBaldo.Start();
            thCarlo.Start();

            do
            {
                lock (_lock)
                {
                    StampaMenu();
                }
                if (Console.KeyAvailable) AccettaComandi();
            } while (thAndrea.IsAlive || thBaldo.IsAlive || thCarlo.IsAlive);

            ////gestione input utente
            //while (thAndrea.IsAlive || thCarlo.IsAlive || thCarlo.IsAlive)
            //    if (KeyAvailable) AccettaComandi(); //se c'è un carattere nel buffer della console allora l'utente ha interagito

            thAndrea.Join();
            thBaldo.Join();
            thCarlo.Join();

            Stato();


            //perchè l'ordine della partenza dei thread non corrisponde al risultato finale?
            //il tempo cpu assegnato dallo scheduler del sistema operativo non è scelto dal programmatore, perciò il thread a cui è stato assegnato + tempo cpu finisce per primo (vince).

            ReadLine();
        }
    }
}
