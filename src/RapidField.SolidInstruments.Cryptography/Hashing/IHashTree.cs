// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;
using System.Collections.Generic;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Represents hash values for an ordered series of data block objects and produces a deterministic root hash using a Merkle
    /// tree.
    /// </summary>
    /// <typeparam name="TBlock">
    /// The type of the data block objects underlying the hash tree.
    /// </typeparam>
    public interface IHashTree<TBlock> : IHashTree
        where TBlock : class
    {
        /// <summary>
        /// Calculates a hash value for the specified data block and adds it to the tree.
        /// </summary>
        /// <param name="block">
        /// A data block object to add to the hash tree.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="block" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public void AddBlock(TBlock block);

        /// <summary>
        /// Calculates hash values for the specified ordered data block collection and adds them to the tree.
        /// </summary>
        /// <param name="blocks">
        /// An ordered collection of data block objects to add to the hash tree.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blocks" /> is <see langword="null" /> -or- <paramref name="blocks" /> contains a null data block object.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public void AddBlockRange(IEnumerable<TBlock> blocks);
    }

    /// <summary>
    /// Represents hash values for an ordered series of data block objects and produces a deterministic root hash using a Merkle
    /// tree.
    /// </summary>
    public interface IHashTree : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets the number of leaf nodes in the current <see cref="IHashTree" />.
        /// </summary>
        public Int32 LeafCount
        {
            get;
        }

        /// <summary>
        /// Gets the root node for the current <see cref="IHashTree" />.
        /// </summary>
        public ITreeNode<Byte[]> RootNode
        {
            get;
        }
    }
}