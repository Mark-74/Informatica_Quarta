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

        //dichiarazione posizioni di partenza
        static int posAndrea = 0;
        static int posBaldo = 0;
        static int posCarlo = 0;
        static int classifica = 0;

        //"testimone", chi lo possiede può accedere alla console, necessario per risolvere i conflitti tra thread
        static Object _lock = new Object();

        //dichiarazione parti degli omini
        static string[] teste = {  "  [] ",   "  () ",   "  <> " };
        static string[] corpi = { @" /▓▓\ ", @" ╔▓▓╗ ", @" ═▓▓═ " };
        static string[] gambe = {  "  ┘└ ",  @"  /\ ",   "  ╝╚ " };

        static Random r = new Random();
        
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
            SetCursorPosition (posBaldo, 5);
            Write("Baldo");
            SetCursorPosition (posBaldo, 6);
            Write(teste[1]);
            SetCursorPosition (posBaldo, 7);
            Write(corpi[1]);
            SetCursorPosition (posBaldo, 8);
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
                //aggiornamento posizione omino, prina gambe, poi corpo e infine testa
                posAndrea++;
                Thread.Sleep(speed); 
                lock (_lock) //se il testimone è libero: lo prende e va avanti; altrimenti si blocca prima di eseguire questo blocco di codice
                {
                    //se mettessi Thread.Sleep dentro al blocco lock allora il thread terrebbe il testimone per troppo tempo ed il programma rallenta -> dentro al lock solo istruzioni per accedere a risorse, niente di più
                    SetCursorPosition(posAndrea, 4);
                    Write(gambe[0]);
                }
                Thread.Sleep(speed); //stato sleep
                lock (_lock)
                {
                    //stato running
                    SetCursorPosition(posAndrea, 3);
                    Write(corpi[0]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posAndrea, 2);
                    Write(teste[0]);
                }
                speed = r.Next(0, 100);

            } while (posAndrea < 115);

            //il primo che arriva si prende la posizone 1, il secondo la 2 e così via. Dato che un thread può finire prima di un altro la classifica mostra l'ordine di fine.
            lock (_lock)
            {
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
                //aggiornamento posizione omino, prina gambe, poi corpo e infine testa
                posBaldo++;
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posBaldo, 8);
                    Write(gambe[1]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posBaldo, 7);
                    Write(corpi[1]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posBaldo, 6);
                    Write(teste[1]);
                }
                speed = r.Next(0, 100);

            } while (posBaldo < 115);

            //il primo che arriva si prende la posizone 1, il secondo la 2 e così via perchè la variabile classifica è nello spazio comune dei thread. Dato che un thread può finire prima di un altro la classifica mostra l'ordine di fine.
            lock (_lock)
            {
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
                //aggiornamento posizione omino, prina gambe, poi corpo e infine testa
                posCarlo++;
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posCarlo, 12);
                    Write(gambe[2]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posCarlo, 11);
                    Write(corpi[2]);
                }
                Thread.Sleep(speed);
                lock (_lock)
                {
                    SetCursorPosition(posCarlo, 10);
                    Write(teste[2]);
                }
                speed = r.Next(0, 100);

            } while (posCarlo < 115);

            //il primo che arriva si prende la posizone 1, il secondo la 2 e così via. Dato che un thread può finire prima di un altro la classifica mostra l'ordine di fine.
            lock (_lock)
            {
                classifica++;
                SetCursorPosition(posCarlo, 9);
                Write(classifica);
            }
        }

        #endregion

        static void Main(string[] args)
        {
            //impostazioni per la console
            Console.OutputEncoding = Encoding.Unicode;
            CursorVisible = false;

            //stampo griglia di partenza
            Pronti();

            //dichiaro e istanzio i thread
            Thread thAndrea = new Thread(Andrea);
            Thread thBaldo = new Thread(Baldo);
            Thread thCarlo = new Thread(Carlo);

            //faccio partire i thread
            thAndrea.Start();
            thBaldo.Start();
            thCarlo.Start();

            //perchè l'ordine della partenza dei thread non corrisponde al risultato finale?
            //il tempo cpu assegnato dallo scheduler del sistema operativo non è scelto dal programmatore, perciò il thread a cui è stato assegnato + tempo cpu finisce per primo (vince).

            ReadLine();
        }
    }
}
