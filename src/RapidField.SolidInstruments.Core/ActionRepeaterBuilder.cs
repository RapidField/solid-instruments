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
        /// Configures an action that will be repeated.
        /// </summary>
        /// <param name="repeatedAction">
        /// An action that will be repeated.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repeatedAction" /> is <see langword="null" />.
        /// </exception>
        public IActionRepeaterBuilder Repeat(Action repeatedAction)
        {
            _ = repeatedAction.RejectIf().IsNull(nameof(repeatedAction));
            Configure(configuration => configuration.RepeatedAction = repeatedAction);
            return this;
        }

        /// <summary>
        /// Configures a predicate function that defines the conditions for action repetition.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing predicate configuration.
        /// </remarks>
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
            Configure(configuration => configuration.Predicate = predicate);
            return this;
        }

        /// <summary>
        /// Configures the initial length of time to delay between action repetitions, with subsequent delays growing at a linear,
        /// arithmetic cadence.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="initialDelayDurationInMilliseconds">
        /// The constant length of time, in milliseconds, that serves as the functional input for scaling of successive delays.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDurationInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithArithmeticScalingDelay(Int32 initialDelayDurationInMilliseconds) => WithArithmeticScalingDelay(TimeSpan.FromMilliseconds(initialDelayDurationInMilliseconds.RejectIf().IsLessThanOrEqualTo(0, nameof(initialDelayDurationInMilliseconds))));

        /// <summary>
        /// Configures the initial length of time to delay between action repetitions, with subsequent delays growing at a linear,
        /// arithmetic cadence.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="initialDelayDuration">
        /// The constant length of time that serves as the functional input for scaling of successive delays.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDuration" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithArithmeticScalingDelay(TimeSpan initialDelayDuration) => WithDelay(initialDelayDuration, ActionRepeaterDelayScaleFunction.Arithmetic);

        /// <summary>
        /// Configures the constant length of time to delay between action repetitions.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="delayDurationInMilliseconds">
        /// The constant length of time, in milliseconds, to delay between action repetitions.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayDurationInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithConstantDelay(Int32 delayDurationInMilliseconds) => WithConstantDelay(TimeSpan.FromMilliseconds(delayDurationInMilliseconds.RejectIf().IsLessThanOrEqualTo(0, nameof(delayDurationInMilliseconds))));

        /// <summary>
        /// Configures the constant length of time to delay between action repetitions.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="delayDuration">
        /// The constant length of time to delay between action repetitions.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayDuration" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithConstantDelay(TimeSpan delayDuration) => WithDelay(delayDuration, ActionRepeaterDelayScaleFunction.Constant);

        /// <summary>
        /// Configures the initial length of time to delay between action repetitions, with subsequent delays growing at an
        /// accelerating, geometric cadence.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="initialDelayDurationInMilliseconds">
        /// The constant length of time, in milliseconds, that serves as the functional input for scaling of successive delays.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDurationInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithGeometricScalingDelay(Int32 initialDelayDurationInMilliseconds) => WithGeometricScalingDelay(TimeSpan.FromMilliseconds(initialDelayDurationInMilliseconds.RejectIf().IsLessThanOrEqualTo(0, nameof(initialDelayDurationInMilliseconds))));

        /// <summary>
        /// Configures the initial length of time to delay between action repetitions, with subsequent delays growing at an
        /// accelerating, geometric cadence.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="initialDelayDuration">
        /// The constant length of time that serves as the functional input for scaling of successive delays.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDuration" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithGeometricScalingDelay(TimeSpan initialDelayDuration) => WithDelay(initialDelayDuration, ActionRepeaterDelayScaleFunction.Geometric);

        /// <summary>
        /// Configures the maximum number of times that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the action repeater will repeat the specified action.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRepetitionCount" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithMaximumRepetitionCount(Int32 maximumRepetitionCount) => WithMaximumRepetitionCount(maximumRepetitionCount, DefaultRaisesExceptionValue);

        /// <summary>
        /// Configures the maximum number of times that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the action repeater will repeat the specified action.
        /// </param>
        /// <param name="raisesException">
        /// A value indicating whether or not the action repeater raises a <see cref="TimeoutException" /> when the timeout
        /// threshold or maximum repetition count is exceeded. The default value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRepetitionCount" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithMaximumRepetitionCount(Int32 maximumRepetitionCount, Boolean raisesException) => WithMaximumRepetitionCount(maximumRepetitionCount, raisesException ? ActionRepeaterTerminalBehavior.RaiseException : ActionRepeaterTerminalBehavior.Desist);

        /// <summary>
        /// Configures the maximum amount of time that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="timeoutThresholdInMilliseconds">
        /// A duration, in milliseconds, that defines the maximum amount of time that the action repeater will repeat the specified
        /// action.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThresholdInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(Int32 timeoutThresholdInMilliseconds) => WithTimeoutThreshold(timeoutThresholdInMilliseconds, DefaultRaisesExceptionValue);

        /// <summary>
        /// Configures the maximum amount of time that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="timeoutThreshold">
        /// A duration that defines the maximum amount of time that the action repeater will repeat the specified action.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(TimeSpan timeoutThreshold) => WithTimeoutThreshold(timeoutThreshold, DefaultRaisesExceptionValue);

        /// <summary>
        /// Configures the maximum amount of time that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="timeoutThresholdInMilliseconds">
        /// A duration, in milliseconds, that defines the maximum amount of time that the action repeater will repeat the specified
        /// action.
        /// </param>
        /// <param name="raisesException">
        /// A value indicating whether or not the action repeater raises a <see cref="TimeoutException" /> when the timeout
        /// threshold or maximum repetition count is exceeded. The default value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThresholdInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(Int32 timeoutThresholdInMilliseconds, Boolean raisesException) => WithTimeoutThreshold(TimeSpan.FromMilliseconds(timeoutThresholdInMilliseconds.RejectIf().IsLessThanOrEqualTo(0, nameof(timeoutThresholdInMilliseconds))), raisesException);

        /// <summary>
        /// Configures the maximum amount of time that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="timeoutThreshold">
        /// A duration that defines the maximum amount of time that the action repeater will repeat the specified action.
        /// </param>
        /// <param name="raisesException">
        /// A value indicating whether or not the action repeater raises a <see cref="TimeoutException" /> when the timeout
        /// threshold or maximum repetition count is exceeded. The default value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(TimeSpan timeoutThreshold, Boolean raisesException) => WithTimeoutThreshold(timeoutThreshold, raisesException ? ActionRepeaterTerminalBehavior.RaiseException : ActionRepeaterTerminalBehavior.Desist);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ActionRepeaterBuilder" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Configures the initial length of time to delay between action repetitions, with subsequent delays growing at a specified
        /// cadence.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing delay configuration.
        /// </remarks>
        /// <param name="delayScaleConstant">
        /// The constant length of time that serves as the functional input for scaling of successive delays.
        /// </param>
        /// <param name="delayScaleFunction">
        /// A function that is used to scale successive delays.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayScaleConstant" /> is less than or equal to <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="delayScaleFunction" /> is equal to <see cref="ActionRepeaterDelayScaleFunction.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private IActionRepeaterBuilder WithDelay(TimeSpan delayScaleConstant, ActionRepeaterDelayScaleFunction delayScaleFunction)
        {
            _ = delayScaleConstant.RejectIf().IsLessThanOrEqualTo(TimeSpan.Zero, nameof(delayScaleConstant));
            _ = delayScaleFunction.RejectIf().IsEqualToValue(ActionRepeaterDelayScaleFunction.Unspecified, nameof(delayScaleFunction));
            Configure(configuration =>
            {
                configuration.Behavior.DelayScaleConstant = delayScaleConstant;
                configuration.Behavior.DelayScaleFunction = delayScaleFunction;
            });

            return this;
        }

        /// <summary>
        /// Configures the maximum number of times that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="maximumRepetitionCount">
        /// The maximum number of times that the action repeater will repeat the specified action.
        /// </param>
        /// <param name="terminalBehavior">
        /// A value indicating whether or not the action repeater raises a <see cref="TimeoutException" /> when the timeout
        /// threshold or maximum repetition count is exceeded. The default value is <see langword="false" />.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRepetitionCount" /> is less than or equal to zero (0) -or- <paramref name="terminalBehavior" />
        /// is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private IActionRepeaterBuilder WithMaximumRepetitionCount(Int32 maximumRepetitionCount, ActionRepeaterTerminalBehavior terminalBehavior)
        {
            _ = maximumRepetitionCount.RejectIf().IsLessThanOrEqualTo(0, nameof(maximumRepetitionCount));
            _ = terminalBehavior.RejectIf().IsEqualToValue(ActionRepeaterTerminalBehavior.Unspecified, nameof(terminalBehavior));
            Configure(configuration =>
            {
                configuration.Behavior.TerminalBehavior = terminalBehavior;
                configuration.Behavior.MaximumRepititionCount = maximumRepetitionCount;
            });

            return this;
        }

        /// <summary>
        /// Configures the maximum amount of time that the action repeater will repeat the specified action before stopping or
        /// raising a <see cref="TimeoutException" />.
        /// </summary>
        /// <remarks>
        /// Subsequent invocations of this method will replace the existing timeout configuration.
        /// </remarks>
        /// <param name="timeoutThreshold">
        /// A duration that defines the maximum amount of time that the action repeater will repeat the specified action.
        /// </param>
        /// <param name="terminalBehavior">
        /// A value indicating whether or not the action repeater raises a <see cref="TimeoutException" /> when the timeout
        /// threshold or maximum repetition count is exceeded.
        /// </param>
        /// <returns>
        /// The current <see cref="ActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> -or-
        /// <paramref name="terminalBehavior" /> is equal to <see cref="ActionRepeaterTerminalBehavior.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private IActionRepeaterBuilder WithTimeoutThreshold(TimeSpan timeoutThreshold, ActionRepeaterTerminalBehavior terminalBehavior)
        {
            _ = timeoutThreshold.RejectIf().IsLessThanOrEqualTo(TimeSpan.Zero, nameof(timeoutThreshold));
            _ = terminalBehavior.RejectIf().IsEqualToValue(ActionRepeaterTerminalBehavior.Unspecified, nameof(terminalBehavior));
            Configure(configuration =>
            {
                configuration.Behavior.TerminalBehavior = terminalBehavior;
                configuration.Behavior.TimeoutThreshold = timeoutThreshold;
            });

            return this;
        }

        /// <summary>
        /// Represents the default value indicating whether or not action repeaters raise a <see cref="TimeoutException" /> when the
        /// timeout threshold or maximum repetition count is exceeded.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultRaisesExceptionValue = false;
    }
}