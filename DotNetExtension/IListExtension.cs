/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !DISABLE_WINFORMS
using System.Windows.Forms;
#endif
using System.ComponentModel;

namespace WDToolbox//.DotNetExtension
{
    public static class IListExtension
    {

#if !DISABLE_WINFORMS
        /// <summary>
        /// Gets a datasource which is populated with this array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public static BindingSource GenerateBindingSourced<T>(this IList<T> list, bool readOnly=false)
        {
            if (list == null)
            {
                return null;
            }

            BindingList<T> bl = new BindingList<T>(list);
            if (readOnly)
            {
                bl.AllowRemove = false;
                bl.AllowNew = false;
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = bl;
            return bs;
        }

#endif


        //--------------------------------------------------------------------------------------------------
        // Random access and shuffling
        //--------------------------------------------------------------------------------------------------
        static Random internalRandomGen = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            Shuffle(list, internalRandomGen);
        }

        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            if ((list == null) || (list.Count <= 0))
            {
                return;
            }

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /*public static T GetRandomItem<T>(this IList<T> list, Random rnd)
        {
            if (list == null)
            {
                return default(T);
            }
            return list[rnd.Next(0, list.Count)];
        }*/

        public static T GetRandomItem<T>(this IReadOnlyList<T> list, Random rnd)
        {
            if (list == null)
            {
                return default(T);
            }
            if (list.Count == 0)
            {
                return default(T);
            }
            return list[rnd.Next(0, list.Count)];
        }

        /*
        public static T GetRandomItem<T>(this IList<T> list)
        {
            return GetRandomItem(list, internalRandomGen);
        }
        */

        //--------------------------------------------------------------------------------------------------
        public static T GetRandomItem<T>(this IReadOnlyList<T> list)
        {
            return GetRandomItem(list, internalRandomGen);
        }

        public static int GetQuickashCode<T>(this IList<T> list, int maxElementsToInspect = 32)
        {
            maxElementsToInspect = Math.Max(1, maxElementsToInspect);
            //we will inspect at most 32 numbers
            int increment = Math.Max(1, list.Count / maxElementsToInspect);
            int hc = list.Count;
            for (int i = 0; i < list.Count; i += increment)
            {
                hc = unchecked(hc * 133331 + list[i].GetHashCode());
            }
            return hc;
        }

