// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an application event.
    /// </summary>
    [DataContract]
    public class ApplicationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        public ApplicationEvent()
            : this(DefaultCategory)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="ApplicationEventCategory.Unspecified" />.
        /// </exception>
        public ApplicationEvent(ApplicationEventCategory category)
            : this(category, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="ApplicationEventCategory.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ApplicationEvent(ApplicationEventCategory category, ApplicationEventVerbosity verbosity)
            : this(category, verbosity, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event.
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="ApplicationEventCategory.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ApplicationEvent(ApplicationEventCategory category, ApplicationEventVerbosity verbosity, String description)
            : this(category, verbosity, description, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event.
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="environmentName">
        /// The name of the environment on which the event originated, or <see langword="null" /> to indicate the current
        /// environment.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="ApplicationEventCategory.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ApplicationEvent(ApplicationEventCategory category, ApplicationEventVerbosity verbosity, String description, String environmentName)
            : this(category, verbosity, description, environmentName, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event.
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="environmentName">
        /// The name of the environment on which the event originated, or <see langword="null" /> to indicate the current
        /// environment.
        /// </param>
        /// <param name="userInformation">
        /// Information about a user whose action(s) caused the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="ApplicationEventCategory.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ApplicationEvent(ApplicationEventCategory category, ApplicationEventVerbosity verbosity, String description, String environmentName, String userInformation)
            : this(category, verbosity, description, environmentName, userInformation, TimeStamp.Current)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="category">
        /// The category of the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level associated with the event.
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="environmentName">
        /// The name of the environment on which the event originated, or <see langword="null" /> to indicate the current
        /// environment.
        /// </param>
        /// <param name="userInformation">
        /// Information about a user whose action(s) caused the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="occurrenceDateTime">
        /// A <see cref="DateTime" /> that indicates when the event occurred. The default value is <see cref="TimeStamp.Current" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="category" /> is equal to <see cref="ApplicationEventCategory.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="ApplicationEventVerbosity.Unspecified" />.
        /// </exception>
        public ApplicationEvent(ApplicationEventCategory category, ApplicationEventVerbosity verbosity, String description, String environmentName, String userInformation, DateTime occurrenceDateTime)
        {
            Category = category.RejectIf().IsEqualToValue(ApplicationEventCategory.Unspecified, nameof(category));
            Description = description;
            EnvironmentName = environmentName ?? CurrentEnvironmentName;
            OccurrenceDateTime = occurrenceDateTime;
            UserInformation = userInformation;
            Verbosity = verbosity.RejectIf().IsEqualToValue(ApplicationEventVerbosity.Unspecified, nameof(verbosity));
        }

        /// <summary>
        /// Converts the current <see cref="ApplicationEvent" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ApplicationEvent" />.
        /// </returns>
        public override String ToString() => Summary;

        /// <summary>
        /// Safely returns the name of the current environment, or <see langword="null" /> if the name is not available.
        /// </summary>
        /// <returns>
        /// The name of the current environment, or <see langword="null" /> if the name is not available.
        /// </returns>
        [DebuggerHidden]
        private static String GetEnvironmentName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the category of the event.
        /// </summary>
        [DataMember]
        public ApplicationEventCategory Category
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
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the environment on which the event originated.
        /// </summary>
        [DataMember]
        public String EnvironmentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a <see cref="DateTime" /> that indicates when the event occurred.
        /// </summary>
        [DataMember]
        public DateTime OccurrenceDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a detailed summary of the event.
        /// </summary>
        [IgnoreDataMember]
        public String Summary
        {
            get
            {
                var summaryBuilder = new StringBuilder();

                switch (Category)
                {
                    case ApplicationEventCategory.ApplicationState:

                        summaryBuilder.Append($"An application state change occurred.{Environment.NewLine}");
                        break;

                    case ApplicationEventCategory.Error:

                        summaryBuilder.Append($"An error occurred.{Environment.NewLine}");
                        break;

                    case ApplicationEventCategory.Information:

                        summaryBuilder.Append($"An informational event occurred.{Environment.NewLine}");
                        break;

                    case ApplicationEventCategory.Security:

                        summaryBuilder.Append($"An application security event occurred.{Environment.NewLine}");
                        break;

                    case ApplicationEventCategory.Transaction:

                        summaryBuilder.Append($"A transaction occurred.{Environment.NewLine}");
                        break;

                    case ApplicationEventCategory.UserAction:

                        summaryBuilder.Append($"A user action occurred.{Environment.NewLine}");
                        break;

                    default:

                        summaryBuilder.Append($"An event occurred.{Environment.NewLine}");
                        break;
                }

                summaryBuilder.Append($"{Environment.NewLine}Occurrence date and time:{Environment.NewLine}");
                summaryBuilder.Append($"{OccurrenceDateTime.ToFullDetailString()}{Environment.NewLine}");

                if (EnvironmentName.IsNullOrEmpty() == false)
                {
                    summaryBuilder.Append($"{Environment.NewLine}Environment name:{Environment.NewLine}");
                    summaryBuilder.Append($"{EnvironmentName}{Environment.NewLine}");
                }

                if (UserInformation.IsNullOrEmpty() == false)
                {
                    summaryBuilder.Append($"{Environment.NewLine}User information:{Environment.NewLine}");
                    summaryBuilder.Append($"{UserInformation}{Environment.NewLine}");
                }

                if (Description.IsNullOrEmpty() == false)
                {
                    summaryBuilder.Append($"{Environment.NewLine}Description:{Environment.NewLine}");
                    summaryBuilder.Append($"{Description}{Environment.NewLine}");
                }

                return summaryBuilder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets information about a user whose action(s) caused the event.
        /// </summary>
        [DataMember]
        public String UserInformation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the verbosity level associated with the event.
        /// </summary>
        [DataMember]
        public ApplicationEventVerbosity Verbosity
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the default event category.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ApplicationEventCategory DefaultCategory = ApplicationEventCategory.Information;

        /// <summary>
        /// Represents the default event verbosity.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ApplicationEventVerbosity DefaultVerbosity = ApplicationEventVerbosity.Normal;

        /// <summary>
        /// Represents the name of the current environment name, or <see langword="null" /> if the name cannot be obtained.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly String CurrentEnvironmentName = GetEnvironmentName();
    }
}