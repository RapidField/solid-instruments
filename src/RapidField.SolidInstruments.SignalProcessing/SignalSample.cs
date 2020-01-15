// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents the result of a read operation performed against an <see cref="IChannel{T}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="SignalSample{T}" /> is the default implementation of <see cref="ISignalSample{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the sampled channel's output value.
    /// </typeparam>
    public sealed class SignalSample<T> : ISignalSample<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignalSample{T}" /> class.
        /// </summary>
        /// <param name="unitOfOutput">
        /// The discrete unit of output that was read from the channel's output stream.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="unitOfOutput" /> is <see langword="null" />.
        /// </exception>
        public SignalSample(IDiscreteUnitOfOutput<T> unitOfOutput)
            : this(unitOfOutput, new OutputRange<T>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalSample{T}" /> class.
        /// </summary>
        /// <param name="unitOfOutput">
        /// The discrete unit of output that was read from the channel's output stream.
        /// </param>
        /// <param name="lookBehindRange">
        /// A range of discrete units of output preceding <paramref name="unitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-behind was not requested.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="unitOfOutput" /> is <see langword="null" /> -or- <paramref name="lookBehindRange" /> is
        /// <see langword="null" />.
        /// </exception>
        public SignalSample(IDiscreteUnitOfOutput<T> unitOfOutput, IOutputRange<T> lookBehindRange)
            : this(unitOfOutput, lookBehindRange, new OutputRange<T>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalSample{T}" /> class.
        /// </summary>
        /// <param name="unitOfOutput">
        /// The discrete unit of output that was read from the channel's output stream.
        /// </param>
        /// <param name="lookBehindRange">
        /// A range of discrete units of output preceding <paramref name="unitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-behind was not requested.
        /// </param>
        /// <param name="lookAheadRange">
        /// A range of discrete units of output following <paramref name="unitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-ahead was not requested.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="unitOfOutput" /> is <see langword="null" /> -or- <paramref name="lookAheadRange" /> is
        /// <see langword="null" /> -or- <paramref name="lookBehindRange" /> is <see langword="null" />.
        /// </exception>
        public SignalSample(IDiscreteUnitOfOutput<T> unitOfOutput, IOutputRange<T> lookBehindRange, IOutputRange<T> lookAheadRange)
        {
            UnitOfOutput = unitOfOutput.RejectIf().IsNull(nameof(unitOfOutput)).TargetArgument;
            LookAheadRange = lookAheadRange.RejectIf().IsNull(nameof(lookAheadRange)).TargetArgument;
            LookBehindRange = lookBehindRange.RejectIf().IsNull(nameof(lookBehindRange)).TargetArgument;
        }

        /// <summary>
        /// Gets a range of discrete units of output following <see cref="UnitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-ahead was not requested.
        /// </summary>
        public IOutputRange<T> LookAheadRange
        {
            get;
        }

        /// <summary>
        /// Gets a range of discrete units of output preceding <see cref="UnitOfOutput" /> in the channel's output stream, or an
        /// empty range if look-behind was not requested.
        /// </summary>
        public IOutputRange<T> LookBehindRange
        {
            get;
        }

        /// <summary>
        /// Gets the discrete unit of output that was read from the channel's output stream.
        /// </summary>
        public IDiscreteUnitOfOutput<T> UnitOfOutput
        {
            get;
        }
    }
}