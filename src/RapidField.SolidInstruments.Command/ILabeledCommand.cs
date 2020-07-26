// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command that is labeled with categorical and/or contextual information.
    /// </summary>
    public interface ILabeledCommand : ICommand, ILabeledObject
    {
    }

    /// <summary>
    /// Represents a command that emits a result when processed and that is labeled with categorical and/or contextual information.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted when processing the command.
    /// </typeparam>
    public interface ILabeledCommand<out TResult> : ICommand<TResult>, ILabeledObject
    {
    }
}