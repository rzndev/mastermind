using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MasterMind
{
    // вершина дерева
    public class TreeNode<T> : Statistics
    {
        public T _item;
        public TreeNode<T> _parentNode;
        public List<TreeNode<T>> _children;

        public TreeNode(T item)
        {
            _item = item;
            _parentNode = null;
            _children = new List<TreeNode<T>>();
            O = 0;
            L = 0;
            N = 0;
            
            
            this.T = 0;
            V = 0;
        }

        public void SetParentNode(T parent)
        {
            _parentNode.Item = parent;
        }

        public T Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public void AddChild(T child)
        {
            _children.Add(new TreeNode<T>(child));
            N++;
        }

        public void RemoveChild(T child)
        {
            var node = _children.FirstOrDefault(e => e.Item.Equals(child));
            if (node != null)
            {
                N--;
                _children.Remove(node);
            }
        }

        public void BreadthFirstSearch()
        {

        }



    }
}
