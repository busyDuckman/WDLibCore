/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

#if !DISABLE_WINFORMS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WDToolbox.DotNetExtension;
using System.Windows.Forms;
using System.Collections;
using WDToolbox.AplicationFramework;

namespace WDToolbox//.DotNetExtension
{
    public static class TreeViewExtension
    {
        /// <summary>
        /// A path to an item in a treeview.
        /// Includes sensible Equals, HashCode and ToString methods.
        /// </summary>
        public class TreeNodePath
        {
            public List<string> path;
            public TreeNodePath (TreeNode node)
            {
                path = new List<string>();

                TreeNode current = node;
                while (current != null)
                {
                    path.Insert(0, current.Text ?? "");
                    current = current.Parent;
                }
            }

            public override int GetHashCode()
            {
                int hc = 0;
                foreach(string s in path)
                {
                    hc += s.GetHashCode();
                }

                return hc;
            }

            public override bool Equals(object obj)
            {
                TreeNodePath tnp = obj as TreeNodePath;
                if (tnp == null)
                {
                    return false;
                }

                return tnp.path.SequenceEqual(path);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in path)
                {
                    sb.Append(s + @"\");
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// The state of a treeview control
        /// </summary>
        public class TreeViewState
        {
            
            public class TreeNodeState
            {
                public bool isExpanded = false;
                public bool isChecked = false;

                public TreeNodeState(TreeNode node)
                {
                    isExpanded = node.IsExpanded;
                    isChecked = node.Checked;
                }

                public override string ToString()
                {
                    return String.Format("node {0} expanded and {1} checked",
                                        (isExpanded ? "is" : "not"),
                                        (isChecked  ? "is" : "not"));
                }
            }

            public Dictionary<TreeNodePath, TreeNodeState> nodes;
            public TreeNodePath selectedNode;

            public TreeViewState()
            {
                nodes = new Dictionary<TreeNodePath, TreeNodeState>();
                selectedNode = null;
            }
        }

        /// <summary>
        /// Returns a TreeNodePath which acuratly identifies a given node
        /// Uses .Text not .Name
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Empty path on error.</returns>
        public static TreeNodePath GetPath(this TreeNode node)
        {
            return new TreeNodePath(node);
        }

        /// <summary>
        /// Gets the state of the treeview nodes.
        /// Use with ApplyOldStateInfo to restore state after repopulating controll.
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        public static TreeViewState GetState(this TreeView tv)
        {
            TreeViewState state = new TreeViewState();

            //initial list of nodes
            List<TreeNode> nodes = new List<TreeNode>();
            foreach(TreeNode node in tv.Nodes)
            {
                nodes.Add(node);
            }
            
            //interate all nodes
            while(nodes.Count > 0)
            {
                TreeNode node = nodes[0];
                nodes.RemoveAt(0);

                //add state to results
                TreeNodePath path = new TreeNodePath(node);
                TreeViewState.TreeNodeState tns = new TreeViewState.TreeNodeState(node);
                if (!state.nodes.ContainsKey(path))
                {
                    state.nodes.Add(path, tns);
                }
                else
                {
                    WDAppLog.logError(ErrorLevel.SmallError, "tree view has two identical node pathways");
                    continue; //loop back rather then examine children, so as to stop any infinite loops.
                }

                
                //add child nodes to list
                foreach(TreeNode childNode in node.Nodes)
                {
                    nodes.Add(childNode);
                }
            }

            //store selected node
            state.selectedNode = GetPath(tv.SelectedNode);

            return state;
        }

        /// <summary>
        /// Uses existing state info to relayout a treeview, that was assumably repopulated.
        /// Is fairly tolerant of nodes that are new or missing.
        /// Not accurate when notes .Text feild changes
        /// Uses .Text not .Name
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="oldState"></param>
        /// <param name="restoreExpansion"></param>
        /// <param name="restoreCheckBox"></param>
        /// <param name="restoreSelection"></param>
        public static void ApplyOldStateInfo(this TreeView tv, 
                                                TreeViewState oldState, 
                                                bool restoreExpansion,
                                                bool restoreCheckBox, 
                                                bool restoreSelection)
        {
            //initial list of nodes
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode node in tv.Nodes)
            {
                nodes.Add(node);
            }

            //interate all nodes
            while (nodes.Count > 0)
            {
                TreeNode node = nodes[0];
                nodes.RemoveAt(0);

                //restore old state
                TreeNodePath path = new TreeNodePath(node);
                if (oldState.nodes.ContainsKey(path))
                {
                    TreeViewState.TreeNodeState tns = oldState.nodes[path];

                    if (restoreExpansion)
                    {
                        if (tns.isExpanded && (!node.IsExpanded))
                        {
                            node.Expand();
                        }
                        else if ((!tns.isExpanded) && node.IsExpanded)
                        {
                            node.Collapse(true);
                        }
                    }

                    if (restoreCheckBox)
                    {
                        node.Checked = tns.isChecked;
                    }
                }

                //add child nodes to list
                foreach (TreeNode childNode in node.Nodes)
                {
                    nodes.Add(childNode);
                }
            }

            //restore selected node
            if (restoreSelection)
            {
                TreeNode selNode = GetNode(tv, oldState.selectedNode);
                if (selNode != null)
                {
                    tv.SelectedNode = selNode;
                }
            }
        }

        /// <summary>
        /// Reterns a node with the specified path.
        /// </summary>
        /// <param name="tv">A tree view control</param>
        /// <param name="treeNodePath">The path of the node to find (uses .Text not .Name accesors of nodes.)</param>
        /// <returns>A TreeNode; null on error.</returns>
        public static TreeNode GetNode(TreeView tv, TreeNodePath treeNodePath)
        {
            TreeNodeCollection tnc = tv.Nodes;
            TreeNode node = null;
            foreach(string token in treeNodePath.path)
            {
                node = tnc.FindNodeWithText(token);
                if (node == null)
                {
                    return null;
                }

                tnc = node.Nodes;
            }
            return node;
        }

        /// <summary>
        /// tnc["foobar"] finds a node by its .Name this does it by .Text
        /// </summary>
        /// <param name="tnc"></param>
        /// <param name="text">case sensitive.</param>
        /// <returns>A node; does not recursivly search. Null on error.</returns>
        public static TreeNode FindNodeWithText(this TreeNodeCollection tnc, string text)
        {
            foreach (TreeNode node in tnc)
            {
                if (node.Text == text)
                {
                    return node;
                }
            }

            return null;
        }
    }
}
#endif