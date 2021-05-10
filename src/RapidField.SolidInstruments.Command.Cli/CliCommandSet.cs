// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents a collection of CLI command definitions.
    /// </summary>
    /// <remarks>
    /// <see cref="CliCommndSet" /> is the default implementation of <see cref="ICliCommandSet" />.
    /// </remarks>
    internal class CliCommndSet : CliTokenSet<ICliCommandDefinition>, ICliCommandSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommndSet" /> class.
        /// </summary>
        [DebuggerHidden]
        internal CliCommndSet()
            : base()
        {
            return;
        }
    }
}