/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WDToolbox;

namespace WDToolbox.Maths.Geometry
{
    /// <summary>
    /// There are a few things coomon to maintaining a list of points, this hadle that.
    /// Notablely the list is closed. ie. you can acces an aditional ellement N[C.Count]
    /// whhich holds a duplicate of the first value.
    /// </summary>
    public sealed class PointList : IList<Point2D>, ICloneable
    {
        List<Point2D> Data;

        /// <summary>
        /// Maximum amount of points [inclusive]. (-1 to disable).
        /// </summary>
        public int MaxCount { get; private set; }

        /// <summary>
        /// Minimum amount of points [inclusive] (0 to disable)
        /// </summary>
        public int MinCount { get; private set; }

        private bool CanAdd { get { return (MaxCount >= 0) ? (Count < MaxCount) : true; } }
        private bool CanRemove { get { return Count > Math.Max(MinCount, 0); } }

        [NonSerialized]
        private object lockObj = new object();

        public EventHandler<PointList> OnValueChanged;
        public EventHandler<PointList> OnNumberOfItemsChanged;

        //--------------------------------------------------------------------------------------------------
        // Construtors and factory methods
        //--------------------------------------------------------------------------------------------------
        public PointList() : this(0, -1)
        {
            
        }

        public PointList(int minCout, int maxCount=-1)
        {
            Data = new List<Point2D>();
            MinCount = minCout;
            MaxCount = maxCount;
        }

        public PointList(IList<Point2D> points, int minCout=0, int maxCount = -1)
        {
            Data = new List<Point2D>(points);
            MinCount = minCout;
            MaxCount = maxCount;
        }

        private PointList(PointList another)
        {
            Data =  (List<Point2D>)another.Data.DeepCopy();
            MinCount = another.MinCount;
            MaxCount = another.MaxCount;
        }

        //--------------------------------------------------------------------------------------------------
        // Alteration handeling
        //--------------------------------------------------------------------------------------------------
        private void onValueChanged()
        {
            OnValueChanged.SafeCall(this, this);
        }

        private void onNumberOfItemsChanged()
        {
            OnValueChanged.SafeCall(this, this);
        }

        private void fixListAfterEdit()
        {
            if(Data.Count == 1)
            {
                //First and only item was removed.
                Data.Clear();
            }
            else
            {
                //Just do the copy, checking first is probably slower.
                Data[Data.Count - 1] = Data[0];
            }
        }


        //--------------------------------------------------------------------------------------------------
        // IList<Point2D>
        //--------------------------------------------------------------------------------------------------
        public IEnumerator<Point2D> GetEnumerator()
        {
            //This does not do what it appears to do at first glance.
            //Using the default Data.GetEnumerator() would cause the enumerator to 
            //visit the last duplicated element at the end of the list.
            for (int i=0; i<Count; i++)
            {
                yield return Data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //This does not do what it appears to do at first glance.
            //Using the default Data.GetEnumerator() would cause the enumerator to 
            //visit the last duplicated element at the end of the list.
            for (int i = 0; i < Count; i++)
            {
                yield return Data[i];
            }
        }


        //--------------------------------------------------------------------------------------------------
        // Read
        //--------------------------------------------------------------------------------------------------
        public int Count { get { return (Data.Count > 0) ? (Data.Count - 1) : 0; } }

        public bool IsReadOnly { get { return false; } }

        public bool Contains(Point2D item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(Point2D[] array, int arrayIndex)
        {
            lock (lockObj)
            {
                Data.CopyTo(array, arrayIndex);
            }
        }
        
        public int IndexOf(Point2D item)
        {
            return Data.IndexOf(item);
        }

        //--------------------------------------------------------------------------------------------------
        // Create
        //--------------------------------------------------------------------------------------------------

        public void Insert(int index, Point2D item)
        {
            lock (lockObj)
            {
                if (CanAdd)
                {
                    Data.Insert(index, item);
                    fixListAfterEdit();
                }
            }
            onNumberOfItemsChanged();
        }
        
        public void Add(Point2D item)
        {
            lock (lockObj)
            {
                //NOTE: nothing here upsets the list so fixListAfterEdit(); does not need to be called.
                if (CanAdd)
                {
                    if (Data.Count == 0)
                    {
                        Data.Add(item);
                        Data.Add(item);
                    }
                    else
                    {
                        Data[Data.Count - 1] = item;
                        Data.Add(Data[0]);
                    }
                }
            }
            onNumberOfItemsChanged();
        }

        //--------------------------------------------------------------------------------------------------
        // Update
        //--------------------------------------------------------------------------------------------------

        /// <summary>
        /// Note get is valid for 1 more index value than the length of the array (wrap around).
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Point2D this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                lock (lockObj)
                {
                    return Data[index];
                }
            }

            set
            {
                if (index >= (Data.Count - 1))
                {
                    throw new IndexOutOfRangeException();
                }
                lock (lockObj)
                {
                    Data[index] = value;
                    fixListAfterEdit();
                }
                onValueChanged();

            }
        }

        //--------------------------------------------------------------------------------------------------
        // Destroy
        //--------------------------------------------------------------------------------------------------
        public void Clear()
        {
            lock (lockObj)
            {
                if (MinCount <= 0)
                {
                    Data.Clear();
                }
            }
            onNumberOfItemsChanged();
        }
        public bool Remove(Point2D item)
        {
            bool r;
            lock (lockObj)
            {
                if (CanRemove)
                {
                    r = Data.Remove(item);
                    fixListAfterEdit();
                }
                else
                {
                    r = false;
                }                
            }

            if (r)
            {
                onNumberOfItemsChanged();
            }
            return r;
        }

        public void RemoveAt(int index)
        {
            lock (lockObj)
            {
                if (CanRemove)
                {
                    Data.RemoveAt(index);
                    fixListAfterEdit();
                }
            }
            onNumberOfItemsChanged();
        }

        public object Clone()
        {
            return new PointList(this);
        }
    }
}
