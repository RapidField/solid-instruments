// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Mathematics.Statistics.Extensions;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Statistics.Extensions
{
    [TestClass]
    public sealed class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.ComputeDescriptives();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDecimalCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Decimal?[] { 1m, 2m, 3m, null, 4m };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(2.5m);
            result.Median.Should().Be(2.5m);
            result.Midrange.Should().Be(2.5m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(3m);
            result.Size.Should().Be(4);
            result.StandardDeviation.Should().Be(1.11803398874989m);
            result.Sum.Should().Be(10m);
            result.Variance.Should().Be(1.25m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(1m);
            result.Mean.Should().Be(1m);
            result.Median.Should().Be(1m);
            result.Midrange.Should().Be(1m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(0m);
            result.Size.Should().Be(1);
            result.StandardDeviation.Should().Be(0m);
            result.Sum.Should().Be(1m);
            result.Variance.Should().Be(0m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDecimalCollection_OfSizeSeven()
        {
            // Arrange.
            var target = new Decimal?[] { -2.41m, -0.19m, 0.6m, 13.4m, 29.73m, 41.7m, 84.3m };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(84.3m);
            result.Mean.Should().Be(23.875714285714285714285714286m);
            result.Median.Should().Be(13.4m);
            result.Midrange.Should().Be(40.945m);
            result.Minimum.Should().Be(-2.41m);
            result.Range.Should().Be(86.71m);
            result.Size.Should().Be(7);
            result.StandardDeviation.Should().Be(29.0926327724504m);
            result.Sum.Should().Be(167.13m);
            result.Variance.Should().Be(846.3812816326530612244897959m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { 2m, 3m, null, 4m };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(3m);
            result.Median.Should().Be(3m);
            result.Midrange.Should().Be(3m);
            result.Minimum.Should().Be(2m);
            result.Range.Should().Be(2m);
            result.Size.Should().Be(3);
            result.StandardDeviation.Should().Be(0.816496580927726m);
            result.Sum.Should().Be(9m);
            result.Variance.Should().Be(0.666666666666666666666666667m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDoubleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Double?[] { 1d, 2d, 3d, null, 4d };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(2.5m);
            result.Median.Should().Be(2.5m);
            result.Midrange.Should().Be(2.5m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(3m);
            result.Size.Should().Be(4);
            result.StandardDeviation.Should().Be(1.11803398874989m);
            result.Sum.Should().Be(10m);
            result.Variance.Should().Be(1.25m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(1m);
            result.Mean.Should().Be(1m);
            result.Median.Should().Be(1m);
            result.Midrange.Should().Be(1m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(0m);
            result.Size.Should().Be(1);
            result.StandardDeviation.Should().Be(0m);
            result.Sum.Should().Be(1m);
            result.Variance.Should().Be(0m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDoubleCollection_OfSizeSeven()
        {
            // Arrange.
            var target = new Double?[] { -2.41d, -0.19d, 0.6d, 13.4d, 29.73d, 41.7d, 84.3d };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(84.3m);
            result.Mean.Should().Be(23.8757142857143m);
            result.Median.Should().Be(13.4m);
            result.Midrange.Should().Be(40.945m);
            result.Minimum.Should().Be(-2.41m);
            result.Range.Should().Be(86.71m);
            result.Size.Should().Be(7);
            result.StandardDeviation.Should().Be(29.0926327724504m);
            result.Sum.Should().Be(167.13m);
            result.Variance.Should().Be(846.381281632653m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { 2d, 3d, null, 4d };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(3m);
            result.Median.Should().Be(3m);
            result.Midrange.Should().Be(3m);
            result.Minimum.Should().Be(2m);
            result.Range.Should().Be(2m);
            result.Size.Should().Be(3);
            result.StandardDeviation.Should().Be(0.816496580927726m);
            result.Sum.Should().Be(9m);
            result.Variance.Should().Be(0.666666666666666m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForInt32Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int32?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(2.5m);
            result.Median.Should().Be(2.5m);
            result.Midrange.Should().Be(2.5m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(3m);
            result.Size.Should().Be(4);
            result.StandardDeviation.Should().Be(1.11803398874989m);
            result.Sum.Should().Be(10m);
            result.Variance.Should().Be(1.25m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(1m);
            result.Mean.Should().Be(1m);
            result.Median.Should().Be(1m);
            result.Midrange.Should().Be(1m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(0m);
            result.Size.Should().Be(1);
            result.StandardDeviation.Should().Be(0m);
            result.Sum.Should().Be(1m);
            result.Variance.Should().Be(0m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { 2, 3, null, 4 };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(3m);
            result.Median.Should().Be(3m);
            result.Midrange.Should().Be(3m);
            result.Minimum.Should().Be(2m);
            result.Range.Should().Be(2m);
            result.Size.Should().Be(3);
            result.StandardDeviation.Should().Be(0.816496580927726m);
            result.Sum.Should().Be(9m);
            result.Variance.Should().Be(0.666666666666666m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForInt64Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int64?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(2.5m);
            result.Median.Should().Be(2.5m);
            result.Midrange.Should().Be(2.5m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(3m);
            result.Size.Should().Be(4);
            result.StandardDeviation.Should().Be(1.11803398874989m);
            result.Sum.Should().Be(10m);
            result.Variance.Should().Be(1.25m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(1m);
            result.Mean.Should().Be(1m);
            result.Median.Should().Be(1m);
            result.Midrange.Should().Be(1m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(0m);
            result.Size.Should().Be(1);
            result.StandardDeviation.Should().Be(0m);
            result.Sum.Should().Be(1m);
            result.Variance.Should().Be(0m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { 2, 3, null, 4 };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(3m);
            result.Median.Should().Be(3m);
            result.Midrange.Should().Be(3m);
            result.Minimum.Should().Be(2m);
            result.Range.Should().Be(2m);
            result.Size.Should().Be(3);
            result.StandardDeviation.Should().Be(0.816496580927726m);
            result.Sum.Should().Be(9m);
            result.Variance.Should().Be(0.666666666666666m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForSingleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Single?[] { 1f, 2f, 3f, null, 4f };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(2.5m);
            result.Median.Should().Be(2.5m);
            result.Midrange.Should().Be(2.5m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(3m);
            result.Size.Should().Be(4);
            result.StandardDeviation.Should().Be(1.11803398874989m);
            result.Sum.Should().Be(10m);
            result.Variance.Should().Be(1.25m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(1m);
            result.Mean.Should().Be(1m);
            result.Median.Should().Be(1m);
            result.Midrange.Should().Be(1m);
            result.Minimum.Should().Be(1m);
            result.Range.Should().Be(0m);
            result.Size.Should().Be(1);
            result.StandardDeviation.Should().Be(0m);
            result.Sum.Should().Be(1m);
            result.Variance.Should().Be(0m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForSingleCollection_OfSizeSeven()
        {
            // Arrange.
            var target = new Single?[] { -2.4f, -0.19f, 0.6f, 13.4f, 29.73f, 41.7f, 84.3f };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(84.3m);
            result.Mean.Should().Be(23.8771427699498m);
            result.Median.Should().Be(13.3999996185303m);
            result.Midrange.Should().Be(40.95m);
            result.Minimum.Should().Be(-2.4m);
            result.Range.Should().Be(86.7m);
            result.Size.Should().Be(7);
            result.StandardDeviation.Should().Be(29.0913440699329m);
            result.Sum.Should().Be(167.14m);
            result.Variance.Should().Be(846.306299795221m);
        }

        [TestMethod]
        public void ComputeDescriptives_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { 2f, 3f, null, 4f };

            // Act.
            var result = target.ComputeDescriptives();

            // Assert.
            result.Maximum.Should().Be(4m);
            result.Mean.Should().Be(3m);
            result.Median.Should().Be(3m);
            result.Midrange.Should().Be(3m);
            result.Minimum.Should().Be(2m);
            result.Range.Should().Be(2m);
            result.Size.Should().Be(3);
            result.StandardDeviation.Should().Be(0.816496580927726m);
            result.Sum.Should().Be(9m);
            result.Variance.Should().Be(0.666666666666666m);
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Mean_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Mean();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(1m);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { 2m, 3m, null, 4m };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(3m);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { 2d, 3d, null, 4d };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { 2, 3, null, 4 };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { 2, 3, null, 4 };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Mean_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { 2f, 3f, null, 4f };

            // Act.
            var result = target.Mean();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Median_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Median();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForDecimalCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Decimal?[] { 1m, 2m, 3m, null, 4m };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(2.5m);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(1m);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { 2m, 3m, null, 4m };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(3m);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForDoubleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Double?[] { 1d, 2d, 3d, null, 4d };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(2.5d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { 2d, 3d, null, 4d };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForInt32Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int32?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(2.5d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { 2, 3, null, 4 };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForInt64Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int64?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(2.5d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { 2, 3, null, 4 };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForSingleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Single?[] { 1f, 2f, 3f, null, 4f };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(2.5d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Median_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { 2f, 3f, null, 4f };

            // Act.
            var result = target.Median();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Midrange_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Midrange();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForDecimalCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Decimal?[] { -1m, -2m, -3m, null, -4m };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(-2.5m);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1m);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { -2m, 3m, null, 4m };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1m);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForDoubleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Double?[] { -1d, -2d, -3d, null, -4d };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(-2.5d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { -2d, 3d, null, 4d };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForInt32Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int32?[] { -1, -2, -3, null, -4 };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(-2.5d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { -2, 3, null, 4 };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForInt64Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int64?[] { -1, -2, -3, null, -4 };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(-2.5d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { -2, 3, null, 4 };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForSingleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Single?[] { -1f, -2f, -3f, null, -4f };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(-2.5d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Midrange_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { -2f, 3f, null, 4f };

            // Act.
            var result = target.Midrange();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Range_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Range();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForDecimalCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Decimal?[] { -1m, -2m, -3m, null, -4m };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(3m);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(0m);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { -2m, 3m, null, 4m };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(6m);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForDoubleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Double?[] { -1d, -2d, -3d, null, -4d };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { -2d, 3d, null, 4d };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(6d);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForInt32Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int32?[] { -1, -2, -3, null, -4 };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(3);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { -2, 3, null, 4 };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForInt64Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int64?[] { -1, -2, -3, null, -4 };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(3);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { -2, 3, null, 4 };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForSingleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Single?[] { -1f, -2f, -3f, null, -4f };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(3f);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(0f);
        }

        [TestMethod]
        public void Range_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { -2f, 3f, null, 4f };

            // Act.
            var result = target.Range();

            // Assert.
            result.Should().Be(6f);
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.StandardDeviation();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForDecimalCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Decimal?[] { 1m, 2m, 3m, null, 4m };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(1.11803398874989m);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0m);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { 2m, 3m, null, 4m };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0.816496580927726m);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForDoubleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Double?[] { 1d, 2d, 3d, null, 4d };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(1.1180339887498949d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { 2d, 3d, null, 4d };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0.8164965809277257d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForInt32Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int32?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(1.1180339887498949d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { 2, 3, null, 4 };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0.8164965809277257d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForInt64Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int64?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(1.1180339887498949d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { 2, 3, null, 4 };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0.8164965809277257d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForSingleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Single?[] { 1f, 2f, 3f, null, 4f };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(1.1180339887498949d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void StandardDeviation_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { 2f, 3f, null, 4f };

            // Act.
            var result = target.StandardDeviation();

            // Assert.
            result.Should().Be(0.8164965809277257d);
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentEmptyException_ForEmptyDecimalCollection()
        {
            // Arrange.
            var target = Array.Empty<Decimal>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentEmptyException_ForEmptyDoubleCollection()
        {
            // Arrange.
            var target = Array.Empty<Double>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentEmptyException_ForEmptyInt32Collection()
        {
            // Arrange.
            var target = Array.Empty<Int32>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentEmptyException_ForEmptyInt64Collection()
        {
            // Arrange.
            var target = Array.Empty<Int64>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentEmptyException_ForEmptySingleCollection()
        {
            // Arrange.
            var target = Array.Empty<Single>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentNullException_ForNullDecimalCollection()
        {
            // Arrange.
            var target = (Decimal[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentNullException_ForNullDoubleCollection()
        {
            // Arrange.
            var target = (Double[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentNullException_ForNullInt32Collection()
        {
            // Arrange.
            var target = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentNullException_ForNullInt64Collection()
        {
            // Arrange.
            var target = (Int64[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Variance_ShouldRaiseArgumentNullException_ForNullSingleCollection()
        {
            // Arrange.
            var target = (Single[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Variance();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForDecimalCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Decimal?[] { 1m, 2m, 3m, null, 4m };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(1.25m);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForDecimalCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Decimal[] { 1m };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0m);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForDecimalCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Decimal?[] { 2m, 3m, null, 4m };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0.666666666666666666666666667m);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForDoubleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Double?[] { 1d, 2d, 3d, null, 4d };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(1.25d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForDoubleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Double[] { 1d };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForDoubleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Double?[] { 2d, 3d, null, 4d };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0.66666666666666607d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForInt32Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int32?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(1.25d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForInt32Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int32[] { 1 };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForInt32Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int32?[] { 2, 3, null, 4 };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0.66666666666666607d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForInt64Collection_OfSizeFour()
        {
            // Arrange.
            var target = new Int64?[] { 1, 2, 3, null, 4 };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(1.25d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForInt64Collection_OfSizeOne()
        {
            // Arrange.
            var target = new Int64[] { 1 };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForInt64Collection_OfSizeThree()
        {
            // Arrange.
            var target = new Int64?[] { 2, 3, null, 4 };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0.66666666666666607d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForSingleCollection_OfSizeFour()
        {
            // Arrange.
            var target = new Single?[] { 1f, 2f, 3f, null, 4f };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(1.25d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForSingleCollection_OfSizeOne()
        {
            // Arrange.
            var target = new Single[] { 1f };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void Variance_ShouldReturnValidResult_ForSingleCollection_OfSizeThree()
        {
            // Arrange.
            var target = new Single?[] { 2f, 3f, null, 4f };

            // Act.
            var result = target.Variance();

            // Assert.
            result.Should().Be(0.66666666666666607d);
        }
    }
}