// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections.Extensions;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a node in a tree structure.
    /// </summary>
    /// <remarks>
    /// <see cref="TreeNode{T, TChildNode}" /> is the default implementation of <see cref="ITreeNode{T, TChildNode}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The value type of the node.
    /// </typeparam>
    /// <typeparam name="TChildNode">
    /// The type of the node's children.
    /// </typeparam>
    public class TreeNode<T, TChildNode> : TreeNode<T>, ITreeNode<T, TChildNode>
        where TChildNode : TreeNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T, TChildNode}" /> class.
        /// </summary>
        public TreeNode()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T, TChildNode}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        public TreeNode(T value)
            : base(value)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T, TChildNode}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        /// <param name="capacity">
        /// The maximum number of children permitted by the node, or -1 if no limit is imposed. The default value is -1.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity" /> is less than -1.
        /// </exception>
        public TreeNode(T value, Int32 capacity)
            : base(capacity)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        /// <param name="children">
        /// The child nodes of the new node, or <see langword="null" /> if the new node is a terminal node. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="children" /> contains a null, duplicate or invalid node.
        /// </exception>
        public TreeNode(T value, IEnumerable<TChildNode> children)
            : base(value, children)
        {
            return;
        }

        /// <summary>
        /// Adds the specified node to <see cref="TreeNode{T}.Children" /> and sets its <see cref="TreeNode{T}.Parent" /> to the
        /// current node.
        /// </summary>
        /// <param name="childNode">
        /// The child node to add.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified node was added to <see cref="TreeNode{T}.Children" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="childNode" /> is <see langword="null" />.
        /// </exception>
        public Boolean AddChild(TChildNode childNode) => AddChild(childNode, true);

        /// <summary>
        /// Destroys all references to and from associated tree nodes and sets <see cref="TreeNode{T}.Value" /> equal to the default
        /// value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The state of the node or the associated tree was changed during the destroy operation.
        /// </exception>
        public void Destroy()
        {
            if (IsRoot == false)
            {
                ((TreeNode<T>)Parent).RemoveChild(this);
            }

            var children = Children.ToArray();
            var childrenLength = children.Length;

            for (var i = 0; i < childrenLength; i++)
            {
                try
                {
                    if (RemoveChild(children.ElementAt(i) as TreeNode<T>))
                    {
                        continue;
                    }
                }
                catch (ArgumentNullException exception)
                {
                    throw new InvalidOperationException("The tree node is in an invalid state.", exception);
                }

                throw new InvalidOperationException("The tree structure has become corrupt.");
            }

            Value = default;
        }
    }

    /// <summary>
    /// Represents a node in a tree structure.
    /// </summary>
    /// <remarks>
    /// <see cref="TreeNode{T}" /> is the default implementation of <see cref="ITreeNode{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The value type of the node.
    /// </typeparam>
    public class TreeNode<T> : ITreeNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        public TreeNode()
            : this(default(T))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        public TreeNode(T value)
            : this(value, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        /// <param name="children">
        /// The child nodes of the new node, or <see langword="null" /> if the new node is a terminal node. The default value is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="children" /> contains a null, duplicate or invalid node.
        /// </exception>
        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
            : this(children is null ? UnlimitedCapacityValue : children.Count())
        {
            Value = value;

            if (children is null)
            {
                return;
            }

            foreach (var node in children)
            {
                if (node is null)
                {
                    throw new ArgumentException("The specified collection contains a null node.", nameof(children));
                }
                else if (AddChild(node, true) == false)
                {
                    throw new ArgumentException("The specified collection contains a duplicate or invalid node.", nameof(children));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}" /> class.
        /// </summary>
        /// <param name="capacity">
        /// The maximum number of children permitted by the node, or -1 if no limit is imposed. The default value is -1.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity" /> is less than -1.
        /// </exception>
        protected TreeNode(Int32 capacity)
        {
            Capacity = capacity.RejectIf().IsLessThan(UnlimitedCapacityValue, nameof(capacity));
            ChildrenReference = Capacity == UnlimitedCapacityValue ? new List<TreeNode<T>>() : new List<TreeNode<T>>(Capacity);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the child nodes of the current <see cref="TreeNode{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the child nodes of the current <see cref="TreeNode{T}" />.
        /// </returns>
        public IEnumerator<ITreeNode<T>> GetEnumerator() => Children.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the child nodes of the current <see cref="TreeNode{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the child nodes of the current <see cref="TreeNode{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Removes the specified node from <see cref="Children" />.
        /// </summary>
        /// <param name="childNode">
        /// The child node to remove.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified node was present in <see cref="Children" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="childNode" /> is <see langword="null" />.
        /// </exception>
        public Boolean RemoveChild(TreeNode<T> childNode)
        {
            childNode.RejectIf().IsNull(nameof(childNode));

            lock (SyncRoot)
            {
                childNode.ParentReference = null;
                return ChildrenReference.Remove(childNode);
            }
        }

        /// <summary>
        /// Adds the specified node to <see cref="Children" /> and sets its <see cref="Parent" /> to the current node.
        /// </summary>
        /// <param name="childNode">
        /// The child node to add.
        /// </param>
        /// <param name="enforceDuplicateProhibition">
        /// A value indicating whether or not to reject <paramref name="childNode" /> if the current node already contains a
        /// reference to it.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified node was added to <see cref="Children" />, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="childNode" /> is <see langword="null" />.
        /// </exception>
        protected Boolean AddChild(TreeNode<T> childNode, Boolean enforceDuplicateProhibition)
        {
            lock (SyncRoot)
            {
                if (Capacity > UnlimitedCapacityValue && ChildrenReference.Count() >= Capacity)
                {
                    return false;
                }
                else if (enforceDuplicateProhibition && ChildrenReference.Contains(childNode.RejectIf().IsNull(nameof(childNode))))
                {
                    return false;
                }

                childNode.ParentReference = this;
                ChildrenReference.Add(childNode);
            }

            return true;
        }

        /// <summary>
        /// Gets the child nodes of the current <see cref="TreeNode{T}" />, or an empty collection if the current
        /// <see cref="TreeNode{T}" /> is a terminal node.
        /// </summary>
        public IEnumerable<ITreeNode<T>> Children
        {
            get
            {
                lock (SyncRoot)
                {
                    foreach (var child in ChildrenReference)
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of connections from the tree's root node to the current <see cref="TreeNode{T}" />.
        /// </summary>
        public Int32 Depth => this.TraverseUp(DepthFunction);

        /// <summary>
        /// Gets the number of connections on the longest path between the current <see cref="TreeNode{T}" /> and a leaf node.
        /// </summary>
        public Int32 Height => this.TraverseDown(HeightFunction);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="TreeNode{T}" /> is a leaf node (a node without children).
        /// </summary>
        /// <remarks>
        /// When true, <see cref="Children" /> is an empty collection.
        /// </remarks>
        public Boolean IsLeaf => (ChildrenReference.Any() == false);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="TreeNode{T}" /> is a root node (a node without a parent).
        /// </summary>
        /// <remarks>
        /// When true, <see cref="Parent" /> is a null reference.
        /// </remarks>
        public Boolean IsRoot => (ParentReference is null);

        /// <summary>
        /// Gets the parent node of the current <see cref="TreeNode{T}" />, or <see langword="null" /> if the current
        /// <see cref="TreeNode{T}" /> is a root node.
        /// </summary>
        public ITreeNode<T> Parent => ParentReference;

        /// <summary>
        /// Gets or sets the value of the current <see cref="TreeNode{T}" />.
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Represents a value of <see cref="Capacity" /> which indicates that no limit is imposed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 UnlimitedCapacityValue = -1;

        /// <summary>
        /// Represents the maximum count of <see cref="Children" /> permitted for the current <see cref="TreeNode{T}" />, or -1 if
        /// no limit is imposed.
        /// </summary>
        protected readonly Int32 Capacity;

        /// <summary>
        /// Represents the child nodes of the current <see cref="TreeNode{T}" />, or an empty collection if the current
        /// <see cref="TreeNode{T}" /> is a terminal node.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICollection<TreeNode<T>> ChildrenReference;

        /// <summary>
        /// Represents a function used to calculate the node's depth in the tree.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<ITreeNode<T>, Int32, Int32> DepthFunction = (node, parentResult) =>
        {
            return node.IsRoot ? 0 : (parentResult + 1);
        };

        /// <summary>
        /// Represents a function used to calculate the node's height in the tree.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<ITreeNode<T>, IEnumerable<Int32>, Int32> HeightFunction = (node, childResults) =>
        {
            return node.IsLeaf ? 0 : (childResults.Max() + 1);
        };

        /// <summary>
        /// Represents an object that is used to synchronize access to the associated resource(s).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Object SyncRoot = new Object();

        /// <summary>
        /// Represents the parent node of the current <see cref="TreeNode{T}" />, or <see langword="null" /> if the current
        /// <see cref="TreeNode{T}" /> is a root node.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ITreeNode<T> ParentReference = null;
    }
}