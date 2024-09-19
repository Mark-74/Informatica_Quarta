namespace Esercizio_1
{
    internal class Program
    {

        static void readInput(out DateOnly dateOnly)
        {
            Console.Write("inserisci una data nel formato gg/mm/aaaa\n > ");
            while (true)
            {
                String input = Console.ReadLine()!;
                if (DateOnly.TryParse(input, out dateOnly))
                    break;
                else
                    Console.Write("\n\nData non valida, riprova\n > ");
            }
        }
        static void Main(string[] args)
        {

            DateOnly giorno_uno;
            DateOnly giorno_due;

            readInput(out giorno_uno);

            Console.WriteLine("\ndata ricevuta\n");

            readInput(out giorno_due);

            Console.WriteLine($"Giorni trascorsi tra le due date: {giorno_due.DayNumber - giorno_uno.DayNumber}");
        }
    }
}
