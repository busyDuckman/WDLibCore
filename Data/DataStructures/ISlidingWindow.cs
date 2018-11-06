/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;


namespace WDToolbox.Data.DataStructures
{
    /// <summary>
    /// This is an interface to something like a circular buffer.
    /// Once the buffer is full adding a new item drops the oldest item.
    /// </summary>
    public interface ISlidingWindow<T>
    {
        /// <summary>
        /// Add the next item to the buffer
        /// </summary>
        void Next(T value);
        
        /// <summary>
        /// Access to the buffer
        /// </summary>
        T this[int index] { get; }
        
        /// <summary>
        /// The size of the window/buffer (readonly)
        /// </summary>
        int WindowSize { get; }
        
        /// <summary>
        /// Size of the buffer (will be less than WindowSize, until the buffer is at max size).
        /// </summary>
        double WindowSizeInUse { get; }
        
        /// <summary>
        /// Add a list of values.
        /// </summary>
        /// <param name="values"></param>
        void AddAll(IEnumerable<T> values);
    }
}
