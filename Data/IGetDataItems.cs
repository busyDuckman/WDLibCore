/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.AplicationFramework;

namespace WDToolbox.Data
{
    /// <summary>
    /// This class provides for clone/compare et al., boiler plate to be semi-automated.
    /// A class declares as set of DataNames, and the extension methods provide basic
    /// implementations of deep copy etc.
    /// 
    /// The order of the items returned in DataNames affects the CompareTo method and 
    /// the hashing function.
    /// </summary>
    public interface IGetDataItems
    {
        string[] DataNames { get; }
    }

    public class WrappedDataItems<T> : IGetDataItems
        where T : class
    {
        public string[] DataNames { get; protected set; }


        public WrappedDataItems(T item)
        {

        }
    }


    /// <summary>
    /// This class provides for clone/compare et al., boiler plate to be semi-automated.
    /// A class declares as set of DataNames, and the extension methods provide basic
    /// implementations of deep copy etc.
    /// 
    /// The order of the items returned in DataNames affects the CompareTo method and 
    /// the hashing function.
    /// </summary>
    public static class IGetDataItemsExtension
    {
        public sealed class DataAcessor
        {
            public string Name {get; private set;}
            public Func<object> Get { get; private set; }
            public Action<object> Set { get; private set; }

            public DataAcessor(string name, Func<object> get, Action<object> set)
            {
                Name = name;
                //TODO: optimise this code
                //Get = () => get();
                //Set = S => set(S);

                Get = get;
                Set = set;
            }
        }

        public static ConditionalWeakTable<IGetDataItems, Dictionary<string, DataAcessor>> DataAcessorLut = new ConditionalWeakTable<IGetDataItems, Dictionary<string, DataAcessor>>();

        private static DataAcessor makeDataAcessor(IGetDataItems data, string name)
        {
            //create a new one
            var prop = data.GetType().GetProperties().Where(P => P.Name == name).FirstOrDefault();
            if (prop != null)
            {
                var acc = prop.GetAccessors(true);
                if(acc.Length == 2)
                {
                    var setter = acc.FirstOrDefault(A => A.Name.StartsWith("set", StringComparison.OrdinalIgnoreCase));
                    var getter = acc.FirstOrDefault(A => A.Name.StartsWith("get", StringComparison.OrdinalIgnoreCase));
                    if(getter != null && setter != null)
                    {
                        //TODO: optimise this code (remove Invoke)
                        return new DataAcessor(name, 
                                                () => getter.Invoke(data, null), 
                                                S => setter.Invoke(data, new object[] {S})
                                                );
                    }
                }
            }
            
            throw new ArgumentException(string.Format("Could find no property or field called {0} in {1}", name, data.GetType().Name));

        }
        private static Dictionary<string, DataAcessor> makeDataAccessors(IGetDataItems data)
        {
            Dictionary<string, DataAcessor> Accessors = new Dictionary<string, DataAcessor>();
            foreach (var name in data.DataNames)
            {
                var acc = makeDataAcessor(data, name);
                if (acc != null)
                {
                    Accessors.Add(name, acc);
                }
                else
                {
                    WDAppLog.LogNeverSupposedToBeHere();
                }
            }

            return Accessors;
        }

        private static DataAcessor GetDataAcessor(this IGetDataItems data, string name)
        {
            return data.GetDataAccessors()[name];
        }

        private static Dictionary<string, DataAcessor> GetDataAccessors(this IGetDataItems data)
        {
            //do acessos already exist
            Dictionary<string, DataAcessor> Accessors;
            if (DataAcessorLut.TryGetValue(data, out Accessors))
            {
                return Accessors;
            }
            else
            {
                //make all Accessors for this type and cache
                Accessors = makeDataAccessors(data);
                DataAcessorLut.Add(data, Accessors);

                //return the result
                return Accessors;
            }
        }


        public static object GetDataItem(this IGetDataItems data, string name)
        {
            return data.GetDataAcessor(name).Get();
        }

