// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents configuration information for an <see cref="ObjectContainer" /> instance.
    /// </summary>
    public sealed class ObjectContainerConfiguration : InstrumentConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainerConfiguration" /> class.
        /// </summary>
        public ObjectContainerConfiguration()
            : base()
        {
            Definitions = new ObjectContainerConfigurationDefinitions();
        }

        /// <summary>
        /// Gets a collection of supported request-product type pairs for the associated <see cref="ObjectContainer" />.
        /// </summary>
        public IObjectContainerConfigurationDefinitions Definitions
        {
            get;
        }
    }
}