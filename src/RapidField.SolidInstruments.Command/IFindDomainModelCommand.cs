// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to find and return an object that models a domain construct.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    public interface IFindDomainModelCommand<TModel> : IDomainCommand<TModel>
        where TModel : class, IDomainModel
    {
    }
}