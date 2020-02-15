// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a version identifier that is compliant with the Semantic Versioning Specification (SemVer 2.0.0).
    /// </summary>
    /// <remarks>
    /// <see cref="SemanticVersion" /> is the default implementation of <see cref="ISemanticVersion" />.
    /// </remarks>
    [DataContract]
    public sealed class SemanticVersion : ISemanticVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion" /> class.
        /// </summary>
        public SemanticVersion()
            : this(0)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion" /> class.
        /// </summary>
        /// <param name="majorVersion">
        /// The major version number, which is incremented for compatibility-breaking feature changes. The default value is 0.
        /// </param>
        public SemanticVersion(UInt64 majorVersion)
            : this(majorVersion, 0)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion" /> class.
        /// </summary>
        /// <param name="majorVersion">
        /// The major version number, which is incremented for compatibility-breaking feature changes. The default value is 0.
        /// </param>
        /// <param name="minorVersion">
        /// The minor version number, which is incremented for compatibility-retaining feature changes. The default value is 0.
        /// </param>
        public SemanticVersion(UInt64 majorVersion, UInt64 minorVersion)
            : this(majorVersion, minorVersion, 0)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion" /> class.
        /// </summary>
        /// <param name="majorVersion">
        /// The major version number, which is incremented for compatibility-breaking feature changes. The default value is 0.
        /// </param>
        /// <param name="minorVersion">
        /// The minor version number, which is incremented for compatibility-retaining feature changes. The default value is 0.
        /// </param>
        /// <param name="patchVersion">
        /// The patch version number, which is incremented for compatibility-retaining bug fix changes. The default value is 0.
        /// </param>
        public SemanticVersion(UInt64 majorVersion, UInt64 minorVersion, UInt64 patchVersion)
            : this(majorVersion, minorVersion, patchVersion, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion" /> class.
        /// </summary>
        /// <param name="majorVersion">
        /// The major version number, which is incremented for compatibility-breaking feature changes. The default value is 0.
        /// </param>
        /// <param name="minorVersion">
        /// The minor version number, which is incremented for compatibility-retaining feature changes. The default value is 0.
        /// </param>
        /// <param name="patchVersion">
        /// The patch version number, which is incremented for compatibility-retaining bug fix changes. The default value is 0.
        /// </param>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid.
        /// </exception>
        public SemanticVersion(UInt64 majorVersion, UInt64 minorVersion, UInt64 patchVersion, String preReleaseLabel)
            : this(majorVersion, minorVersion, patchVersion, preReleaseLabel, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion" /> class.
        /// </summary>
        /// <param name="majorVersion">
        /// The major version number, which is incremented for compatibility-breaking feature changes.
        /// </param>
        /// <param name="minorVersion">
        /// The minor version number, which is incremented for compatibility-retaining feature changes.
        /// </param>
        /// <param name="patchVersion">
        /// The patch version number, which is incremented for compatibility-retaining bug fix changes.
        /// </param>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label.
        /// </param>
        /// <param name="buildMetadata">
        /// The build metadata, <see langword="null" /> if there is no metadata.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid -or- <paramref name="buildMetadata" /> is invalid.
        /// </exception>
        public SemanticVersion(UInt64 majorVersion, UInt64 minorVersion, UInt64 patchVersion, String preReleaseLabel, String buildMetadata)
        {
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PatchVersion = patchVersion;
            PreReleaseLabel = preReleaseLabel;
            BuildMetadata = buildMetadata;
        }

        /// <summary>
        /// Returns the <see cref="SemanticVersion" /> of the specified <see cref="Assembly" />, or <see cref="Zero" /> if the
        /// version isn't available.
        /// </summary>
        /// <param name="assembly">
        /// The assembly to evaluate.
        /// </param>
        /// <returns>
        /// The <see cref="SemanticVersion" /> of the specified <see cref="Assembly" />, or <see cref="Zero" /> if the version isn't
        /// available.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembly" /> is <see langword="null" />.
        /// </exception>
        public static SemanticVersion OfAssembly(Assembly assembly)
        {
            var assemblyVersion = assembly.RejectIf().IsNull(nameof(assembly)).TargetArgument.GetName()?.Version;

            if (assemblyVersion is null)
            {
                return Zero;
            }

            var majorVersion = Convert.ToUInt64(assemblyVersion.Major < 0 ? 0 : assemblyVersion.Major);
            var minorVersion = Convert.ToUInt64(assemblyVersion.Minor < 0 ? 0 : assemblyVersion.Minor);
            var patchVersion = Convert.ToUInt64(assemblyVersion.Revision < 0 ? 0 : assemblyVersion.Revision);
            var buildMetadata = assemblyVersion.Build > 0 ? assemblyVersion.Build.ToString() : null;
            return new SemanticVersion(majorVersion, minorVersion, patchVersion, null, buildMetadata);
        }

        /// <summary>
        /// Returns the <see cref="SemanticVersion" /> of the executing assembly, or <see cref="Zero" /> if the version isn't
        /// available.
        /// </summary>
        /// <returns>
        /// The <see cref="SemanticVersion" /> of the executing assembly, or <see cref="Zero" /> if the version isn't available.
        /// </returns>
        public static SemanticVersion OfExecutingAssembly() => OfAssembly(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Returns the <see cref="SemanticVersion" /> of the assembly that defines the specified type, or <see cref="Zero" /> if
        /// the version isn't available.
        /// </summary>
        /// <param name="type">
        /// The type to evaluate.
        /// </param>
        /// <returns>
        /// The <see cref="SemanticVersion" /> of the assembly that defines the specified type, or <see cref="Zero" /> if the
        /// version isn't available.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <see langword="null" />.
        /// </exception>
        public static SemanticVersion OfType(Type type) => OfAssembly(Assembly.GetAssembly(type.RejectIf().IsNull(nameof(type))));

        /// <summary>
        /// Determines whether or not two specified <see cref="ISemanticVersion" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(SemanticVersion a, ISemanticVersion b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="ISemanticVersion" /> instance is less than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(SemanticVersion a, ISemanticVersion b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a specified <see cref="ISemanticVersion" /> instance is less than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(SemanticVersion a, ISemanticVersion b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="ISemanticVersion" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(SemanticVersion a, ISemanticVersion b)
        {
            if ((Object)a is null && (Object)b is null)
            {
                return true;
            }
            else if ((Object)a is null || (Object)b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="ISemanticVersion" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(SemanticVersion a, ISemanticVersion b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a specified <see cref="ISemanticVersion" /> instance is greater than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="ISemanticVersion" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(SemanticVersion a, ISemanticVersion b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a semantic version to its <see cref="SemanticVersion" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a semantic version to convert.
        /// </param>
        /// <returns>
        /// A <see cref="SemanticVersion" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a semantic version.
        /// </exception>
        public static SemanticVersion Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a semantic version to its <see cref="SemanticVersion" />
        /// equivalent. The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a semantic version to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out SemanticVersion result)
        {
            if (Parse(input, out var value, false))
            {
                result = value;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion" /> that is an identical copy of the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="SemanticVersion" /> that is an identical copy of the current <see cref="SemanticVersion" />.
        /// </returns>
        public Object Clone() => new SemanticVersion(MajorVersion, MinorVersion, PatchVersion, PreReleaseLabel, BuildMetadata);

        /// <summary>
        /// Compares the current <see cref="SemanticVersion" /> to the specified object and returns an indication of their relative
        /// values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ISemanticVersion" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(ISemanticVersion other)
        {
            var result = MajorVersion.CompareTo(other.MajorVersion);

            if (result != 0)
            {
                return result;
            }

            result = MinorVersion.CompareTo(other.MinorVersion);

            if (result != 0)
            {
                return result;
            }

            result = PatchVersion.CompareTo(other.PatchVersion);

            if (result != 0)
            {
                return result;
            }

            if (IsPreRelease && other.IsPreRelease)
            {
                result = PreReleaseLabel.CompareTo(other.PreReleaseLabel);
            }
            else if (IsStable && other.IsPreRelease)
            {
                return 1;
            }
            else if (IsPreRelease && other.IsStable)
            {
                return -1;
            }

            if (result != 0)
            {
                return result;
            }

            if (HasBuildMetadata)
            {
                return other.HasBuildMetadata ? BuildMetadata.CompareTo(other.BuildMetadata) : 1;
            }
            else if (other.HasBuildMetadata)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Determines whether or not the current <see cref="SemanticVersion" /> is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is ISemanticVersion)
            {
                return Equals((ISemanticVersion)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="ISemanticVersion" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ISemanticVersion" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(ISemanticVersion other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (GetHashCode() != other.GetHashCode())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode()
        {
            var hashCode = (433494437 ^ (MajorVersion * 514229) ^ (MinorVersion * 28657) ^ (PatchVersion * 1597) ^ 233).GetHashCode();

            if (PreReleaseLabel.IsNullOrEmpty() == false)
            {
                hashCode ^= PreReleaseLabel.GetHashCode();
            }

            if (BuildMetadata.IsNullOrEmpty() == false)
            {
                hashCode ^= BuildMetadata.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Returns the next major version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <returns>
        /// The next major version after the current <see cref="SemanticVersion" />.
        /// </returns>
        public SemanticVersion NextMajorVersion() => NextMajorVersion(null);

        /// <summary>
        /// Returns the next major version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The next major version after the current <see cref="SemanticVersion" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid.
        /// </exception>
        public SemanticVersion NextMajorVersion(String preReleaseLabel) => NextMajorVersion(preReleaseLabel, null);

        /// <summary>
        /// Returns the next major version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label. The default value is <see langword="null" />.
        /// </param>
        /// <param name="buildMetadata">
        /// The build metadata string for the resulting version, or <see langword="null" /> to exclude build metadata. The default
        /// value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The next major version after the current <see cref="SemanticVersion" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid -or- <paramref name="buildMetadata" /> is invalid.
        /// </exception>
        public SemanticVersion NextMajorVersion(String preReleaseLabel, String buildMetadata) => new SemanticVersion((MajorVersion + 1), 0, 0, preReleaseLabel, buildMetadata);

        /// <summary>
        /// Returns the next minor version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <returns>
        /// The next minor version after the current <see cref="SemanticVersion" />.
        /// </returns>
        public SemanticVersion NextMinorVersion() => NextMinorVersion(null);

        /// <summary>
        /// Returns the next minor version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The next minor version after the current <see cref="SemanticVersion" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid.
        /// </exception>
        public SemanticVersion NextMinorVersion(String preReleaseLabel) => NextMinorVersion(preReleaseLabel, null);

        /// <summary>
        /// Returns the next minor version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label. The default value is <see langword="null" />.
        /// </param>
        /// <param name="buildMetadata">
        /// The build metadata string for the resulting version, or <see langword="null" /> to exclude build metadata. The default
        /// value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The next minor version after the current <see cref="SemanticVersion" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid -or- <paramref name="buildMetadata" /> is invalid.
        /// </exception>
        public SemanticVersion NextMinorVersion(String preReleaseLabel, String buildMetadata) => new SemanticVersion(MajorVersion, (MinorVersion + 1), 0, preReleaseLabel, buildMetadata);

        /// <summary>
        /// Returns the next patch version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <returns>
        /// The next patch version after the current <see cref="SemanticVersion" />.
        /// </returns>
        public SemanticVersion NextPatchVersion() => NextPatchVersion(null);

        /// <summary>
        /// Returns the next patch version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The next patch version after the current <see cref="SemanticVersion" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid.
        /// </exception>
        public SemanticVersion NextPatchVersion(String preReleaseLabel) => NextPatchVersion(preReleaseLabel, null);

        /// <summary>
        /// Returns the next patch version after the current <see cref="SemanticVersion" />.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// The pre-release label, or <see langword="null" /> if there is no label. The default value is <see langword="null" />.
        /// </param>
        /// <param name="buildMetadata">
        /// The build metadata string for the resulting version, or <see langword="null" /> to exclude build metadata. The default
        /// value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The next patch version after the current <see cref="SemanticVersion" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid -or- <paramref name="buildMetadata" /> is invalid.
        /// </exception>
        public SemanticVersion NextPatchVersion(String preReleaseLabel, String buildMetadata) => new SemanticVersion(MajorVersion, MinorVersion, (PatchVersion + 1), preReleaseLabel, buildMetadata);

        /// <summary>
        /// Converts the value of the current <see cref="SemanticVersion" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="SemanticVersion" />.
        /// </returns>
        public override String ToString()
        {
            var versionNumbers = $"{MajorVersion}{DelimiterForVersionNumber}{MinorVersion}{DelimiterForVersionNumber}{PatchVersion}";
            var stringBuilder = new StringBuilder(versionNumbers);

            if (PreReleaseLabel.IsNullOrEmpty() == false)
            {
                stringBuilder.Append($"{DelimiterPrefixForPreReleaseLabel}{PreReleaseLabel}");
            }

            if (BuildMetadata.IsNullOrEmpty() == false)
            {
                stringBuilder.Append($"{DelimiterPrefixForBuildMetadata}{BuildMetadata}");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a semantic version to its <see cref="SemanticVersion" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a semantic version to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="SemanticVersion" /> value, or <see langword="null" /> if the operation is unsuccessful.
        /// </param>
        /// <param name="raiseExceptionOnFail">
        /// A value indicating whether or not an exception should be raised if the parse operation fails.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a semantic version.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out SemanticVersion result, Boolean raiseExceptionOnFail)
        {
            if (raiseExceptionOnFail)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }
                else if (input.Length == 0)
                {
                    throw new ArgumentEmptyException(nameof(input));
                }
            }
            else if (input.IsNullOrEmpty())
            {
                result = default;
                return false;
            }

            try
            {
                var solidifiedString = input.Solidify();
                var processedString = solidifiedString.StartsWith(IgnoredPrefixWord, true, CultureInfo.InvariantCulture) ? solidifiedString.Skip(IgnoredPrefixWord.Length).ToString() : solidifiedString.TrimStart(IgnoredPrefixCharacters);

                if (processedString.Length == 0)
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessage, new ArgumentException("The input string does not contain any version information.", nameof(input)));
                    }

                    result = default;
                    return false;
                }

                var regularExpression = new Regex(RegularExpressionPatternForCompleteVersion);
                var matchGroups = regularExpression.IsMatch(processedString) ? regularExpression.Match(processedString).Groups : null;

                if (matchGroups.IsNullOrEmpty())
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessage, new ArgumentException("The input string is invalid.", nameof(input)));
                    }

                    result = default;
                    return false;
                }

                var majorVersionString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForMajorVersion).SingleOrDefault()?.Value;
                var minorVersionString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForMinorVersion).SingleOrDefault()?.Value;
                var patchVersionString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForPatchVersion).SingleOrDefault()?.Value;

                if (majorVersionString.IsNullOrEmpty() || minorVersionString.IsNullOrEmpty() || patchVersionString.IsNullOrEmpty())
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessage, new ArgumentException("One or more of the specified version numbers are invalid.", nameof(input)));
                    }

                    result = default;
                    return false;
                }

                var majorVersion = UInt64.Parse(majorVersionString);
                var minorVersion = UInt64.Parse(minorVersionString);
                var patchVersion = UInt64.Parse(patchVersionString);
                var preReleaseLabel = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForPreReleaseLabel).SingleOrDefault()?.Value;
                var buildMetadata = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForBuildMetadata).SingleOrDefault()?.Value;
                result = new SemanticVersion(majorVersion, minorVersion, patchVersion, preReleaseLabel, buildMetadata);
                return true;
            }
            catch (Exception exception)
            {
                if (raiseExceptionOnFail)
                {
                    throw new FormatException(ParseFormatExceptionMessage, exception);
                }

                result = default;
                return false;
            }
        }

        /// <summary>
        /// Validates and scrubs the specified build metadata string.
        /// </summary>
        /// <param name="buildMetadata">
        /// A build metadata string to evaluate.
        /// </param>
        /// <returns>
        /// A validated and scrubbed build metadata string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="buildMetadata" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        private static String ProcessBuildMetadata(String buildMetadata)
        {
            if (buildMetadata is null)
            {
                return null;
            }

            var processedString = buildMetadata.Solidify();

            if (processedString.Length == 0)
            {
                return null;
            }
            else if (processedString.MatchesRegularExpression($"^{RegularExpressionPatternForBuildMetadata}$"))
            {
                return processedString;
            }

            throw new ArgumentException($"The specified build metadata is invalid: \"{processedString}\"", nameof(buildMetadata));
        }

        /// <summary>
        /// Validates and scrubs the specified pre-release label string.
        /// </summary>
        /// <param name="preReleaseLabel">
        /// A pre-release label string to evaluate.
        /// </param>
        /// <returns>
        /// A validated and scrubbed pre-release label string.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="preReleaseLabel" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        private static String ProcessPreReleaseLabel(String preReleaseLabel)
        {
            if (preReleaseLabel is null)
            {
                return null;
            }

            var processedString = preReleaseLabel.Solidify();

            if (processedString.Length == 0)
            {
                return null;
            }
            else if (processedString.MatchesRegularExpression($"^{RegularExpressionPatternForPreReleaseLabel}$"))
            {
                return processedString;
            }

            throw new ArgumentException($"The specified pre-release label is invalid: \"{processedString}\"", nameof(preReleaseLabel));
        }

        /// <summary>
        /// Gets a <see cref="SemanticVersion" /> equivalent to version 1.0.0.
        /// </summary>
        public static SemanticVersion One => new SemanticVersion(1);

        /// <summary>
        /// Gets a <see cref="SemanticVersion" /> equivalent to version 0.0.0.
        /// </summary>
        public static SemanticVersion Zero => new SemanticVersion(0);

        /// <summary>
        /// Gets or sets the build metadata, <see langword="null" /> if there is no metadata.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The build metadata is invalid.
        /// </exception>
        [DataMember]
        public String BuildMetadata
        {
            get => BuildMetadataReference;
            set => BuildMetadataReference = ProcessBuildMetadata(value);
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SemanticVersion" /> includes build metadata.
        /// </summary>
        [IgnoreDataMember]
        public Boolean HasBuildMetadata => BuildMetadata.IsNullOrEmpty() == false;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SemanticVersion" /> represents a new major version (eg.
        /// x.0.0).
        /// </summary>
        [IgnoreDataMember]
        public Boolean IsMajor => (MinorVersion == 0 && PatchVersion == 0);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SemanticVersion" /> represents a new minor version (eg.
        /// x.x.0).
        /// </summary>
        [IgnoreDataMember]
        public Boolean IsMinor => (MinorVersion > 0 && PatchVersion == 0);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SemanticVersion" /> represents a patch version (
        /// <see cref="PatchVersion" /> is greater than zero).
        /// </summary>
        [IgnoreDataMember]
        public Boolean IsPatch => (PatchVersion > 0);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SemanticVersion" /> represents a pre-release version.
        /// </summary>
        [IgnoreDataMember]
        public Boolean IsPreRelease => PreReleaseLabel.IsNullOrEmpty() == false;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="SemanticVersion" /> represents a stable version.
        /// </summary>
        [IgnoreDataMember]
        public Boolean IsStable => (IsPreRelease == false);

        /// <summary>
        /// Gets or sets the major version number, which is incremented for compatibility-breaking feature changes.
        /// </summary>
        [DataMember]
        public UInt64 MajorVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minor version number, which is incremented for compatibility-retaining feature changes.
        /// </summary>
        [DataMember]
        public UInt64 MinorVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the patch version number, which is incremented for compatibility-retaining bug fix changes.
        /// </summary>
        [DataMember]
        public UInt64 PatchVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pre-release label, or <see langword="null" /> if there is no label.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The pre-release label is invalid.
        /// </exception>
        [DataMember]
        public String PreReleaseLabel
        {
            get => PreReleaseLabelReference;
            set => PreReleaseLabelReference = ProcessPreReleaseLabel(value);
        }

        /// <summary>
        /// Represents the delimiting character that separates version numbers.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char DelimiterForVersionNumber = '.';

        /// <summary>
        /// Represents the delimiting character the precedes the build metadata string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char DelimiterPrefixForBuildMetadata = '+';

        /// <summary>
        /// Represents the delimiting character the precedes the pre-release label string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char DelimiterPrefixForPreReleaseLabel = '-';

        /// <summary>
        /// Represents a prefix word that is ignored by <see cref="Parse(String)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String IgnoredPrefixWord = "version";

        /// <summary>
        /// Represents a message for format exceptions raised by <see cref="Parse(String)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessage = "The specified string could not be parsed as a semantic version. See the inner exception for details.";

        /// <summary>
        /// Represents the group name for the build metadata portion of the full regular expression.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForBuildMetadata = "buildmetadata";

        /// <summary>
        /// Represents the group name for the major version portion of the full regular expression.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForMajorVersion = "major";

        /// <summary>
        /// Represents the group name for the minor version portion of the full regular expression.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForMinorVersion = "minor";

        /// <summary>
        /// Represents the group name for the patch version portion of the full regular expression.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForPatchVersion = "patch";

        /// <summary>
        /// Represents the group name for the pre-release label portion of the full regular expression.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForPreReleaseLabel = "prerelease";

        /// <summary>
        /// Represents a regular expression that is used to validate build metadata strings.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String RegularExpressionPatternForBuildMetadata = "[0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*";

        /// <summary>
        /// Represents a regular expression that is used to validate pre-release label strings.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String RegularExpressionPatternForPreReleaseLabel = "(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*";

        /// <summary>
        /// Represents a regular expression that is used to validate version number strings.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String RegularExpressionPatternForVersionNumber = "0|[1-9]\\d*";

        /// <summary>
        /// Represents prefix characters that are ignored by <see cref="Parse(String)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Char[] IgnoredPrefixCharacters = new Char[2] { 'v', 'V' };

        /// <summary>
        /// Represents a regular expression that is used to validate a complete semantic version.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly String RegularExpressionPatternForCompleteVersion = $"^(?<{PatternGroupNameForMajorVersion}>{RegularExpressionPatternForVersionNumber})\\{DelimiterForVersionNumber}(?<{PatternGroupNameForMinorVersion}>{RegularExpressionPatternForVersionNumber})\\{DelimiterForVersionNumber}(?<{PatternGroupNameForPatchVersion}>{RegularExpressionPatternForVersionNumber})(?:{DelimiterPrefixForPreReleaseLabel}(?<{PatternGroupNameForPreReleaseLabel}>{RegularExpressionPatternForPreReleaseLabel}))?(?:\\{DelimiterPrefixForBuildMetadata}(?<{PatternGroupNameForBuildMetadata}>{RegularExpressionPatternForBuildMetadata}))?$";

        /// <summary>
        /// Represents the build metadata, <see langword="null" /> if there is no metadata.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String BuildMetadataReference;

        /// <summary>
        /// Represents the pre-release label, or <see langword="null" /> if there is no label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String PreReleaseLabelReference;
    }
}