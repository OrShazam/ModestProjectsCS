using System;
using System.Collections.Generic;

namespace ConsoleApp5
{
//A simple dictionary class
    class MyDictionary<TKey, TVal>
    {
        private List<TKey> keys = new List<TKey>();
        private List<TVal> vals = new List<TVal>();

        public MyDictionary(List<TKey> keys, List<TVal> vals){
            // we can't use the default parameter values sugar syntax here, we'll have to imply polymorphism
            // as default values must be known at compile time and the default value will be a new list of unknown type.
            this.keys = keys; this.vals = vals;
        }
        public MyDictionary() {; } 
        public void AddPair(TKey key, TVal val)
        {
            keys.Add(key); vals.Add(val);
        }

        public TVal GetValueByKey(TKey key)
        {
            int keyIdx = FindKeyIndex(key);
            if (keyIdx == -1)
                throw new SystemException("Key does not exist.");
            else
                return vals[keyIdx]; 
        }
        private int FindKeyIndex(TKey key)
        {
            int count = 0;
            foreach (TKey currKey in keys)
            {
                if (EqualityComparer<TKey>.Default.Equals(currKey,key))
                    return count;
                count++;
            }
            return -1; // in case there's no such key
        }
        public List<TKey> Keys()
        {
            return keys;
        }
        public List<TVal> Values()
        {
            return vals;
        }
        public int Length()
        {
            return keys.Count;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // some sample code
            MyDictionary<int, string> dict = new MyDictionary<int, string>();
            for(int i = 0; i < 10; i++)
            {
                dict.AddPair(i, $"{i}, what a lovely number!");
            }
            for (int i = 0; i < dict.Length(); i++)
            {
                Console.WriteLine(dict.GetValueByKey(i));
            }
            Console.ReadKey();
        }
    }
}