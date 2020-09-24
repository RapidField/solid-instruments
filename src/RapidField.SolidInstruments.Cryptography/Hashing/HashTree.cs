// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Represents hash values for an ordered series of data block objects and produces a deterministic root hash using a Merkle
    /// tree.
    /// </summary>
    /// <remarks>
    /// <see cref="HashTree{TBlock}" /> is the default implementation of <see cref="IHashTree{TBlock}" />.
    /// </remarks>
    /// <typeparam name="TBlock">
    /// The type of the data block objects underlying the hash tree.
    /// </typeparam>
    public class HashTree<TBlock> : Instrument, IHashTree<TBlock>
        where TBlock : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashTree{T}" /> class.
        /// </summary>
        /// <param name="hashingProcessor">
        /// A processor that produces hash values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashingProcessor" /> is <see langword="null" />.
        /// </exception>
        public HashTree(IHashingProcessor<TBlock> hashingProcessor)
            : this(hashingProcessor, DefaultAlgorithm)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTree{T}" /> class.
        /// </summary>
        /// <param name="hashingProcessor">
        /// A processor that produces hash values.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm used to produce hash values. The default value is <see cref="HashingAlgorithmSpecification.ShaTwo256" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashingProcessor" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="HashingAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public HashTree(IHashingProcessor<TBlock> hashingProcessor, HashingAlgorithmSpecification algorithm)
            : this(hashingProcessor, algorithm, Array.Empty<TBlock>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTree{T}" /> class.
        /// </summary>
        /// <param name="hashingProcessor">
        /// A processor that produces hash values.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm used to produce hash values. The default value is <see cref="HashingAlgorithmSpecification.ShaTwo256" />.
        /// </param>
        /// <param name="blocks">
        /// An ordered collection of data block objects underlying the tree.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashingProcessor" /> is <see langword="null" /> -or- <paramref name="blocks" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="HashingAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        public HashTree(IHashingProcessor<TBlock> hashingProcessor, HashingAlgorithmSpecification algorithm, IEnumerable<TBlock> blocks)
            : base()
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(HashingAlgorithmSpecification.Unspecified, nameof(algorithm));
            HashingProcessor = hashingProcessor.RejectIf().IsNull(nameof(hashingProcessor)).TargetArgument;
            LeafNodes = new List<HashTreeNode>();
            RootNode = new HashTreeNode();
            AddBlockRange(blocks);
        }

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
        public void AddBlock(TBlock block) => AddBlockRange(new TBlock[] { block });

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
        public void AddBlockRange(IEnumerable<TBlock> blocks)
        {
            if (blocks.Any() == false)
            {
                return;
            }

            using (var controlToken = StateControl.Enter())
            {
                var newNodes = blocks.RejectIf().IsNull(nameof(blocks)).TargetArgument.Select(block => CalculateHash(block.RejectIf().IsNull(nameof(blocks)).TargetArgument)).Select(hashValue => new HashTreeNode(hashValue));

                foreach (var node in newNodes)
                {
                    LeafNodes.Add(node);
                }

                PermuteRow(LeafNodes);
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="HashTree{TBlock}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="HashTree{TBlock}" />.
        /// </returns>
        public override String ToString() => ((HashTreeNode)RootNode).ToString();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="HashTree{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Calculates a hash value for the specified plaintext data block object.
        /// </summary>
        /// <param name="block">
        /// The plaintext data block object to hash.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        [DebuggerHidden]
        private Byte[] CalculateHash(TBlock block) => HashingProcessor.CalculateHash(block, Algorithm, SaltingMode.Unsalted);

        /// <summary>
        /// Calculates a hash value for the specified plaintext data block object.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext byte array hash.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        [DebuggerHidden]
        private Byte[] CalculateHash(Byte[] plaintext) => HashingProcessor.CalculateHash(plaintext, Algorithm);

        /// <summary>
        /// Calculates a hash value for the specified hash value pair.
        /// </summary>
        /// <param name="firstHashValue">
        /// The first hash value component.
        /// </param>
        /// <param name="secondHashValue">
        /// The second hash value component.
        /// </param>
        /// <returns>
        /// The resulting hash value.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised during hashing or serialization.
        /// </exception>
        [DebuggerHidden]
        private Byte[] CalculateHash(Byte[] firstHashValue, Byte[] secondHashValue)
        {
            var concatenatedHashes = new List<Byte>(firstHashValue.Length + secondHashValue.Length);
            concatenatedHashes.AddRange(firstHashValue);
            concatenatedHashes.AddRange(secondHashValue);
            return CalculateHash(concatenatedHashes.ToArray());
        }

        /// <summary>
        /// Creates a new parent node for the specified child nodes.
        /// </summary>
        /// <param name="leftChild">
        /// The left child of the node.
        /// </param>
        /// <param name="rightChild">
        /// The right child of the node.
        /// </param>
        /// <returns>
        /// The new node.
        /// </returns>
        [DebuggerHidden]
        private HashTreeNode CreateNode(HashTreeNode leftChild, HashTreeNode rightChild) => new HashTreeNode(CalculateHash(leftChild.Value, rightChild.Value), leftChild, rightChild);

        /// <summary>
        /// Processes the specified row in the tree recursively toward the root node.
        /// </summary>
        /// <param name="row">
        /// The row to process.
        /// </param>
        [DebuggerHidden]
        private void PermuteRow(IEnumerable<HashTreeNode> row)
        {
            var rowCount = row.Count();

            if (rowCount == 1)
            {
                RootNode = row.First();
                return;
            }

            var pairCount = rowCount / 2;
            var pairCountModulus = rowCount % 2;
            var parentRowLength = pairCount + pairCountModulus;
            var parentRow = new HashTreeNode[parentRowLength];

            for (var i = 0; i < pairCount; i++)
            {
                var pair = row.Skip(i * 2).Take(2);
                var leftChild = pair.First();
                var rightChild = pair.Last();

                if (leftChild.IsRoot)
                {
                    parentRow[i] = CreateNode(leftChild, rightChild);
                }
                else if (ReferenceEquals(leftChild.Parent, rightChild.Parent))
                {
                    parentRow[i] = leftChild.Parent as HashTreeNode;
                }
                else
                {
                    ((HashTreeNode)leftChild.Parent).Destroy();
                    parentRow[i] = CreateNode(leftChild, rightChild);
                }
            }

            if (pairCountModulus == 1)
            {
                // Carry the leftover node up to the next row.
                parentRow[parentRowLength - 1] = row.Last();
            }

            PermuteRow(parentRow);
        }

        /// <summary>
        /// Gets the number of leaf nodes in the current <see cref="HashTree{TBlock}" />.
        /// </summary>
        public Int32 LeafCount => LeafNodes.Count();

        /// <summary>
        /// Gets the root node for the current <see cref="HashTree{TBlock}" />.
        /// </summary>
        public ITreeNode<Byte[]> RootNode
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents the default algorithm used to produce hash values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const HashingAlgorithmSpecification DefaultAlgorithm = HashingAlgorithmSpecification.ShaTwo256;

        /// <summary>
        /// Represents the algorithm used to produce hash values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HashingAlgorithmSpecification Algorithm;

        /// <summary>
        /// Represents a processor that produces hash values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IHashingProcessor<TBlock> HashingProcessor;

        /// <summary>
        /// Represents hash values for the associated ordered data block objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<HashTreeNode> LeafNodes;

        /// <summary>
        /// Represents a node in a hash tree.
        /// </summary>
        private sealed class HashTreeNode : BinaryTreeNode<Byte[]>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HashTreeNode" /> class.
            /// </summary>
            [DebuggerHidden]
            internal HashTreeNode()
                : this(Array.Empty<Byte>())
            {
                return;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="HashTreeNode" /> class.
            /// </summary>
            /// <param name="value">
            /// The value of the node.
            /// </param>
            [DebuggerHidden]
            internal HashTreeNode(Byte[] value)
                : base(value)
            {
                return;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="HashTreeNode" /> class.
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
            /// <paramref name="leftChild" /> is <see langword="null" /> -or- <paramref name="rightChild" /> is
            /// <see langword="null" />.
            /// </exception>
            [DebuggerHidden]
            internal HashTreeNode(Byte[] value, HashTreeNode leftChild, HashTreeNode rightChild)
                : base(value, leftChild, rightChild)
            {
                return;
            }

            /// <summary>
            /// Converts the value of the current <see cref="HashTreeNode" /> to its equivalent string representation.
            /// </summary>
            /// <returns>
            /// A string representation of the current <see cref="HashTreeNode" />.
            /// </returns>
            public override String ToString() => Convert.ToBase64String(Value);
        }
    }
}