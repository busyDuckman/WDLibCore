/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System.Collections.Generic;

namespace WDToolbox.Data.DataStructures
{
    /// <summary>
    /// This is something like a circular buffer.
    /// Once the buffer is full adding a new item drops the oldest item.
    /// Though, apparently I didn't use a circular buffer.
    /// I assume this was to allow linq operations to be easily integrated or something.  
    /// </summary>
    public class SlidingWindow<TYPE> : ISlidingWindow<TYPE>
    {
        protected List<TYPE> _list;
        int _windowSize;

        public SlidingWindow(int windowSize)
        {
            _list = new List<TYPE>();
            this._windowSize = windowSize;
        }

        public int WindowSize
        {
            get { return _windowSize; }
        }

        public double WindowSizeInUse
        {
            get { return _list.Count; }
        }

        public virtual void Next(TYPE value)
        {
            _list.Add(value);
            while (_list.Count > WindowSize)
            {
                _list.RemoveAt(0);
            }
        }

        public TYPE this[int index]
        {
            get { return _list[index]; }
        }

        public void AddAll(IEnumerable<TYPE> values)
        {
            foreach (TYPE value in values)
            {
                Next(value);
            }
        }

    }
}
