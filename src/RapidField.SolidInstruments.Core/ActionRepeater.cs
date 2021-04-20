// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a utility that performs configurable, scaling action repetition.
    /// </summary>
    /// <remarks>
    /// <see cref="ActionRepeater" /> is the default implementation of <see cref="IActionRepeater" />.
    /// </remarks>
    public sealed class ActionRepeater : BuildableInstrument<ActionRepeaterConfiguration>, IActionRepeater
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionRepeater" /> class.
        /// </summary>
        [DebuggerHidden]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ActionRepeater()
            : base()
        {
            DelayDurationNMinusOne = InitialDelayDuration;
            DelayDurationNMinusTwo = InitialDelayDuration;
            LazyMaximumRepititionCount = new(InitializeMaximumRepititionCount, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyTimeoutThreshold = new(InitializeTimeoutThreshold, LazyThreadSafetyMode.ExecutionAndPublication);
            RepititionCount = InitialRepetitionCount;
            RunStopwatch = new();
        }

        /// <summary>
        /// Configures and runs an <see cref="ActionRepeater" />.
        /// </summary>
        /// <param name="buildAction">
        /// An action that configures the <see cref="ActionRepeater" />.
        /// </param>
        /// <exception cref="Exception">
        /// The configured predicate or action raised an exception.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The builder was disposed during configuration.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The maximum repetition count and/or timeout threshold were exceeded.
        /// </exception>
        public static void BuildAndRun(Action<IActionRepeaterBuilder> buildAction)
        {
            _ = buildAction.RejectIf().IsNull(nameof(buildAction));
            using var actionRepeaterBuilder = new ActionRepeaterBuilder();
            buildAction(actionRepeaterBuilder);
            using var actionRepeater = actionRepeaterBuilder.ToResult();
            actionRepeater.Run();
        }

        /// <summary>
        /// Asynchronously configures and runs an <see cref="ActionRepeater" />.
        /// </summary>
        /// <param name="buildAction">
        /// An action that configures the <see cref="ActionRepeater" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exception">
        /// The configured predicate or action raised an exception.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The builder was disposed during configuration.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The maximum repetition count and/or timeout threshold were exceeded.
        /// </exception>
        public static Task BuildAndRunAsync(Action<IActionRepeaterBuilder> buildAction) => Task.Factory.StartNew(() => BuildAndRun(buildAction));

        /// <summary>
        /// Executes a configured action repeatedly until the specified predicate returns <see langword="false" /> or until the
        /// specified terminal conditions are met.
        /// </summary>
        /// <exception cref="Exception">
        /// The configured predicate or action raised an exception.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The maximum repetition count and/or timeout threshold were exceeded.
        /// </exception>
        public void Run() => WithStateControl(() =>
        {
            try
            {
                Start();

                while (TerminalConditionsAreNotMet)
                {
                    RejectIfDisposed();
                    PerformAction();
                }
            }
            finally
            {
                Reset();
            }
        });

        /// <summary>
        /// Asynchronously executes a configured action repeatedly until the specified predicate returns <see langword="false" /> or
        /// until the specified terminal conditions are met.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exception">
        /// The configured predicate or action raised an exception.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The maximum repetition count and/or timeout threshold were exceeded.
        /// </exception>
        public Task RunAsync() => Task.Factory.StartNew(Run);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ActionRepeater" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                Reset();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Suspends the current thread for <see cref="DelayDuration" /> and records new values for
        /// <see cref="DelayDurationNMinusOne" /> and <see cref="DelayDurationNMinusTwo" />.
        /// </summary>
        [DebuggerHidden]
        private void Delay()
        {
            var delayDuration = DelayDuration;

            try
            {
                if (delayDuration > TimeSpan.Zero)
                {
                    Thread.Sleep(delayDuration);
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                throw new UnsupportedSpecificationException(delayDuration, $"The resulting delay duration is too long: {delayDuration}", exception);
            }
            finally
            {
                DelayDurationNMinusTwo = DelayDurationNMinusOne;
                DelayDurationNMinusOne = delayDuration;
            }
        }

        /// <summary>
        /// Initializes the maximum number of times that the current <see cref="ActionRepeater" /> will perform an action.
        /// </summary>
        [DebuggerHidden]
        private Int32 InitializeMaximumRepititionCount() => Behavior.MaximumRepititionCount == ActionRepeaterBehavior.InfiniteRepititionCount ? Int32.MaxValue : Behavior.MaximumRepititionCount;

        /// <summary>
        /// Initializes the maximum length of time that the current <see cref="ActionRepeater" /> will repeat an action.
        /// </summary>
        /// <returns>
        /// The maximum length of time that the current <see cref="ActionRepeater" /> will repeat an action.
        /// </returns>
        [DebuggerHidden]
        private TimeSpan InitializeTimeoutThreshold() => Behavior.TimeoutThreshold == ActionRepeaterBehavior.InfiniteTimeoutThreshold ? TimeSpan.MaxValue : Behavior.TimeoutThreshold;

        /// <summary>
        /// Executes <see cref="Delay" />, then <see cref="RepeatedAction" /> and iterates <see cref="RepititionCount" />.
        /// </summary>
        /// <exception cref="Exception">
        /// <see cref="RepeatedAction" /> raised an exception.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The action repeater configuration resulted in very long delay durations.
        /// </exception>
        [DebuggerHidden]
        private void PerformAction()
        {
            try
            {
                try
                {
                    Delay();
                }
                catch (UnsupportedSpecificationException exception)
                {
                    throw new TimeoutException("The action repeater configuration resulted in very long delay durations.", exception);
                }

                RepeatedAction();
            }
            finally
            {
                RepititionCount++;
            }
        }

        /// <summary>
        /// Resets <see cref="RunStopwatch" /> and sets <see cref="RepititionCount" /> to zero (0).
        /// </summary>
        [DebuggerHidden]
        private void Reset()
        {
            DelayDurationNMinusOne = InitialDelayDuration;
            DelayDurationNMinusTwo = InitialDelayDuration;
            RepititionCount = InitialRepetitionCount;
            RunStopwatch.Reset();
        }

        /// <summary>
        /// Starts <see cref="RunStopwatch" />.
        /// </summary>
        [DebuggerHidden]
        private void Start()
        {
            Reset();
            RunStopwatch.Start();
        }

        /// <summary>
        /// Gets the information that defines the behavior of the current <see cref="ActionRepeater" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IActionRepeaterBehavior Behavior => Configuration.Behavior;

        /// <summary>
        /// Gets the constant length of time that serves as the functional input for scaling of successive delays.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan DelayScaleConstant => Behavior.DelayScaleConstant;

        /// <summary>
        /// Gets the function that is used to scale successive delays.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ActionRepeaterDelayScaleFunction DelayScaleFunction => Behavior.DelayScaleFunction;

        /// <summary>
        /// Gets a predicate function that defines the conditions for <see cref="RepeatedAction" /> repetition.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<Boolean> Predicate => Configuration.Predicate;

        /// <summary>
        /// Gets the action that is performed repetitively by the current <see cref="ActionRepeater" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action RepeatedAction => Configuration.RepeatedAction;

        /// <summary>
        /// Gets the behavior of the current <see cref="ActionRepeater" /> after it has exhausted its timeout threshold or maximum
        /// repetition count.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ActionRepeaterTerminalBehavior TerminalBehavior => Behavior.TerminalBehavior;

        /// <summary>
        /// Gets the current length of time to wait between permutations, or <see cref="DelayScaleConstant" /> if <see cref="Run" />
        /// is not executing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal TimeSpan DelayDuration => RepititionCount == InitialRepetitionCount ? InitialDelayDuration : DelayScaleFunction switch
        {
            ActionRepeaterDelayScaleFunction.Constant => DelayScaleConstant,
            ActionRepeaterDelayScaleFunction.Arithmetic => DelayDurationNMinusOne + DelayScaleConstant,
            ActionRepeaterDelayScaleFunction.Geometric => RepititionCount == (InitialRepetitionCount + 1) ? DelayScaleConstant : DelayDurationNMinusOne + DelayDurationNMinusTwo,
            _ => throw new UnsupportedSpecificationException(UnsupportedDelayScaleFunctionExceptionMessage)
        };

        /// <summary>
        /// Gets or sets the number of times that the current <see cref="ActionRepeater" /> has performed
        /// <see cref="RepeatedAction" />, or zero (0) if <see cref="Run" /> is not executing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Int32 RepititionCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets an exception message that indicates that <see cref="MaximumRepititionCount" /> was exceeded.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String MaximumRepetitionCountExceededExceptionMessage => $"The maximum repetition count, {MaximumRepititionCount}, was exceeded.";

        /// <summary>
        /// Gets the maximum number of times that the current <see cref="ActionRepeater" /> will perform an action.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 MaximumRepititionCount => LazyMaximumRepititionCount.Value;

        /// <summary>
        /// Gets a value indicating whether or not <see cref="RepititionCount" /> has exceeded
        /// <see cref="MaximumRepititionCount" />.
        /// </summary>
        /// <exception cref="TimeoutException">
        /// <see cref="RepititionCount" /> has exceeded <see cref="MaximumRepititionCount" /> and <see cref="TerminalBehavior" /> is
        /// equal to <see cref="ActionRepeaterTerminalBehavior.RaiseException" />.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean MaximumRepititionCountIsExceeded => RepititionCount <= MaximumRepititionCount ? false : TerminalBehavior switch
        {
            ActionRepeaterTerminalBehavior.Desist => true,
            ActionRepeaterTerminalBehavior.RaiseException => throw new TimeoutException(MaximumRepetitionCountExceededExceptionMessage),
            _ => throw new UnsupportedSpecificationException(UnsupportedTerminalBehaviorExceptionMessage)
        };

        /// <summary>
        /// Gets the duration of the current invocation of <see cref="Run" />, or <see cref="TimeSpan.Zero" /> if <see cref="Run" />
        /// is not executing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan RunDuration => RunStopwatch.IsRunning ? RunStopwatch.Elapsed : InitialDelayDuration;

        /// <summary>
        /// Gets a value indicating whether or not <see cref="Predicate" /> is <see langword="true" />,
        /// <see cref="MaximumRepititionCountIsExceeded" /> is <see langword="false" /> and
        /// <see cref="TimeoutThresholdIsExceeded" /> is <see langword="false" />.
        /// </summary>
        /// <exception cref="Exception">
        /// <see cref="Predicate" /> raised an exception.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// <see cref="RepititionCount" /> has exceeded <see cref="MaximumRepititionCount" /> and <see cref="TerminalBehavior" /> is
        /// equal to <see cref="ActionRepeaterTerminalBehavior.RaiseException" /> -or- <see cref="RunDuration" /> has exceeded
        /// <see cref="TimeoutThreshold" /> and <see cref="TerminalBehavior" /> is equal to
        /// <see cref="ActionRepeaterTerminalBehavior.RaiseException" />.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean TerminalConditionsAreNotMet => Predicate() && MaximumRepititionCountIsExceeded is false && TimeoutThresholdIsExceeded is false;

        /// <summary>
        /// Gets the maximum length of time that the current <see cref="ActionRepeater" /> will repeat an action.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan TimeoutThreshold => LazyTimeoutThreshold.Value;

        /// <summary>
        /// Gets an exception message that indicates that <see cref="TimeoutThreshold" /> was exceeded.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String TimeoutThresholdExceededExceptionMessage => $"The timeout threshold, {TimeoutThreshold}, was exceeded.";

        /// <summary>
        /// Gets a value indicating whether or not <see cref="RunDuration" /> has exceeded <see cref="TimeoutThreshold" />.
        /// </summary>
        /// <exception cref="TimeoutException">
        /// <see cref="RunDuration" /> has exceeded <see cref="TimeoutThreshold" /> and <see cref="TerminalBehavior" /> is equal to
        /// <see cref="ActionRepeaterTerminalBehavior.RaiseException" />.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean TimeoutThresholdIsExceeded => RunDuration <= TimeoutThreshold ? false : TerminalBehavior switch
        {
            ActionRepeaterTerminalBehavior.Desist => true,
            ActionRepeaterTerminalBehavior.RaiseException => throw new TimeoutException(TimeoutThresholdExceededExceptionMessage),
            _ => throw new UnsupportedSpecificationException(UnsupportedTerminalBehaviorExceptionMessage)
        };

        /// <summary>
        /// Gets an exception message that indicates that <see cref="DelayScaleFunction" /> is an unsupported delay scale function.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String UnsupportedDelayScaleFunctionExceptionMessage => $"The specified delay scale function, {DelayScaleFunction}, is not supported.";

        /// <summary>
        /// Gets an exception message that indicates that <see cref="TerminalBehavior" /> is an unsupported terminal behavior.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String UnsupportedTerminalBehaviorExceptionMessage => $"The specified terminal behavior, {TerminalBehavior}, is not supported.";

        /// <summary>
        /// Represents the immediately previous delay duration, or <see cref="TimeSpan.Zero" /> if <see cref="Run" /> is not
        /// executing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal TimeSpan DelayDurationNMinusOne;

        /// <summary>
        /// Represents the second previous delay duration, or <see cref="TimeSpan.Zero" /> if <see cref="Run" /> is not executing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal TimeSpan DelayDurationNMinusTwo;

        /// <summary>
        /// Represents the initial number of times that an <see cref="ActionRepeater" /> has performed
        /// <see cref="RepeatedAction" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 InitialRepetitionCount = 0;

        /// <summary>
        /// Represents the initial length of time to wait between permutations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan InitialDelayDuration = TimeSpan.Zero;

        /// <summary>
        /// Represents the lazily-initialized maximum number of times that the current <see cref="ActionRepeater" /> will perform an
        /// action.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Int32> LazyMaximumRepititionCount;

        /// <summary>
        /// Represents the lazily-initialized maximum length of time that the current <see cref="ActionRepeater" /> will repeat an
        /// action.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TimeSpan> LazyTimeoutThreshold;

        /// <summary>
        /// Represents a stopwatch that measures the duration of <see cref="Run" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Stopwatch RunStopwatch;
    }
}