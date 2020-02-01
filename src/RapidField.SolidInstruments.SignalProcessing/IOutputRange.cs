// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Collections.Generic;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a contiguous collection of discrete units of output from an <see cref="IChannel" />.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the associated channel's output value.
    /// </typeparam>
    public interface IOutputRange<T> : IReadOnlyCollection<IDiscreteUnitOfOutput<T>>, IReadOnlyList<IDiscreteUnitOfOutput<T>>
    {
    }
}