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
    public interface ITreeNode<T> : IEnumerable<ITreeNode<T>>, IEnumerable
    {
        /// <summary>
        /// Gets the child elements of the node, or an empty collection if the current <see cref="ITreeNode{T}" /> is a terminal
        /// node.
        /// </summary>
        IEnumerable<ITreeNode<T>> Children
        {
            get;
        }

        /// <summary>
        /// Gets the number of connections from the tree's root node to the current <see cref="ITreeNode{T}" />.
        /// </summary>
        Int32 Depth
        {
            get;
        }

        /// <summary>
        /// Gets the number of connections on the longest path between the current <see cref="ITreeNode{T}" /> and a leaf node.
        /// </summary>
        Int32 Height
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ITreeNode{T}" /> is a leaf node (a node without children).
        /// </summary>
        /// <remarks>
        /// When true, <see cref="Children" /> is an empty collection.
        /// </remarks>
        Boolean IsLeaf
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ITreeNode{T}" /> is a root node (a node without a parent).
        /// </summary>
        /// <remarks>
        /// When true, <see cref="Parent" /> is a null reference.
        /// </remarks>
        Boolean IsRoot
        {
            get;
        }

        /// <summary>
        /// Gets the parent element of the node, or <see langword="null" /> if the current <see cref="ITreeNode{T}" /> is a root
        /// node.
        /// </summary>
        ITreeNode<T> Parent
        {
            get;
        }

        /// <summary>
        /// Gets or sets the value of the current node.
        /// </summary>
        T Value
        {
            get;
            set;
        }
    }
}