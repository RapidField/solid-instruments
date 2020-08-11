// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.DataAccess.DotNetNative.Ef.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support Entity
    /// Framework data access abstractions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a collection of types that establish support for data access functionality.
        /// </summary>
        /// <typeparam name="TContext">
        /// The type of the database session that is used by the produced repositories.
        /// </typeparam>
        /// <typeparam name="TRepositoryFactory">
        /// The type of the data access repository factory that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForEntityFrameworkDataAccess<TContext, TRepositoryFactory>(this IServiceCollection target, IConfiguration applicationConfiguration)
            where TContext : DbContext
            where TRepositoryFactory : class, IEntityFrameworkRepositoryFactory<TContext> => target.AddSupportingTypesForEntityFrameworkDataAccess(applicationConfiguration, typeof(TContext), typeof(TRepositoryFactory));

        /// <summary>
        /// Registers a collection of types that establish support for data access functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="contextType">
        /// The type of the database session that is used by the produced repositories.
        /// </param>
        /// <param name="repositoryFactoryType">
        /// The type of the data access repository factory that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="contextType" /> is
        /// <see langword="null" /> -or- <paramref name="repositoryFactoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="contextType" /> does not derive from <see cref="DbContext" />-or-
        /// <paramref name="repositoryFactoryType" /> does not implement <see cref="IEntityFrameworkRepositoryFactory{TContext}" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForEntityFrameworkDataAccess(this IServiceCollection target, IConfiguration applicationConfiguration, Type contextType, Type repositoryFactoryType)
        {
            var entityFrameworkRepositoryFactoryInterfaceType = EntityFrameworkRepositoryFactoryInterfaceType.MakeGenericType(contextType.RejectIf().IsNull(nameof(contextType)).OrIf().IsNotSupportedType(DbContextType, nameof(contextType)));
            target.AddApplicationConfiguration(applicationConfiguration);
            target.TryAddScoped(contextType);
            target.TryAddScoped(repositoryFactoryType.RejectIf().IsNull(nameof(repositoryFactoryType)).OrIf().IsNotSupportedType(entityFrameworkRepositoryFactoryInterfaceType, nameof(repositoryFactoryType)));
            target.TryAddScoped(entityFrameworkRepositoryFactoryInterfaceType, repositoryFactoryType);
            return target;
        }

        /// <summary>
        /// Registers a collection of types that establish support for platform-targeted data access functionality.
        /// </summary>
        /// <typeparam name="TBaseContext">
        /// The type of the database session that is used by the produced repositories.
        /// </typeparam>
        /// <typeparam name="TTargetedContext">
        /// The derived, platform-specific type of the database session.
        /// </typeparam>
        /// <typeparam name="TRepositoryFactory">
        /// The type of the data access repository factory that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForPlatformTargetedDataAccess<TBaseContext, TTargetedContext, TRepositoryFactory>(this IServiceCollection target, IConfiguration applicationConfiguration)
            where TBaseContext : ConfiguredContext
            where TTargetedContext : TBaseContext
            where TRepositoryFactory : class, IEntityFrameworkRepositoryFactory<TBaseContext>
        {
            target.AddApplicationConfiguration(applicationConfiguration);
            target.TryAddScoped<TTargetedContext>();
            target.TryAddScoped<TBaseContext, TTargetedContext>();
            target.TryAddScoped<TRepositoryFactory>();
            target.TryAddScoped<IEntityFrameworkRepositoryFactory<TBaseContext>, TRepositoryFactory>();
            return target;
        }

        /// <summary>
        /// Represents the <see cref="DbContext" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DbContextType = typeof(DbContext);

        /// <summary>
        /// Represents the <see cref="IEntityFrameworkRepositoryFactory{TContext}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EntityFrameworkRepositoryFactoryInterfaceType = typeof(IEntityFrameworkRepositoryFactory<>);
    }
}