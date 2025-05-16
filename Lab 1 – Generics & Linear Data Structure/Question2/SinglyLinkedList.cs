using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question2
{
   public class SinglyLinkedList<T>
    {

        private int size { get; set; } = 0;
       private Node head { get; set; } = null;
      private  Node tail { get; set; } = null;

        private class Node
        {


          public T data { get; set; }
          public Node  next { get; set; }
            public Node(T data)
            {
                this.data = data;
            }




        }

        public void Add(T data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.next = newNode;
                tail = newNode;
            }
            size++;
        }


        public void InsertAtEnd(T data)
        {
            this.Add(data);
        }



        public void InsertAtHead(T data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                newNode.next = head;
                head = newNode;
            }
            size++;
        }


        public void printAll()
        {
            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.data);
                current = current.next;
            }
            Console.WriteLine();
        }



       




    }
}
