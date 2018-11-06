/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using WDToolbox.AplicationFramework;
using WDToolbox.Data.DataStructures;
using WDToolbox;

namespace WDToolbox
{
    public enum NodeVisitOrder { DepthFirstPreOrder, DepthFirstPostOrder, BredthFirst }
    public enum CircularRefernceBehaviour { DontCheck, ThrowException, Skip }
    public class NodeTraversalException : Exception
    {
        public NodeTraversalException(string message) : base(message) { }
        public NodeTraversalException() : base() { }
        public static NodeTraversalException VisitedTwice(object item) { return new NodeTraversalException("Node already visied: " + item.ToString()); }
    }

    public static class Misc
    {
        public static string LoremIpsum { get { return "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."; } }

        private static string[] trueStrings = new string[] { "1", "true", "yes" };
        private static string[] falseStrings = new string[] { "0", "false", "no" };
        public static bool DoesStringMeanTrue(string s)
        {
            if (s == null)
            {
                return false;
            }
            return trueStrings.Contains(s.ToLower().Trim());
        }
        public static bool DoesStringMeanFalse(string s)
        {
            if (s == null)
            {
                return false;
            }
            return falseStrings.Contains(s.ToLower().Trim());
        }

        public static T CloneIfPossible<T>(T item)
            where T : class
        {
            if (item == null)
            {
                return null;
            }
            else if (item is ICloneable)
            {
                return (T)((ICloneable)item).Clone();
            }
            else
            {
                return item;
            }
        }


        private const uint FnvPrime = unchecked(16777619);
        private const uint FnvOffsetBasis = unchecked(2166136261);
        public static int HashItems(params object[] items)
        {
            unchecked
            {
                uint hash = FnvOffsetBasis;
                byte[] bytes = new byte[4];

                for (var i = 0; i < items.Length; i++)
                {

                    //get items hash code, as four bytes
                    int itemHash = items[i].GetHashCode();
                    bytes[0] = (byte)(itemHash >> 24);
                    bytes[1] = (byte)(itemHash >> 16);
                    bytes[2] = (byte)(itemHash >> 8);
                    bytes[3] = (byte)(itemHash);

                    //apply basic hashing over those bytes
                    for (int j = 0; j < 4; j++)
                    {
                        hash ^= bytes[i];
                        hash *= FnvPrime;
                    }
                }

                return (int)hash;
            }
        }

