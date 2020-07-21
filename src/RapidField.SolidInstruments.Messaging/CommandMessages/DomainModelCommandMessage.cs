﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Represents a message that contains a command related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelCommandMessage{TModel, TCommand}" /> is the default implementation of
    /// <see cref="IDomainModelCommandMessage{TModel, TCommand}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    [DataContract]
    public abstract class DomainModelCommandMessage<TModel, TCommand> : DomainCommandMessage<TCommand>, IDomainModelCommandMessage<TModel, TCommand>
        where TModel : class, IDomainModel
        where TCommand : DomainModelCommand<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand}" /> class.
        /// </summary>
        protected DomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCommandMessage{TModel, TCommand}" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelCommandMessage(TCommand commandObject)
            : base(commandObject)
        {
            return;
        }

        /// <summary>
        /// Gets the desired state of the associated domain model.
        /// </summary>
        [IgnoreDataMember]
        public TModel Model => Command.Model;
    }
}