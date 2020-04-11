// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a node in a tree structure.
    /// </summary>
    /// <typeparam name="T">
    /// The value type of the node.
    /// </typeparam>
    /// <typeparam name="TChildNode">
    /// The type of the node's children.
    /// </typeparam>
    public interface ITreeNode<T, TChildNode> : ITreeNode<T>
        where TChildNode : ITreeNode<T>
    {
        /// <summary>
        /// Destroys all references to and from associated tree nodes and sets <see cref="ITreeNode{T}.Value" /> equal to the
        /// default value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The state of the node or the associated tree was changed during the destroy operation.
        /// </exception>
        public void Destroy();
    }

    /// <summary>
    /// Represents a node in a tree structure.
    /// </summary>
    /// <typeparam name="T">
    /// The value type of the node.
    /// </typeparam>
    public interface ITreeNode<T> : IEnumerable<ITreeNode<T>>, ITreeNode
    {
        /// <summary>
        /// Gets the child elements of the node, or an empty collection if the current <see cref="ITreeNode{T}" /> is a terminal
        /// node.
        /// </summary>
        public IEnumerable<ITreeNode<T>> Children
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ITreeNode{T}" /> is a leaf node (a node without children).
        /// </summary>
        /// <remarks>
        /// When true, <see cref="Children" /> is an empty collection.
        /// </remarks>
        public Boolean IsLeaf
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ITreeNode{T}" /> is a root node (a node without a parent).
        /// </summary>
        /// <remarks>
        /// When true, <see cref="Parent" /> is a null reference.
        /// </remarks>
        public Boolean IsRoot
        {
            get;
        }

        /// <summary>
        /// Gets the parent element of the node, or <see langword="null" /> if the current <see cref="ITreeNode{T}" /> is a root
        /// node.
        /// </summary>
        public ITreeNode<T> Parent
        {
            get;
        }

        /// <summary>
        /// Gets or sets the value of the current node.
        /// </summary>
        public T Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a node in a tree structure.
    /// </summary>
    public interface ITreeNode : IEnumerable
    {
        /// <summary>
        /// Gets the number of connections from the tree's root node to the current <see cref="ITreeNode" />.
        /// </summary>
        public Int32 Depth
        {
            get;
        }

        /// <summary>
        /// Gets the number of connections on the longest path between the current <see cref="ITreeNode" /> and a leaf node.
        /// </summary>
        public Int32 Height
        {
            get;
        }
    }
}