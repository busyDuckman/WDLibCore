/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System.Collections.Generic;

namespace WDToolbox
{
    public static class StackExtension
    {
        /// <summary>
        /// Pushes a set of items on a stack, in the order they are enumerated.
        /// </summary>
        public static void PushAll<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                stack.Push(item);
            }
        }
    }
}
