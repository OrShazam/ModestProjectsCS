using System;
using System.Collections.Generic;
namespace NodeExt
{
//a custom node class with fancy methods for list management (or you can just use the List class)
    class NodeExt<T>
    {
        private T value;
        private NodeExt<T> next;
        public NodeExt() {; }
        public NodeExt(T value) { this.value = value; this.next = null; }
        public T GetValue() { return value; }
        public void SetValue(T value) { this.value = value; }
        public NodeExt<T> GetNext() { return next; }
        public void SetNext(NodeExt<T> next) { this.next = next; }
        public bool HasNext()
        {
            return next != null;
        }
        public void ReverseList()
        {
            NodeExt<T> prev = null;
            NodeExt<T> head = new NodeExt<T>(this.value);
            head.SetNext(this.next);
            while (head != null)
            {
                NodeExt<T> next = head.GetNext();
                head.SetNext(prev);
                prev = head;
                head = next;
            }
            this.value = prev.GetValue();
            this.next = prev.GetNext();
        }
        public T HeadValue() { 
            return this.value; 
        }
        public T TailValue()
        {
            return ValueAtI(GetLength() - 1);
        }
        public void DeleteValue(T value)
        {
            NodeExt<T> head = new NodeExt<T>(this.value);
            head.SetNext(this.next);
            dynamic originalNext = head.GetNext();
            while (head != null && EqualityComparer<T>.Default.Equals(head.GetValue(), value))
            {
                head = head.GetNext();
            }
            if (head == null) return;
            head.SetNext(head.GetNext().GetNext());
            this.next = originalNext;
        }
        public void ChangeAtI(int i, T value)
        {
            if (i > GetLength())
                return;
            NodeExt<T> head = new NodeExt<T>(this.value);
            dynamic originalNext = head.GetNext();
            head.SetNext(this.next);
            while ((--i) > 0)
                head = head.GetNext();
            head.SetValue(value);
            this.next = originalNext;
        }
        public T ValueAtI(int i)
        {
            if (i > GetLength())
                return TailValue();
            NodeExt<T> head = new NodeExt<T>(this.value);
            head.SetNext(this.next);
            while ((--i) > 0)
                head = head.GetNext();
            return head.GetValue();
        }
        public void AppendAtI(int i, T value) 
        {
            if (i > GetLength())
                return;
            NodeExt<T> head = new NodeExt<T>(this.value);
            head.SetNext(this.next);
            dynamic originalNext = head.GetNext();
            while ((--i) > 0)
                head = head.GetNext();
            dynamic nextFromI = head.GetNext();
            head.SetNext(new NodeExt<T>(value));
            head.GetNext().SetNext(nextFromI);
            this.next = originalNext;
        }
        public void RemoveAtI(int i)
        {
            NodeExt<T> head = new NodeExt<T>(this.value);
            head.SetNext(this.next);
            dynamic originalNext = head.GetNext();
            while ((--i) > 0)
                head = head.GetNext();
            head.SetNext(head.GetNext().GetNext());
            this.next = originalNext;
        }
        public int GetLength()
        {
            NodeExt<T> head = new NodeExt<T>(this.value);
            head.SetNext(this.next);
            int count = 1;
            while (head.GetNext() != null)
                count++;
            return count;
        }
        public void Append(T value)
        {
            AppendAtI(GetLength() - 1, value);
        }

    } 
    // I'll add later on methods like List slices and IDK
}
