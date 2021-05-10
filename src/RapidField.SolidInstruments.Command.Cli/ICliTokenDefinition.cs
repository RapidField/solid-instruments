// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command.Cli
{
    /// <summary>
    /// Represents identifying information about a CLI token.
    /// </summary>
    public interface ICliTokenDefinition
    {
        /// <summary>
        /// Gets or sets an abbreviated or alternate alphanumeric name of the CLI token, or <see langword="null" /> if the token
        /// does not have an abbreviated form.
        /// </summary>
        public String Alias
        {
            get;
        }

        /// <summary>
        /// Gets or sets the alphanumeric name of the CLI token, or <see langword="null" /> if the token is unnamed.
        /// </summary>
        public String Name
        {
            get;
        }
    }
}