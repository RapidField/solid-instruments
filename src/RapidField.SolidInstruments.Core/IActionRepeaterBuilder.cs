// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that configures and produces new <see cref="ActionRepeater" /> instances.
    /// </summary>
    public interface IActionRepeaterBuilder : IInstrumentBuilder<ActionRepeaterConfiguration, ActionRepeater>
    {
    }
}