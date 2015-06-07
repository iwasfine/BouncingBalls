using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBalls
{
    public class PriorityQueue<T> : IEnumerable<T> //where T : IComparable<T>
    {
        private T[] data;
        private int capacity;
        private Comparison<T> comparison;

        public int Count { get; private set; }

        public PriorityQueue()
        {
            if (typeof(T).GetInterface("IComparable") != null) { initialize(); return; }
            else if (typeof(T).IsInterface && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(IComparable<>)) { initialize(); return; }
            else
            {
                foreach (var i in typeof(T).GetInterfaces())
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IComparable<>)) { initialize(); return; }
            }
            throw new InvalidOperationException("Data is not comparable");
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            initialize();
            comparison += comparer.Compare;
        }

        public PriorityQueue(Comparison<T> comparison)
        {
            initialize();
            this.comparison = comparison;
        }

        public PriorityQueue(IEnumerable<T> collection)
            : this()
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer)
            : this(comparer)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public PriorityQueue(IEnumerable<T> collection, Comparison<T> comparison)
            : this(comparison)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }
        private void initialize()
        {
            capacity = 128;
            data = new T[capacity];
            Count = 0;
        }

        public void Add(T item)
        {
            if (++Count == capacity) resize();
            data[Count] = item;
            swim(Count);
        }

        private void resize()
        {
            T[] newData = new T[2 * capacity];
            for (int i = 0; i < capacity; i++)
                newData[i] = data[i];
            data = newData;
            capacity *= 2;
        }

        public T Poll()
        {
            if (Count == 0) throw new InvalidOperationException("The PriorityQueue is empty!");
            T item = data[1];
            swap(1, Count--);
            sink(1);
            return item;
        }

        public T Peek()
        {
            if (Count == 0) throw new InvalidOperationException("The PriorityQueue is empty!");
            return data[1];
        }

        private void sink(int i)
        {
            while (2 * i <= Count)
            {
                int j = 2 * i;
                if (j < Count && less(j + 1, j)) j++;
                if (less(i, j)) break;
                swap(i, j);
                i = j;
            }
        }

        private void swim(int i)
        {
            while (i > 1 && less(i, i / 2))
            {
                swap(i, i / 2);
                i = i / 2;
            }
        }

        private void swap(int i, int j)
        {
            T temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        private bool less(int i, int j)
        {
            if (comparison == null)
            {
                if (typeof(T).GetInterface("IComparable") != null) return (data[i] as IComparable).CompareTo(data[j]) < 0;
                return (data[i] as IComparable<T>).CompareTo(data[j]) < 0;
            }
            return comparison(data[i], data[j]) < 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 1; i <= Count; i++)
            {
                yield return data[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
