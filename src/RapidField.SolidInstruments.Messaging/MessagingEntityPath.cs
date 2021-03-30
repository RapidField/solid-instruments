// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a textual path that defines a route to a messaging entity.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagingEntityPath" /> is the default implementation of <see cref="IMessagingEntityPath" />.
    /// </remarks>
    [DataContract]
    public sealed class MessagingEntityPath : IMessagingEntityPath
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntityPath" /> class.
        /// </summary>
        public MessagingEntityPath()
        {
            LabelOneValue = null;
            LabelTwoValue = null;
            LabelThreeValue = null;
            MessageTypeValue = null;
            PrefixValue = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntityPath" /> class.
        /// </summary>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <param name="prefix">
        /// The prefix token, or <see langword="null" /> if there is not a prefix.
        /// </param>
        /// <param name="labels">
        /// A collection of labels, or an empty collection if there are no labels.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="labels" /> contains more than three elements -or- an exception was raised while evaluating the data
        /// contract information for <paramref name="messageType" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or more of the specified values is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// One or more of the specified values contains invalid characters.
        /// </exception>
        [DebuggerHidden]
        internal MessagingEntityPath(Type messageType, String prefix, params String[] labels)
            : this(ExtractMessageTypeName(messageType), prefix, ExtractLabel(labels, 0), ExtractLabel(labels, 1), ExtractLabel(labels, 2))
        {
            if (labels.IsNullOrEmpty())
            {
                return;
            }
            else if (labels.Length > 3)
            {
                throw new ArgumentException("Messaging entity paths may contain a maximum of three labels.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntityPath" /> class.
        /// </summary>
        /// <param name="messageType">
        /// The message type token.
        /// </param>
        /// <param name="prefix">
        /// The prefix token, or <see langword="null" /> if there is not a prefix.
        /// </param>
        /// <param name="labelOne">
        /// The first label token, or <see langword="null" /> if there is not a first label.
        /// </param>
        /// <param name="labelTwo">
        /// The second label token, or <see langword="null" /> if there is not a second label.
        /// </param>
        /// <param name="labelThree">
        /// The third label token, or <see langword="null" /> if there is not a third label.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="messageType" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or more of the specified values is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// One or more of the specified values contains invalid characters.
        /// </exception>
        [DebuggerHidden]
        private MessagingEntityPath(String messageType, String prefix, String labelOne, String labelTwo, String labelThree)
            : this()
        {
            MessageType = messageType;

            if (prefix.IsNullOrEmpty() is false)
            {
                Prefix = prefix;
            }

            if (labelOne.IsNullOrEmpty() is false)
            {
                LabelOne = labelOne;
            }

            if (labelTwo.IsNullOrEmpty() is false)
            {
                LabelTwo = labelTwo;
            }

            if (labelThree.IsNullOrEmpty() is false)
            {
                LabelThree = labelThree;
            }
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IMessagingEntityPath" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(MessagingEntityPath a, IMessagingEntityPath b) => (a == b) is false;

        /// <summary>
        /// Determines whether or not a specified <see cref="IMessagingEntityPath" /> instance is less than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(MessagingEntityPath a, IMessagingEntityPath b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IMessagingEntityPath" /> instance is less than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(MessagingEntityPath a, IMessagingEntityPath b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="IMessagingEntityPath" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(MessagingEntityPath a, IMessagingEntityPath b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="IMessagingEntityPath" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(MessagingEntityPath a, IMessagingEntityPath b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IMessagingEntityPath" /> instance is greater than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IMessagingEntityPath" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(MessagingEntityPath a, IMessagingEntityPath b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a messaging entity path to its
        /// <see cref="MessagingEntityPath" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a messaging entity path to convert.
        /// </param>
        /// <returns>
        /// A <see cref="MessagingEntityPath" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a messaging entity path.
        /// </exception>
        public static MessagingEntityPath Parse(String input) => Parse(input, out var value, true) ? value : default;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a messaging entity path to its
        /// <see cref="MessagingEntityPath" /> equivalent. The method returns a value that indicates whether the conversion
        /// succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a messaging entity path to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out MessagingEntityPath result)
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
        /// Compares the current <see cref="MessagingEntityPath" /> to the specified object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IMessagingEntityPath" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IMessagingEntityPath other) => ToString().CompareTo(other?.ToString() ?? String.Empty);

        /// <summary>
        /// Determines whether or not two specified <see cref="IMessagingEntityPath" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IMessagingEntityPath" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(IMessagingEntityPath other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (ToString() != other.ToString())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether or not the current <see cref="MessagingEntityPath" /> is equal to the specified
        /// <see cref="Object" />.
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
            else if (obj is IMessagingEntityPath path)
            {
                return Equals(path);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode()
        {
            var hashCode = 433494437 ^ MessageType.GetHashCode();

            if (Prefix.IsNullOrEmpty() is false)
            {
                hashCode ^= Prefix.GetHashCode();
            }

            if (LabelOne.IsNullOrEmpty() is false)
            {
                hashCode ^= LabelOne.GetHashCode();
            }

            if (LabelTwo.IsNullOrEmpty() is false)
            {
                hashCode ^= LabelTwo.GetHashCode();
            }

            if (LabelThree.IsNullOrEmpty() is false)
            {
                hashCode ^= LabelThree.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessagingEntityPath" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessagingEntityPath" />.
        /// </returns>
        public override String ToString()
        {
            var stringBuilder = new StringBuilder();

            if (Prefix.IsNullOrEmpty() is false)
            {
                _ = stringBuilder.Append($"{Prefix}{DelimitingCharacterForPrefix}");
            }

            if (MessageType.IsNullOrEmpty() is false)
            {
                _ = stringBuilder.Append(MessageType);
            }

            if (LabelOne.IsNullOrEmpty() is false)
            {
                _ = stringBuilder.Append($"{DelimitingCharacterForLabelToken}{LabelOne}");
            }

            if (LabelTwo.IsNullOrEmpty() is false)
            {
                _ = stringBuilder.Append($"{DelimitingCharacterForLabelToken}{LabelTwo}");
            }

            if (LabelThree.IsNullOrEmpty() is false)
            {
                _ = stringBuilder.Append($"{DelimitingCharacterForLabelToken}{LabelThree}");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Attempts to extract a label at the specified index from the specified collection of labels.
        /// </summary>
        /// <param name="labels">
        /// A collection of labels, or an empty collection if there are no labels.
        /// </param>
        /// <param name="index">
        /// The zero-based index of the label to extract.
        /// </param>
        /// <returns>
        /// The extracted label, or <see langword="null" /> if no label was extracted.
        /// </returns>
        [DebuggerHidden]
        private static String ExtractLabel(String[] labels, Int32 index)
        {
            if (labels.IsNullOrEmpty())
            {
                return null;
            }
            else if (index >= labels.Length)
            {
                return null;
            }

            var label = labels[index].Trim();
            return label.IsNullOrEmpty() ? null : label;
        }

        /// <summary>
        /// Determines an appropriate message type name for the specified message type.
        /// </summary>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The resulting message type name.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// An exception was raised while evaluating the data contract information for <paramref name="messageType" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageType" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static String ExtractMessageTypeName(Type messageType)
        {
            try
            {
                if (messageType.RejectIf().IsNull(nameof(messageType)).TargetArgument.GetCustomAttributes(typeof(DataContractAttribute), false).FirstOrDefault() is DataContractAttribute dataContractAttribute && dataContractAttribute.Name.IsNullOrEmpty() is false)
                {
                    return dataContractAttribute.Name.Compress();
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"An exception was raised while evaluating the data contract information for {messageType}. See inner exception.", nameof(messageType), exception);
            }

            return messageType.Name;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a messaging entity path to its
        /// <see cref="MessagingEntityPath" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a messaging entity path to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="MessagingEntityPath" /> value, or <see langword="null" /> if the operation is unsuccessful.
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
        /// <paramref name="input" /> does not contain a valid representation of a messaging entity path.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out MessagingEntityPath result, Boolean raiseExceptionOnFail)
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
                var processedString = input.Solidify();

                if (processedString.Length == 0)
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessage, new ArgumentException("The input string does not contain any path information.", nameof(input)));
                    }

                    result = default;
                    return false;
                }

                var regularExpression = new Regex(RegularExpressionPatternForCompletePath);
                var matchGroups = (regularExpression.IsMatch(processedString) ? regularExpression.Match(processedString).Groups : null) as IEnumerable<Group>;

                if (matchGroups.IsNullOrEmpty())
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessage, new ArgumentException("The input string is invalid.", nameof(input)));
                    }

                    result = default;
                    return false;
                }

                var messageTypeString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForMessageTypeToken).SingleOrDefault()?.Value;

                if (messageTypeString.IsNullOrEmpty())
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessage, new ArgumentException("The message type is invalid.", nameof(input)));
                    }

                    result = default;
                    return false;
                }

                var prefixString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForPrefixToken).SingleOrDefault()?.Value;
                var labelOneString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForLabelTokenOne).SingleOrDefault()?.Value;
                var labelTwoString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForLabelTokenTwo).SingleOrDefault()?.Value;
                var labelThreeString = matchGroups.Where(group => group.Success && group.Name == PatternGroupNameForLabelTokenThree).SingleOrDefault()?.Value;
                result = new(messageTypeString, prefixString, labelOneString, labelTwoString, labelThreeString);
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
        /// Raises an <see cref="StringArgumentPatternException" /> if the specified label token is invalid.
        /// </summary>
        /// <param name="token">
        /// The token to evaluate.
        /// </param>
        /// <returns>
        /// The specified token.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="token" /> is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// <paramref name="token" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        private static String ValidateLabelToken(String token) => ValidateToken(token?.RejectIf().LengthIsGreaterThan(MaximumCharacterLengthForLabelToken, nameof(token)), RegularExpressionPatternForLabelToken);

        /// <summary>
        /// Raises an <see cref="StringArgumentPatternException" /> if the specified message type token is invalid.
        /// </summary>
        /// <param name="token">
        /// The token to evaluate.
        /// </param>
        /// <returns>
        /// The specified token.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="token" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="token" /> is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// <paramref name="token" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        private static String ValidateMessageTypeToken(String token) => ValidateToken(token.RejectIf().IsNullOrEmpty(nameof(token)).OrIf().LengthIsGreaterThan(MaximumCharacterLengthForMessageTypeToken, nameof(token)), RegularExpressionPatternForMessageTypeToken);

        /// <summary>
        /// Raises an <see cref="StringArgumentPatternException" /> if the specified prefix token is invalid.
        /// </summary>
        /// <param name="token">
        /// The token to evaluate.
        /// </param>
        /// <returns>
        /// The specified token.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="token" /> is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// <paramref name="token" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        private static String ValidatePrefixToken(String token) => ValidateToken(token?.RejectIf().LengthIsGreaterThan(MaximumCharacterLengthForPrefixToken, nameof(token)), RegularExpressionPatternForPrefixToken);

        /// <summary>
        /// Raises an <see cref="StringArgumentPatternException" /> if the specified token is invalid.
        /// </summary>
        /// <param name="token">
        /// The token to evaluate.
        /// </param>
        /// <param name="pattern">
        /// The token pattern against which to compare <paramref name="token" />.
        /// </param>
        /// <returns>
        /// The specified token.
        /// </returns>
        /// <exception cref="StringArgumentPatternException">
        /// <paramref name="token" /> is invalid.
        /// </exception>
        [DebuggerHidden]
        private static String ValidateToken(String token, String pattern) => token.IsNullOrEmpty() ? null : token.RejectIf().DoesNotMatchRegularExpression(pattern, nameof(token));

        /// <summary>
        /// Gets or sets the first label for the current <see cref="MessagingEntityPath" />, or <see langword="null" /> if there is
        /// not a first label.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified value is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value contains invalid characters.
        /// </exception>
        [DataMember]
        public String LabelOne
        {
            get => LabelOneValue;
            set => LabelOneValue = ValidateLabelToken(value);
        }

        /// <summary>
        /// Gets or sets the third label for the current <see cref="MessagingEntityPath" />, or <see langword="null" /> if there is
        /// not a third label.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified value is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value contains invalid characters.
        /// </exception>
        [DataMember]
        public String LabelThree
        {
            get => LabelThreeValue;
            set => LabelThreeValue = ValidateLabelToken(value);
        }

        /// <summary>
        /// Gets or sets the second label for the current <see cref="MessagingEntityPath" />, or <see langword="null" /> if there is
        /// not a second label.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified value is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value contains invalid characters.
        /// </exception>
        [DataMember]
        public String LabelTwo
        {
            get => LabelTwoValue;
            set => LabelTwoValue = ValidateLabelToken(value);
        }

        /// <summary>
        /// Gets or sets the message type for the current <see cref="MessagingEntityPath" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified value is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified value is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified value is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value contains invalid characters.
        /// </exception>
        [DataMember]
        public String MessageType
        {
            get
            {
                if (MessageTypeValue.IsNullOrEmpty())
                {
                    MessageTypeValue = EnhancedReadabilityGuid.New().ToString();
                }

                return MessageTypeValue;
            }
            set => MessageTypeValue = ValidateMessageTypeToken(value);
        }

        /// <summary>
        /// Gets or sets the prefix for the current <see cref="MessagingEntityPath" />, or <see langword="null" /> if there is not a
        /// prefix.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified value is too long.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value contains invalid characters.
        /// </exception>
        [DataMember]
        public String Prefix
        {
            get => PrefixValue;
            set => PrefixValue = ValidatePrefixToken(value);
        }

        /// <summary>
        /// Gets a regular expression that is used to validate the complete path.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForCompletePath => $"^{RegularExpressionPatternForPrefix}?{RegularExpressionPatternForMessageType}{RegularExpressionPatternForLabelOne}?{RegularExpressionPatternForLabelTwo}?{RegularExpressionPatternForLabelThree}?$";

        /// <summary>
        /// Gets a regular expression that is used to validate the first label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForLabelOne => $"(?<{PatternGroupNameForLabelOne}>(?<{PatternGroupNameForLabelDelimiterOne}>[{DelimitingCharacterForLabelToken}])(?<{PatternGroupNameForLabelTokenOne}>{RegularExpressionPatternForLabelToken}))";

        /// <summary>
        /// Gets a regular expression that is used to validate the third label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForLabelThree => $"(?<{PatternGroupNameForLabelThree}>(?<{PatternGroupNameForLabelDelimiterThree}>[{DelimitingCharacterForLabelToken}])(?<{PatternGroupNameForLabelTokenThree}>{RegularExpressionPatternForLabelToken}))";

        /// <summary>
        /// Gets a regular expression that is used to validate label tokens.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForLabelToken => $"[a-zA-Z0-9{DelimitingCharacterForLabelTokenSubParts}]{{1,{MaximumCharacterLengthForLabelToken}}}";

        /// <summary>
        /// Gets a regular expression that is used to validate the second label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForLabelTwo => $"(?<{PatternGroupNameForLabelTwo}>(?<{PatternGroupNameForLabelDelimiterTwo}>[{DelimitingCharacterForLabelToken}])(?<{PatternGroupNameForLabelTokenTwo}>{RegularExpressionPatternForLabelToken}))";

        /// <summary>
        /// Gets a regular expression that is used to validate the full message type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForMessageType => $"(?<{PatternGroupNameForMessageTypeToken}>{RegularExpressionPatternForMessageTypeToken})";

        /// <summary>
        /// Gets a regular expression that is used to validate the message type token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForMessageTypeToken => $"[a-zA-Z0-9]{{1,{MaximumCharacterLengthForMessageTypeToken}}}";

        /// <summary>
        /// Gets a regular expression that is used to validate the full prefix.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForPrefix => $"(?<{PatternGroupNameForPrefix}>(?<{PatternGroupNameForPrefixToken}>{RegularExpressionPatternForPrefixToken})(?<{PatternGroupNameForPrefixDelimiter}>[{DelimitingCharacterForPrefix}]))";

        /// <summary>
        /// Gets a regular expression that is used to validate the prefix token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static String RegularExpressionPatternForPrefixToken => $"[a-zA-Z0-9]{{1,{MaximumCharacterLengthForPrefixToken}}}";

        /// <summary>
        /// Represents the maximum number of allowed characters for a label token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const Int32 MaximumCharacterLengthForLabelToken = 34;

        /// <summary>
        /// Represents the maximum number of allowed characters for a message type token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const Int32 MaximumCharacterLengthForMessageTypeToken = 89;

        /// <summary>
        /// Represents the maximum number of allowed characters for a prefix token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const Int32 MaximumCharacterLengthForPrefixToken = 21;

        /// <summary>
        /// Represents the delimiting character that precedes label tokens.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Char DelimitingCharacterForLabelToken = '_';

        /// <summary>
        /// Represents the delimiting character that is permitted for label token sub-parts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Char DelimitingCharacterForLabelTokenSubParts = '+';

        /// <summary>
        /// Represents the delimiting character that follows the prefix token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Char DelimitingCharacterForPrefix = '-';

        /// <summary>
        /// Represents a message for format exceptions raised by <see cref="Parse(String)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessage = "The specified string could not be parsed as a messaging entity path. See the inner exception for details.";

        /// <summary>
        /// Represents the group name for the first label delimiter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelDelimiterOne = "labeldelimiterone";

        /// <summary>
        /// Represents the group name for the third label delimiter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelDelimiterThree = "labeldelimiterthree";

        /// <summary>
        /// Represents the group name for the second label delimiter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelDelimiterTwo = "labeldelimitertwo";

        /// <summary>
        /// Represents the group name for the first label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelOne = "labelone";

        /// <summary>
        /// Represents the group name for the third label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelThree = "labelthree";

        /// <summary>
        /// Represents the group name for the first label token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelTokenOne = "labeltokenone";

        /// <summary>
        /// Represents the group name for the third label token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelTokenThree = "labeltokenthree";

        /// <summary>
        /// Represents the group name for the second label token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelTokenTwo = "labeltokentwo";

        /// <summary>
        /// Represents the group name for the second label.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForLabelTwo = "labeltwo";

        /// <summary>
        /// Represents the group name for the message type token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForMessageTypeToken = "messagetypetoken";

        /// <summary>
        /// Represents the group name for the full prefix.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForPrefix = "prefix";

        /// <summary>
        /// Represents the group name for the prefix delimiter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForPrefixDelimiter = "prefixdelimiter";

        /// <summary>
        /// Represents the group name for the prefix token.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PatternGroupNameForPrefixToken = "prefixtoken";

        /// <summary>
        /// Represents the first label for the current <see cref="MessagingEntityPath" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String LabelOneValue;

        /// <summary>
        /// Represents the third label for the current <see cref="MessagingEntityPath" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String LabelThreeValue;

        /// <summary>
        /// Represents the second label for the current <see cref="MessagingEntityPath" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String LabelTwoValue;

        /// <summary>
        /// Represents the message type for the current <see cref="MessagingEntityPath" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String MessageTypeValue;

        /// <summary>
        /// Represents the prefix for the current <see cref="MessagingEntityPath" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String PrefixValue;
    }
}