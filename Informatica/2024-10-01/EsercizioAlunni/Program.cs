/*
 * Marco Balducci 4H 20/09/2024
 * Esercizio ripasso Lettura da File e class
*/

using System;
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
        //Dictionary per memorizzare le classi
        private Dictionary<string, Classe> _classi = new Dictionary<string, Classe>();

        //Metodi per accedere al Dictionary
        public bool EsisteClasse(string nomeClasse) => _classi.ContainsKey(nomeClasse.ToUpper());

        public Classe GetClasse(string nomeClasse) => _classi[nomeClasse.ToUpper()];

        public List<Classe> GetClassiI() => _classi.Values.ToList();

        public void AddClasse(Classe classe) => _classi.Add(classe.NomeClasse, classe);
    }

    class Classe
    {
        //variabili private
        private int _anno;
        private char _sezione;
        private List<Alunno> _iscritti;

        //Proprietà
        public int Anno { get { return _anno; } private set { 
                if (value < 1 || value > 5)
                    throw new ArgumentException("Anno non valido"); 

                _anno = value;
            } 
        }
        public char Sezione { get { return _sezione; } private set {
                value = $"{value}".ToUpper()[0];

                if (value < 65 || value > 90)
                    throw new ArgumentException("Sezione non valida");

                _sezione = value;
            } 
        }
        public string NomeClasse { get { return $"{_anno}{_sezione}".ToUpper(); } }
        public Indirizzo Indirizzo { get; private set; }

        //costruttore
        public Classe(byte anno, char sezione, Indirizzo indirizzo)
        {
            Anno = anno;
            Sezione = sezione;
            Indirizzo = indirizzo;
            _iscritti = new List<Alunno>();
        }

        //metodi pubblici

        //questa proprietà permette di indicizzare con le parentesi quadre
        public string this [int indice] { get { return _iscritti[indice].ToString(); } }
        
        public void AddAlunno(Alunno alunno) => _iscritti.Add(alunno);

        public List<Alunno> GetAlunni() => _iscritti.GetRange(0, _iscritti.Count);

        public override string ToString() => $"{NomeClasse} - {Indirizzo}";

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
        static Indirizzo ParseIndirizzo(string value)
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
                    throw new ArgumentException("Indirizzo non valido");
            }
        }

        static void Main()
        {
            Istituto istituto = new Istituto();

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
                        Classe classe;
                        if (istituto.EsisteClasse(lineaLetta[4]))
                        {
                            classe = istituto.GetClasse(lineaLetta[4]);
                        }
                        else
                        {
                            //se non esiste la classe la creo e la aggiungo all'istituto
                            classe = new Classe(byte.Parse($"{lineaLetta[4][0]}"), lineaLetta[4][1], ParseIndirizzo(lineaLetta[5]));
                            istituto.AddClasse(classe);
                        }

                        //aggiungo alunno alla classe
                        classe.AddAlunno(new Alunno(lineaLetta[0], lineaLetta[1], lineaLetta[2][0], DateOnly.Parse(lineaLetta[3])));
                    }
                    catch(Exception e) //Aggiungo Alunno alla classe, se viene tirato un errore il catch termina la lettura
                    {
                        //Messaggio di Errore e stop lettura
                        Console.WriteLine(e);
                        Console.WriteLine($"Errore nella linea numero {linea}, stop lettura");
                        sr.Close();
                    }
                }
            }

            //Stampa a video dei dati letti
            foreach (Classe classe in istituto.GetClassiI())
            {
                Console.WriteLine($"\nClasse {classe}\n");
                foreach (Alunno alunno in classe.GetAlunni())
                    Console.WriteLine(alunno);
            }

        }
    }
}
