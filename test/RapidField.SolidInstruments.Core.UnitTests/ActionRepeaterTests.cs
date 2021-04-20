// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class ActionRepeaterTests
    {
        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055_WithMaximumRepetitionCount_0002_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0002_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055_WithMaximumRepetitionCount_0003_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055_WithTimeoutThreshold_0233_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055_WithTimeoutThreshold_4181_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_4181_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055_WithMaximumRepetitionCount_0002_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0002_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055_WithMaximumRepetitionCount_0003_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055_WithTimeoutThreshold_0233_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055_WithTimeoutThreshold_4181_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_4181_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055_WithMaximumRepetitionCount_0002_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0002_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055_WithMaximumRepetitionCount_0003_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055_WithTimeoutThreshold_0233_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055_WithTimeoutThreshold_4181_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_4181_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithArithmeticDelay_0013_WithMaximumRepetitionCount_0008_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithArithmeticDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0008_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithArithmeticDelay_0013_WithMaximumRepetitionCount_0013_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithArithmeticDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0013_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithConstantDelay_0013_WithMaximumRepetitionCount_0008_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithConstantDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0008_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithConstantDelay_0013_WithMaximumRepetitionCount_0013_RaisesException_True() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithConstantDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0013_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithGeometricDelay_0013_WithMaximumRepetitionCount_0008_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithGeometricDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0008_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055_WithArithmeticDelay_0003_WithMaximumRepetitionCount_0034_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055_WithArithmeticDelay_0003(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0034_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055_WithConstantDelay_0003_WithMaximumRepetitionCount_0034_RaisesException_False() => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055_WithConstantDelay_0003(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0034_RaisesException_False);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithArithmeticDelay_0055_WithMaximumRepetitionCount_0002_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithArithmeticDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithArithmeticDelay_0055_WithTimeoutThreshold_0233_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithArithmeticDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithConstantDelay_0055_WithMaximumRepetitionCount_0002_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithConstantDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithConstantDelay_0055_WithTimeoutThreshold_0233_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithConstantDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithGeometricDelay_0055_WithMaximumRepetitionCount_0002_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithGeometricDelay_0055(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithGeometricDelay_0055_WithTimeoutThreshold_0233_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithGeometricDelay_0055(TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithArithmeticDelay_0013_WithMaximumRepetitionCount_0008_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithArithmeticDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0013_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithConstantDelay_0013_WithMaximumRepetitionCount_0008_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithConstantDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0013_RaisesException_True);

        [TestMethod]
        public void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithGeometricDelay_0013_WithMaximumRepetitionCount_0008_RaisesException_True() => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithGeometricDelay_0013(TimeoutConfigurationFunction_WithMaximumRepetitionCount_0013_RaisesException_True);

        private static void ArrangeCounterExercise(Int32 permutationLimit, out Func<Boolean> predicate, out Action repeatedAction)
        {
            // Arrange.
            var permutationCount = 0;
            predicate = () => permutationCount <= permutationLimit;
            repeatedAction = () =>
            {
                // Assert.
                permutationCount.Should().BeLessOrEqualTo(permutationLimit);
                permutationCount++;
            };
        }

        private static void BuildAndRun_ShouldNotRaiseException(Func<Boolean> predicate, Action repeatedAction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldProduceDesiredResults<Exception>(predicate, repeatedAction, delayConfigurationFunction, timeoutConfigurationFunction, Constant_False);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise(Int32 permutationLimit, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction)
        {
            // Arrange.
            ArrangeCounterExercise(permutationLimit, out var predicate, out var repeatedAction);

            // Act.
            BuildAndRun_ShouldNotRaiseException(predicate, repeatedAction, delayConfigurationFunction, timeoutConfigurationFunction);
        }

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise(Constant_0003, delayConfigurationFunction, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithArithmeticDelay_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003(DelayConfigurationFunction_WithArithmeticDelay_0055, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithConstantDelay_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003(DelayConfigurationFunction_WithConstantDelay_0055, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003_WithGeometricDelay_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0003(DelayConfigurationFunction_WithGeometricDelay_0055, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise(Constant_0013, delayConfigurationFunction, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithArithmeticDelay_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013(DelayConfigurationFunction_WithArithmeticDelay_0013, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithConstantDelay_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013(DelayConfigurationFunction_WithConstantDelay_0013, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013_WithGeometricDelay_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0013(DelayConfigurationFunction_WithGeometricDelay_0013, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise(Constant_0055, delayConfigurationFunction, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055_WithArithmeticDelay_0003(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055(DelayConfigurationFunction_WithArithmeticDelay_0003, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055_WithConstantDelay_0003(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldNotRaiseException_ForCounterExercise_UsingPermutationCount_0055(DelayConfigurationFunction_WithConstantDelay_0003, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldProduceDesiredResults<TException>(Func<Boolean> predicate, Action repeatedAction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction, Boolean shouldRaiseException)
            where TException : Exception => BuildAndRun_ShouldProduceDesiredResults<TException>(builder => delayConfigurationFunction(timeoutConfigurationFunction(builder)).While(predicate).Repeat(repeatedAction), shouldRaiseException);

        private static void BuildAndRun_ShouldProduceDesiredResults<TException>(Action<IActionRepeaterBuilder> buildAction, Boolean shouldRaiseException)
            where TException : Exception
        {
            // Arrange.
            Action testAction = () => ActionRepeater.BuildAndRun(buildAction);

            if (shouldRaiseException)
            {
                // Assert.
                testAction.Should().Throw<TException>();
            }
            else
            {
                // Assert.
                testAction.Should().NotThrow();
            }
        }

        private static void BuildAndRun_ShouldRaiseException<TException>(Func<Boolean> predicate, Action repeatedAction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction)
            where TException : Exception => BuildAndRun_ShouldProduceDesiredResults<TException>(predicate, repeatedAction, delayConfigurationFunction, timeoutConfigurationFunction, Constant_True);

        private static void BuildAndRun_ShouldRaiseTimeoutException(Func<Boolean> predicate, Action repeatedAction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseException<TimeoutException>(predicate, repeatedAction, delayConfigurationFunction, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise(Int32 permutationLimit, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction)
        {
            // Arrange.
            ArrangeCounterExercise(permutationLimit, out var predicate, out var repeatedAction);

            // Act.
            BuildAndRun_ShouldRaiseTimeoutException(predicate, repeatedAction, delayConfigurationFunction, timeoutConfigurationFunction);
        }

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise(Constant_0005, delayConfigurationFunction, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithArithmeticDelay_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005(DelayConfigurationFunction_WithArithmeticDelay_0055, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithConstantDelay_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005(DelayConfigurationFunction_WithConstantDelay_0055, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005_WithGeometricDelay_0055(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0005(DelayConfigurationFunction_WithGeometricDelay_0055, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> delayConfigurationFunction, Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise(Constant_0021, delayConfigurationFunction, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithArithmeticDelay_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021(DelayConfigurationFunction_WithArithmeticDelay_0013, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithConstantDelay_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021(DelayConfigurationFunction_WithConstantDelay_0013, timeoutConfigurationFunction);

        private static void BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021_WithGeometricDelay_0013(Func<IActionRepeaterBuilder, IActionRepeaterBuilder> timeoutConfigurationFunction) => BuildAndRun_ShouldRaiseTimeoutException_ForCounterExercise_UsingPermutationCount_0021(DelayConfigurationFunction_WithGeometricDelay_0013, timeoutConfigurationFunction);

        private const Int32 Constant_0002 = 2;
        private const Int32 Constant_0003 = 3;
        private const Int32 Constant_0005 = 5;
        private const Int32 Constant_0008 = 8;
        private const Int32 Constant_0013 = 13;
        private const Int32 Constant_0021 = 21;
        private const Int32 Constant_0034 = 34;
        private const Int32 Constant_0055 = 55;
        private const Int32 Constant_0233 = 233;
        private const Int32 Constant_4181 = 4181;
        private const Boolean Constant_False = false;
        private const Boolean Constant_True = true;
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithArithmeticDelay_0003 = builder => builder.WithArithmeticScalingDelay(Constant_0003);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithArithmeticDelay_0013 = builder => builder.WithArithmeticScalingDelay(Constant_0013);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithArithmeticDelay_0055 = builder => builder.WithArithmeticScalingDelay(Constant_0055);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithConstantDelay_0003 = builder => builder.WithConstantDelay(Constant_0003);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithConstantDelay_0013 = builder => builder.WithConstantDelay(Constant_0013);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithConstantDelay_0055 = builder => builder.WithConstantDelay(Constant_0055);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithGeometricDelay_0013 = builder => builder.WithGeometricScalingDelay(Constant_0013);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> DelayConfigurationFunction_WithGeometricDelay_0055 = builder => builder.WithGeometricScalingDelay(Constant_0055);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithMaximumRepetitionCount_0002_RaisesException_False = builder => builder.WithMaximumRepetitionCount(Constant_0002, Constant_False);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithMaximumRepetitionCount_0003_RaisesException_True = builder => builder.WithMaximumRepetitionCount(Constant_0003, Constant_True);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithMaximumRepetitionCount_0008_RaisesException_False = builder => builder.WithMaximumRepetitionCount(Constant_0008, Constant_False);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithMaximumRepetitionCount_0013_RaisesException_True = builder => builder.WithMaximumRepetitionCount(Constant_0013, Constant_True);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithMaximumRepetitionCount_0034_RaisesException_False = builder => builder.WithMaximumRepetitionCount(Constant_0034, Constant_False);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_False = builder => builder.WithTimeoutThreshold(Constant_0233, Constant_False);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithTimeoutThreshold_0233_RaisesException_True = builder => builder.WithTimeoutThreshold(Constant_0233, Constant_True);
        private static readonly Func<IActionRepeaterBuilder, IActionRepeaterBuilder> TimeoutConfigurationFunction_WithTimeoutThreshold_4181_RaisesException_True = builder => builder.WithTimeoutThreshold(Constant_4181, Constant_True);
    }
}