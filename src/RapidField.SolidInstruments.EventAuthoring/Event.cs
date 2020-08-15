// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using SolidInstrumentsCommand = RapidField.SolidInstruments.Command.Command;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event.
    /// </summary>
    /// <remarks>
    /// <see cref="Event" /> is the default implementation of <see cref="IEvent" />.
    /// </remarks>
    [DataContract]
    public class Event : SolidInstrumentsCommand, IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        public Event()
            : this(DefaultCategory)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public Event(Guid correlationIdentifier)
            : this(DefaultCategory, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" />.
        /// </exception>
        public Event(EventCategory category)
            : this(category, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public Event(EventCategory category, Guid correlationIdentifier)
            : this(category, DefaultVerbosity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or- <paramref name="verbosity" /> is
        /// equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public Event(EventCategory category, EventVerbosity verbosity)
            : this(category, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or- <paramref name="verbosity" /> is
        /// equal to <see cref="EventVerbosity.Unspecified" /> -or- <paramref name="correlationIdentifier" /> is equal to
        /// <see cref="Guid.Empty" />.
        /// </exception>
        public Event(EventCategory category, EventVerbosity verbosity, Guid correlationIdentifier)
            : this(category, verbosity, null, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or- <paramref name="verbosity" /> is
        /// equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public Event(EventCategory category, EventVerbosity verbosity, String description)
            : this(category, verbosity, description, Core.TimeStamp.Current)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or- <paramref name="verbosity" /> is
        /// equal to <see cref="EventVerbosity.Unspecified" /> -or- <paramref name="correlationIdentifier" /> is equal to
        /// <see cref="Guid.Empty" />.
        /// </exception>
        public Event(EventCategory category, EventVerbosity verbosity, String description, Guid correlationIdentifier)
            : this(category, verbosity, description, Core.TimeStamp.Current, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="timeStamp">
        /// A <see cref="DateTime" /> that indicates when the event occurred. The default value is <see cref="TimeStamp.Current" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or- <paramref name="verbosity" /> is
        /// equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public Event(EventCategory category, EventVerbosity verbosity, String description, DateTime timeStamp)
            : base()
        {
            Category = category.RejectIf().IsEqualToValue(EventCategory.Unspecified, nameof(category));
            Description = description;
            TimeStamp = timeStamp;
            Verbosity = verbosity.RejectIf().IsEqualToValue(EventVerbosity.Unspecified, nameof(verbosity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event. The default value is <see cref="EventCategory.GeneralInformation" />.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="timeStamp">
        /// A <see cref="DateTime" /> that indicates when the event occurred. The default value is <see cref="TimeStamp.Current" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="EventCategory.Unspecified" /> -or- <paramref name="verbosity" /> is
        /// equal to <see cref="EventVerbosity.Unspecified" /> -or- <paramref name="correlationIdentifier" /> is equal to
        /// <see cref="Guid.Empty" />.
        /// </exception>
        public Event(EventCategory category, EventVerbosity verbosity, String description, DateTime timeStamp, Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            Category = category.RejectIf().IsEqualToValue(EventCategory.Unspecified, nameof(category));
            Description = description;
            TimeStamp = timeStamp;
            Verbosity = verbosity.RejectIf().IsEqualToValue(EventVerbosity.Unspecified, nameof(verbosity));
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IEvent" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Event a, IEvent b) => a == b == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="IEvent" /> instance is less than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Event a, IEvent b) => a is null ? b is Object : a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IEvent" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Event a, IEvent b) => a is null || a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="IEvent" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Event a, IEvent b)
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
        /// Determines whether or not a specified <see cref="IEvent" /> instance is greater than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Event a, IEvent b) => a is Object && a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a specified <see cref="IEvent" /> instance is greater than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="IEvent" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Event a, IEvent b) => a is null ? b is null : a.CompareTo(b) >= 0;

        /// <summary>
        /// Compares the current <see cref="IEvent" /> to the specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEvent" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(IEvent other)
        {
            var thisInstanceTimeStamp = TimeStamp;
            var thisInstanceVerbosity = Verbosity;
            var otherInstanceTimeStamp = other.TimeStamp;
            var otherInstanceVerbosity = other.Verbosity;

            if (thisInstanceTimeStamp < otherInstanceTimeStamp)
            {
                return -1;
            }
            else if (thisInstanceTimeStamp > otherInstanceTimeStamp)
            {
                return 1;
            }
            else if (thisInstanceVerbosity < otherInstanceVerbosity)
            {
                return -1;
            }
            else if (thisInstanceVerbosity > otherInstanceVerbosity)
            {
                return 1;
            }
            else
            {
                var thisInstanceHashCode = GetHashCode();
                var otherInstanceHashCode = other.GetHashCode();

                if (thisInstanceHashCode < otherInstanceHashCode)
                {
                    return -1;
                }
                else if (thisInstanceHashCode > otherInstanceHashCode)
                {
                    return 1;
                }

                return 0;
            }
        }

        /// <summary>
        /// Determines whether or not the current <see cref="IEvent" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is IEvent eventObject)
            {
                return Equals(eventObject);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="IEvent" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Event" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(IEvent other)
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
        public override Int32 GetHashCode() => ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Converts the current <see cref="Event" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Event" />.
        /// </returns>
        public Byte[] ToByteArray() => DynamicSerializer.Serialize(this, GetType(), ToByteArraySerializationFormat);

        /// <summary>
        /// Converts the current <see cref="Event" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Event" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Description)}\": \"{Description}\" }}";

        /// <summary>
        /// Gets or sets the category of the event.
        /// </summary>
        [DataMember]
        public EventCategory Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a textual description of the event.
        /// </summary>
        [DataMember]
        public String Description
        {
            get
            {
                if (DescriptionReference.IsNullOrEmpty())
                {
                    DescriptionReference = DefaultDescription;
                }

                return DescriptionReference;
            }
            set => DescriptionReference = value;
        }

        /// <summary>
        /// Gets or sets a <see cref="DateTime" /> that indicates when the event occurred.
        /// </summary>
        [DataMember]
        public DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the verbosity level of the event.
        /// </summary>
        [DataMember]
        public EventVerbosity Verbosity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default <see cref="DateTime" /> that indicates when the event occurred.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static DateTime DefaultTimeStamp => Core.TimeStamp.Current;

        /// <summary>
        /// Gets the default textual description of the event.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String DefaultDescription => Category switch
        {
            EventCategory.ApplicationState => "An application state change occurred.",
            EventCategory.Domain => "A domain event occurred.",
            EventCategory.Error => "An error occurred.",
            EventCategory.GeneralInformation => "A general event occurred.",
            EventCategory.Security => "A security event occurred.",
            EventCategory.SystemState => "A system state change occurred.",
            EventCategory.Transaction => "A transaction occurred.",
            EventCategory.UserAction => "A user action was performed.",
            _ => "An unspecified event occurred."
        };

        /// <summary>
        /// Represents the default event verbosity.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const EventVerbosity DefaultVerbosity = EventVerbosity.Normal;

        /// <summary>
        /// Represents the serialization format that is used to produce a byte array when <see cref="ToByteArray" /> is invoked.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const SerializationFormat ToByteArraySerializationFormat = SerializationFormat.CompressedJson;

        /// <summary>
        /// Represents the default event category.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const EventCategory DefaultCategory = EventCategory.GeneralInformation;

        /// <summary>
        /// Represents a textual description of the event.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String DescriptionReference;
    }
}