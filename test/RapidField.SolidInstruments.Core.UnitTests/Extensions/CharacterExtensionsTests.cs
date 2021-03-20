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
    public class CharacterExtensionsTests
    {
        [TestMethod]
        public void IsAlphabetic_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsAlphabetic();
            var resultTwo = uppercaseAlphabeticCharacter.IsAlphabetic();
            var resultThree = numericCharacter.IsAlphabetic();
            var resultFour = symbolicCharacer.IsAlphabetic();
            var resultFive = whiteSpaceCharacter.IsAlphabetic();
            var resultSix = controlCharacter.IsAlphabetic();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsAlphabetic();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsAlphabetic();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeFalse();
            resultFive.Should().BeFalse();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
            resultEight.Should().BeTrue();
        }

        [TestMethod]
        public void IsBasicLatin_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsBasicLatin();
            var resultTwo = uppercaseAlphabeticCharacter.IsBasicLatin();
            var resultThree = numericCharacter.IsBasicLatin();
            var resultFour = symbolicCharacer.IsBasicLatin();
            var resultFive = whiteSpaceCharacter.IsBasicLatin();
            var resultSix = controlCharacter.IsBasicLatin();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsBasicLatin();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsBasicLatin();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeTrue();
            resultFour.Should().BeTrue();
            resultFive.Should().BeTrue();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeFalse();
        }

        [TestMethod]
        public void IsControlCharacter_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsControlCharacter();
            var resultTwo = uppercaseAlphabeticCharacter.IsControlCharacter();
            var resultThree = numericCharacter.IsControlCharacter();
            var resultFour = symbolicCharacer.IsControlCharacter();
            var resultFive = whiteSpaceCharacter.IsControlCharacter();
            var resultSix = controlCharacter.IsControlCharacter();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsControlCharacter();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsControlCharacter();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeFalse();
            resultFour.Should().BeFalse();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeFalse();
        }

        [TestMethod]
        public void IsLowercaseAlphabetic_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsLowercaseAlphabetic();
            var resultTwo = uppercaseAlphabeticCharacter.IsLowercaseAlphabetic();
            var resultThree = numericCharacter.IsLowercaseAlphabetic();
            var resultFour = symbolicCharacer.IsLowercaseAlphabetic();
            var resultFive = whiteSpaceCharacter.IsLowercaseAlphabetic();
            var resultSix = controlCharacter.IsLowercaseAlphabetic();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsLowercaseAlphabetic();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsLowercaseAlphabetic();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeFalse();
            resultFour.Should().BeFalse();
            resultFive.Should().BeFalse();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeFalse();
        }

        [TestMethod]
        public void IsNumeric_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsNumeric();
            var resultTwo = uppercaseAlphabeticCharacter.IsNumeric();
            var resultThree = numericCharacter.IsNumeric();
            var resultFour = symbolicCharacer.IsNumeric();
            var resultFive = whiteSpaceCharacter.IsNumeric();
            var resultSix = controlCharacter.IsNumeric();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsNumeric();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsNumeric();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeFalse();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeFalse();
        }

        [TestMethod]
        public void IsSymbolic_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsSymbolic();
            var resultTwo = uppercaseAlphabeticCharacter.IsSymbolic();
            var resultThree = numericCharacter.IsSymbolic();
            var resultFour = symbolicCharacer.IsSymbolic();
            var resultFive = whiteSpaceCharacter.IsSymbolic();
            var resultSix = controlCharacter.IsSymbolic();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsSymbolic();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsSymbolic();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeFalse();
        }

        [TestMethod]
        public void IsUppercaseAlphabetic_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsUppercaseAlphabetic();
            var resultTwo = uppercaseAlphabeticCharacter.IsUppercaseAlphabetic();
            var resultThree = numericCharacter.IsUppercaseAlphabetic();
            var resultFour = symbolicCharacer.IsUppercaseAlphabetic();
            var resultFive = whiteSpaceCharacter.IsUppercaseAlphabetic();
            var resultSix = controlCharacter.IsUppercaseAlphabetic();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsUppercaseAlphabetic();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsUppercaseAlphabetic();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeFalse();
            resultFive.Should().BeFalse();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeTrue();
        }

        [TestMethod]
        public void IsWhiteSpaceCharacter_ShouldReturnValidResult()
        {
            // Arrange.
            var lowercaseAlphabeticCharacter = 'f';
            var uppercaseAlphabeticCharacter = 'X';
            var numericCharacter = '6';
            var symbolicCharacer = '+';
            var whiteSpaceCharacter = ' ';
            var controlCharacter = '\x2';
            var uncasedNonLatinAlphabeticCharacter = '台';
            var uppercaseNonLatinAlphabeticCharacter = 'Ü';

            // Act.
            var resultOne = lowercaseAlphabeticCharacter.IsWhiteSpaceCharacter();
            var resultTwo = uppercaseAlphabeticCharacter.IsWhiteSpaceCharacter();
            var resultThree = numericCharacter.IsWhiteSpaceCharacter();
            var resultFour = symbolicCharacer.IsWhiteSpaceCharacter();
            var resultFive = whiteSpaceCharacter.IsWhiteSpaceCharacter();
            var resultSix = controlCharacter.IsWhiteSpaceCharacter();
            var resultSeven = uncasedNonLatinAlphabeticCharacter.IsWhiteSpaceCharacter();
            var resultEight = uppercaseNonLatinAlphabeticCharacter.IsWhiteSpaceCharacter();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeFalse();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeFalse();
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 'x';

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            BitConverter.ToChar(result).Should().Be(target);
        }
    }
}