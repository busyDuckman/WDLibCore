/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Data
{

    /// <summary>
    /// Used for crating permutations where you can select one item from every list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <example>
    /// object[] a = new object[] { 1, 2};
    /// object[] b = new object[] { "cat(s)", "dog(s)"};
    /// object[] c = new object[] { "ran", "jumped"};
    /// Cascader<object> cascader = new Cascader<object>(new List<IEnumerable<object>>() { a, b, c });

    /// foreach (var vec in cascader)
    /// {
    ///     Console.WriteLine(vec.ListAll());
    /// }
            
    /// //1, cat(s), ran
    /// //2, cat(s), ran
    /// //1, dog(s), ran
    /// //2, dog(s), ran
    /// //1, cat(s), jumped
    /// //2, cat(s), jumped
    /// //1, dog(s), jumped
    /// //2, dog(s), jumped
    /// </example>
    public class ArrayValuePermuter<T> : IEnumerable<T[]>
    {
        List<IEnumerable<T>> vectors;
        
        
        public ArrayValuePermuter(List<IEnumerable<T>> vectorList)
        {
            this.vectors = vectorList;
        }

        public IEnumerator<T[]> GetEnumerator()
        {
            List<IEnumerator<T>> eArray = (from E in vectors select E.GetEnumerator()).ToList();
            eArray.ForEach(E => E.MoveNext());

            T[] r = new T[vectors.Count];
            for (int i = 0; i < r.Length; i++)
            {
                //eArray[i].MoveNext();
                r[i] = eArray[i].Current;
            }

            bool done = false;

            while (!done)
            {
                r[0] = eArray[0].Current;
                yield return r;

                int index = 0;
                while (!eArray[index].MoveNext())
                {
                    eArray[index].Reset();
                    eArray[index].MoveNext();
                    r[index] = eArray[index].Current;
                    index++;
                    if (index >= eArray.Count)
                    {
                        done = true;
                        break;
                    }
                    r[index] = eArray[index].Current;
                }
                if (!done)
                {
                    r[index] = eArray[index].Current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
