// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents configuration information for an <see cref="ActionRepeater" /> instance.
    /// </summary>
    public sealed class ActionRepeaterConfiguration : InstrumentConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterConfiguration" /> class.
        /// </summary>
        public ActionRepeaterConfiguration()
            : base()
        {
            Behavior = DefaultBehavior;
            Predicate = DefaultPredicate;
            RepeatedAction = DefaultRepeatedAction;
        }

        /// <summary>
        /// Gets or sets information that defines the behavior of an <see cref="ActionRepeater" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal IActionRepeaterBehavior Behavior
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a predicate function that defines the conditions for <see cref="RepeatedAction" /> repetition.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Func<Boolean> Predicate
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the action that is performed repetitively by the <see cref="ActionRepeater" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Action RepeatedAction
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default information that defines the behavior of an <see cref="ActionRepeater" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly IActionRepeaterBehavior DefaultBehavior = ActionRepeaterBehavior.Default;

        /// <summary>
        /// Represents the default predicate function that defines the conditions for <see cref="RepeatedAction" /> repetition.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Func<Boolean> DefaultPredicate = () => true;

        /// <summary>
        /// Represents the default action that is performed repetitively by the <see cref="ActionRepeater" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Action DefaultRepeatedAction = () => { return; };
    }
}