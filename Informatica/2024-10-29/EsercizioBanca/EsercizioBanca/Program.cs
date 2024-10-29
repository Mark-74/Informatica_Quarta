/*
 * Marco Balducci 4H 2024-10-29 
 * Programma per la gestione dei prestiti di una banca, con ereditarietà e cardinalità
*/

using System.ComponentModel;

namespace EsercizioBanca
{
    class Banca 
    {
        private List<Cliente> clienti;

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

            return null;
        }
        public 
    }

    class Cliente
    {
        private List<PrestitoSemplice> prestiti;
        public string Nome { get; private set; }
        public string Cognome {  get; private set; }
        public string CodiceFiscale { get; private set; }
        public double Stipendio { get; private set; }

        public Cliente(string nome, string cognome, string codiceFiscale, double stipendio)
        {
            Nome = nome;
            Cognome = cognome;
            CodiceFiscale = codiceFiscale;
            Stipendio = stipendio;
        }

        public void StampaCliente() => Console.WriteLine($"{Nome} {Cognome} - {CodiceFiscale} - {Stipendio}");

    }

    class PrestitoSemplice
    {
        public double Capitale { get; private set; }
        public double Interesse { get; private set; }
        public DateOnly DataInizio { get; private set; }
        public DateOnly DataFine { get; private set; }
        public string CodiceFiscale { get; private set; }

        public PrestitoSemplice(double capitale, double interesse, DateOnly dataInizio, DateOnly dataFine, string codiceFiscale)
        {
            Capitale = capitale;
            Interesse = interesse;
            DataInizio = dataInizio;
            DataFine = dataFine;
            CodiceFiscale = codiceFiscale;
        }
        public double Rata()
        {

        }

        public virtual double Montante()
        {
            return Capitale * (1 + );
        }

        public double Durata()
        {
            
            return (DataFine.DayNumber - DataInizio.DayNumber) / 30;
        }

        public void StampaPrestito() => Console.WriteLine();
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
