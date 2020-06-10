// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void EvaluateSecureHashString_ShouldProduceDesiredResults()
        {
            // Arrange.
            using (var referenceManager = new ReferenceManager())
            {
                var passwords = new IPassword[]
                {
                    Password.FromAsciiString("0"),
                    Password.FromAsciiString("1"),
                    Password.FromAsciiString("2"),
                    Password.FromAsciiString(" "),
                    Password.FromAsciiString("foo"),
                    Password.FromAsciiString("bar"),
                    Password.FromAsciiString("baz"),
                    Password.NewRandomStrongPassword(),
                    Password.NewRandomStrongPassword(),
                    Password.NewRandomStrongPassword()
                };
                var passwordHashStringsOne = new String[]
                {
                    passwords[0].CalculateSecureHashString(),
                    passwords[1].CalculateSecureHashString(),
                    passwords[2].CalculateSecureHashString(),
                    passwords[3].CalculateSecureHashString(),
                    passwords[4].CalculateSecureHashString(),
                    passwords[5].CalculateSecureHashString(),
                    passwords[6].CalculateSecureHashString(),
                    passwords[7].CalculateSecureHashString(),
                    passwords[8].CalculateSecureHashString(),
                    passwords[9].CalculateSecureHashString()
                };
                var passwordHashStringsTwo = new String[]
                {
                    passwords[0].CalculateSecureHashString(),
                    passwords[1].CalculateSecureHashString(),
                    passwords[2].CalculateSecureHashString(),
                    passwords[3].CalculateSecureHashString(),
                    passwords[4].CalculateSecureHashString(),
                    passwords[5].CalculateSecureHashString(),
                    passwords[6].CalculateSecureHashString(),
                    passwords[7].CalculateSecureHashString(),
                    passwords[8].CalculateSecureHashString(),
                    passwords[9].CalculateSecureHashString()
                };

                // Assert.
                passwordHashStringsOne.Should().NotContainNulls();
                passwordHashStringsTwo.Should().NotContainNulls();
                passwordHashStringsOne.Should().NotIntersectWith(passwordHashStringsTwo);

                for (var i = 0; i < passwords.Length; i++)
                {
                    // Act.
                    var password = passwords[i];
                    referenceManager.AddObject(password);
                    var passwordHashStringOne = passwordHashStringsOne[i];
                    var passwordHashStringTwo = passwordHashStringsTwo[i];

                    // Assert.
                    passwordHashStringOne.Should().NotBeNullOrEmpty();
                    passwordHashStringTwo.Should().NotBeNullOrEmpty();

                    // Act.
                    var passwordHashStringOneMathcesPassword = password.EvaluateSecureHashString(passwordHashStringOne);
                    var passwordHashStringTwoMathcesPassword = password.EvaluateSecureHashString(passwordHashStringTwo);

                    // Assert.
                    passwordHashStringOne.Should().NotBe(passwordHashStringTwo);
                    passwordHashStringOneMathcesPassword.Should().BeTrue();
                    passwordHashStringTwoMathcesPassword.Should().BeTrue();
                }

                // Assert.
                passwords[0].EvaluateSecureHashString(passwordHashStringsOne[1]).Should().BeFalse();
                passwords[1].EvaluateSecureHashString(passwordHashStringsOne[2]).Should().BeFalse();
                passwords[2].EvaluateSecureHashString(passwordHashStringsOne[3]).Should().BeFalse();
                passwords[3].EvaluateSecureHashString(passwordHashStringsOne[4]).Should().BeFalse();
                passwords[4].EvaluateSecureHashString(passwordHashStringsOne[5]).Should().BeFalse();
                passwords[5].EvaluateSecureHashString(passwordHashStringsOne[6]).Should().BeFalse();
                passwords[6].EvaluateSecureHashString(passwordHashStringsOne[7]).Should().BeFalse();
                passwords[7].EvaluateSecureHashString(passwordHashStringsOne[8]).Should().BeFalse();
                passwords[8].EvaluateSecureHashString(passwordHashStringsOne[9]).Should().BeFalse();
            }
        }

        [TestMethod]
        public void EvaluateSecureHashValue_ShouldProduceDesiredResults()
        {
            // Arrange.
            using (var referenceManager = new ReferenceManager())
            {
                var passwords = new IPassword[]
                {
                    Password.FromAsciiString("0"),
                    Password.FromAsciiString("1"),
                    Password.FromAsciiString("2"),
                    Password.FromAsciiString(" "),
                    Password.FromAsciiString("foo"),
                    Password.FromAsciiString("bar"),
                    Password.FromAsciiString("baz"),
                    Password.NewRandomStrongPassword(),
                    Password.NewRandomStrongPassword(),
                    Password.NewRandomStrongPassword()
                };
                var passwordHashValuesOne = new IReadOnlyPinnedMemory<Byte>[]
                {
                    passwords[0].CalculateSecureHashValue(),
                    passwords[1].CalculateSecureHashValue(),
                    passwords[2].CalculateSecureHashValue(),
                    passwords[3].CalculateSecureHashValue(),
                    passwords[4].CalculateSecureHashValue(),
                    passwords[5].CalculateSecureHashValue(),
                    passwords[6].CalculateSecureHashValue(),
                    passwords[7].CalculateSecureHashValue(),
                    passwords[8].CalculateSecureHashValue(),
                    passwords[9].CalculateSecureHashValue()
                };
                var passwordHashValuesTwo = new IReadOnlyPinnedMemory<Byte>[]
                {
                    passwords[0].CalculateSecureHashValue(),
                    passwords[1].CalculateSecureHashValue(),
                    passwords[2].CalculateSecureHashValue(),
                    passwords[3].CalculateSecureHashValue(),
                    passwords[4].CalculateSecureHashValue(),
                    passwords[5].CalculateSecureHashValue(),
                    passwords[6].CalculateSecureHashValue(),
                    passwords[7].CalculateSecureHashValue(),
                    passwords[8].CalculateSecureHashValue(),
                    passwords[9].CalculateSecureHashValue()
                };

                for (var i = 0; i < passwords.Length; i++)
                {
                    // Act.
                    var password = passwords[i];
                    referenceManager.AddObject(password);
                    var passwordHashValueOne = passwordHashValuesOne[i];
                    var passwordHashValueTwo = passwordHashValuesTwo[i];

                    // Assert.
                    passwordHashValueOne.Should().NotBeNullOrEmpty();
                    passwordHashValueTwo.Should().NotBeNullOrEmpty();

                    // Act.
                    var passwordHashStringOneMathcesPassword = password.EvaluateSecureHashValue(passwordHashValueOne.ToArray());
                    var passwordHashStringTwoMathcesPassword = password.EvaluateSecureHashValue(passwordHashValueTwo.ToArray());

                    // Assert.
                    passwordHashValueOne.Should().NotBeEquivalentTo(passwordHashValueTwo);
                    passwordHashStringOneMathcesPassword.Should().BeTrue();
                    passwordHashStringTwoMathcesPassword.Should().BeTrue();
                }

                // Assert.
                passwords[0].EvaluateSecureHashValue(passwordHashValuesOne[1].ToArray()).Should().BeFalse();
                passwords[1].EvaluateSecureHashValue(passwordHashValuesOne[2].ToArray()).Should().BeFalse();
                passwords[2].EvaluateSecureHashValue(passwordHashValuesOne[3].ToArray()).Should().BeFalse();
                passwords[3].EvaluateSecureHashValue(passwordHashValuesOne[4].ToArray()).Should().BeFalse();
                passwords[4].EvaluateSecureHashValue(passwordHashValuesOne[5].ToArray()).Should().BeFalse();
                passwords[5].EvaluateSecureHashValue(passwordHashValuesOne[6].ToArray()).Should().BeFalse();
                passwords[6].EvaluateSecureHashValue(passwordHashValuesOne[7].ToArray()).Should().BeFalse();
                passwords[7].EvaluateSecureHashValue(passwordHashValuesOne[8].ToArray()).Should().BeFalse();
                passwords[8].EvaluateSecureHashValue(passwordHashValuesOne[9].ToArray()).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FromAsciiString_ShouldProduceDesiredResults_ForValidPasswordString()
        {
            // Arrange.
            var password = "foo BAR 123 !@# ";

            // Act.
            using var target = Password.FromAsciiString(password);

            // Assert.
            target.Should().NotBeNull();
            target.ReadAsync((String plaintext) =>
            {
                plaintext.Should().NotBeNullOrEmpty();
                plaintext.Should().Be(password);
            }).Wait();
        }

        [TestMethod]
        public void FromUnicodeString_ShouldProduceDesiredResults_ForValidPasswordString()
        {
            // Arrange.
            var password = "foo BAR 123 !@# ";

            // Act.
            using var target = Password.FromUnicodeString(password);

            // Assert.
            target.Should().NotBeNull();
            target.ReadAsync((String plaintext) =>
            {
                plaintext.Should().NotBeNullOrEmpty();
                plaintext.Should().Be(password);
            }).Wait();
        }

        [TestMethod]
        public void FromUtf8String_ShouldProduceDesiredResults_ForValidPasswordString()
        {
            // Arrange.
            var password = "foo BAR 123 !@# ";

            // Act.
            using var target = Password.FromUtf8String(password);

            // Assert.
            target.Should().NotBeNull();
            target.ReadAsync((String plaintext) =>
            {
                plaintext.Should().NotBeNullOrEmpty();
                plaintext.Should().Be(password);
            }).Wait();
        }

        [TestMethod]
        public void MeetsRequirements_ShouldProduceDesiredResults()
        {
            // Arrange.
            var requirementsNone = PasswordCompositionRequirements.None;
            var requirementsNist = PasswordCompositionRequirements.Nist800633;
            var requirementsStrict = PasswordCompositionRequirements.Strict;
            using var passwordOne = Password.FromAsciiString("0");
            using var passwordTwo = Password.FromAsciiString("foo123!");
            using var passwordThree = Password.FromAsciiString("FooBar 123!");
            using var passwordFour = Password.FromAsciiString("1Q2w3e4r5t");
            using var passwordFive = Password.FromAsciiString("FooBarBazBuzz 123!");

            // Act.
            var passwordOneMeetsRequirementsNone = passwordOne.MeetsRequirements(requirementsNone);
            var passwordOneMeetsRequirementsNist = passwordOne.MeetsRequirements(requirementsNist);
            var passwordOneMeetsRequirementsStrict = passwordOne.MeetsRequirements(requirementsStrict);
            var passwordTwoMeetsRequirementsNone = passwordTwo.MeetsRequirements(requirementsNone);
            var passwordTwoMeetsRequirementsNist = passwordTwo.MeetsRequirements(requirementsNist);
            var passwordTwoMeetsRequirementsStrict = passwordTwo.MeetsRequirements(requirementsStrict);
            var passwordThreeMeetsRequirementsNone = passwordThree.MeetsRequirements(requirementsNone);
            var passwordThreeMeetsRequirementsNist = passwordThree.MeetsRequirements(requirementsNist);
            var passwordThreeMeetsRequirementsStrict = passwordThree.MeetsRequirements(requirementsStrict);
            var passwordFourMeetsRequirementsNone = passwordFour.MeetsRequirements(requirementsNone);
            var passwordFourMeetsRequirementsNist = passwordFour.MeetsRequirements(requirementsNist);
            var passwordFourMeetsRequirementsStrict = passwordFour.MeetsRequirements(requirementsStrict);
            var passwordFiveMeetsRequirementsNone = passwordFive.MeetsRequirements(requirementsNone);
            var passwordFiveMeetsRequirementsNist = passwordFive.MeetsRequirements(requirementsNist);
            var passwordFiveMeetsRequirementsStrict = passwordFive.MeetsRequirements(requirementsStrict);

            // Assert.
            passwordOneMeetsRequirementsNone.Should().BeTrue();
            passwordOneMeetsRequirementsNist.Should().BeFalse();
            passwordOneMeetsRequirementsStrict.Should().BeFalse();
            passwordTwoMeetsRequirementsNone.Should().BeTrue();
            passwordTwoMeetsRequirementsNist.Should().BeFalse();
            passwordTwoMeetsRequirementsStrict.Should().BeFalse();
            passwordThreeMeetsRequirementsNone.Should().BeTrue();
            passwordThreeMeetsRequirementsNist.Should().BeTrue();
            passwordThreeMeetsRequirementsStrict.Should().BeFalse();
            passwordFourMeetsRequirementsNone.Should().BeTrue();
            passwordFourMeetsRequirementsNist.Should().BeFalse();
            passwordFourMeetsRequirementsStrict.Should().BeFalse();
            passwordFiveMeetsRequirementsNone.Should().BeTrue();
            passwordFiveMeetsRequirementsNist.Should().BeTrue();
            passwordFiveMeetsRequirementsStrict.Should().BeTrue();
        }

        [TestMethod]
        public void NewRandomStrongPassword_ShouldProduceDesiredResults()
        {
            // Arrange.
            var iterationCount = 30;

            for (var i = 0; i < iterationCount; i++)
            {
                // Act.
                using var password = Password.NewRandomStrongPassword();

                // Assert.
                password.MeetsRequirements(PasswordCompositionRequirements.Strict).Should().BeTrue();
            }
        }
    }
}