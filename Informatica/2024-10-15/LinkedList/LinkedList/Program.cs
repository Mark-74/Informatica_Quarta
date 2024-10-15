using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LinkedList
{

    class LinkedList
    {
        private class Node
        {
            // Essendo "private" l'intera class, posso usare "public" per i campi
            public int value;
            public Node next;
        }

        private Node head;
        public LinkedList()
        {
            Count = 0;
            head = null;
        }

        public int Count { get; set; }
        public int this[int idx] { get { return getValueAt(idx); } set { getNodeAt(idx).value = value; } }

        public int getValueAt(int idx)
        {
            Node n = getNodeAt(idx);
            return n.value;
        }

        public void Add(int value)
        {
            // Crea il nuovo nodo
            Node n = new Node();
            n.value = value;
            n.next = null;

            if (head == null)  // lista vuota?
            {
                head = n;
            }
            else
            {
                Node prev = head;
                while (prev.next != null)
                    prev = prev.next;

                prev.next = n;
            }

            //aggiorno numero di nodi
            Count++;
        }
        public void RemoveAt(int idx)
        {
            Node n = getNodeAt(idx);
            
            n.next = idx < Count - 1 ? n.next.next : null;
            Count--;
        }
        public void RemoveValue(int value)
        {
            int index = Search(value);
            if (index != -1) 
                RemoveAt(index);
        }
        public int Search(int value)
        {
            int i = 0;
            for (Node curr = head; curr != null; curr = curr.next, ++i)
            {
                if (curr.value == value)
                    return i;
            }

            return -1;
        }
        private Node getNodeAt(int idx)
        {
            if (idx < 0 || idx >= Count)
                throw new IndexOutOfRangeException();

            Node n = head;
            for (int i = 0; i < idx; i++)
                n = n.next;

            return n;
        }
    }
    class ArrayList
    {
        private int[] data;
        private int count;
        public ArrayList(int capacity)
        {
            data = new int[capacity];
            count = 0;
        }

        public int Count { get { return count; } }
        public int this[int idx]
        {
            get
            {
                if (idx < 0 || count < idx)
                    throw new IndexOutOfRangeException();
                return data[idx];
            }
            set
            {
                if (idx < 0 || count < idx)
                    throw new IndexOutOfRangeException();
                data[idx] = value;
            }
        }
        public void Add(int value)
        {
            if (count == data.Length)
                Realloc(2 * data.Length);

            data[count++] = value;
        }
        public void RemoveAt(int idx)
        {
            if (idx < 0 || count < idx)
                throw new IndexOutOfRangeException();
            ShiftLeft(idx);
        }
        public void RemoveValue(int value)
        {
            int idx = Search(value);
            if (idx >= 0)
                RemoveAt(idx);
        }
        public int Search(int value)
        {
            for (int i = 0; i < count; ++i)
            {
                if (data[i] == value)
                    return i;
            }

            return -1;
        }

        // i metodi che seguono sono stati presi da https://classroom.google.com/c/NjI0MDAwODEyNDMx/m/NjcyOTQ3NjgwMjM5/details
        private void Realloc(int new_capacity)
        {
            int[] new_data = new int[new_capacity];
            int idx_max = Math.Min(data.Length, new_data.Length);
            for (int i = 0; i < idx_max; ++i)
                new_data[i] = data[i];
            data = new_data;
        }
        private void ShiftRight(int idx)
        {
            if (idx < 0 || count < idx)
                throw new IndexOutOfRangeException();
            if (count == data.Length)
                Realloc(2 * data.Length);
            int move_count = count - idx; // numero di elementi da spostare
            for (int k = move_count; k > 0; --k)
                data[idx + k] = data[idx + k - 1];
            ++count;
        }
        private void ShiftLeft(int idx)
        {
            if (idx < 0 || count <= idx)
                throw new IndexOutOfRangeException();
            int move_count = count - idx - 1; // numero di elementi da spostare
            for (int k = 0; k < move_count; ++k)
                data[idx + k] = data[idx + k + 1];
            --count;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            LinkedList list = new LinkedList();
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);

            Console.WriteLine(list.Count);
            Console.WriteLine();
            for(int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
            Console.WriteLine();

            list[list.Count - 1] = 0;

            Console.WriteLine(list.Count);
            Console.WriteLine();
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
            Console.WriteLine() ;

            Console.WriteLine(list.Count);
            Console.WriteLine();
            list.RemoveAt(1);
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
        }
    }
}