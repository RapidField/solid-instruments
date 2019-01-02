// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Prototype.WebApplication.Models.FibonacciSequence
{
    /// <summary>
    /// Represents a view model for the path ~/FibonacciSequence
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel" /> class.
        /// </summary>
        public IndexModel()
            : this(null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel" /> class.
        /// </summary>
        /// <param name="terms">
        /// A collection of Fibonacci sequence terms.
        /// </param>
        public IndexModel(ICollection<Int64> terms)
        {
            Terms = terms;
        }

        /// <summary>
        /// Gets or sets a collection of Fibonacci sequence terms.
        /// </summary>
        public ICollection<Int64> Terms
        {
            get;
            set;
        }
    }
}