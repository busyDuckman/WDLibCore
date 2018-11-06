/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Data.DataStructures
{
    /// <summary>
    /// A List where the IList derived methods can be overriden
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VList<T> : IList<T>
    {
        protected List<T> list;

        public VList()
        {
            list = new List<T>();
        }

        public VList(int capacity)
        {
            list = new List<T>(capacity);
        }

        public VList(IEnumerable<T> collection)
        {
            list = new List<T>(collection);
        }

        //----------------------------------------------------------------------------------------
        // IList<T>
        //----------------------------------------------------------------------------------------
        public virtual int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public virtual T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public virtual void Add(T item)
        {
            list.Add(item);
        }

        public virtual void Clear()
        {
            list.Clear();
        }

        public virtual bool Contains(T item)
        {
            return list.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return list.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            return list.Remove(item);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            //return ((System.Collections.IEnumerable)list).GetEnumerator();
            return this.GetEnumerator();
        }

        //----------------------------------------------------------------------------------------
        // Stuff you would expect, that isn't in the interface
        //----------------------------------------------------------------------------------------
        /// <summary>
        /// This iterativly calls Add.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }
    }
}
