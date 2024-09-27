/*
 * Marco Balducci 4H 20/09/2024
 * Esercizio ripasso Lettura da File e class
*/

using System.Runtime.InteropServices;

namespace EsercizioAlunni
{

    enum Indirizzo
    {
        Informatica,
        Automazione,
        Biotecnologie
    }

    class Classe
    {
        private static Dictionary<string, Classe> istanze = new Dictionary<string, Classe>();
        public int Anno { get; private set; }
        public char Sezione { get; private set; }
        public Indirizzo Indirizzo { get; private set; }
        private List<Alunno> _iscritti;



        //costruttore

        public Classe(byte anno, char sezione, Indirizzo indirizzo)
        {
            //controllo errori parametri
            if (anno < 1 || anno > 5) throw new Exception("Invalid Anno");
            if (sezione < 65 || sezione > 90) //controllo se sezione è valida e maiuscola
            {
                if (sezione < 97 || sezione > 122) //controllo se sezione è valida e minuscola
                    throw new Exception("Invalid char for sezione");
                else
                    sezione = (char)(sezione - 32); //sezione è resa maiuscola se minuscola
            }

            //assegnazioni valori
            Anno = anno;
            Sezione = sezione;
            Indirizzo = indirizzo;
            _iscritti = new List<Alunno>();

            if (AlreadyInstantiated(anno, sezione))
                istanze.Add($"{anno}{sezione}", this);
            else 
                throw new Exception("Classe already instantiated");
        }



        //metodi pubblici
        
        public void AddAlunno(Alunno alunno) => _iscritti.Add(alunno);

        public List<Alunno> GetAlunni() => _iscritti.GetRange(0, _iscritti.Count);

        public string NomeClasse() => $"{Anno}{Sezione}";  //"1A", "1B", ...

        public override string ToString() => $"{Anno}{Sezione} - {Indirizzo}";



        //metodi statici

        public static Indirizzo ParseIndirizzo(string value)
        {
            switch (value.ToLower())
            {
                case "informatica":
                    return Indirizzo.Informatica;

                case "automazione":
                    return Indirizzo.Automazione;

                case "biotecnologie":
                    return Indirizzo.Biotecnologie;

                default:
                    throw new Exception("Invalid Indirizzo");
            }
        }

        //metodi statici per ottenere le classi istanziate in precedenza

        public static bool AlreadyInstantiated(byte anno, char sezione) => istanze.ContainsKey($"{anno}{sezione}".ToUpper());

        public static Classe GetInstance(byte anno, char sezione) => istanze[$"{anno}{sezione}".ToUpper()];

        public static List<Classe> GetClassiIstanziate() => istanze.Values.ToList();
    }

    class Alunno
    {
        //Dichiarazione proprietà Alunno
        public string Cognome { get; private set; }
        public string Nome { get; private set; }
        public char Sesso { get; private set; }
        public DateOnly DataDiNascità { get; private set; }

        //Costruttore
        public Alunno(string Cognome, string Nome, char Sesso, DateOnly DataDiNascità)
        {
            this.Cognome = Cognome;
            this.Nome = Nome;
            this.Sesso = Sesso;
            this.DataDiNascità = DataDiNascità;
        }

        //metodo di formattazione per stampa a video
        override public string ToString() => $"{Cognome}\t{Nome}\t{Sesso}\t{DataDiNascità}";

        public int CalcolaEtà() => (int)(DateTime.Now - DataDiNascità.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))).TotalDays / 365;
        //public int CalcolaEtà() => (int)(DateTime.Now - DateTime.Parse(DataDiNascità.ToString())).TotalDays / 365;

    }
    internal class Program
    {
        static void Main()
        {
            //Istanzio StreamReader per leggere da file
            using (StreamReader sr = new StreamReader(@"..\..\..\elenco-alunni-classi.txt"))
            {
                //Ciclo di lettura con conteggio linea
                for (int linea = 0; !sr.EndOfStream; linea++)
                {
                    //lettura linea
                    string[] lineaLetta = sr.ReadLine()!.Split("\t");
                    try
                    {
                        //controllo iniziale
                        if (lineaLetta.Length != 6) throw new Exception();

                        //ottengo istanza della classe (quella già istanziata se esistente oppure una nuova)
                        Classe classe = Classe.AlreadyInstantiated(byte.Parse($"{lineaLetta[4][0]}"), lineaLetta[4][1]) ? Classe.GetInstance(byte.Parse($"{lineaLetta[4][0]}"), lineaLetta[4][1]) : new Classe(byte.Parse($"{lineaLetta[4][0]}"), lineaLetta[4][1], Classe.ParseIndirizzo(lineaLetta[5]));
                        classe.AddAlunno(new Alunno(lineaLetta[0], lineaLetta[1], lineaLetta[2][0], DateOnly.Parse(lineaLetta[3])));
                    }
                    catch //Aggiungo Alunno alla classe, se viene tirato un errore il catch termina la lettura
                    {
                        //Messaggio di Errore e stop lettura
                        Console.WriteLine($"Error reading at line {linea}");
                        sr.Close();
                    }
                }
            }

            foreach (Classe classe in Classe.GetClassiIstanziate())
            {
                Console.WriteLine($"\nClasse {classe}\n");
                foreach (Alunno alunno in classe.GetAlunni())
                {
                    Console.WriteLine(alunno.ToString());
                }
            }

        }
    }
}
