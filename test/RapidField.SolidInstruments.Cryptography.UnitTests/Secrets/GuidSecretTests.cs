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
    public class GuidSecretTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var name = "foo";
            var valueOne = Guid.NewGuid();
            var valueTwo = Guid.Empty;
            var valueThree = Guid.NewGuid();

            using (var target = new GuidSecret(name))
            {
                // Assert.
                target.Name.Should().Be(name);
                target.HasValue.Should().BeFalse();
                target.ValueType.Should().Be(typeof(Guid));

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
                target.Read(secret =>
                {
                    secret.Should().Be(valueTwo);
                });

                // Act.
                target.Write(() => valueThree);

                // Assert.
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