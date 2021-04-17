// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a message that emits a result when processed.
    /// </summary>
    /// <remarks>
    /// <see cref="Message{TResult}" /> is the default implementation of <see cref="IMessage{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The type of the result that is produced by handling the message.
    /// </typeparam>
    [DataContract]
    public abstract class Message<TResult> : Message, IMessage<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message{TResult}" /> class.
        /// </summary>
        protected Message()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message{TResult}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected Message(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message{TResult}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected Message(Guid correlationIdentifier, Guid identifier)
            : base(correlationIdentifier, identifier)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the result that is produced by handling the message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreDataMember]
        public sealed override Type ResultType => ResultTypeReference;

        /// <summary>
        /// Represents the type of the result that is produced by handling the message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResultTypeReference = typeof(TResult);
    }

    /// <summary>
    /// Represents a message.
    /// </summary>
    /// <remarks>
    /// <see cref="Message" /> is the default implementation of <see cref="IMessage" />.
    /// </remarks>
    [DataContract]
    [KnownType(typeof(MessageProcessingInformation))]
    public abstract class Message : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        protected Message()
        {
            CorrelationIdentifierField = null;
            IdentifierField = null;
            ProcessingInformationField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected Message(Guid correlationIdentifier)
        {
            CorrelationIdentifierField = correlationIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(correlationIdentifier));
            IdentifierField = null;
            ProcessingInformationField = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related messages.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the message.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="identifier" /> is
        /// equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected Message(Guid correlationIdentifier, Guid identifier)
        {
            CorrelationIdentifierField = correlationIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(correlationIdentifier));
            IdentifierField = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            ProcessingInformationField = null;
        }

        /// <summary>
        /// Converts the value of the current <see cref="Message" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Message" />.
        /// </returns>
        public override String ToString() => $"{GetType().Name} | {Identifier.ToSerializedString()}";

        /// <summary>
        /// Gets or sets a unique identifier that is assigned to related messages.
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
        /// Gets or sets a unique identifier for the message.
        /// </summary>
        [DataMember]
        public Guid Identifier
        {
            get
            {
                if (IdentifierField.HasValue is false)
                {
                    IdentifierField = Guid.NewGuid();
                }

                return IdentifierField.Value;
            }
            set => IdentifierField = value;
        }

        /// <summary>
        /// Gets or sets instructions and contextual information relating to processing for the current <see cref="Message" />.
        /// </summary>
        [DataMember]
        public IMessageProcessingInformation ProcessingInformation
        {
            get
            {
                if (ProcessingInformationField is null)
                {
                    ProcessingInformationField = new MessageProcessingInformation();
                }

                return ProcessingInformationField;
            }
            set => ProcessingInformationField = value;
        }

        /// <summary>
        /// Gets the type of the result that is produced by processing the message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreDataMember]
        public virtual Type ResultType => Nix.Type;

        /// <summary>
        /// Represents the entity type that is used for transmitting and listening for request messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static MessagingEntityType RequestEntityType = MessagingEntityType.Queue;

        /// <summary>
        /// Represents the entity type that is used for transmitting and listening for response messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static MessagingEntityType ResponseEntityType = MessagingEntityType.Topic;

        /// <summary>
        /// Represents the standard noun appendage to the name that is used when representing this type in serialization and
        /// transport contexts.
        /// </summary>
        protected internal const String DataContractNameSuffix = "Message";

        /// <summary>
        /// Represents a unique identifier that is assigned to related messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Guid? CorrelationIdentifierField;

        /// <summary>
        /// Represents a unique identifier for the message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Guid? IdentifierField;

        /// <summary>
        /// Represents instructions and contextual information relating to processing for the current <see cref="Message" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private IMessageProcessingInformation ProcessingInformationField;
    }
}