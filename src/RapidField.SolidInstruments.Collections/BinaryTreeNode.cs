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
    /// <remarks>
    /// <see cref="BinaryTreeNode{T}" /> is the default implementation of <see cref="IBinaryTreeNode{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The value type of the node.
    /// </typeparam>
    public class BinaryTreeNode<T> : TreeNode<T, BinaryTreeNode<T>>, IBinaryTreeNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}" /> class.
        /// </summary>
        public BinaryTreeNode()
            : this(default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        public BinaryTreeNode(T value)
            : this(value, null, null, false, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        /// <param name="leftChild">
        /// The left child of the node.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="leftChild" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="leftChild" /> is <see langword="null" />.
        /// </exception>
        public BinaryTreeNode(T value, BinaryTreeNode<T> leftChild)
            : this(value, leftChild, null, true, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        /// <param name="leftChild">
        /// The left child of the node.
        /// </param>
        /// <param name="rightChild">
        /// The right child of the node.
        /// </param>
        /// <exception cref="ArgumentException">
        /// One of the specified children is invalid -or- the children both reference the same object.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="leftChild" /> is null or <paramref name="rightChild" /> is <see langword="null" />.
        /// </exception>
        public BinaryTreeNode(T value, BinaryTreeNode<T> leftChild, BinaryTreeNode<T> rightChild)
            : this(value, leftChild, rightChild, true, true)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the node.
        /// </param>
        /// <param name="leftChild">
        /// The left child of the node.
        /// </param>
        /// <param name="rightChild">
        /// The right child of the node.
        /// </param>
        /// <param name="rejectNullLeftChild">
        /// A value indicating whether or not to raise an <see cref="ArgumentNullException" /> if <paramref name="leftChild" /> is
        /// <see langword="null" />.
        /// </param>
        /// <param name="rejectNullRightChild">
        /// A value indicating whether or not to raise an <see cref="ArgumentNullException" /> if <paramref name="rightChild" /> is
        /// <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// One of the specified children is invalid -or- the children both reference the same object.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rejectNullLeftChild" /> is <see langword="true" /> and <paramref name="leftChild" /> is
        /// <see langword="null" /> -or- <paramref name="rejectNullRightChild" /> is <see langword="true" /> and
        /// <paramref name="rightChild" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private BinaryTreeNode(T value, BinaryTreeNode<T> leftChild, BinaryTreeNode<T> rightChild, Boolean rejectNullLeftChild, Boolean rejectNullRightChild)
            : base(value, 2)
        {
            if ((leftChild is null) == false)
            {
                if (AddChild(leftChild) == false)
                {
                    throw new ArgumentException("The specified left child is invalid.", nameof(leftChild));
                }
            }
            else if (rejectNullLeftChild)
            {
                leftChild.RejectIf().IsNull(nameof(leftChild));
            }

            if ((rightChild is null) == false)
            {
                if (AddChild(rightChild) == false)
                {
                    throw new ArgumentException("The specified right child is invalid.", nameof(rightChild));
                }
            }
            else if (rejectNullRightChild)
            {
                rightChild.RejectIf().IsNull(nameof(rightChild));
            }
        }

        /// <summary>
        /// Gets the left child of the current <see cref="BinaryTreeNode{T}" />.
        /// </summary>
        public IBinaryTreeNode<T> LeftChild => Children.ElementAtOrDefault(0) as IBinaryTreeNode<T>;

        /// <summary>
        /// Gets the right child of the current <see cref="BinaryTreeNode{T}" />.
        /// </summary>
        public IBinaryTreeNode<T> RightChild => Children.ElementAtOrDefault(1) as IBinaryTreeNode<T>;
    }
}