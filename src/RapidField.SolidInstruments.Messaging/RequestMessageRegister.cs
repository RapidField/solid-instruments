// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an extensible catalog of available request messages.
    /// </summary>
    /// <remarks>
    /// <see cref="RequestMessageRegister" /> is the default implementation of <see cref="IRequestMessageRegister" />.
    /// </remarks>
    public sealed class RequestMessageRegister : IRequestMessageRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageRegister" /> class.
        /// </summary>
        [DebuggerHidden]
        private RequestMessageRegister()
        {
            return;
        }

        /// <summary>
        /// Represents a singleton instance of the <see cref="RequestMessageRegister" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IRequestMessageRegister Instance = new RequestMessageRegister();
    }
}