// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents a textual path that defines a route to a messaging entity.
    /// </summary>
    public interface IMessagingEntityPath : IComparable<IMessagingEntityPath>, IEquatable<IMessagingEntityPath>
    {
        /// <summary>
        /// Gets or sets the first label for the current <see cref="IMessagingEntityPath" />, or <see langword="null" /> if there is
        /// not a first label.
        /// </summary>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value is invalid.
        /// </exception>
        String LabelOne
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the third label for the current <see cref="IMessagingEntityPath" />, or <see langword="null" /> if there is
        /// not a third label.
        /// </summary>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value is invalid.
        /// </exception>
        String LabelThree
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second label for the current <see cref="IMessagingEntityPath" />, or <see langword="null" /> if there
        /// is not a second label.
        /// </summary>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value is invalid.
        /// </exception>
        String LabelTwo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message type for the current <see cref="IMessagingEntityPath" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified value is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified value is <see langword="null" />.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value is invalid.
        /// </exception>
        String MessageType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the prefix for the current <see cref="IMessagingEntityPath" />, or <see langword="null" /> if there is not
        /// a prefix.
        /// </summary>
        /// <exception cref="StringArgumentPatternException">
        /// The specified value is invalid.
        /// </exception>
        String Prefix
        {
            get;
            set;
        }
    }
}