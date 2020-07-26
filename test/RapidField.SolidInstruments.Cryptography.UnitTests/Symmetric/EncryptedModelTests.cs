// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Serialization;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Symmetric
{
    [TestClass]
    public class EncryptedModelTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForValidCascadingSymmetricKey()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var model = SimulatedModel.Random(randomnessProvider);
                var modelType = model.GetType();
                var key = CascadingSymmetricKey.New();

                // Act.
                var targetOne = EncryptedModel.FromPlaintextModel(model, key);
                var targetTwo = EncryptedModel.FromPlaintextModel(model, key);

                // Assert.
                targetOne.Should().NotBeNull();
                targetTwo.Should().NotBeNull();
                targetOne.Should().NotBeSameAs(targetTwo);
                targetOne.Ciphertext.Should().NotBeNullOrEmpty();
                targetTwo.Ciphertext.Should().NotBeNullOrEmpty();
                targetOne.Ciphertext.Should().NotBe(targetTwo.Ciphertext);
                targetOne.ModelType.Should().Be(modelType);
                targetTwo.ModelType.Should().Be(modelType);
                targetOne.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);
                targetTwo.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);

                // Act.
                var decryptedModelOne = targetOne.ToPlaintextModel(key);
                var decryptedModelTwo = targetTwo.ToPlaintextModel(key);
                var decryptedModelThree = targetTwo.ToPlaintextModel(key);

                // Assert.
                decryptedModelOne.Should().NotBeNull();
                decryptedModelTwo.Should().NotBeNull();
                decryptedModelThree.Should().NotBeNull();
                decryptedModelOne.Should().BeOfType(modelType);
                decryptedModelTwo.Should().BeOfType(modelType);
                decryptedModelThree.Should().BeOfType(modelType);
                decryptedModelOne.Should().Be(model);
                decryptedModelTwo.Should().Be(model);
                decryptedModelThree.Should().Be(model);
                decryptedModelOne.Should().NotBeSameAs(model);
                decryptedModelTwo.Should().NotBeSameAs(model);
                decryptedModelThree.Should().NotBeSameAs(model);
                decryptedModelOne.Should().NotBeSameAs(decryptedModelTwo);
                decryptedModelTwo.Should().NotBeSameAs(decryptedModelThree);

                // Act.
                var targetThree = EncryptedModel.FromPlaintextModel(decryptedModelThree, key);

                // Assert.
                targetThree.Should().NotBeNull();
                targetThree.Should().NotBeSameAs(targetOne);
                targetThree.Should().NotBeSameAs(targetTwo);
                targetThree.Ciphertext.Should().NotBeNullOrEmpty();
                targetThree.Ciphertext.Should().NotBe(targetOne.Ciphertext);
                targetThree.Ciphertext.Should().NotBe(targetTwo.Ciphertext);
                targetThree.ModelType.Should().Be(modelType);
                targetThree.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForValidSymmetricKey()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var model = SimulatedModel.Random(randomnessProvider);
                var modelType = model.GetType();
                var key = SymmetricKey.New();

                // Act.
                var targetOne = EncryptedModel.FromPlaintextModel(model, key);
                var targetTwo = EncryptedModel.FromPlaintextModel(model, key);

                // Assert.
                targetOne.Should().NotBeNull();
                targetTwo.Should().NotBeNull();
                targetOne.Should().NotBeSameAs(targetTwo);
                targetOne.Ciphertext.Should().NotBeNullOrEmpty();
                targetTwo.Ciphertext.Should().NotBeNullOrEmpty();
                targetOne.Ciphertext.Should().NotBe(targetTwo.Ciphertext);
                targetOne.ModelType.Should().Be(modelType);
                targetTwo.ModelType.Should().Be(modelType);
                targetOne.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);
                targetTwo.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);

                // Act.
                var decryptedModelOne = targetOne.ToPlaintextModel(key);
                var decryptedModelTwo = targetTwo.ToPlaintextModel(key);
                var decryptedModelThree = targetTwo.ToPlaintextModel(key);

                // Assert.
                decryptedModelOne.Should().NotBeNull();
                decryptedModelTwo.Should().NotBeNull();
                decryptedModelThree.Should().NotBeNull();
                decryptedModelOne.Should().BeOfType(modelType);
                decryptedModelTwo.Should().BeOfType(modelType);
                decryptedModelThree.Should().BeOfType(modelType);
                decryptedModelOne.Should().Be(model);
                decryptedModelTwo.Should().Be(model);
                decryptedModelThree.Should().Be(model);
                decryptedModelOne.Should().NotBeSameAs(model);
                decryptedModelTwo.Should().NotBeSameAs(model);
                decryptedModelThree.Should().NotBeSameAs(model);
                decryptedModelOne.Should().NotBeSameAs(decryptedModelTwo);
                decryptedModelTwo.Should().NotBeSameAs(decryptedModelThree);

                // Act.
                var targetThree = EncryptedModel.FromPlaintextModel(decryptedModelThree, key);

                // Assert.
                targetThree.Should().NotBeNull();
                targetThree.Should().NotBeSameAs(targetOne);
                targetThree.Should().NotBeSameAs(targetTwo);
                targetThree.Ciphertext.Should().NotBeNullOrEmpty();
                targetThree.Ciphertext.Should().NotBe(targetOne.Ciphertext);
                targetThree.Ciphertext.Should().NotBe(targetTwo.Ciphertext);
                targetThree.ModelType.Should().Be(modelType);
                targetThree.ModelTypeAssemblyQualifiedName.Should().Be(modelType.AssemblyQualifiedName);
            }
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingBinaryFormat()
        {
            // Arrange.
            var format = SerializationFormat.Binary;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingCompressedJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedJson;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingCompressedXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedXml;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.Json;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.Xml;

            // Assert.
            ShouldBeSerializable(format);
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var model = SimulatedModel.Random(randomnessProvider);
                var serializer = new DynamicSerializer<EncryptedModel>(format);
                var key = SymmetricKey.New();
                var target = EncryptedModel.FromPlaintextModel(model, key).ToSerializableModel();

                // Act.
                var serializedTarget = serializer.Serialize(target);
                var deserializedResult = serializer.Deserialize(serializedTarget);

                // Assert.
                deserializedResult.Should().Be(target);
                deserializedResult.ToPlaintextModel<SimulatedModel>(key).Should().Be(model);
            }
        }
    }
}