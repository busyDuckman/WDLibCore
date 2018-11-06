#define HIDE_EXCEPTIONS

/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Collections;


namespace WDToolbox.Data.DataStructures
{
/// <summary>
/// A first in, first or last out data storage structure
/// quack (QUeue stACK)
/// </summary>
[Serializable]
	public class Quack<T> : ICollection<T>, IList<T>, IQuack<T>
	{
	#region instance data
	protected List<T> buffer;
	#endregion
	
	#region procedural accessors
	#endregion	
	
	public Quack()
		{
		buffer = new List<T>();
		}
	
	public Quack(int initialSize)
		{
		buffer = new List<T>(initialSize);
		}

	#region IQuack<datumType> Members


	/// <summary>
	/// Remove from begining (fifo) 
	/// </summary>
	/// <returns></returns>
	public object Dequeue()
	{
		#if HIDE_EXCEPTIONS
		if(IsEmpty())
			return null;
		#endif
		
		object item = buffer[0];
		buffer.RemoveAt(0);
		return item;
	}

	/// <summary>
	/// Adds an item to the front of the quack
	/// aka. top od the stack, or head of the queue
	/// Same operation as push
	/// </summary>
	/// <param name="item"></param>
	public void Enqueue(T item)
	{
		Push(item);
	}

	/// <summary>
	/// Returns true if the quack is empty; otherwise false
	/// </summary>
	public bool IsEmpty()
	{
		return (buffer.Count == 0);
	}

	/// <summary>
	/// Peek from the queue perspective.
	/// </summary>
	/// <returns>Next item in queue.</returns>
	public T PeekBottom()
	{
	#if HIDE_EXCEPTIONS
	if(IsEmpty())
		return default(T);
	#endif
	
	return buffer[0];
	}

	/// <summary>
	/// Peek from the stack perspective
	/// </summary>
	/// <returns>Top of stack/</returns>
	public T PeekTop()
	{
		#if HIDE_EXCEPTIONS
		if(IsEmpty())
			return default(T);
		#endif
		
		return buffer[buffer.Count-1];
	}

	/// <summary>
	/// Remove last in
	/// </summary>
	/// <param name="item"></param>
	public T Pop()
	{
		#if HIDE_EXCEPTIONS
		if(IsEmpty())
			return default(T);
		#endif

		T item = buffer[buffer.Count - 1];
		buffer.RemoveAt(buffer.Count-1);
		return item;
	}

	/// <summary>
	/// Adds an item to the front of the quack
	/// aka. top od the stack, or head of the queeue
	/// Same operation as Enqueue
	/// </summary>
	/// <param name="item"></param>
	public void Push(T item)
	{
		buffer.Add(item);
	}

	#endregion	

	#region ICollection<datumType> Members

	public void Add(T item)
	{
		buffer.Add(item);
	}

	public void Clear()
		{
		buffer.Clear();
		}

	public bool Contains(T item)
	{
		return buffer.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		buffer.CopyTo(array, arrayIndex);
	}

	public int Count
	{
		get { return buffer.Count; }
	}

	public bool IsReadOnly
	{
		get { return false; }
	}

	public bool Remove(T item)
	{
		return buffer.Remove(item);
	}

	#endregion

	#region IEnumerable<datumType> Members

	public IEnumerator<T> GetEnumerator()
	{
		return buffer.GetEnumerator();
	}

	#endregion

	#region IEnumerable Members

	IEnumerator IEnumerable.GetEnumerator()
	{
		return buffer.GetEnumerator();
	}

	#endregion

	#region IList<datumType> Members

		public int IndexOf(T item)
		{
			return buffer.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			buffer.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			buffer.RemoveAt(index);
		}

		public T this[int index]
		{
			get => buffer[index];
			set => buffer[index] = value;
		}

		#endregion
	}
}
