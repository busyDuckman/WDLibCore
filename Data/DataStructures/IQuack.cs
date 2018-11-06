/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

namespace WDToolbox.Data.DataStructures
{
	/// <summary>
	/// Used for the Queue / Stack Data Type.
	/// </summary>
	/// <typeparam name="datumType"></typeparam>
	interface IQuack<datumType>
	{
		/// <summary>
		/// Clears all date in the quack.
		/// </summary>
		void Clear();
		
		/// <summary>
		/// Returns true if this item is in the quack.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool Contains(datumType value);
		
		/// <summary>
		/// Remove from begining (fifo) 
		/// </summary>
		/// <returns></returns>
		object Dequeue();
		
		/// <summary>
		/// Adds an item to the front of the quack
		/// aka. top od the stack, or head of the queeue
		/// Same operation as push
		/// </summary>
		/// <param name="item"></param>
		void Enqueue(datumType item);
		
		/// <summary>
		/// Returns true if the quack is empty; otherwise false
		/// </summary>
		bool IsEmpty();
		
		/// <summary>
		/// Peek from the queue perspective.
		/// </summary>
		/// <returns>Next item in queue.</returns>
		datumType PeekBottom();
		
		/// <summary>
		/// Peek from the stack perspective
		/// </summary>
		/// <returns>Top of stack/</returns>
		datumType PeekTop();
		
		/// <summary>
		/// Remove last in
		/// </summary>
		/// <param name="item"></param>
		datumType Pop();
		
		/// <summary>
		/// Adds an item to the front of the quack
		/// aka. top od the stack, or head of the queeue
		/// Same operation as Enqueue
		/// </summary>
		/// <param name="item"></param>
		void Push(datumType item);
	}
}
