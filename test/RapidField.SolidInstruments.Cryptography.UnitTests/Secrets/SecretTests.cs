﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class SecretTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var name = "foo";
            var valueOne = new Byte[] { 0x01 };
            var valueTwo = Array.Empty<Byte>();
            var valueThree = new Byte[] { 0xcc, 0xff };
            var hashCode = 0;
            var derivedIdentity = Guid.Empty;

            using (var target = new Secret(name))
            {
                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.Name.Should().Be(name);
                target.HasValue.Should().BeFalse();
                target.ValueType.Should().Be(typeof(IReadOnlyPinnedMemory<Byte>));
                derivedIdentity = target.DerivedIdentity;
                derivedIdentity.Should().NotBe(Guid.Empty);

                // Act.
                target.Write(() => new PinnedMemory(valueOne));

                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.HasValue.Should().BeTrue();
                target.DerivedIdentity.Should().NotBe(derivedIdentity);
                derivedIdentity = target.DerivedIdentity;
                target.Read(secret =>
                {
                    secret.ReadOnlySpan.ToArray().Should().BeEquivalentTo(valueOne);
                });

                // Act.
                target.WriteAsync(() => new PinnedMemory(valueTwo)).Wait();

                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.DerivedIdentity.Should().NotBe(derivedIdentity);
                derivedIdentity = target.DerivedIdentity;
                target.ReadAsync(secret =>
                {
                    secret.ReadOnlySpan.ToArray().Should().BeEquivalentTo(valueTwo);
                }).Wait();

                // Act.
                target.Write(() => new PinnedMemory(valueThree));

                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.DerivedIdentity.Should().NotBe(derivedIdentity);
                derivedIdentity = target.DerivedIdentity;
                target.Read(secret =>
                {
                    secret.ReadOnlySpan.ToArray().Should().BeEquivalentTo(valueThree);
                });
            }

            // Assert.
            valueOne.Should().NotBeEmpty();
            valueTwo.Should().BeEmpty();
            valueThree.Should().NotBeEmpty();
        }
    }
}