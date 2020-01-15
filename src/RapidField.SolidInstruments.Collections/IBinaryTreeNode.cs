// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a node in a binary tree structure.
    /// </summary>
    /// <typeparam name="T">
    /// The value type of the node.
    /// </typeparam>
    public interface IBinaryTreeNode<T> : ITreeNode<T, IBinaryTreeNode<T>>
    {
        /// <summary>
        /// Gets the left child of the current <see cref="BinaryTreeNode{T}" />.
        /// </summary>
        IBinaryTreeNode<T> LeftChild
        {
            get;
        }

        /// <summary>
        /// Gets the right child of the current <see cref="BinaryTreeNode{T}" />.
        /// </summary>
        IBinaryTreeNode<T> RightChild
        {
            get;
        }
    }
}