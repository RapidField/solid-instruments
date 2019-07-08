// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Collections.Extensions
{
    /// <summary>
    /// Extends the <see cref="ITreeNode{T}" /> interface with general purpose features.
    /// </summary>
    public static class ITreeNodeExtensions
    {
        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its descendants.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="action">
        /// The <see cref="Action{T}" /> delegate to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public static void TraverseDown<T>(this ITreeNode<T> target, Action<ITreeNode<T>> action) => target.TraverseDown(action, (node) => true);

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its descendants.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="action">
        /// The <see cref="Action{T}" /> delegate to perform.
        /// </param>
        /// <param name="predicate">
        /// A function to test each node for a condition. <paramref name="action" /> is not performed and traversal stops when the
        /// result is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is null or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public static void TraverseDown<T>(this ITreeNode<T> target, Action<ITreeNode<T>> action, Predicate<ITreeNode<T>> predicate)
        {
            action.RejectIf().IsNull(nameof(action));
            predicate.RejectIf().IsNull(nameof(predicate));

            if (predicate(target) == false)
            {
                return;
            }

            action(target);

            foreach (var node in target.Children)
            {
                node.TraverseDown(action, predicate);
            }
        }

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its descendants.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result type of the action.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="function">
        /// The <see cref="Func{T1, T2, TResult}" /> delegate to perform.
        /// </param>
        /// <returns>
        /// The result of the recursive action.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        public static TResult TraverseDown<T, TResult>(this ITreeNode<T> target, Func<ITreeNode<T>, IEnumerable<TResult>, TResult> function) => target.TraverseDown(function, (node) => true);

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its descendants.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result type of the action.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="function">
        /// The <see cref="Func{T1, T2, TResult}" /> delegate to perform.
        /// </param>
        /// <param name="predicate">
        /// A function to test each node for a condition. When the result is <see langword="false" />, <paramref name="function" />
        /// is not performed and traversal stops.
        /// </param>
        /// <returns>
        /// The result of the recursive action.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is null or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public static TResult TraverseDown<T, TResult>(this ITreeNode<T> target, Func<ITreeNode<T>, IEnumerable<TResult>, TResult> function, Predicate<ITreeNode<T>> predicate)
        {
            function.RejectIf().IsNull(nameof(function));
            predicate.RejectIf().IsNull(nameof(predicate));

            if (predicate(target) == false)
            {
                return default;
            }

            var results = new List<TResult>();

            foreach (var node in target.Children)
            {
                results.Add(node.TraverseDown(function, predicate));
            }

            return function(target, results);
        }

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its ancestors.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="action">
        /// The <see cref="Action{T}" /> delegate to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public static void TraverseUp<T>(this ITreeNode<T> target, Action<ITreeNode<T>> action) => target.TraverseUp(action, (node) => true);

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its ancestors.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="action">
        /// The <see cref="Action{T}" /> delegate to perform.
        /// </param>
        /// <param name="predicate">
        /// A function to test each node for a condition. When the result is <see langword="false" />, <paramref name="action" /> is
        /// not performed and traversal stops.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is null or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public static void TraverseUp<T>(this ITreeNode<T> target, Action<ITreeNode<T>> action, Predicate<ITreeNode<T>> predicate)
        {
            action.RejectIf().IsNull(nameof(action));
            predicate.RejectIf().IsNull(nameof(predicate));

            if (predicate(target) == false)
            {
                return;
            }

            action(target);

            if (target.IsRoot)
            {
                return;
            }

            target.Parent.TraverseUp(action, predicate);
        }

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its ancestors.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result type of the action.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="function">
        /// The <see cref="Func{T1, T2, TResult}" /> delegate to perform.
        /// </param>
        /// <returns>
        /// The result of the recursive action.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        public static TResult TraverseUp<T, TResult>(this ITreeNode<T> target, Func<ITreeNode<T>, TResult, TResult> function) => target.TraverseUp(function, (node) => true);

        /// <summary>
        /// Performs the specified action on the current <see cref="ITreeNode{T}" /> and, recursively, on its ancestors.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the tree.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result type of the action.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ITreeNode{T}" />.
        /// </param>
        /// <param name="function">
        /// The <see cref="Func{T1, T2, TResult}" /> delegate to perform.
        /// </param>
        /// <param name="predicate">
        /// A function to test each node for a condition. When the result is <see langword="false" />, <paramref name="function" />
        /// is not performed and traversal stops.
        /// </param>
        /// <returns>
        /// The result of the recursive action.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is null or <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public static TResult TraverseUp<T, TResult>(this ITreeNode<T> target, Func<ITreeNode<T>, TResult, TResult> function, Predicate<ITreeNode<T>> predicate)
        {
            function.RejectIf().IsNull(nameof(function));
            predicate.RejectIf().IsNull(nameof(predicate));

            if (predicate(target) == false)
            {
                return default;
            }

            return function(target, (target.IsRoot ? default : target.Parent.TraverseUp(function, predicate)));
        }
    }
}
