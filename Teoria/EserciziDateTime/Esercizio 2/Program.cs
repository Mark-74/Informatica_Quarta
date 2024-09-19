namespace Esercizio_2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            TimeOnly ora;

            Console.Write("Insersci un orario (formato ora:minuti)\n > ");

            while (true)
            {
                String input = Console.ReadLine()!;
                if (TimeOnly.TryParse(input, out ora))
                    break;
                else
                    Console.Write("orario non valido, riprova\n > ");
            }

            TimeOnly inizio = new TimeOnly(16, 0);
            TimeOnly fine = new TimeOnly(22, 0);

            if (ora >= inizio && ora <= fine)
                Console.WriteLine("interno");
            else 
                Console.WriteLine("esterno");


        }
    }
}
