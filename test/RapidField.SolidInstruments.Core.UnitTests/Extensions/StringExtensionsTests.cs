// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ApplyFormat_ShouldReturnValidResult()
        {
            // Arrange.
            var target = "{0} | {1} | {2} | {3} | {4}";

            // Act.
            var result = target.ApplyFormat("Foo", 42, null, String.Empty, new Guid("f41ec631-33ac-481f-807a-47ec1295e3a6"));

            // Assert.
            result.Should().Be("Foo | 42 |  |  | f41ec631-33ac-481f-807a-47ec1295e3a6");
        }

        [TestMethod]
        public void Compress_ShouldReturnValidResult()
        {
            // Arrange.
            var target = "This			   is  a		    sentence.";

            // Act.
            var result = target.Compress();

            // Assert.
            result.Should().Be("This is a sentence.");
        }

        [TestMethod]
        public void CompressAndCrop_ShouldReturnValidResult()
        {
            // Arrange.
            var target = "1  2       3   4   5   6      7 8   9 0.xRh    u4m ar5   B      em7 kl   SFjfd s  a1j";

            // Act.
            var result = target.CompressAndCrop(20);

            // Assert.
            result.Should().Be("1 2 3 4 5 6 7 8 9 0.");
        }

        [TestMethod]
        public void Crop_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = "ABCDEFGHIJ";
            var targetTwo = " ABC DEFGHIJ ";
            var targetThree = " ABCDEF G HIJ";

            // Act.
            var resultOne = targetOne.Crop(7);
            var resultTwo = targetTwo.Crop(7);
            var resultThree = targetThree.Crop(7);

            // Assert.
            resultOne.Should().Be("ABCDEFG");
            resultTwo.Should().Be("ABC DEF");
            resultThree.Should().Be("ABCDEF");
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = (String)null;
            var targetTwo = String.Empty;
            var targetThree = "foo";

            // Act.
            var resultOne = targetOne.IsNullOrEmpty();
            var resultTwo = targetTwo.IsNullOrEmpty();
            var resultThree = targetThree.IsNullOrEmpty();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
        }

        [TestMethod]
        public void MatchesRegularExpression_ShouldRaiseArgumentEmptyException_ForEmptyExpression()
        {
            // Arrange.
            var target = "Foo1234";
            var regularExpressionPattern = String.Empty;
            var expectedExceptionResultType = typeof(ArgumentEmptyException);

            // Act.
            var action = new Action(() =>
            {
                var result = target.MatchesRegularExpression(regularExpressionPattern);
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>($"because {nameof(regularExpressionPattern)} is empty");
        }

        [TestMethod]
        public void MatchesRegularExpression_ShouldRaiseArgumentException_ForInvalidExpression()
        {
            // Arrange.
            var target = "Foo1234";
            var regularExpressionPattern = "[";

            // Act.
            var action = new Action(() =>
            {
                var result = target.MatchesRegularExpression(regularExpressionPattern);
            });

            // Assert.
            action.Should().Throw<ArgumentException>($"because {nameof(regularExpressionPattern)} is invalid");
        }

        [TestMethod]
        public void MatchesRegularExpression_ShouldRaiseArgumentNullException_ForNullExpression()
        {
            // Arrange.
            var target = "Foo1234";
            var regularExpressionPattern = (String)null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.MatchesRegularExpression(regularExpressionPattern);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(regularExpressionPattern)} is null");
        }

        [TestMethod]
        public void MatchesRegularExpression_ShouldReturnFalse_ForNonMatchingExpression()
        {
            // Arrange.
            var target = "Foo1234_";
            var regularExpressionPattern = "^[A-Za-z0-9]+$";

            // Act.
            var result = target.MatchesRegularExpression(regularExpressionPattern);

            // Assert.
            result.Should().BeFalse();
        }

        [TestMethod]
        public void MatchesRegularExpression_ShouldReturnTrue_ForMatchingExpression()
        {
            // Arrange.
            var target = "Foo1234";
            var regularExpressionPattern = "^[A-Za-z0-9]+$";

            // Act.
            var result = target.MatchesRegularExpression(regularExpressionPattern);

            // Assert.
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Solidify_ShouldReturnValidResult()
        {
            // Arrange.
            var target = " f o   o       fo  o  ";

            // Act.
            var result = target.Solidify();

            // Assert.
            result.Should().Be("foofoo");
        }

        [TestMethod]
        public void SolidifyAndCrop_ShouldReturnValidResult()
        {
            // Arrange.
            var target = " f o   o       fo  o  ";

            // Act.
            var result = target.SolidifyAndCrop(4);

            // Assert.
            result.Should().Be("foof");
        }
    }
}