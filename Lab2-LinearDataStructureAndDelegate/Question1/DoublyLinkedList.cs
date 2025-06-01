using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    public class DoublyLinkedListNode<TKey, TValue>
    {
        private TKey key;
        private TValue value;
        private DoublyLinkedListNode<TKey, TValue> prev;
        private DoublyLinkedListNode<TKey, TValue> next;

        public DoublyLinkedListNode(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey getKey()
        {
            return key;
        }
        public DoublyLinkedListNode<TKey, TValue> getNext()
        {
            return next;
        }

  
        public DoublyLinkedListNode<TKey, TValue> getPrev()
        {
            return prev;
        }

        public TValue getValue()
        {
            return value;
        }

        public void setKey(TKey key)
        {
            this.key = key;
        }


        public void setValue(TValue value)
        {
            this.value = value;
        }

        public void setPrev(DoublyLinkedListNode<TKey, TValue> node)
        {
            this.prev = node;
        }

        public void setNext(DoublyLinkedListNode<TKey, TValue> node)
        {
            this.next = node;
        }
    }

 
    public class DoublyLinkedList<TKey, TValue>
    {
        private DoublyLinkedListNode<TKey, TValue> head;
        private DoublyLinkedListNode<TKey, TValue> tail;
        public int Size { get; private set; } = 0;


        public DoublyLinkedList() { }

        public DoublyLinkedList(List<TKey> keys, List<TValue> values)
        {
            if (keys.Count != values.Count)
            {
                throw new ArgumentException("Keys and values must have the same number of elements.");
            }

            for (int i = 0; i < keys.Count; i++)
            {
                AddLast(keys[i], values[i]);
            }
        }

        public void AddFirst(TKey key, TValue value)
        {
            var node = new DoublyLinkedListNode<TKey, TValue>(key, value);
            if (head == null)
            {
                head = tail = node;
            }
            else
            {
                node.setNext(head);
                head.setPrev(node);
                head = node;
            }
            Size++;
        }

        public void AddLast(TKey key, TValue value)
        {
            var node = new DoublyLinkedListNode<TKey, TValue>(key, value);
            if (tail == null)
            {
                head = tail = node;
            }
            else
            {
                tail.setNext(node);
                node.setPrev(tail);
                tail = node;
            }
            Size++;
        }

        public TValue First()
        {
            if (IsEmpty()) throw new InvalidOperationException("List is empty");
            return head.getValue();
        }

        public TValue Last()
        {
            if (IsEmpty()) throw new InvalidOperationException("List is empty");
            return tail.getValue();
        }

        public bool IsEmpty()
        {
            return Size == 0;
        }

        public bool Remove(TKey key)
        {
            var current = head;
            while (current != null)
            {
                if (current.getKey().Equals(key))
                {
                    if (current.getPrev() != null)
                        current.getPrev().setNext(current.getNext());
                    else
                        head = current.getNext();

                    if (current.getNext() != null)
                        current.getNext().setPrev(current.getPrev());
                    else
                        tail = current.getPrev();

                    Size--;
                    return true;
                }
                current = current.getNext();
            }
            return false;
        }

        public List<TKey> getKeys()
        {
            List<TKey> keys = new List<TKey>();
            var current = head;
            while (current != null)
            {
                keys.Add(current.getKey());
                current = current.getNext();
            }
            return keys;
        }

        public List<TValue> getValues()
        {
            List<TValue> values = new List<TValue>();
            var current = head;
            while (current != null)
            {
                values.Add(current.getValue());
                current = current.getNext();
            }
            return values;
        }

        public TValue GetValue(TKey key)
        {
            var current = head;
            while (current != null)
            {
                if (current.getKey().Equals(key))
                    return current.getValue();
                current = current.getNext();
            }
            throw new KeyNotFoundException("Key not found.");
        }

        public bool ContainsKey(TKey key)
        {
            var current = head;
            while (current != null)
            {
                if (current.getKey().Equals(key))
                    return true;
                current = current.getNext();
            }
            return false;
        }
    }
}
