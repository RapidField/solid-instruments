// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Diagnostics;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents an extensible catalog of available commands.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandRegister" /> is the default implementation of <see cref="ICommandRegister" />.
    /// </remarks>
    public sealed class CommandRegister : ICommandRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRegister" /> class.
        /// </summary>
        [DebuggerHidden]
        private CommandRegister()
        {
            return;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="CommandRegister" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly ICommandRegister Instance = new CommandRegister();
    }
}