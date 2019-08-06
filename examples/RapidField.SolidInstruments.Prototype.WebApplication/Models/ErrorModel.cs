// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Example.WebApplication.Models
{
    /// <summary>
    /// Represents a view model for the path ~/Error
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel" /> class.
        /// </summary>
        public ErrorModel()
            : this(null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel" /> class.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        public ErrorModel(String requestId)
        {
            RequestId = requestId;
        }

        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public String RequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether or not to display the request identifier.
        /// </summary>
        public Boolean ShowRequestId => RequestId.IsNullOrEmpty() == false;
    }
}