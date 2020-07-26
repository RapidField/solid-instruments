// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command related to an object that models a domain construct.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    public interface IDomainModelCommandMessage<TModel, TCommand> : IDomainCommandMessage<TCommand>
        where TModel : class, IDomainModel
        where TCommand : class, IDomainModelCommand<TModel>
    {
        /// <summary>
        /// Gets the desired state of the associated domain model.
        /// </summary>
        public TModel Model
        {
            get;
        }
    }
}