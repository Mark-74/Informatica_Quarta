using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsercizioSemaforo
{
    class TheClub
    {
        static SemaphoreSlim _sem = new SemaphoreSlim(5);    // Capacity of 3

        public static void Main()
        {
            Console.Title = "Marco Balducci 4H";
            for (int i = 1; i <= 10; i++) new Thread(Enter).Start(i);
            Console.ReadKey(true);
        }

        static void Enter(object id)
        {
            Console.WriteLine(id + " wants to enter");
            _sem.Wait();                                // Se c'è posto allora il codice continua ad eseguire, altrimenti il Wait diventa bloccante finchè non si libera un posto
            Console.WriteLine(id + " is in!");           // Only three threads
            Thread.Sleep(1000 * (int)id);               // can be here at
            Console.WriteLine(id + " is leaving");       // a time.
            _sem.Release();                             // Libera un posto
        }
    }
}
