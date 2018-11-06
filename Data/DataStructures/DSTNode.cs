/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace WDToolbox.Data.DataStructures
{
    /// <summary>
    /// A "Digital Search Tree" aka Trie. 
    /// TODO: This is not production ready code.
    /// </summary>
    /// <typeparam name="elementType"></typeparam>
    /// <typeparam name="resultType"></typeparam>
    internal struct DSTNode<elementType, resultType> : IDictionary<IList<elementType>, resultType>, IList<resultType>,
        ISerializable
        where resultType : IComparable<resultType>
        where elementType : IComparable<elementType>
    {
        Dictionary<elementType, DSTNode<elementType, resultType>> nodes;
        IList<resultType> result;
        int count;

        public void init()
        {
            nodes = new Dictionary<elementType, DSTNode<elementType, resultType>>();
            result = null;
            count = 0;
        }

        internal bool isValid()
        {
            return true;
        }

        private enum DSTOperation
        {
            find,
            replace,
            remove
        };


        /// <summary>
        /// Performs the DST operation based on a key search.
        /// 
        /// Because the search is the part that often incurs bugs we are only using one search to perform
        /// multiple operations.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="keyPos">The key pos (set to 0).</param>
        /// <param name="operation">The operationto be performed.</param>
        /// <returns>True if the operation succeeded; otherwise false.</returns>
        private bool performDSTOperation(IList<elementType> key, out resultType value, resultType newValue, int keyPos,
            DSTOperation operation)
        {
            int size = key.Count - keyPos;

            if (nodes.ContainsKey(key[keyPos]))
            {
                if (size > 0)
                {
                    bool done = nodes[key[keyPos]].performDSTOperation(key, out value, newValue, keyPos++, operation);

                    if (done && (operation == DSTOperation.remove))
                    {
                        //if a child was removed it may be empty and we may need to prune it.
                        if (nodes[key[keyPos]].isRedyForPruning())
                        {
                            nodes.Remove(key[keyPos]);
                        }
                    }

                    return done;
                }
                else
                {
                    if (result != null)
                    {
                        switch (operation)
                        {
                            case DSTOperation.find:
                                //done
                                break;
                            case DSTOperation.replace:
                                result[0] = newValue;
                                break;
                            case DSTOperation.remove:
                                //use the newValue param for key value pair based removes.
                                if (newValue != null)
                                {
                                    if (result[0].CompareTo(newValue) != 0)
                                    {
                                        value = default(resultType);
                                        return false;
                                    }
                                }

                                result.Clear();
                                result = null;
                                count--;
                                break;
                            default:
                                break;
                        }

                        value = result[0];
                        return true;
                    }
                }
            }

            value = default(resultType);
            return false;
        }


        public bool ItemAtIndex(int index, out KeyValuePair<LinkedList<elementType>, resultType> item)
        {
            LinkedList<elementType> key = new LinkedList<elementType>();
            resultType value;
            if (ItemAtIndex(index, 0, ref key, out value))
            {
                item = new KeyValuePair<LinkedList<elementType>, resultType>(key, value);
                return true;
            }

            item = default(KeyValuePair<LinkedList<elementType>, resultType>);
            return false;
        }


        /// <summary>
        /// Get the item at an index.
        /// NOTE: this should be the only way to map a index to an item.
        /// All operations that are index bound must call this. This insures that there is only one key to item mapping in place.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private bool ItemAtIndex(int index, int count, ref LinkedList<elementType> key, out resultType value)
        {
            int c = count;

            Dictionary<elementType, DSTNode<elementType, resultType>>.KeyCollection keyList = nodes.Keys;
            List<elementType> sortedKeyList = new List<elementType>(keyList);
            //keyList.CopyTo(sortedKeyList, 0);
            sortedKeyList.Sort();

            foreach (elementType keyItem in sortedKeyList)
            {
                DSTNode<elementType, resultType> n = nodes[keyItem];
                if ((c + n.count) >= index)
                {
                    if (count == index)
                    {
                        //found a node whose final child is the one.
                        if (result != null)
                        {
                            value = this.result[0];
                            return true;
                        }
                    }


                    if (ItemAtIndex(index, c, ref key, out value))
                    {
                        key.AddFirst(keyItem);
                        return true;
                    }
                }

                c += n.count;
            }

            value = default(resultType);
            return false;
        }

        /// <summary>
        /// A routine to test if a node is actuly used in the structure.
        /// </summary>
        /// <returns>True id the node needs to be pruned.</returns>
        private bool isRedyForPruning()
        {
            return ((nodes.Count == 0) && (result == null));
        }

        static InvalidOperationException makeNotfundException(int index)
        {
            return new InvalidOperationException(string.Format("The DST did not contain an item at position {0}.",
                index));
        }

        static InvalidOperationException makeNotfundException(string key)
        {
            return new InvalidOperationException(string.Format("The key {0} could not be found in the DST.", key));
        }


        #region IDictionary<IList<elementType>,resultType> Members

        public void Add(IList<elementType> key, resultType value)
        {
            Add(key, value, 0);
        }

        private void Add(IList<elementType> key, resultType value, int keyPos)
        {
            int size = key.Count - keyPos;
            if (size > 0)
            {
                //add the node needed if nessesary
                if (!nodes.ContainsKey(key[0]))
                {
                    DSTNode<elementType, resultType> next = new DSTNode<elementType, resultType>();
                    next.init();
                    nodes.Add(key[0], next);
                }

                nodes[key[0]].Add(key, value, keyPos++);
            }
            else
            {
                if (result == null)
                {
                    result = new List<resultType>();
                    result.Add(value);
                    count++;
                }
                else
                {
                    throw new ArgumentException("An element with the same key already exists in the DST.");
                }
            }
        }

        public bool ContainsKey(IList<elementType> key)
        {
            resultType tempValue;
            return performDSTOperation(key, out tempValue, default(resultType), 0, DSTOperation.find);
        }

        public ICollection<IList<elementType>> Keys
        {
            get { throw new Exception("Operation not supported."); }
        }

        public bool Remove(IList<elementType> key)
        {
            resultType tempValue;
            return performDSTOperation(key, out tempValue, default(resultType), 0, DSTOperation.remove);
        }

        public bool TryGetValue(IList<elementType> key, out resultType value)
        {
            return performDSTOperation(key, out value, default(resultType), 0, DSTOperation.find);
        }

        public ICollection<resultType> Values
        {
            get { throw new Exception("Operation not supported."); }
        }

        public resultType this[IList<elementType> key]
        {
            get
            {
                resultType value;
                if (performDSTOperation(key, out value, default(resultType), 0, DSTOperation.find))
                {
                    return value;
                }

                throw makeNotfundException(key.ToString());
            }
            set
            {
                resultType temp;
                if (!performDSTOperation(key, out temp, value, 0, DSTOperation.replace))
                {
                    throw makeNotfundException(key.ToString());
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<IList<elementType>,resultType>> Members

        public void Add(KeyValuePair<IList<elementType>, resultType> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            if (nodes != null)
            {
                foreach (KeyValuePair<elementType, DSTNode<elementType, resultType>> node in nodes)
                {
                    node.Value.Clear();
                }
            }

            if (result != null)
            {
                result.Clear();
                result = null;
            }
        }

        public bool Contains(KeyValuePair<IList<elementType>, resultType> item)
        {
            resultType tempValue;
            if (performDSTOperation(item.Key, out tempValue, default(resultType), 0, DSTOperation.find))
            {
                return (tempValue.CompareTo(item.Value) == 0);
            }

            return false;
        }

        public void CopyTo(KeyValuePair<IList<elementType>, resultType>[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<IList<elementType>, resultType> item)
        {
            resultType tempValue;
            return performDSTOperation(item.Key, out tempValue, item.Value, 0, DSTOperation.remove);
        }

        #endregion

        #region IEnumerable<KeyValuePair<IList<elementType>,resultType>> Members

        public IEnumerator<KeyValuePair<IList<elementType>, resultType>> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IList<resultType> Members

        public int IndexOf(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public resultType this[int index]
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region ICollection<resultType> Members

        public void Add(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(resultType[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Remove(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<resultType> Members

        IEnumerator<resultType> IEnumerable<resultType>.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}