// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a version identifier that is compliant with the Semantic Versioning Specification (SemVer 2.0.0).
    /// </summary>
    public interface ISemanticVersion : ICloneable, IComparable<ISemanticVersion>, IEquatable<ISemanticVersion>
    {
        /// <summary>
        /// Gets the build metadata, <see langword="null" /> if there is no metadata.
        /// </summary>
        String BuildMetadata
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> includes build metadata.
        /// </summary>
        Boolean HasBuildMetadata
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a new major version (eg.
        /// x.0.0).
        /// </summary>
        Boolean IsMajor
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a new minor version (eg.
        /// x.x.0).
        /// </summary>
        Boolean IsMinor
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a patch version (
        /// <see cref="PatchVersion" /> is greater than zero).
        /// </summary>
        Boolean IsPatch
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a pre-release version.
        /// </summary>
        Boolean IsPreRelease
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a stable version.
        /// </summary>
        Boolean IsStable
        {
            get;
        }

        /// <summary>
        /// Gets the major version number, which is incremented for compatibility-breaking feature changes.
        /// </summary>
        UInt64 MajorVersion
        {
            get;
        }

        /// <summary>
        /// Gets the minor version number, which is incremented for compatibility-retaining feature changes.
        /// </summary>
        UInt64 MinorVersion
        {
            get;
        }

        /// <summary>
        /// Gets the patch version number, which is incremented for compatibility-retaining bug fix changes.
        /// </summary>
        UInt64 PatchVersion
        {
            get;
        }

        /// <summary>
        /// Gets the pre-release label, or <see langword="null" /> if there is no label.
        /// </summary>
        String PreReleaseLabel
        {
            get;
        }
    }
}