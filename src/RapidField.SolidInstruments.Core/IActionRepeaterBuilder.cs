// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that configures and produces new <see cref="ActionRepeater" /> instances.
    /// </summary>
    public interface IActionRepeaterBuilder : IInstrumentBuilder<ActionRepeaterConfiguration, ActionRepeater>
    {
        /// <summary>
        /// Configures an action that will be repeated.
        /// </summary>
        /// <param name="repeatedAction">
        /// An action that will be repeated.
        /// </param>
        /// <returns>
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repeatedAction" /> is <see langword="null" />.
        /// </exception>
        public IActionRepeaterBuilder Repeat(Action repeatedAction);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        public IActionRepeaterBuilder While(Func<Boolean> predicate);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDurationInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithArithmeticScalingDelay(Int32 initialDelayDurationInMilliseconds);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDuration" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithArithmeticScalingDelay(TimeSpan initialDelayDuration);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayDurationInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithConstantDelay(Int32 delayDurationInMilliseconds);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="delayDuration" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithConstantDelay(TimeSpan delayDuration);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDurationInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithGeometricScalingDelay(Int32 initialDelayDurationInMilliseconds);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialDelayDuration" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithGeometricScalingDelay(TimeSpan initialDelayDuration);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRepetitionCount" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithMaximumRepetitionCount(Int32 maximumRepetitionCount);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maximumRepetitionCount" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithMaximumRepetitionCount(Int32 maximumRepetitionCount, Boolean raisesException);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThresholdInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(Int32 timeoutThresholdInMilliseconds);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(TimeSpan timeoutThreshold);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThresholdInMilliseconds" /> is less than or equal to zero (0).
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(Int32 timeoutThresholdInMilliseconds, Boolean raisesException);

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
        /// The current <see cref="IActionRepeaterBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" />.
        /// </exception>
        public IActionRepeaterBuilder WithTimeoutThreshold(TimeSpan timeoutThreshold, Boolean raisesException);
    }
}