// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents information that defines the general behavior of an <see cref="IActionRepeater" />.
    /// </summary>
    /// <remarks>
    /// <see cref="ActionRepeaterBehavior" /> is the default implementation of <see cref="IActionRepeaterBehavior" />.
    /// </remarks>
    public sealed class ActionRepeaterBehavior : IActionRepeaterBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ActionRepeaterBehavior()
            : this(DefaultDelayScaleConstant)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant)
            : this(delayScaleConstant, DefaultDelayScaleFunction)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction)
            : this(delayScaleConstant, delayScaleFunction, DefaultTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <param name="timeoutThreshold">
        /// The maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="timeoutThreshold" /> is less than <see cref="TimeSpan.Zero" /> and not equal to
        /// <see cref="InfiniteTimeoutThreshold" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction, TimeSpan timeoutThreshold)
            : this(delayScaleConstant, delayScaleFunction, timeoutThreshold, DefaultTerminalBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or negative one
        /// (-1) if repetition count is unlimited. The default value is negative one (-1).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" /> -or-
        /// <paramref name="maximumRepetitionCount" /> is less than <see cref="InfiniteRepititionCount" /> (negative one).
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction, Int32 maximumRepetitionCount)
            : this(delayScaleConstant, delayScaleFunction, maximumRepetitionCount, DefaultTerminalBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <param name="timeoutThreshold">
        /// The maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <param name="terminalBehavior">
        /// The behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold or maximum
        /// repetition count. The default value is <see cref="ActionRepeaterTerminalBehavior.Desist" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="timeoutThreshold" /> is less than <see cref="TimeSpan.Zero" /> and not equal to
        /// <see cref="InfiniteTimeoutThreshold" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" /> -or-
        /// <paramref name="terminalBehavior" /> is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction, TimeSpan timeoutThreshold, ActionRepeaterTerminalBehavior terminalBehavior)
            : this(delayScaleConstant, delayScaleFunction, timeoutThreshold, DefaultMaximumRepetitionCount, terminalBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or negative one
        /// (-1) if repetition count is unlimited. The default value is negative one (-1).
        /// </param>
        /// <param name="terminalBehavior">
        /// The behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold or maximum
        /// repetition count. The default value is <see cref="ActionRepeaterTerminalBehavior.Desist" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" /> -or-
        /// <paramref name="maximumRepetitionCount" /> is less than <see cref="InfiniteRepititionCount" /> (negative one) -or-
        /// <paramref name="terminalBehavior" /> is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction, Int32 maximumRepetitionCount, ActionRepeaterTerminalBehavior terminalBehavior)
            : this(delayScaleConstant, delayScaleFunction, DefaultTimeoutThreshold, maximumRepetitionCount, terminalBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <param name="timeoutThreshold">
        /// The maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or negative one
        /// (-1) if repetition count is unlimited. The default value is negative one (-1).
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="timeoutThreshold" /> is less than <see cref="TimeSpan.Zero" /> and not equal to
        /// <see cref="InfiniteTimeoutThreshold" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" /> -or-
        /// <paramref name="maximumRepetitionCount" /> is less than <see cref="InfiniteRepititionCount" /> (negative one).
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction, TimeSpan timeoutThreshold, Int32 maximumRepetitionCount)
            : this(delayScaleConstant, delayScaleFunction, timeoutThreshold, maximumRepetitionCount, DefaultTerminalBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeaterBehavior" /> class.
        /// </summary>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation, or <see cref="TimeSpan.Zero" /> to suppress delays. The default value is
        /// <see cref="TimeSpan.Zero" />.
        /// </param>
        /// <param name="delayScaleFunction">
        /// The function that is used to scale successive delays during <see cref="IActionRepeater" /> operation. The default value
        /// is <see cref="ActionRepeaterDelayScaleFunction.Constant" />.
        /// </param>
        /// <param name="timeoutThreshold">
        /// The maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or negative one
        /// (-1) if repetition count is unlimited. The default value is negative one (-1).
        /// </param>
        /// <param name="terminalBehavior">
        /// The behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold or maximum
        /// repetition count. The default value is <see cref="ActionRepeaterTerminalBehavior.Desist" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="timeoutThreshold" /> is less than <see cref="TimeSpan.Zero" /> and not equal to
        /// <see cref="InfiniteTimeoutThreshold" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" /> -or-
        /// <paramref name="maximumRepetitionCount" /> is less than <see cref="InfiniteRepititionCount" /> (negative one) -or-
        /// <paramref name="terminalBehavior" /> is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal ActionRepeaterBehavior(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction, TimeSpan timeoutThreshold, Int32 maximumRepetitionCount, ActionRepeaterTerminalBehavior terminalBehavior)
        {
            // Do not reorder these assignments without carefully reviewing the property setter logic.
            DelayScaleConstant = delayScaleConstant;
            DelayScaleFunction = delayScaleFunction;
            TimeoutThreshold = timeoutThreshold;
            MaximumRepititionCount = maximumRepetitionCount;
            TerminalBehavior = terminalBehavior;
        }

        /// <summary>
        /// Gets or sets the constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="DelayScaleConstant" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        public TimeSpan DelayScaleConstant
        {
            [DebuggerHidden]
            get => DelayScaleConstantValue;
            [DebuggerHidden]
            set => DelayScaleConstantValue = value.RejectIf().IsLessThan(TimeSpan.Zero, nameof(DelayScaleConstant));
        }

        /// <summary>
        /// Gets or sets the function that is used to scale successive delays during <see cref="IActionRepeater" /> operation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="DelayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" />.
        /// </exception>
        public ActionRepeaterDelayScaleFunction DelayScaleFunction
        {
            [DebuggerHidden]
            get => DelayScaleFunctionValue;
            [DebuggerHidden]
            set => DelayScaleFunctionValue = DelayScaleConstant == TimeSpan.Zero ? ActionRepeaterDelayScaleFunction.Constant : value.RejectIf().IsEqualToValue(ActionRepeaterDelayScaleFunction.Unspecified, nameof(DelayScaleFunction));
        }

        /// <summary>
        /// Gets or sets the maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or
        /// negative one (-1) if repetition count is unlimited.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="MaximumRepititionCount" /> is less than <see cref="InfiniteRepititionCount" /> (negative one).
        /// </exception>
        public Int32 MaximumRepititionCount
        {
            [DebuggerHidden]
            get => MaximumRepititionCountValue;
            [DebuggerHidden]
            set => MaximumRepititionCountValue = value.RejectIf().IsLessThan(InfiniteRepititionCount, nameof(MaximumRepititionCount));
        }

        /// <summary>
        /// Gets or sets the behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold
        /// or maximum repetition count.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="TerminalBehavior" /> is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        public ActionRepeaterTerminalBehavior TerminalBehavior
        {
            [DebuggerHidden]
            get => TerminalBehaviorValue;
            [DebuggerHidden]
            set => TerminalBehaviorValue = TimeoutThreshold == InfiniteTimeoutThreshold && MaximumRepititionCount == InfiniteRepititionCount ? ActionRepeaterTerminalBehavior.Desist : value.RejectIf().IsEqualToValue(ActionRepeaterTerminalBehavior.Unspecified, nameof(TerminalBehavior));
        }

        /// <summary>
        /// Gets or sets the maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <see cref="TimeoutThreshold" /> is less than <see cref="TimeSpan.Zero" /> and not equal to
        /// <see cref="InfiniteTimeoutThreshold" />.
        /// </exception>
        public TimeSpan TimeoutThreshold
        {
            [DebuggerHidden]
            get => TimeoutThresholdValue;
            [DebuggerHidden]
            set => TimeoutThresholdValue = value.RejectIf(argument => argument < TimeSpan.Zero && argument != InfiniteTimeoutThreshold, nameof(TimeoutThreshold), "The specified timeout threshold is invalid.");
        }

        /// <summary>
        /// Gets a new default instance of an <see cref="IActionRepeaterBehavior" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static IActionRepeaterBehavior Default => new ActionRepeaterBehavior();

        /// <summary>
        /// Represents a <see cref="MaximumRepititionCount" /> value that specifies an unlimited repetition count.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Int32 InfiniteRepititionCount = -1;

        /// <summary>
        /// Represents a <see cref="TimeoutThreshold" /> value that specifies an unlimited timeout threshold.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan InfiniteTimeoutThreshold = Timeout.InfiniteTimeSpan;

        /// <summary>
        /// Represents the default function that is used to scale successive delays during <see cref="IActionRepeater" /> operation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ActionRepeaterDelayScaleFunction DefaultDelayScaleFunction = ActionRepeaterDelayScaleFunction.Constant;

        /// <summary>
        /// Represents the default maximum number of times that the associated <see cref="IActionRepeater" /> will perform an
        /// action.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DefaultMaximumRepetitionCount = -1;

        /// <summary>
        /// Represents the default behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout
        /// threshold or maximum repetition count.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ActionRepeaterTerminalBehavior DefaultTerminalBehavior = ActionRepeaterTerminalBehavior.Desist;

        /// <summary>
        /// Represents the default constant length of time that serves as the functional input for scaling of successive delays
        /// during <see cref="IActionRepeater" /> operation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultDelayScaleConstant = TimeSpan.Zero;

        /// <summary>
        /// Represents the default maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultTimeoutThreshold = Timeout.InfiniteTimeSpan;

        /// <summary>
        /// Represents the constant length of time that serves as the functional input for scaling of successive delays during
        /// <see cref="IActionRepeater" /> operation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan DelayScaleConstantValue;

        /// <summary>
        /// Represents the function that is used to scale successive delays during <see cref="IActionRepeater" /> operation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ActionRepeaterDelayScaleFunction DelayScaleFunctionValue;

        /// <summary>
        /// Represents the maximum number of times that the associated <see cref="IActionRepeater" /> will perform an action, or
        /// negative one (-1) if repetition count is unlimited.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 MaximumRepititionCountValue;

        /// <summary>
        /// Represents the behavior of the associated <see cref="IActionRepeater" /> after it has exhausted its timeout threshold or
        /// maximum repetition count.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ActionRepeaterTerminalBehavior TerminalBehaviorValue;

        /// <summary>
        /// Represents the maximum length of time that the associated <see cref="IActionRepeater" /> will repeat an action, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if repetition duration is unlimited.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan TimeoutThresholdValue;
    }
}