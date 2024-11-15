/*
 * Marco Balducci 4H 2024-11-15 
 * Classi Grafo diretto e indiretto che verranno utilizzate dall'implementazione di Dijkstra fornita
 * L'esercizio prevede l'utilizzo di classi derivate, metodi virtuali e metodi override
*/

namespace Grafi
{
    public class GrafoDiretto
    {
        public struct Arco
        {
            //rappresenta un collegamento pesato tra due nodi
            public int nodo_partenza;
            public int nodo_arrivo;
            public int costo;

            public Arco(int start, int end, int cost)
            {
                //costruttore, permette di velocizzare la creazione di archi successivamente
                nodo_partenza = start;
                nodo_arrivo = end;
                costo = cost;
            }
        }

        public int NumeroNodi { get; private set; } // proprietà che torna il numero attuale di nodi inseriti

        private List<Arco> archi;  //contiene archi aggiunti
        private List<string> nodi; //contiene i nodi aggiunti

        public GrafoDiretto(int numero_max_nodi = 1000)
        {
            //costruisce la classe, è garantito che non si supererà numero_max_nodi

            archi = new List<Arco>();
            nodi = new List<string>(numero_max_nodi);
        }

        #region metodi nodi

        public bool ControllaIndiceNodo(int nodo)
        {
            //controlla che il nodo alla posizione [nodo] esista
            return nodi[nodo] != string.Empty;
        }

        public void AggiungiNodo(string nome) // aggiunge un nodo
        {
            //controllo che il numero dei nodi sia minore di quelli massimi
            if (nodi.Capacity == NumeroNodi)
                throw new Exception("Reached maximum number of nodes");

            //controllo che il nome sia valido e che non sia già presente
            if (nome == string.Empty) throw new ArgumentException("An empty string is not a valid name");
            if (nodi.IndexOf(nome) != -1) throw new ArgumentException("Cannot add a node that already exists");

            //aggiungo nodo alla lista e aggiorno contatore
            nodi.Add(nome);
            NumeroNodi++;
        }

        #endregion

        #region metodi archi
        public virtual void AggiungiArco(Arco arco)
        {
            //controllo che il percorso tracciato dall'arco abbia senso (partenza e arrivo devono essere diversi ed esistere)
            if (!ControllaIndiceNodo(arco.nodo_partenza)) throw new ArgumentException("Start node does not exist");
            if (!ControllaIndiceNodo(arco.nodo_arrivo)) throw new ArgumentException("Ending node does not exist");
            if (arco.nodo_partenza == arco.nodo_arrivo) throw new ArgumentException("Cannot add an edge that starts and ends in the same node");

            archi.Add(arco);
        }
        public virtual void AggiungiArco(string nome_nodo_partenza, string nome_nodo_arrivo, int costo) // aggiunge un arco fra i due nodi dati, con il costo indicato
        {
            //richiamo altro metodo che contiene già i controlli
            AggiungiArco(new Arco(this[nome_nodo_partenza], this[nome_nodo_arrivo], costo));
        }

        #endregion

        #region indicizzatori

        public string this[int nodo] // indicizzatore per i nodi, restituisce il nome del nodo partendo dall'indice nella lista
        {
            get { return nodi[nodo]; }
        }
        public int this[string nome_nodo] // indicizzatore per i nodi, restituisce l'indice nella lista partendo dal nome
        {
            get { return nodi.IndexOf(nome_nodo); }
        }

        #endregion

        #region IEnumerable

        public IEnumerable<Arco> ArchiUscenti(int nodo)  // enumeratore per gli archi uscenti dal nodo dato
        {
            //calcolo archi uscenti
            foreach (Arco arco in archi)
                if (arco.nodo_partenza == nodo)
                    yield return arco;

        }
        public IEnumerable<Arco> ArchiEntranti(int nodo)  // enumeratore per gli archi entranti nell nodo dato
        {
            //calcolo archi entranti
            foreach (Arco arco in archi)
                if (arco.nodo_arrivo == nodo)
                    yield return arco;
        }

        #endregion
    }



    public class GrafoNonDiretto : GrafoDiretto  // specializzazione di GrafoDiretto per grafi "non diretti" (se si può andare da A a B con costo 10, allora si va acnhe da B ad A con lo stesso costo)
    {
        public GrafoNonDiretto(int numero_max_nodi = 1000) : base(numero_max_nodi) { } //costruttore della classe, non modifica nulla dal metodo base.

        #region metodi archi
        public override void AggiungiArco(string nome_nodo_partenza, string nome_nodo_arrivo, int costo) // aggiunge un arco fra i due nodi dati, con il costo indicato
        {
            //richiamo altro metodo che contiene già i controlli
            AggiungiArco(new Arco(this[nome_nodo_partenza], this[nome_nodo_arrivo], costo));
        }
        public override void AggiungiArco(Arco arco)  // aggiunge un arco fra i due nodi dati, con il costo indicato
        {
            //aggiungo arco in una direzione
            base.AggiungiArco(arco);
            //aggiungo arco nell'altra direzione
            base.AggiungiArco(new Arco(arco.nodo_arrivo, arco.nodo_partenza, arco.costo));

            //Il metodo AggiungiArco della classe base esegue già i controlli, per questo richiamandolo evito di farli anche in questo ridefinizione
        }

        #endregion
    }
}