/*
 * Marco Balducci 4H 2024-10-29 
 * Programma per la gestione dei prestiti di una banca, con ereditarietà e cardinalità
*/

using System.ComponentModel;

namespace EsercizioBanca
{
    class Banca 
    {
        //Classe Banca che gestisce una lista di clienti ed i relativi prestiti.

        private List<Cliente> clienti;

        //Costruttore della classe Banca
        public Banca() => clienti = new List<Cliente>();

        #region Metodi per clienti
        public void AddCliente(Cliente cliente) => clienti.Add(cliente);
        public void RemoveCliente(string CodiceFiscale)
        {
            for(int i = 0; i < clienti.Count; i++)
            {
                if (clienti[i].CodiceFiscale == CodiceFiscale)
                {
                    clienti.RemoveAt(i);
                    return;
                }
            }
        }
        public Cliente? SearchCliente(string codiceFiscale)
        {
            foreach(Cliente cliente in clienti)
            {
                if(cliente.CodiceFiscale == codiceFiscale)
                    return cliente;
            }

            //Se il cliente non è presente nella lista, restituisce null
            return null;
        }

        public void StampaClienti()
        {
            foreach(Cliente cliente in clienti)
                cliente.StampaCliente();
        }

        #endregion

        #region Metodi per prestiti
        public void AddPrestito(PrestitoSemplice prestito)
        {
            foreach(Cliente cliente in clienti)
            {
                if(cliente.CodiceFiscale == prestito.CodiceFiscale)
                {
                    cliente.AddPrestito(prestito);
                    return;
                }   
            }

            //Se il cliente non è presente nella lista, lancia un'eccezione
            throw new Exception("Cliente non trovato");
        }

        public List<PrestitoSemplice> SearchPrestiti(string codiceFiscale)
        {
            foreach(Cliente cliente in clienti)
            {
                if(cliente.CodiceFiscale == codiceFiscale) 
                    return cliente.GetPrestiti();
            }

            //Se il cliente non è presente nella lista, lancia un'eccezione
            throw new Exception("Cliente non trovato");
        }

        public double TotalePrestiti()
        {
            double totale = 0;
            foreach(Cliente cliente in clienti)
            {
                totale += cliente.GetPrestiti().Count;
            }

            return totale;
        }

        #endregion
    }

    class Cliente
    {
        //Classe che rappresenta un cliente della banca

        //Proprietà della classe
        private List<PrestitoSemplice> prestiti;
        public string Nome { get; private set; }
        public string Cognome {  get; private set; }
        public string CodiceFiscale { get; private set; }
        public double Stipendio { get; private set; }

        //Costruttore della classe
        public Cliente(string nome, string cognome, string codiceFiscale, double stipendio)
        {
            Nome = nome;
            Cognome = cognome;
            CodiceFiscale = codiceFiscale;
            Stipendio = stipendio;
            prestiti = new List<PrestitoSemplice>();
        }

        #region metodi per prestiti
        public void AddPrestito(PrestitoSemplice prestito) => prestiti.Add(prestito);
        public List<PrestitoSemplice> GetPrestiti() => prestiti.GetRange(0, prestiti.Count);
        
        #endregion

        public void StampaCliente() => Console.WriteLine($"{Nome} {Cognome} - {CodiceFiscale} - {Stipendio} euro");

    }

    class PrestitoSemplice
    {
        //Classe che rappresenta un prestito semplice con interesse semplice

        //Proprietà della classe
        public double Capitale { get; private set; }
        public double Interesse { get; private set; }
        public DateOnly DataInizio { get; private set; }
        public DateOnly DataFine { get; private set; }
        public string CodiceFiscale { get; private set; }

        //Costruttore della classe
        public PrestitoSemplice(double capitale, double interesse, DateOnly dataInizio, DateOnly dataFine, string codiceFiscale)
        {
            Capitale = capitale;
            Interesse = interesse;
            DataInizio = dataInizio;
            DataFine = dataFine;
            CodiceFiscale = codiceFiscale;
        }

        //restituisce la rata mensile del prestito
        public double Rata() => Montante() / Durata() / 12;

        //restituisce il montante da restituire al termine del prestito
        public virtual double Montante() => Capitale * (1 + Durata() * Interesse);

        //restituisce la durata del prestito in anni
        public double Durata() => (DataFine.DayNumber - DataInizio.DayNumber) / 365;

        public void StampaPrestito() => Console.WriteLine($"Durata prestito: {Durata():0.00} ({DataInizio}, {DataFine}), Capitale da restituire: {Capitale}, Interesse annuale: {Interesse}, Montante: {Montante():0.00}, Rata mensile: {Rata():0.00}.");
    }

    class PrestitoComposto : PrestitoSemplice
    {
        //Classe derivata da PrestitoSemplice che calcola il montante con l'interesse composto invece che quello semplice
        public PrestitoComposto(double capitale, double interesse, DateOnly dataInizio, DateOnly dataFine, string codiceFiscale)
            : base(capitale, interesse, dataInizio, dataFine, codiceFiscale) { }

        public override double Montante() => Capitale * Math.Pow(1 + Interesse, Durata());
    }

        internal class Program
    {
        static void Main(string[] args)
        {
            Banca banca = new Banca();

            Cliente cliente1 = new Cliente("Mario", "Rossi", "MRARSI65S67M126L", 1500);
            Cliente cliente2 = new Cliente("Luca", "Verdi", "LCUVRD65S67M126L", 2000);
            Cliente cliente3 = new Cliente("Giovanni", "Bianchi", "GVNBNC65S67M126L", 2500);
            Cliente cliente4 = new Cliente("Pietro", "Malzone", "MLZPTR07M28A246N", 1742);
            banca.AddCliente(cliente1);
            banca.AddCliente(cliente2);
            banca.AddCliente(cliente3);
            banca.AddCliente(cliente4);

            PrestitoSemplice prestito1 = new PrestitoSemplice(100000, 0.05, DateOnly.Parse("2024-10-29"), DateOnly.Parse("2029-10-29"), "MRARSI65S67M126L");
            PrestitoComposto prestito2 = new PrestitoComposto(150000, 0.05, DateOnly.Parse("2024-10-29"), DateOnly.Parse("2027-10-29"), "LCUVRD65S67M126L");
            PrestitoComposto prestito3 = new PrestitoComposto(25000, 0.05, DateOnly.Parse("2024-10-29"), DateOnly.Parse("2034-10-29"), "MLZPTR07M28A246N");

            banca.AddPrestito(prestito1);
            banca.AddPrestito(prestito2);
            banca.AddPrestito(prestito3);

            banca.StampaClienti();

            Console.WriteLine("\nPrestiti cliente 1:");
            banca.SearchPrestiti("MRARSI65S67M126L")[0].StampaPrestito();
            Console.WriteLine("\nPrestiti cliente 2:");
            banca.SearchPrestiti("LCUVRD65S67M126L")[0].StampaPrestito();
            Console.WriteLine("\nPrestiti cliente 3:");
            banca.SearchPrestiti("MLZPTR07M28A246N")[0].StampaPrestito();
        }
    }
}
