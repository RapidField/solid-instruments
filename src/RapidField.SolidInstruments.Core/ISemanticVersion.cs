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
        public String BuildMetadata
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> includes build metadata.
        /// </summary>
        public Boolean HasBuildMetadata
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a new major version (eg.
        /// x.0.0).
        /// </summary>
        public Boolean IsMajor
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a new minor version (eg.
        /// x.x.0).
        /// </summary>
        public Boolean IsMinor
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a patch version (
        /// <see cref="PatchVersion" /> is greater than zero).
        /// </summary>
        public Boolean IsPatch
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a pre-release version.
        /// </summary>
        public Boolean IsPreRelease
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISemanticVersion" /> represents a stable version.
        /// </summary>
        public Boolean IsStable
        {
            get;
        }

        /// <summary>
        /// Gets the major version number, which is incremented for compatibility-breaking feature changes.
        /// </summary>
        public UInt64 MajorVersion
        {
            get;
        }

        /// <summary>
        /// Gets the minor version number, which is incremented for compatibility-retaining feature changes.
        /// </summary>
        public UInt64 MinorVersion
        {
            get;
        }

        /// <summary>
        /// Gets the patch version number, which is incremented for compatibility-retaining bug fix changes.
        /// </summary>
        public UInt64 PatchVersion
        {
            get;
        }

        /// <summary>
        /// Gets the pre-release label, or <see langword="null" /> if there is no label.
        /// </summary>
        public String PreReleaseLabel
        {
            get;
        }
    }
}