// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    /// <summary>
    /// Represents a collection of paired <see cref="Number" /> objects and their underlying primitive numeric structures which are
    /// made available for testing.
    /// </summary>
    internal sealed class NumericTestPairList : List<NumericTestPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTestPairList" /> class.
        /// </summary>
        /// <param name="collection">
        /// A collection of numeric test pairs from which to create the new list.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        public NumericTestPairList(IEnumerable<NumericTestPair> collection)
            : base(collection)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTestPairList" /> class.
        /// </summary>
        /// <param name="collections">
        /// A collection of collections of numeric test pairs from which to create the new list.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collections" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal NumericTestPairList(IEnumerable<IEnumerable<NumericTestPair>> collections)
            : this(collections.RejectIf().IsNull(nameof(collections)).TargetArgument.Flatten())
        {
            return;
        }

        /// <summary>
        /// Represents a static collection of <see cref="NumericTestPair" /> objects that are used for testing.
        /// </summary>
        public static readonly NumericTestPairList Instance = new(new IEnumerable<NumericTestPair>[]
        {
            ByteNumericTestPair.Cases,
            SByteNumericTestPair.Cases
        });
    }
}