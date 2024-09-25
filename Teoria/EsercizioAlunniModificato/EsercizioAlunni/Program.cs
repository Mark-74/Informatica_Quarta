/*
 * Marco Balducci 4H 20/09/2024
 * Esercizio ripasso Lettura da File e struct
*/

namespace EsercizioAlunni
{
    internal class Program
    {
        class Alunno
        {
            //Dichiarazione proprietà Alunno
            public string Cognome {  get; private set; } 
            public string Nome { get; private set; }
            public char Sesso { get; private set; }
            public DateOnly DataDiNascità { get; private set; }
            public byte Anno { get; private set; }      //es. 1
            public char Classe { get; private set; }    //es. A
            public string Indirizzo { get; private set; }  //es.Informatica

            //Costruttore
            public Alunno(string Cognome, string Nome, char Sesso, DateOnly DataDiNascità, byte Anno, char Classe, string Indirizzo)
            {
                this.Cognome = Cognome;
                this.Nome = Nome;
                this.Sesso = Sesso;
                this.DataDiNascità = DataDiNascità;
                this.Anno = Anno;
                this.Classe = Classe;
                this.Indirizzo = Indirizzo;
            }

            //metodo di formattazione per stampa a video
            override public string ToString() => $"{Cognome}\t{Nome}\t{Sesso}\t{DataDiNascità}\t{Anno}{Classe}\t{Indirizzo}";

            public int CalcolaEtà() => (int)(DateTime.Now - DataDiNascità.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))).TotalDays / 365;
            //public int CalcolaEtà() => (int)(DateTime.Now - DateTime.Parse(DataDiNascità.ToString())).TotalDays / 365;

        }
        static void Main()
        {
            //Istanzio Lista dove verranno salvati gli alunni
            List<Alunno> list = new List<Alunno>();

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
                        //Aggiungo Alunno alla lista, se viene tirato un errore il catch termina la lettura
                        list.Add(new Alunno(lineaLetta[0], lineaLetta[1], lineaLetta[2][0], DateOnly.Parse(lineaLetta[3]), byte.Parse($"{lineaLetta[4][0]}"), lineaLetta[4][1], lineaLetta[5]));
                    } catch 
                    {
                        //Messaggio di Errore e stop lettura
                        Console.WriteLine($"Error reading at line {linea}");
                        sr.Close();
                    }
                }
            }

            //Stampa a video valori letti
            foreach (Alunno studente in list)
            {
                Console.WriteLine(studente);
                Console.WriteLine(studente.CalcolaEtà());
            }

        }
    }
}
