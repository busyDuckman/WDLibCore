/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox
{
    /// <summary>
    /// These extensions make usage of the LinkedList more consise.
    /// </summary>
    public static class LinkedListExtension
    {
        /// <summary>
        /// Pops the last value of the linked list.
        /// </summary>
        /// <returns> Value of the (formerly) last item in the list. </returns>
        /// <exception cref="System.InvalidOperationException"> The LinkedList is empty.</exception>
        public static T Pop<T>(this LinkedList<T> list)
        {
            T item = list.Last.Value;
            list.RemoveLast(); //throws InvalidOperationException
            return item;
        }

        /// <summary>
        /// Dequeues the first value of the linked list.
        /// </summary>
        /// <returns> Value of the (formerly) first item in the list. </returns>
        /// <exception cref="System.InvalidOperationException"> The LinkedList is empty.</exception>
        public static T Dequeue<T>(this LinkedList<T> list)
        {
            T item = list.First.Value;
            list.RemoveFirst(); //throws InvalidOperationException
            return item;
        }

        /// <summary>
        /// Gets the last value of the linked list.
        /// </summary>
        /// <returns> Value of the last item in the list. </returns>
        /// <exception cref="System.InvalidOperationException"> The LinkedList is empty.</exception>
        public static T PeekLast<T>(this LinkedList<T> list)
        {
            if (list.Count <= 0)
            {
                throw new InvalidOperationException("LinkedList is empty (call to PeekLast)");
            }

            T item = list.Last.Value;
            return item;
        }

        /// <summary>
        /// Gets the last value of the linked list.
        /// </summary>
        /// <returns> Value of the last item in the list. </returns>
        /// <exception cref="System.InvalidOperationException"> The LinkedList is empty.</exception>
        public static T PeekFirst<T>(this LinkedList<T> list)
        {
            if (list.Count <= 0)
            {
                throw new InvalidOperationException("LinkedList is empty (call to PeekFirst)");
            }

            T item = list.First.Value;
            return item;
        }

        /// <summary>
        /// Addss items to the end of the list.
        /// </summary>
        /// <param name="items">Items to add.</param>
        public static void AddLastAll<T>(this LinkedList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.AddLast(item);
            }
        }

        /// <summary>
        /// Addss items to the end of the list.
        /// </summary>
        /// <param name="items">Items to add.</param>
        public static void AddFirstAll<T>(this LinkedList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.AddFirst(item);
            }
        }
    }
}
