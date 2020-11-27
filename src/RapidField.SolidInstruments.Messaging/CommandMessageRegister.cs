// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available command messages.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandMessageRegister" /> is the default implementation of <see cref="ICommandMessageRegister" />.
    /// </remarks>
    public sealed class CommandMessageRegister : ICommandMessageRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessageRegister" /> class.
        /// </summary>
        [DebuggerHidden]
        private CommandMessageRegister()
        {
            Commands = CommandRegister.Instance;
        }

        /// <summary>
        /// Gets a catalog of available commands.
        /// </summary>
        public ICommandRegister Commands
        {
            get;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="CommandMessageRegister" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly ICommandMessageRegister Instance = new CommandMessageRegister();
    }
}