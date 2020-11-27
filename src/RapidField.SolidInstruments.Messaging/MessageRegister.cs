// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageRegister" /> is the default implementation of <see cref="IMessageRegister" />.
    /// </remarks>
    public sealed class MessageRegister : IMessageRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRegister" /> class.
        /// </summary>
        [DebuggerHidden]
        private MessageRegister()
        {
            CommandMessages = CommandMessageRegister.Instance;
            EventMessages = EventMessageRegister.Instance;
            RequestMessages = RequestMessageRegister.Instance;
        }

        /// <summary>
        /// Gets a catalog of available command messages.
        /// </summary>
        public ICommandMessageRegister CommandMessages
        {
            get;
        }

        /// <summary>
        /// Gets a catalog of available event messages.
        /// </summary>
        public IEventMessageRegister EventMessages
        {
            get;
        }

        /// <summary>
        /// Gets a catalog of available request messages.
        /// </summary>
        public IRequestMessageRegister RequestMessages
        {
            get;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="MessageRegister" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IMessageRegister Instance = new MessageRegister();
    }
}