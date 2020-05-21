// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Secrets;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class SecretVaultTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForBinarySecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = new Byte[3];
                var secretTwo = new Byte[21];
                var secretThree = new Byte[89];
                randomnessProvider.GetBytes(secretOne);
                randomnessProvider.GetBytes(secretTwo);
                randomnessProvider.GetBytes(secretThree);
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().NotContain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForGuidSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = Guid.NewGuid();
                var secretTwo = Guid.NewGuid();
                var secretThree = Guid.NewGuid();
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Guid value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (Guid value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Guid value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().NotContain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForNumericSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = randomnessProvider.GetDouble();
                var secretTwo = randomnessProvider.GetDouble();
                var secretThree = randomnessProvider.GetDouble();
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Double value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (Double value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Double value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().NotContain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForStringSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = randomnessProvider.GetString(8, false, true, true, true, true, false, false);
                var secretTwo = randomnessProvider.GetString(8, false, true, true, true, true, false, false);
                var secretThree = randomnessProvider.GetString(8, false, true, true, true, true, false, false);
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (String value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (String value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.Names.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (String value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.Names.Should().NotContain(secretOneName);
                    target.Names.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.Names.Should().BeEmpty();
                }
            }
        }
    }
}