        //--------------------------------------------------------------------------------------------------
        // Search
        //--------------------------------------------------------------------------------------------------
        public static int BinarySearch<T>(this IList<T> list, T value, Func<T, T, int> comparer)
        {
            int min = 0;
            int max = list.Count - 1;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                int comparison = comparer(list[mid], value);
                if (comparison == 0)
                {
                    return mid;
                }
                if (comparison < 0)
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid - 1;
                }
            }
            return min;
        }

        //--------------------------------------------------------------------------------------------------
        // Clone (deep copy)
        //--------------------------------------------------------------------------------------------------
        public static List<T> DeepCopy<T>(this List<T> list) 
            where T : ICloneable
        {
            if (list == null)
            {
                return null;
            }

            if (typeof(T).IsValueType)
            {
                return new List<T>(list);
            }
            else
            {
                List<T> list2 = new List<T>(list.Count);
                foreach(T item in list)
                {
                    list2.Add((T)item.Clone());
                }
                return list2;
            }
        }

        public static List<T?> DeepCopy<T>(this List<T?> list)
            where T : struct
        {
            if (list == null)
            {
                return null;
            }

            //sanity checked this, it works.
            return new List<T?>(list);
        }

        public static T[] DeepCopy<T>(this T[] list)
            where T : ICloneable
        {
            if (list == null)
            {
                return null;
            }

            if (typeof(T).IsValueType)
            {
                T[] newList = new T[list.Length];
                Array.Copy(list, newList, list.Length);
                return newList;
            }
            else 
            {
                T[] newList = new T[list.Length];
                for(int i=0; i<list.Length; i++)
                {
                    newList[i] = (T)list[i].Clone();
                }
                return newList;
            }
        }

        public static T[] DeepCopyValues<T>(this T[] list)
            where T : struct
        {
            T[] newList = new T[list.Length];
            Array.Copy(list, newList, list.Length);
            return newList;
        }

        public static T?[] DeepCopy<T>(this T?[] list)
            where T : struct
        {
            T?[] newList = new T?[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                newList[i] = list[i];
            }
            return newList;
        }

        /*
        //test code won't compile.
        List<int?> foo = new List<int?>();
        foo.Add(1);
        foo.Add(null);
        List<int?> bar = foo.CloneToSameListType<List<int?>, int?>();

        public static L CloneToSameListType<T, L>(this L list)
            where T : ICloneable
            where L : IList<T>, new()
        {
            if (typeof(T).IsValueType)
            {
                L newlist = new L();
                foreach (T item in list)
                {
                    newlist.Add(item);
                }
                return newlist;
            }
            else
            {
                L newlist = new L();
                foreach (T item in list)
                {
                    newlist.Add((T)item.Clone());
                }
                return newlist;
            }
        }
        */

        //--------------------------------------------------------------------------------------------------
        // Index
        //--------------------------------------------------------------------------------------------------
        public static bool IsIndexValid<T>(this IList<T> list, int index)
        {
            return ((index >= 0) && (index < list.Count));
        }

        public static T NthOrDefault<T>(this IList<T> list, int index)
        {
            return ((index >= 0) && (index < list.Count)) ? list[index] : default(T);
        }

        /*
        public static bool IsIndexValid(this Array list, int index)
        {
            return ((index >= 0) && (index < list.Length));
        }

        public static T NthOrDefault<T>(this Array list, int index)
        {
            return ((index >= 0) && (index < list.Length)) ? (T)list.GetValue(index) : default(T);
        }*/

        //--------------------------------------------------------------------------------------------------
        // Misc helpers
        //--------------------------------------------------------------------------------------------------
        public static void RemoveAllNulls<T>(this IList<T> list)
            where T : class
        {
            if ((list == null) || (list.Count <= 0))
            {
                return;
            }
            while (list.Remove(null)) ;
        }

        /// <summary>
        /// Returns index of the first item to match predicate; otherwise -1;
        /// </summary>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> isTarget)
        {
            if (list == null)
            {
                return - 1;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (isTarget(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }



        /// <summary>
        /// Compares every item of both arrays using .equals;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool CompareElementsInOrderAndEqual<T>(this IList<T> a, IList<T> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (int i = 0; i < a.Count; i++)
            {
                if(!a[i].Equals(b[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static HashSet<T> ToHashSet<T>(this IList<T> list)
        {
            HashSet<T> set = new HashSet<T>();
            foreach(T item in list)
            {
                set.Add(item);
            }

            return set;
        }

        //--------------------------------------------------------------------------------------------------
        // Queue / Stack type access
        //--------------------------------------------------------------------------------------------------
        public static void PushFront<T>(this IList<T> list, T item)
        {
            if (list.Count == 0)
            {
                list.Add(item);
            }
            else
            {
                list.Insert(0, item);
            }
        }

        public static void PushBack<T>(this IList<T> list, T item)
        {
            list.Add(item);
        }

        public static T PopFront<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("List is empty, can not pop");
            }
            else
            {
                T r = list[0];
                list.RemoveAt(0);
                return r;
            }
        }

        public static T PeekFront<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("List is empty, can not peek");
            }
            else
            {
                return list[0];
            }
        }

        public static T PopBack<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("List is empty, can not pop");
            }
            else
            {
                T r = list[list.Count-1];
                list.RemoveAt(list.Count - 1);
                return r;
            }
        }

        public static T PeekBack<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("List is empty, can not peek");
            }
            else
            {
                return list[list.Count - 1];
            }
        }

        public static void Populate<T>(this T[] list, T value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = value;
            }
        }

        //--------------------------------------------------------------------------------------------------
        // String helpers
        //--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Lists all items as a string using ", " as a seperator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListAll<T>(this IList<T> list)
        {
            return ListAll(list, ", ");
        }

        /// <summary>
        /// Lists all items as a string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="seperator">The  seperator, use null for none.</param>
        /// <returns></returns>
        public static string ListAll<T>(this IList<T> list, string seperator)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append("<NULL>");
                }

                if ((seperator != null) && (i < (list.Count - 1)))
                {
                    sb.Append(seperator);
                }
            }

            return sb.ToString();
        }
    }
}
