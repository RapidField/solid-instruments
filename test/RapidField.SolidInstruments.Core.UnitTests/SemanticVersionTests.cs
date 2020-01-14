// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Data;
using System.Linq;
using SerializationFormat = RapidField.SolidInstruments.Serialization.SerializationFormat;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class SemanticVersionTests
    {
        [TestMethod]
        public void Clone_ShouldCreateIdenticalCopies()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Act.
            var result = target.Clone() as SemanticVersion;

            // Assert.
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(target);
            result.Should().BeEquivalentTo(target);
        }

        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = SemanticVersion.Parse("1.2.3-preview1");
            var targetTwo = SemanticVersion.Parse("1.2.3-preview1");
            var targetThree = SemanticVersion.Parse("1.2.3");
            var targetFour = SemanticVersion.Parse("1.2.3+1234");

            // Act.
            var resultOne = targetOne.CompareTo(targetTwo) == 0;
            var resultTwo = targetTwo.CompareTo(targetThree) == -1;
            var resultThree = targetTwo < targetOne;
            var resultFour = targetThree > targetOne;
            var resultFive = targetFour <= targetThree;
            var resultSix = targetTwo >= targetOne;

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().Be(preReleaseLabel);
            target.BuildMetadata.Should().BeNull();
            target.HasBuildMetadata.Should().BeFalse();
            target.IsMajor.Should().BeTrue();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeTrue();
            target.ToString().Should().Be($"1.0.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().Be(preReleaseLabel);
            target.BuildMetadata.Should().Be(buildMetadata);
            target.HasBuildMetadata.Should().BeTrue();
            target.IsMajor.Should().BeTrue();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeTrue();
            target.ToString().Should().Be($"1.0.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().Be(preReleaseLabel);
            target.BuildMetadata.Should().BeNull();
            target.HasBuildMetadata.Should().BeFalse();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeTrue();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeTrue();
            target.ToString().Should().Be($"1.2.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().Be(preReleaseLabel);
            target.BuildMetadata.Should().Be(buildMetadata);
            target.HasBuildMetadata.Should().BeTrue();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeTrue();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeTrue();
            target.ToString().Should().Be($"1.2.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().Be(preReleaseLabel);
            target.BuildMetadata.Should().BeNull();
            target.HasBuildMetadata.Should().BeFalse();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeTrue();
            target.IsPreRelease.Should().BeTrue();
            target.ToString().Should().Be($"1.2.3-{preReleaseLabel}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().Be(preReleaseLabel);
            target.BuildMetadata.Should().Be(buildMetadata);
            target.HasBuildMetadata.Should().BeTrue();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeTrue();
            target.IsPreRelease.Should().BeTrue();
            target.ToString().Should().Be($"1.2.3-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().BeNull();
            target.BuildMetadata.Should().BeNull();
            target.HasBuildMetadata.Should().BeFalse();
            target.IsMajor.Should().BeTrue();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeFalse();
            target.ToString().Should().Be("1.0.0");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().BeNull();
            target.BuildMetadata.Should().Be(buildMetadata);
            target.HasBuildMetadata.Should().BeTrue();
            target.IsMajor.Should().BeTrue();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeFalse();
            target.ToString().Should().Be($"1.0.0+{buildMetadata}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().BeNull();
            target.BuildMetadata.Should().BeNull();
            target.HasBuildMetadata.Should().BeFalse();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeTrue();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeFalse();
            target.ToString().Should().Be("1.2.0");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().BeNull();
            target.BuildMetadata.Should().Be(buildMetadata);
            target.HasBuildMetadata.Should().BeTrue();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeTrue();
            target.IsPatch.Should().BeFalse();
            target.IsPreRelease.Should().BeFalse();
            target.ToString().Should().Be($"1.2.0+{buildMetadata}");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().BeNull();
            target.BuildMetadata.Should().BeNull();
            target.HasBuildMetadata.Should().BeFalse();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeTrue();
            target.IsPreRelease.Should().BeFalse();
            target.ToString().Should().Be("1.2.3");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);

            // Assert.
            target.MajorVersion.Should().Be(majorVersion);
            target.MinorVersion.Should().Be(minorVersion);
            target.PatchVersion.Should().Be(patchVersion);
            target.PreReleaseLabel.Should().BeNull();
            target.BuildMetadata.Should().Be(buildMetadata);
            target.HasBuildMetadata.Should().BeTrue();
            target.IsMajor.Should().BeFalse();
            target.IsMinor.Should().BeFalse();
            target.IsPatch.Should().BeTrue();
            target.IsPreRelease.Should().BeFalse();
            target.ToString().Should().Be($"1.2.3+{buildMetadata}");
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentException_ForInvalidPreRelease_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview^";
            var target = (SemanticVersion)null;

            // Act.
            var action = new Action(() =>
            {
                target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel);
            });

            // Assert.
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentException_ForInvalidPreRelease_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview^";
            var buildMetadata = "1234";
            var target = (SemanticVersion)null;

            // Act.
            var action = new Action(() =>
            {
                target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);
            });

            // Assert.
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentException_ForValidPreRelease_PatchVersion_WithInvalidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234^";
            var target = (SemanticVersion)null;

            // Act.
            var action = new Action(() =>
            {
                target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);
            });

            // Assert.
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void EqualityComparer_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new SemanticVersion(1, 2, 0);
            var targetTwo = new SemanticVersion(1, 2);
            var targetThree = new SemanticVersion(1, 2, 1);

            // Act.
            var resultOne = targetOne.Equals(targetTwo);
            var resultTwo = targetTwo.Equals(targetThree);
            var resultThree = targetTwo == targetOne;
            var resultFour = targetThree == targetOne;
            var resultFive = targetOne != targetThree;
            var resultSix = targetTwo != targetOne;

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new SemanticVersion[]
            {
                SemanticVersion.Parse("0.0.0"),
                SemanticVersion.Parse("0.0.4"),
                SemanticVersion.Parse("1.2.3"),
                SemanticVersion.Parse("10.20.30"),
                SemanticVersion.Parse("1.1.2-prerelease+meta"),
                SemanticVersion.Parse("1.1.2+meta"),
                SemanticVersion.Parse("1.1.2+meta-valid"),
                SemanticVersion.Parse("1.0.0-alpha"),
                SemanticVersion.Parse("1.0.0-beta"),
                SemanticVersion.Parse("1.0.0-alpha.beta"),
                SemanticVersion.Parse("1.0.0-alpha.beta.1"),
                SemanticVersion.Parse("1.0.0-alpha.1"),
                SemanticVersion.Parse("1.0.0-alpha0.valid"),
                SemanticVersion.Parse("1.0.0-alpha.0valid"),
                SemanticVersion.Parse("1.0.0-alpha-a.b-c-Mississippi+build.1-aef.1-its-okay"),
                SemanticVersion.Parse("1.0.0-rc.1+build.1"),
                SemanticVersion.Parse("2.0.0-rc.1+build.123"),
                SemanticVersion.Parse("1.2.3-beta"),
                SemanticVersion.Parse("10.2.3-DEV-SNAPSHOT"),
                SemanticVersion.Parse("1.2.3-SNAPSHOT-123"),
                SemanticVersion.Parse("1.0.0"),
                SemanticVersion.Parse("2.0.0"),
                SemanticVersion.Parse("1.1.7"),
                SemanticVersion.Parse("2.0.0+build.1848"),
                SemanticVersion.Parse("2.0.1-alpha.1227"),
                SemanticVersion.Parse("1.0.0-alpha+beta"),
                SemanticVersion.Parse("1.2.3----RC-SNAPSHOT.12.9.1--.12+788"),
                SemanticVersion.Parse("1.2.3----R-S.12.9.1--.12+meta"),
                SemanticVersion.Parse("1.2.3----RC-SNAPSHOT.12.9.1--.12"),
                SemanticVersion.Parse("1.0.0+0.build.1-rc.10000aaa-foo-0.1"),
                SemanticVersion.Parse("999999999999.999999999999.999999999999"),
                SemanticVersion.Parse("1.0.0-0A.is.legal"),
            };

            // Act.
            var results = targets.Select(target => target.GetHashCode());

            // Assert.
            results.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"2.0.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"2.0.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"2.0.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"2.0.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"2.0.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"2.0.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion();

            // Assert.
            target.ToString().Should().Be("2.0.0");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"2.0.0+{buildMetadata}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion();

            // Assert.
            target.ToString().Should().Be("2.0.0");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"2.0.0+{buildMetadata}");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion();

            // Assert.
            target.ToString().Should().Be("2.0.0");
        }

        [TestMethod]
        public void NextMajorVersion_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMajorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"2.0.0+{buildMetadata}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"1.1.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.1.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"1.3.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.3.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"1.3.0-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.3.0-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion();

            // Assert.
            target.ToString().Should().Be("1.1.0");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.1.0+{buildMetadata}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion();

            // Assert.
            target.ToString().Should().Be("1.3.0");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.3.0+{buildMetadata}");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion();

            // Assert.
            target.ToString().Should().Be("1.3.0");
        }

        [TestMethod]
        public void NextMinorVersion_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextMinorVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.3.0+{buildMetadata}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"1.0.1-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidPreRelease_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.0.1-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"1.2.1-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidPreRelease_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.2.1-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel);

            // Assert.
            target.ToString().Should().Be($"1.2.4-{preReleaseLabel}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidPreRelease_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.2.4-{preReleaseLabel}+{buildMetadata}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion();

            // Assert.
            target.ToString().Should().Be("1.0.1");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidStable_MajorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)0;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.0.1+{buildMetadata}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion();

            // Assert.
            target.ToString().Should().Be("1.2.1");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidStable_MinorVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)0;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.2.1+{buildMetadata}");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithoutBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion();

            // Assert.
            target.ToString().Should().Be("1.2.4");
        }

        [TestMethod]
        public void NextPatchVersion_ShouldProduceDesiredResults_ForValidStable_PatchVersion_WithValidBuildMetadata()
        {
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = (String)null;
            var buildMetadata = "1234";

            // Act.
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion).NextPatchVersion(preReleaseLabel, buildMetadata);

            // Assert.
            target.ToString().Should().Be($"1.2.4+{buildMetadata}");
        }

        [TestMethod]
        public void OfExecutingAssembly_ShouldProduceDesiredResults()
        {
            // Act.
            var target = SemanticVersion.OfExecutingAssembly();

            // Assert.
            target.Should().NotBeNull();
            target.Should().NotBe(SemanticVersion.Zero);
        }

        [TestMethod]
        public void OfType_ShouldProduceDesiredResults_ForValidTypes()
        {
            // Arrange.
            var type = typeof(Object);

            // Act.
            var target = SemanticVersion.OfType(type);

            // Assert.
            target.Should().NotBeNull();
            target.Should().NotBe(SemanticVersion.Zero);
        }

        [TestMethod]
        public void Parse_ShouldProduceDesiredResults_ForValidVersionStrings()
        {
            // Arrange.
            var validVersionStrings = new String[]
            {
                "0.0.0",
                "0.0.4",
                "1.2.3",
                "10.20.30",
                "1.1.2-prerelease+meta",
                "1.1.2+meta",
                "1.1.2+meta-valid",
                "1.0.0-alpha",
                "1.0.0-beta",
                "1.0.0-alpha.beta",
                "1.0.0-alpha.beta.1",
                "1.0.0-alpha.1",
                "1.0.0-alpha0.valid",
                "1.0.0-alpha.0valid",
                "1.0.0-alpha-a.b-c-Mississippi+build.1-aef.1-its-okay",
                "1.0.0-rc.1+build.1",
                "2.0.0-rc.1+build.123",
                "1.2.3-beta",
                "10.2.3-DEV-SNAPSHOT",
                "1.2.3-SNAPSHOT-123",
                "1.0.0",
                "2.0.0",
                "1.1.7",
                "2.0.0+build.1848",
                "2.0.1-alpha.1227",
                "1.0.0-alpha+beta",
                "1.2.3----RC-SNAPSHOT.12.9.1--.12+788",
                "1.2.3----R-S.12.9.1--.12+meta",
                "1.2.3----RC-SNAPSHOT.12.9.1--.12",
                "1.0.0+0.build.1-rc.10000aaa-foo-0.1",
                "999999999999.999999999999.999999999999",
                "1.0.0-0A.is.legal"
            };

            // Act.
            var target = validVersionStrings.Select(value => SemanticVersion.Parse(value)).ToArray();

            for (var i = 0; i < validVersionStrings.Length; i++)
            {
                // Assert.
                validVersionStrings[i].Should().BeEquivalentTo(target[i].ToString());
            }
        }

        [TestMethod]
        public void Parse_ShouldRaiseFormatException_ForInvalidVersionStrings()
        {
            // Arrange.
            var target = (SemanticVersion)null;
            var invalidVersionStrings = new String[]
            {
                "0",
                "1",
                "1.2",
                "1.2.3-0123",
                "1.2.3-0123.0123",
                "1.1.2+.123",
                "+invalid",
                "-invalid",
                "-invalid+invalid",
                "-invalid.01",
                "alpha",
                "alpha.beta",
                "alpha.beta.1",
                "alpha.1",
                "alpha+beta",
                "alpha_beta",
                "alpha.",
                "alpha..",
                "beta",
                "1.0.0-alpha_beta",
                "-alpha.",
                "1.0.0-alpha..",
                "1.0.0-alpha..1",
                "1.0.0-alpha...1",
                "1.0.0-alpha....1",
                "1.0.0-alpha.....1",
                "1.0.0-alpha......1",
                "1.0.0-alpha.......1",
                "01.1.1",
                "1.01.1",
                "1.1.01",
                "1.2",
                "1.2.3.DEV",
                "1.2-SNAPSHOT",
                "1.2.31.2.3----RC-SNAPSHOT.12.09.1--..12+788",
                "1.2-RC-SNAPSHOT",
                "-1.0.3-gamma+b7718",
                "+metadata",
                "9.8.7+meta+meta",
                "9.8.7-whatever+meta+meta",
                "999999999999.999999999999.999999999999----RC-SNAPSHOT.12.09.1--------------------------------..12"
            };

            for (var i = 0; i < invalidVersionStrings.Length; i++)
            {
                // Act.
                var action = new Action(() =>
                {
                    target = SemanticVersion.Parse(invalidVersionStrings[i]);
                });

                // Assert.
                action.Should().Throw<FormatException>();
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
            // Arrange.
            var majorVersion = (UInt64)1;
            var minorVersion = (UInt64)2;
            var patchVersion = (UInt64)3;
            var preReleaseLabel = "preview1";
            var buildMetadata = "1234";
            var target = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);
            var serializer = new DynamicSerializer<SemanticVersion>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().Be(target);
        }
    }
}