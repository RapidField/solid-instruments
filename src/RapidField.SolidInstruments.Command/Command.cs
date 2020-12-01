// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command that emits a result when processed.
    /// </summary>
    /// <remarks>
    /// <see cref="Command{TResult}" /> is the default implementation of <see cref="ICommand{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted when processing the command.
    /// </typeparam>
    [DataContract]
    public abstract class Command<TResult> : ICommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command{TResult}" /> class.
        /// </summary>
        protected Command()
        {
            CorrelationIdentifierField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command{TResult}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected Command(Guid correlationIdentifier)
        {
            CorrelationIdentifierField = correlationIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(correlationIdentifier));
        }

        /// <summary>
        /// Converts the value of the current <see cref="Command{TResult}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Command{TResult}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ResultType)}\": \"{ResultType.FullName}\" }}";

        /// <summary>
        /// Gets or sets a unique identifier that is assigned to related commands.
        /// </summary>
        [DataMember]
        public Guid CorrelationIdentifier
        {
            get
            {
                if (CorrelationIdentifierField.HasValue is false)
                {
                    CorrelationIdentifierField = Guid.NewGuid();
                }

                return CorrelationIdentifierField.Value;
            }
            set => CorrelationIdentifierField = value;
        }

        /// <summary>
        /// Gets the type of the result that is emitted when processing the command.
        /// </summary>
        [IgnoreDataMember]
        public Type ResultType => ResultTypeReference;

        /// <summary>
        /// Represents a unique identifier that is assigned to related commands.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        internal Guid? CorrelationIdentifierField;

        /// <summary>
        /// Represents the standard noun appendage to the name that is used when representing this type in serialization and
        /// transport contexts.
        /// </summary>
        protected internal const String DataContractNameSuffix = Command.DataContractNameSuffix;

        /// <summary>
        /// Represents the type of the result that is emitted when processing the command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResultTypeReference = typeof(TResult);
    }

    /// <summary>
    /// Represents a command.
    /// </summary>
    /// <remarks>
    /// <see cref="Command" /> is the default implementation of <see cref="ICommand" />.
    /// </remarks>
    [DataContract]
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        protected Command()
        {
            CorrelationIdentifierField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected Command(Guid correlationIdentifier)
        {
            CorrelationIdentifierField = correlationIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(correlationIdentifier));
        }

        /// <summary>
        /// Converts the value of the current <see cref="Command" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Command" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(ResultType)}\": \"{ResultType.FullName}\" }}";

        /// <summary>
        /// Gets or sets a unique identifier that is assigned to related commands.
        /// </summary>
        [DataMember]
        public Guid CorrelationIdentifier
        {
            get
            {
                if (CorrelationIdentifierField.HasValue is false)
                {
                    CorrelationIdentifierField = Guid.NewGuid();
                }

                return CorrelationIdentifierField.Value;
            }
            set => CorrelationIdentifierField = value;
        }

        /// <summary>
        /// Gets the type of the result that is emitted when processing the command.
        /// </summary>
        [IgnoreDataMember]
        public virtual Type ResultType => Nix.Type;

        /// <summary>
        /// Represents the standard noun appendage to the name that is used when representing this type in serialization and
        /// transport contexts.
        /// </summary>
        protected internal const String DataContractNameSuffix = "Command";

        /// <summary>
        /// Represents a unique identifier that is assigned to related commands.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Guid? CorrelationIdentifierField;
    }
}