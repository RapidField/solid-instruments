// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that configures and produces new <see cref="ActionRepeater" /> instances.
    /// </summary>
    /// <remarks>
    /// <see cref="ActionRepeaterBuilder" /> is the default implementation of <see cref="IActionRepeaterBuilder" />.
    /// </remarks>
    public sealed class ActionRepeaterBuilder : InstrumentBuilder<ActionRepeaterConfiguration, ActionRepeater>, IActionRepeaterBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBuilder" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ActionRepeaterBuilder()
            : base()
        {
            return;
        }

        /// <summary>
        /// Configures a predicate function that defines the conditions for action repetition.
        /// </summary>
        /// <param name="predicate">
        /// A function that defines the condition for action repetition.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public IActionRepeaterBuilder While(Func<Boolean> predicate)
        {
            _ = predicate.RejectIf().IsNull(nameof(predicate));
            Configure(configuration =>
            {
                configuration.Predicate = predicate;
            });

            return this;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ActionRepeaterBuilder" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}