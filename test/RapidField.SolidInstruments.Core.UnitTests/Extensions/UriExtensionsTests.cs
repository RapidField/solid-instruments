// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class UriExtensionsTests
    {
        [TestMethod]
        public void AppendQueryStringData_ShoudlRaiseArgumentException_ForEmptyQueryStringData()
        {
            // Arrange.
            var target = new Uri("http://www.foo.bar", UriKind.Absolute);
            var queryStringData = new Dictionary<String, String>();

            // Act.
            var action = new Action(() =>
            {
                var result = target.AppendQueryStringData(queryStringData);
            });

            // Assert.
            action.Should().Throw<ArgumentException>($"because {nameof(queryStringData)} is empty");
        }

        [TestMethod]
        public void AppendQueryStringData_ShouldRaiseArgumentException_ForEmptyKeys()
        {
            // Arrange.
            var target = new Uri("http://www.foo.bar", UriKind.Absolute);
            var queryStringData = new Dictionary<String, String>()
            {
                { String.Empty, "foo" }
            };

            // Act.
            var action = new Action(() =>
            {
                var result = target.AppendQueryStringData(queryStringData);
            });

            // Assert.
            action.Should().Throw<ArgumentException>($"because {nameof(queryStringData)} contains an empty key");
        }

        [TestMethod]
        public void AppendQueryStringData_ShouldRaiseArgumentNullException_ForNullQueryStringData()
        {
            // Arrange.
            var target = new Uri("http://www.foo.bar", UriKind.Absolute);
            var queryStringData = (Dictionary<String, String>)null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.AppendQueryStringData(queryStringData);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(queryStringData)} is null");
        }

        [TestMethod]
        public void AppendQueryStringData_ShouldRaiseInvalidOperationException_ForRelativeUri()
        {
            // Arrange.
            var target = new Uri("api", UriKind.Relative);
            var queryStringData = new Dictionary<String, String>()
            {
                { "widgetCount", "42" }
            };

            // Act.
            var action = new Action(() =>
            {
                var result = target.AppendQueryStringData(queryStringData);
            });

            // Assert.
            action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is a relative URI");
        }

        [TestMethod]
        public void AppendQueryStringData_ShouldReturnValidResult_ForUriWithExistingQueryString()
        {
            // Arrange.
            var target = new Uri("http://www.foo.bar/api?widgetCount=42", UriKind.Absolute);
            var queryStringData = new Dictionary<String, String>()
            {
                { "price", "$1,476.91" },
                { "message", "This is a sentence." }
            };

            // Act.
            var result = target.AppendQueryStringData(queryStringData);

            // Assert.
            result.Should().NotBeNull();
            result.IsAbsoluteUri.Should().BeTrue();
            result.AbsoluteUri.Should().Be("http://www.foo.bar/api?widgetCount=42&price=%241%2C476.91&message=This+is+a+sentence.");
        }

        [TestMethod]
        public void AppendQueryStringData_ShouldReturnValidResult_ForUriWithoutExistingQueryString()
        {
            // Arrange.
            var target = new Uri("http://www.foo.bar/api", UriKind.Absolute);
            var queryStringData = new Dictionary<String, String>()
            {
                { "price", "$1,476.91" },
                { "message", "This is a sentence." }
            };

            // Act.
            var result = target.AppendQueryStringData(queryStringData);

            // Assert.
            result.Should().NotBeNull();
            result.IsAbsoluteUri.Should().BeTrue();
            result.AbsoluteUri.Should().Be("http://www.foo.bar/api?price=%241%2C476.91&message=This+is+a+sentence.");
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForAbsoluteUri()
        {
            // Arrange.
            var target = new Uri("http://www.foo.bar/api?widgetCount=42", UriKind.Absolute);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(37);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForRelativeUri()
        {
            // Arrange.
            var target = new Uri("api?widgetCount=42", UriKind.Relative);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(18);
        }
    }
}