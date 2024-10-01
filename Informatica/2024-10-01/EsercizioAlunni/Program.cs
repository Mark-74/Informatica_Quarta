/*
 * Marco Balducci 4H 20/09/2024
 * Esercizio ripasso Lettura da File e class
*/

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace EsercizioAlunni
{

    enum Indirizzo
    {
        Informatica,
        Automazione,
        Biotecnologie
    }

    class Istituto
    {
        private static Dictionary<string, Classe> _classi = new Dictionary<string, Classe>();

        //metodi statici per ottenere le classi istanziate in precedenza

        public static bool AlreadyInstantiated(string nomeClasse) => _classi.ContainsKey(nomeClasse.ToUpper());

        public static Classe GetInstance(string nomeClasse) => _classi[nomeClasse.ToUpper()];

        public static List<Classe> GetClassiIstanziate() => _classi.Values.ToList();

        public static void Add(Classe classe) => _classi.Add(classe.NomeClasse, classe);
    }

    class Classe
    {
        public int Anno { get; private set; }
        public char Sezione { get; private set; }
        public string NomeClasse { get { return $"{Anno}{Sezione}"; } }
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

            if (!Istituto.AlreadyInstantiated(NomeClasse))
                Istituto.Add(this);
            else 
                throw new Exception("Classe already instantiated");
        }



        //metodi pubblici

        //questa proprietà permette di indicizzare con le parentesi quadre
        public string this [int indice] { get { return _iscritti[indice].ToString(); } }
        
        public void AddAlunno(Alunno alunno) => _iscritti.Add(alunno);

        public List<Alunno> GetAlunni() => _iscritti.GetRange(0, _iscritti.Count);

        public override string ToString() => $"{NomeClasse} - {Indirizzo}";



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

    }

    class Alunno
    {

        private static int ProssimoNumeroMatricola = 1;
        //Dichiarazione proprietà Alunno
        public string Cognome { get; private set; }
        public string Nome { get; private set; }
        public char Sesso { get; private set; }
        public DateOnly DataDiNascità { get; private set; }

        public int Matricola { get; private set; }

        //Costruttore
        public Alunno(string Cognome, string Nome, char Sesso, DateOnly DataDiNascità)
        {
            this.Cognome = Cognome;
            this.Nome = Nome;
            this.Sesso = Sesso;
            this.DataDiNascità = DataDiNascità;
            this.Matricola = ProssimoNumeroMatricola++;
        }

        //metodo di formattazione per stampa a video
        override public string ToString() => $"{Matricola} - {Cognome}\t{Nome}\t{Sesso}\t{DataDiNascità}";

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
                        Classe classe = Istituto.AlreadyInstantiated($"{lineaLetta[4]}") ? Istituto.GetInstance($"{lineaLetta[4]}") : new Classe(byte.Parse($"{lineaLetta[4][0]}"), lineaLetta[4][1], Classe.ParseIndirizzo(lineaLetta[5]));
                        classe.AddAlunno(new Alunno(lineaLetta[0], lineaLetta[1], lineaLetta[2][0], DateOnly.Parse(lineaLetta[3])));
                    }
                    catch(Exception e) //Aggiungo Alunno alla classe, se viene tirato un errore il catch termina la lettura
                    {
                        //Messaggio di Errore e stop lettura
                        Console.WriteLine(e);
                        Console.WriteLine($"Error reading at line {linea}");
                        sr.Close();
                    }
                }
            }

            foreach (Classe classe in Istituto.GetClassiIstanziate())
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
