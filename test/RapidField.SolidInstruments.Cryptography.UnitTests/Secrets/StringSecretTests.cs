﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class StringSecretTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var name = "foo";
            var valueOne = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";
            var valueTwo = String.Empty;
            var valueThree = "foobar";

            using (var target = new StringSecret(name))
            {
                // Assert.
                target.Name.Should().Be(name);
                target.HasValue.Should().BeFalse();
                target.ValueType.Should().Be(typeof(String));

                // Act.
                target.Write(() => valueOne);

                // Assert.
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    secret.Should().Be(valueOne);
                });

                // Act.
                target.Write(() => valueTwo);

                // Assert.
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    secret.Should().Be(valueTwo);
                });

                // Act.
                target.Write(() => valueThree);

                // Assert.
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    secret.Should().Be(valueThree);
                });
            }

            // Assert.
            valueOne.Should().NotBeEmpty();
            valueTwo.Should().BeEmpty();
            valueThree.Should().NotBeEmpty();
        }
    }
}