// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Encapsulates finalization for static members of a class or structure.
    /// </summary>
    public class StaticMemberFinalizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticMemberFinalizer" /> class.
        /// </summary>
        /// <param name="finalizeAction">
        /// An action that is invoked when the object is finalized.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="finalizeAction" /> is <see langword="null" />.
        /// </exception>
        public StaticMemberFinalizer(Action finalizeAction)
        {
            FinalizeAction = finalizeAction.RejectIf().IsNull(nameof(finalizeAction));
        }

        /// <summary>
        /// Finalizes the current instance of the <see cref="StaticMemberFinalizer" /> class.
        /// </summary>
        [DebuggerHidden]
        ~StaticMemberFinalizer()
        {
            FinalizeAction();
        }

        /// <summary>
        /// Represents an action that is invoked when the object is finalized.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action FinalizeAction;
    }
}