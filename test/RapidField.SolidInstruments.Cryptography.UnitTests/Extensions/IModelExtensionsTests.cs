// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Extensions
{
    [TestClass]
    public class IModelExtensionsTests
    {
        [TestMethod]
        public void Encrypt_ShouldProduceDesiredResults_ForValidCascadingSymmetricKey()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = SimulatedModel.Random(randomnessProvider);
                var modelType = target.GetType();
                var key = CascadingSymmetricKey.New();

                // Act.
                var result = target.Encrypt(key);

                // Assert.
                result.Should().NotBeNull();
                result.Ciphertext.Should().NotBeNullOrEmpty();
                result.ModelType.Should().Be(modelType);
                result.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);
            }
        }

        [TestMethod]
        public void Encrypt_ShouldProduceDesiredResults_ForValidSymmetricKey()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = SimulatedModel.Random(randomnessProvider);
                var modelType = target.GetType();
                var key = SymmetricKey.New();

                // Act.
                var result = target.Encrypt(key);

                // Assert.
                result.Should().NotBeNull();
                result.Ciphertext.Should().NotBeNullOrEmpty();
                result.ModelType.Should().Be(modelType);
                result.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);
            }
        }
    }
}