// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command to create an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="CreateDomainModelCommandMessage{TModel, TCommand}" /> is the default implementation of
    /// <see cref="ICreateDomainModelCommandMessage{TModel, TCommand}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    [DataContract]
    public abstract class CreateDomainModelCommandMessage<TModel, TCommand> : DomainModelCommandMessage<TModel, TCommand>, ICreateDomainModelCommandMessage<TModel, TCommand>
        where TModel : class, IDomainModel
        where TCommand : CreateDomainModelCommand<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDomainModelCommandMessage{TModel, TCommand}" /> class.
        /// </summary>
        protected CreateDomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDomainModelCommandMessage{TModel, TCommand}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected CreateDomainModelCommandMessage(TCommand commandObject)
            : base(commandObject)
        {
            return;
        }
    }
}