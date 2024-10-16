/*
 * Marco balducci 4H 15/10/2024
 * implementazione di Linked List
*/


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
            public Node? next;
        }

        private Node? head;
        public LinkedList()
        {
            Count = 0;
            head = null;
        }

        public int Count { get; set; }
        public int this[int idx] { get { return getValueAt(idx); } set { getNodeAt(idx).value = value; } }

        public int getValueAt(int idx)
        {
            if (head == null) throw new IndexOutOfRangeException();

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
            if (head == null) return;

            if (idx != 0)    //Caso 1: non devo rimuovere il primo nodo (head)
            {
                Node n = getNodeAt(idx - 1);
                n.next = idx < Count - 1 ? n.next.next : null;
            } 
            else            //caso 2: devo rimuovere la head
            {
                head = head!.next;
            }

            Count--;
        }
        public void RemoveValue(int value)
        {
            if (head == null) return;

            int index = Search(value);
            if (index != -1) 
                RemoveAt(index);
        }

        public void RemoveAllValue(int value)
        {
            if(head ==  null) return;

            bool end = false;
            // O(n)
            for(Node n = head!; !end && n.next != null; n = n.next)
            {
                while(n.next!.value == value)
                {
                    Count--;
                    if (n.next.next != null)
                        n.next = n.next.next;
                    else //fine LinkedList
                    {
                        n.next = null;
                        end = true; 
                        break;
                    }
                }
            }

            if (head!.value == value)
            {
                head = head.next;
                Count--;
            }

            /* // O(n^2)
            for (int i = Search(value); i != -1; i = Search(value))
                RemoveAt(i);
            */
        }

        public void RemoveAfter(int idx)
        {
            getNodeAt(idx).next = null;
            Count = idx + 1;
        }

        public void Reset()
        {
            Count = 0;
            head = null; 
        }

        public int Search(int value)
        {
            if (head == null) throw new IndexOutOfRangeException();

            int i = 0;
            for (Node curr = head!; curr != null; curr = curr.next!, ++i)
            {
                if (curr.value == value)
                    return i;
            }

            return -1;
        }
        private Node getNodeAt(int idx)
        {
            if (head == null) throw new IndexOutOfRangeException();

            if (idx < 0 || idx >= Count)
                throw new IndexOutOfRangeException();

            Node n = head!;
            for (int i = 0; i < idx; i++)
                n = n.next!;

            return n;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            LinkedList list = new LinkedList();

            list.Add(4);
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(4);
            list.Add(4);
            list.Add(4);
            list.Add(4);
            list.Add(4);
            list.Add(4);
            list.Add(4);
            list.Add(4);

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

            list.RemoveAt(1);
            list.RemoveValue(0);
            Console.WriteLine(list.Count);
            Console.WriteLine();
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
            Console.WriteLine();

            list.RemoveAllValue(4);

            Console.WriteLine(list.Count);
            Console.WriteLine();
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
            Console.WriteLine();

            list.RemoveAfter(1);

            Console.WriteLine(list.Count);
            Console.WriteLine();
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
            Console.WriteLine();

            list.Reset();
            Console.WriteLine(list.Count);
            Console.WriteLine();
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine($"{list[i]} - {i}");
        }
    }
}