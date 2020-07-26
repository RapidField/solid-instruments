// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information that is conveyed when an <see cref="Exception" /> has been raised.
    /// </summary>
    public interface IExceptionRaisedEvent : IErrorEvent
    {
        /// <summary>
        /// Gets or sets the full name of the type of the associated <see cref="Exception" /> that was raised.
        /// </summary>
        public String ExceptionTypeFullName
        {
            get;
            set;
        }
    }
}