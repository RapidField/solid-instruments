﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace RapidField.SolidInstruments.Serialization.UnitTests
{
    [TestClass]
    public class UnicodeSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldRaiseArgumentNullException_ForNullSerializedObjectArgument()
        {
            // Arrange.
            var target = new UnicodeSerializer();
            var serializedObject = (Byte[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Deserialize(serializedObject);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Serialize_ShouldBeReversible()
        {
            // Arrange.
            var target = new UnicodeSerializer();
            var serializationTarget = "aZ09`ಮ";

            // Act.
            var serializeResult = target.Serialize(serializationTarget);
            var deserializeResult = target.Deserialize(serializeResult);

            // Assert.
            deserializeResult.Should().BeEquivalentTo(serializationTarget);
        }

        [TestMethod]
        public void Serialize_ShouldRaiseArgumentNullException_ForNullObjArgument()
        {
            // Arrange.
            var target = new UnicodeSerializer();
            var serializationTarget = (String)null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Serialize(serializationTarget);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Serialize_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new UnicodeSerializer();
            var obserializationTargetj = "aZ09`ಮ";

            // Act.
            var result = target.Serialize(obserializationTargetj);

            // Assert.
            result.Should().BeEquivalentTo(Encoding.Unicode.GetBytes(obserializationTargetj));
        }
    }
}