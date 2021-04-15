// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Specifies whether a command line parameter is a boolean switch or defines a value.
    /// </summary>
    public enum CliCommandParameterKind : Int32
    {
        /// <summary>
        /// The command line parameter type is not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The command line parameter is a boolean switch.
        /// </summary>
        Switch = 1,

        /// <summary>
        /// The command line parameter defines a value.
        /// </summary>
        Value = 2
    }
}