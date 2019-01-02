// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.ObjectComposition;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDataAccessRepository" /> instances that map to database entities.
    /// </summary>
    public interface IDataAccessRepositoryFactory : IObjectFactory<IDataAccessRepository>
    {
    }
}