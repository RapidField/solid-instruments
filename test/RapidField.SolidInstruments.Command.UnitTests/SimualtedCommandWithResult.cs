﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Command.UnitTests
{
    /// <summary>
    /// Represents an <see cref="Command{TResult}" /> derivative that is used for testing.
    /// </summary>
    [DataContract]
    internal class SimulatedCommandWithResult : Command<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedCommandWithResult" /> class.
        /// </summary>
        public SimulatedCommandWithResult()
            : base()
        {
            Identifier = Guid.NewGuid();
            IsProcessed = false;
        }

        /// <summary>
        /// Gets or sets an identifier for the command.
        /// </summary>
        [DataMember]
        public Guid Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the current <see cref="SimulatedCommandWithResult" /> has been processed.
        /// </summary>
        [DataMember]
        public Boolean IsProcessed
        {
            get;
            set;
        }
    }
}