        public static object[] GetDataItems(this IGetDataItems data)
        {
            return (from string D in data.DataNames select GetDataItem(data, D)).ToArray();
        }

        public static void SetDataItem(this IGetDataItems data, int n, object value)
        {
            data.GetDataAcessor(data.DataNames[n]).Set(value);
        }

        public static void SetDataItem(this IGetDataItems data, string name, object value)
        {
            data.GetDataAcessor(name).Set(value);
        }

        public static int HashDataItems(this IGetDataItems data)
        {
            return Misc.HashItems(data.GetDataItems());
        }

        public static int CompareBytDataItems<T>(this T a, T b)
            where T : IGetDataItems
        {
#if DEBUG
            if(!a.DataNames.SequenceEqual(b.DataNames))
            {
                //I don't see immenent need for handeling situations where DataNames are different for objects of the same type.
                //We need to survives a > b == b > a
                WDAppLog.LogNeverSupposedToBeHere();
                throw new InvalidOperationException();
            }
#endif

            var names = a.DataNames;
            for(int i=0; i < names.Length; i++)
            {
                int comp = 0;

                string name = names[i];
                var aItem = a.GetDataItem(name);
                var bItem = b.GetDataItem(name);

                IComparable aComp = aItem as IComparable;
                IComparable bComp = bItem as IComparable;

                //bit sneeky, if neither implement IComparable then (null > null) and (null M null)
                if (aComp == null)
                {
                    comp = (bComp == null) ? 0 : -1;
                }
                else
                {
                    comp = (bComp == null) ? 1 : comp = aComp.CompareTo(bComp);
                }

                if (comp != 0)
                {
                    return comp;
                }
            }

            return 0;

        }

        /// <summary>
        /// Compares equaity of all items.
        /// Note: Use CompareBytDataItems() == 0 insted, if the IGetDataItems implements IComparable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualBytDataItems<T>(this T a, T b)
            where T : IGetDataItems
        {
#if DEBUG
            if (!a.DataNames.SequenceEqual(b.DataNames))
            {
                //I don't see immenent need for handeling situations where DataNames are different for objects of the same type.
                WDAppLog.LogNeverSupposedToBeHere();
                throw new InvalidOperationException();
            }
#endif

            var names = a.DataNames;
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                var aItem = a.GetDataItem(name);
                var bItem = b.GetDataItem(name);

                //uses a clever trick to catch edge cases, look twice
                if ((!object.ReferenceEquals(aItem, bItem)) && (!aItem.Equals(bItem)))
                {
                    return false;
                }
            }
            return true;
        }

        public static void CloneBytDataItems<T>(this T from, T to)
            where T : IGetDataItems
        {
#if DEBUG
            if (!from.DataNames.SequenceEqual(to.DataNames))
            {
                //I don't see immenent need for handeling situations where DataNames are different for objects of the same type.
                WDAppLog.LogNeverSupposedToBeHere();
                throw new InvalidOperationException();
            }
#endif

            var names = from.DataNames;
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                to.SetDataItem(name, from.GetDataItem(name));
            }
        }


       public static string DataItemsToString<T>(this T item)
       where T : IGetDataItems
        {
            StringBuilder sb = new StringBuilder("{");
            bool first = true;

            foreach (string name in item.DataNames)
            {
                sb.Append(first ? "": ", ");
                sb.Append(name);
                sb.Append("=");
                sb.Append(item.GetDataItem(name));
                first = false;
            }

            sb.Append("}");
            return sb.ToString();
        }

       public static byte[] writeToBytes(this IGetDataItems item)
       {
           using (MemoryStream ms = new MemoryStream())
           using (BinaryWriter bw = new BinaryWriter(ms))
           {
               foreach (string name in item.DataNames)
               {
                   bw.Write(name);
                   //bw.Write(item.GetDataItem(name));
               }

               return ms.ToArray();
           }
       }
    }
}
