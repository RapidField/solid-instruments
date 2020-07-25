// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to perform a domain action.
    /// </summary>
    public interface IDomainCommand : ILabeledCommand
    {
    }

    /// <summary>
    /// Represents a command to perform a domain action that emits a result when processed.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted when processing the command.
    /// </typeparam>
    public interface IDomainCommand<out TResult> : ILabeledCommand<TResult>
    {
    }
}