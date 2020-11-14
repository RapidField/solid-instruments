// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a contiguous collection of discrete units of output from an <see cref="IChannel" />.
    /// </summary>
    /// <remarks>
    /// <see cref="OutputRange{T}" /> is the default implementation of <see cref="IOutputRange{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the associated channel's output value.
    /// </typeparam>
    public sealed class OutputRange<T> : ReadOnlyCollection<IDiscreteUnitOfOutput<T>>, IOutputRange<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputRange{T}" /> class.
        /// </summary>
        public OutputRange()
            : base(new List<IDiscreteUnitOfOutput<T>>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputRange{T}" /> class.
        /// </summary>
        /// <param name="unitsOfOutput">
        /// The contiguous list of discrete units of output from an <see cref="IChannel" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="unitsOfOutput" /> contains one or more invalid read indices or is not contiguous.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="unitsOfOutput" /> is <see langword="null" />.
        /// </exception>
        public OutputRange(IList<IDiscreteUnitOfOutput<T>> unitsOfOutput)
            : base(unitsOfOutput)
        {
            var listLength = unitsOfOutput.Count;

            if (listLength == 0)
            {
                return;
            }

            var firstReadIndex = unitsOfOutput.First().ChannelReadIndex;

            if (firstReadIndex < -1)
            {
                throw new ArgumentException("The specified contains one or more invalid read indices.", nameof(unitsOfOutput));
            }
            else if (firstReadIndex == -1)
            {
                if (unitsOfOutput.Any(unit => unit.ChannelReadIndex != -1))
                {
                    throw new ArgumentException("The specified list contains an invalid combination of read indices.", nameof(unitsOfOutput));
                }

                return;
            }

            for (var i = 1; i < listLength; i++)
            {
                if (unitsOfOutput[i].ChannelReadIndex != (firstReadIndex + i))
                {
                    throw new ArgumentException("The specified list is not contiguous.", nameof(unitsOfOutput));
                }
            }
        }
    }
}