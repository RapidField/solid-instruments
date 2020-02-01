// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System.Security.Cryptography.X509Certificates;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class X509CertificateSecretTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var name = "foo";
            var fileNameOne = "TestRootOne.testcert";
            var fileNameTwo = "TestRootTwo.testcert";
            var fileNameThree = "TestRootThree.testcert";
            var subjectOne = "CN=TestRootOne";
            var subjectTwo = "CN=TestRootTwo";
            var subjectThree = "CN=TestRootThree";
            var valueOne = new X509Certificate2(fileNameOne);
            var valueTwo = new X509Certificate2(fileNameTwo);
            var valueThree = new X509Certificate2(fileNameThree);

            using (var target = new X509CertificateSecret(name))
            {
                // Assert.
                target.Name.Should().Be(name);
                target.HasValue.Should().BeFalse();
                target.ValueType.Should().Be(typeof(X509Certificate2));

                // Act.
                target.Write(() => valueOne);

                // Assert.
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    secret.Should().NotBeNull();
                    secret.Subject.Should().NotBeNull();
                    secret.Subject.Should().Be(subjectOne);
                });

                // Act.
                target.Write(() => valueTwo);

                // Assert.
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    secret.Should().NotBeNull();
                    secret.Subject.Should().NotBeNull();
                    secret.Subject.Should().Be(subjectTwo);
                });

                // Act.
                target.Write(() => valueThree);

                // Assert.
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    secret.Should().NotBeNull();
                    secret.Subject.Should().NotBeNull();
                    secret.Subject.Should().Be(subjectThree);
                });
            }

            // Assert.
            valueOne.Should().NotBeNull();
            valueTwo.Should().NotBeNull();
            valueThree.Should().NotBeNull();
        }
    }
}