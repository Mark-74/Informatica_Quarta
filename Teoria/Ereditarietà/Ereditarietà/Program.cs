using System.Reflection.Metadata.Ecma335;

namespace Ereditarietà
{
    class Dipendente
    {
        //il campo private non è accessibile dalle classi derivate, si può usare protected per renderle private all'esterno della classe ma accessibili dalle classi derivate.
        private string cognome, nome;
        private string istituto;
        public Dipendente(string cognome, string nome, string istituto)
        {
            this.cognome = cognome;
            this.nome = nome;
            this.istituto = istituto;
        }
        public string Cognome { get { return cognome; } }
        public string Nome { get { return nome; } }
        public string Istituto { get { return istituto; } }

        public virtual string Riassumiti() => $"Sono {Nome} {Cognome} e lavoro all'istituto {Istituto}. ";
        public override string ToString() => Riassumiti();
    }
    class Docente : Dipendente
    {
        private string materia;
        public Docente(string cognome, string nome, string istituto, string materia)
            : base(cognome, nome, istituto)
        {
            this.materia = materia;
        }
        public string Materia { get { return materia; } }

        public override string Riassumiti() => $"Sono {base.Nome} {base.Cognome} e lavoro all'istituto {Istituto} insegnando {Materia}.";
        public override string ToString() => Riassumiti();
    }
    class ATA : Dipendente
    {
        private string ruolo;  // ufficio o mansione prevalente
        public ATA(string cognome, string nome, string istituto, string ruolo)
            : base(cognome, nome, istituto)
        {
            this.ruolo = ruolo;
        }

        public override string Riassumiti() => $"Sono {base.Nome} {base.Cognome} e lavoro all'istituto {Istituto} svolgendo il ruolo {ruolo}.";
        //Questo no! nome è privato nella classe Dipendente
        //public string Riassumiti() => $"Sono {base.nome} {base.cognome} e lavoro all'istituto {istituto} svolgendo il ruolo {ruolo}.";
    }
    class Dirigente : Docente
    {
        public int anno_inizio_dirigenza;
        public Dirigente(string cognome, string nome, string istituto, string ex_materia, int anno_inizio_dirigenza)
            : base(cognome, nome, istituto, ex_materia)
        {
            this.anno_inizio_dirigenza = anno_inizio_dirigenza;
        }

        public override string Riassumiti() => $"Sono {base.Nome} {base.Cognome} e lavoro all'istituto {Istituto} come Dirigente dal {anno_inizio_dirigenza}.";
        public override string ToString() => Riassumiti();
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Dipendente dip_generico = new Dipendente("Pinco", "Pallino", "Pascal");
            Docente docente = new Docente("Molara", "Federico", "Pascal", "Informatica");
            ATA ata = new ATA("Migliori", "Alessandra", "Pascal", "Uff. DSGA");
            //Console.WriteLine(docente.Cognome);
            //Console.WriteLine(docente.Materia);

            Console.WriteLine(dip_generico.Riassumiti());
            Console.WriteLine(docente.Riassumiti());
            Console.WriteLine(ata.Riassumiti());

            Console.WriteLine();

            Dipendente d1 = new Dipendente("Pinco", "Pallino", "Pascal");
            Dipendente d2 = new Docente("Molara", "Federico", "Pascal", "Informatica");
            Dipendente d3 = new ATA("Migliori", "Alessandra", "Pascal", "Uff. DSGA");

            //Console.WriteLine(d2.Cognome);
            /* Questo no! il riferimento è di tipo Dipendente, non può accedere ai campi di Docente
                        Console.WriteLine(d2.Materia);
            */

            //if (d2 is Docente)
            //{
            //    Console.WriteLine(((Docente)d2).Materia);
            //}
            /* Questo no!
                        Docente docente2 = new Dipendente("Pinco", "Pallino", "Pascal"); ;
            */

            //Il metodo che viene chiamato cambia in base al tipo di riferimento.
            Console.WriteLine(d1.Riassumiti());
            Console.WriteLine(d2.Riassumiti());
            Console.WriteLine(d3.Riassumiti());

            Console.WriteLine();
            //dopo aver aggiunto public override string ToString() => Riassumiti();
            Console.WriteLine(d1);
            Console.WriteLine(d2);
            Console.WriteLine(d3);
        }
    }
}
