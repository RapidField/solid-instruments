// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents a command to perform a domain action which is related to an object that models a domain construct and which can
    /// be described by an <see cref="IEvent" />.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the command.
    /// </typeparam>
    public interface IDomainModelReportableCommand<TModel, TEvent> : IDomainModelCommand<TModel>, IDomainReportableCommand<TEvent>
        where TModel : class, IDomainModel
        where TEvent : class, IDomainModelEvent<TModel>
    {
    }
}