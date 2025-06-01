using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    public class Map_DoublyLinkedList<TKey, TValue>
    {
        private DoublyLinkedList<TKey, TValue> map = new DoublyLinkedList<TKey, TValue>();

        public void Put(TKey key, TValue value)
        {
            if (map.ContainsKey(key))
                throw new ArgumentException("Key already exists.");
            map.AddLast(key, value);
        }

        public TValue Get(TKey key)
        {
            
            return map.GetValue(key);
        }

        public bool Remove(TKey key)
        {
            return map.Remove(key);
        }

        public override string ToString()
        {
            var keys = map.getKeys();
            var values = map.getValues();
            var sb = new StringBuilder();
            for (int i = 0; i < keys.Count; i++)
            {
                sb.AppendLine($"{keys[i]}: {values[i]}");
            }
            return sb.ToString();
        }
    }

}