        /// <summary>
        /// A predictable hash method, constant  across systems
        /// </summary>
        public static int HashBytesPredictably(byte[] bytes)
        {
            return HashBytesPredictably(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// A predictable hash method, constant  across systems
        /// </summary>
        public static int HashBytesPredictably(byte[] bytes, int start, int len)
        {
            unchecked
            {
                uint hash = FnvOffsetBasis;

                for (var i = 0; i < bytes.Length; i++)
                {
                    hash ^= bytes[i];
                    hash *= FnvPrime;
                }

                return (int)hash;
            }
        }

        public static bool Retry(Func<bool> task, int attempts, int delayBetweenAttemptsMS = 0,
                                [CallerLineNumber] int line = 0,
                                [CallerFilePath] string file = "",
                                [CallerMemberName] string member = "")
        {
            int attempt = 0;
            bool lastAttmpt = false;
            string methodName = "?";
            if ((task != null) && (task.Method != null) && (!string.IsNullOrWhiteSpace(task.Method.Name)))
            {
                methodName = task.Method.Name;
            }

            if (delayBetweenAttemptsMS < 0)
            {
                delayBetweenAttemptsMS = 0;
            }

            do
            {
                lastAttmpt = attempt >= (attempts - 1);
                bool result = false;
                string explenation = "Task failed";

                try
                {
                    result = task();
                }
                catch (Exception ex)
                {
                    explenation = string.Format("Exception thrown [{0}]", ex.GetADecentExplination());
                }

                if (result)
                {
                    return true;
                }

                if (lastAttmpt)
                {
                    string msg = string.Format("Could not perform task ({0}) after {1} attempts. {2}",
                                                methodName, attempts, explenation);

                    WDAppLog.logError(ErrorLevel.Error, msg, line, file, member);
                }
                else
                {
                    string msg = string.Format("Attempt ({1} of {2}) to perform task ({0}) failed. {3}",
                                                methodName, attempt + 1, attempts, explenation);

                    WDAppLog.logError(ErrorLevel.Warning, msg, line, file, member);
                }

                //continue loop
                Thread.Sleep(delayBetweenAttemptsMS);
                attempt++;
            } while (!lastAttmpt);

            return false;
        }


        public static Why Retry(Func<Why> task, int attempts, int delayBetweenAttemptsMS = 0)
        {
            int attempt = 0;
            bool lastAttmpt = false;
            Why result = false;
            string methodName = "?";
            if ((task != null) && (task.Method != null) && (!string.IsNullOrWhiteSpace(task.Method.Name)))
            {
                methodName = task.Method.Name;
            }

            if (delayBetweenAttemptsMS < 0)
            {
                delayBetweenAttemptsMS = 0;
            }

            do
            {
                lastAttmpt = attempt >= attempts;
                string explenation = "Task failed";

                try
                {
                    result = task();
                }
                catch (Exception ex)
                {
                    explenation = string.Format("Exception thrown [{0}]", ex.GetADecentExplination());
                }

                if (result)
                {
                    return result;
                }
                else if (string.IsNullOrWhiteSpace(result.Reason))
                {
                    explenation = string.Format("Task failed [{0}]", result.Reason);
                }

                if (lastAttmpt)
                {
                    string msg = string.Format("Could not perform task ({0}) after {1} attempts. {2}",
                                                methodName, attempts, explenation);

                    WDAppLog.logError(ErrorLevel.Error, msg);
                }
                else
                {
                    string msg = string.Format("Attempt ({1} of {2}) to perform task ({0}) failed. {3}",
                                                methodName, attempt, attempts, explenation);

                    WDAppLog.logError(ErrorLevel.Warning, msg);
                }

                //continue loop
                Thread.Sleep(delayBetweenAttemptsMS);
                attempt++;
            } while (!lastAttmpt);

            return result;
        }

        /// <summary>
        /// One line of code solution to catching and re-throwing with an inner exception
        /// </summary>
        /// <param name="action">What to do (in a try block)</param>
        /// <param name="CreateOuterException">New exception, from inner exception. </param>
        public static void WrapException(Action action, Func<Exception, Exception> CreateOuterException)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (CreateOuterException != null)
                {
                    throw CreateOuterException(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }


        public static void Dump(string what, string title = "Dump")
        {
#if DEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(title + " - " + DateTime.Now.ToString());
            sb.AppendLine();
            sb.Append(what);
            File.WriteAllText(@"c:\temp\dump.txt", sb.ToString());
#endif
        }

        /// <summary>
        /// Enumerates any Tree/graph in a non-recursive manor.
        /// Does not check for circularReferences.
        /// </summary>
        /// <param name="node">Root node.</param>
        /// <param name="getSubNodes"> Get the sub-nodes of any given node.</param>
        /// <param name="order">The visit order.</param>
        /// <param name="checkType">If circular references should be checked, and how to handle them. 
        /// Note (1): Checks repeated node, which is not nesesiarly a circular reference (but all circulare references have a repeated node).
        /// Note (2): performance hit reduced if  node generates a good hashcode.
        /// </param>
        public static IEnumerable<T> EnumerateNodes<T>(T node,
                                                        Func<T, IEnumerable<T>> getSubNodes,
                                                        NodeVisitOrder order = NodeVisitOrder.DepthFirstPreOrder,
                                                        CircularRefernceBehaviour checkType = CircularRefernceBehaviour.DontCheck)
            where T : class
        {
            if (node != null) //assuming null indicats an empty tree
            {
                //this acts as a stack or queue to resolve the recursion
                LinkedList<T> list = new LinkedList<T>();
                HashSet<T> visited = new HashSet<T>();

                switch (order)
                {
                    case NodeVisitOrder.DepthFirstPreOrder:
                        list.AddLast(node);
                        while (list.Count > 0)
                        {
                            T item = list.Pop();
                            if (visitOk(item, visited, checkType))
                            {
                                yield return item;
                                list.AddLastAll(getSubNodesSafe(item, getSubNodes).Reverse());
                            }
                        }
                        break;

                    case NodeVisitOrder.DepthFirstPostOrder:
                        //This has a side effects, the first iteeration is slow (also memory consuming) 
                        //as the entire structure is copied into a stack 
                        Stack<T> output = new Stack<T>();
                        list.AddLast(node);
                        while (list.Count > 0)
                        {
                            T item = list.Pop();
                            if (visitOk(item, visited, checkType))
                            {
                                output.Push(item);
                                list.AddLastAll(getSubNodesSafe(item, getSubNodes));
                            }
                        }
                        foreach (T item in output)
                        {
                            yield return item;
                        }
                        break;

                    case NodeVisitOrder.BredthFirst:
                        list.AddLast(node);
                        while (list.Count > 0)
                        {
                            T item = list.Dequeue();
                            if (visitOk(item, visited, checkType))
                            {
                                yield return item;
                                list.AddLastAll(getSubNodesSafe(item, getSubNodes));
                            }
                        }
                        break;
                }
            }
        }

        // Because getSubNodes(T) may return null to indicate no nodes.
        private static IEnumerable<T> getSubNodesSafe<T>(T node, Func<T, IEnumerable<T>> getSubNodes)
        {
            if ((node != null) && (getSubNodes != null))
            {
                IEnumerable<T> subNodes = getSubNodes(node);
                return (subNodes != null) ? subNodes : new List<T>();
            }
            return new List<T>();
        }

        private static bool visitOk<T>(T item, HashSet<T> visited, CircularRefernceBehaviour checkType)
        {
            if (checkType != CircularRefernceBehaviour.DontCheck)
            {
                if (visited.Contains(item))
                {
                    // error
                    if (checkType == CircularRefernceBehaviour.ThrowException)
                    {
                        throw NodeTraversalException.VisitedTwice(item);
                    }
                    return false; //indicate it's not ok to visit (ie skip)
                }

                //no error
                visited.Add(item);
                return true;
            }

            //no check
            return true;
        }


        /// <summary>
        /// Copies a tree structure. (useful in copying a tree structure to a tree view).
        /// </summary>
        /// <typeparam name="A">The node type of the tree to copy.</typeparam>
        /// <typeparam name="B">The destination node type.</typeparam>
        /// /// <param name="getSubNodes"> Get the sub-nodes of any given node.</param>
        /// <param name="newNode">To create a simple non-recursive copy of a node.</param>
        /// <param name="addSubNodes"></param>
        /// <param name="checkType"> Type of circular reference checking to perform.</param>
        /// <returns></returns>
        public static B RebuildTree<A, B>(A srcNode,
                                           Func<A, IEnumerable<A>> getSubNodes,
                                           Func<A, B> newNode,
                                           Action<B, IEnumerable<B>> addSubNodes,
                                           CircularRefernceBehaviour checkType = CircularRefernceBehaviour.DontCheck)
            where A : class
            where B : class
        {
            return RebuildTree(srcNode, getSubNodes, newNode, addSubNodes, P => true, checkType);
        }

        /// <summary>
        /// Copies a tree structure. (useful in copying a tree structure to a tree view).
        /// </summary>
        /// <typeparam name="A">The node type of the tree to copy.</typeparam>
        /// <typeparam name="B">The destination node type.</typeparam>
        /// /// <param name="getSubNodes"> Get the sub-nodes of any given node.</param>
        /// <param name="newNode">To create a simple non-recursive copy of a node.</param>
        /// <param name="addSubNodes"></param>
        /// <param name="where"></param>
        /// <param name="checkType"> Type of circular reference checking to perform.</param>
        /// <returns></returns>
        public static B RebuildTree<A, B>(A srcNode,
                                           Func<A, IEnumerable<A>> getSubNodes,
                                           Func<A, B> newNode,
                                           Action<B, IEnumerable<B>> addSubNodes,
                                           Predicate<A> where,
                                           CircularRefernceBehaviour checkType = CircularRefernceBehaviour.DontCheck)
            where A : class
            where B : class
        {


            LinkedList<Tuple<A, B>> list = new LinkedList<Tuple<A, B>>();
            HashSet<A> visited = new HashSet<A>();
            B newRootNode = null;

            list.AddLast(new Tuple<A, B>(srcNode, null));
            while (list.Count > 0)
            {
                var tuple = list.Pop();
                A item = tuple.Item1;
                B parent = tuple.Item2;

                if (visitOk(item, visited, checkType))
                {
                    if (where(item))
                    {
                        B newItem = newNode(item);
                        if (newRootNode == null) //root node
                        {
                            newRootNode = newItem;
                        }

                        safeAddSubNode(parent, newItem, addSubNodes);

                        list.AddLastAll(getSubNodesSafe(item, getSubNodes).Reverse().Select(N => new Tuple<A, B>(N, newItem)));
                    }
                }
            }

            //done
            return newRootNode;
        }

        private static void safeAddSubNode<B>(B parent, B child, Action<B, IEnumerable<B>> addSubNodes)
        {
            if ((addSubNodes != null) && (parent != null) && (child != null))
            {
                addSubNodes(parent, new B[] { child });
            }
        }

        public static bool? EquaityEcidentByReference(object a, object b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return null;
        }

        public static string ToSystemPathFormat(string s)
        {
            return s.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
