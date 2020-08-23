// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.DataAccess.Autofac.Ef.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with Autofac inversion of control features to support Entity Framework
    /// data access abstractions.
    /// </summary>
    public static class ContainerBuilderExtensions
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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static void RegisterSupportingTypesForEntityFrameworkDataAccess<TContext, TRepositoryFactory>(this ContainerBuilder target, IConfiguration applicationConfiguration)
            where TContext : DbContext
            where TRepositoryFactory : class, IEntityFrameworkRepositoryFactory<TContext> => target.RegisterSupportingTypesForEntityFrameworkDataAccess(applicationConfiguration, typeof(TContext), typeof(TRepositoryFactory));

        /// <summary>
        /// Registers a collection of types that establish support for data access functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="contextType" /> is
        /// <see langword="null" /> -or- <paramref name="repositoryFactoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="contextType" /> does not derive from <see cref="DbContext" />-or-
        /// <paramref name="repositoryFactoryType" /> does not implement <see cref="IEntityFrameworkRepositoryFactory{TContext}" />.
        /// </exception>
        public static void RegisterSupportingTypesForEntityFrameworkDataAccess(this ContainerBuilder target, IConfiguration applicationConfiguration, Type contextType, Type repositoryFactoryType)
        {
            var entityFrameworkRepositoryFactoryInterfaceType = EntityFrameworkRepositoryFactoryInterfaceType.MakeGenericType(contextType.RejectIf().IsNull(nameof(contextType)).OrIf().IsNotSupportedType(DbContextType, nameof(contextType)));
            var entityFrameworkTransactionInterfaceType = EntityFrameworkTransactionInterfaceType.MakeGenericType(contextType);
            var entityFrameworkTransactionType = EntityFrameworkTransactionType.MakeGenericType(contextType);
            target.RegisterApplicationConfiguration(applicationConfiguration);
            target.RegisterType(contextType).IfNotRegistered(contextType).AsSelf().InstancePerLifetimeScope();
            target.RegisterType(repositoryFactoryType.RejectIf().IsNull(nameof(repositoryFactoryType)).OrIf().IsNotSupportedType(entityFrameworkRepositoryFactoryInterfaceType, nameof(repositoryFactoryType))).IfNotRegistered(repositoryFactoryType).As(entityFrameworkRepositoryFactoryInterfaceType).As(DataAccessRepositoryFactoryInterfaceType).AsSelf().InstancePerLifetimeScope();
            target.RegisterType(entityFrameworkTransactionType).IfNotRegistered(entityFrameworkTransactionType).As(entityFrameworkTransactionInterfaceType).As(DataAccessTransactionInterfaceType).AsSelf().InstancePerLifetimeScope();
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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static void RegisterSupportingTypesForPlatformTargetedDataAccess<TBaseContext, TTargetedContext, TRepositoryFactory>(this ContainerBuilder target, IConfiguration applicationConfiguration)
            where TBaseContext : ConfiguredContext
            where TTargetedContext : TBaseContext
            where TRepositoryFactory : class, IEntityFrameworkRepositoryFactory<TBaseContext>
        {
            target.RegisterApplicationConfiguration(applicationConfiguration);
            target.RegisterType<TTargetedContext>().IfNotRegistered(typeof(TTargetedContext)).As<TBaseContext>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<TRepositoryFactory>().IfNotRegistered(typeof(TRepositoryFactory)).As<IEntityFrameworkRepositoryFactory<TBaseContext>>().As<IDataAccessRepositoryFactory>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<EntityFrameworkTransaction<TBaseContext>>().IfNotRegistered(typeof(EntityFrameworkTransaction<TBaseContext>)).As<IEntityFrameworkTransaction<TBaseContext>>().As<IDataAccessTransaction>().AsSelf().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Represents the <see cref="IDataAccessRepositoryFactory" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessRepositoryFactoryInterfaceType = typeof(IDataAccessRepositoryFactory);

        /// <summary>
        /// Represents the <see cref="IDataAccessTransaction" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessTransactionInterfaceType = typeof(IDataAccessTransaction);

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

        /// <summary>
        /// Represents the <see cref="IEntityFrameworkTransaction{TContext}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EntityFrameworkTransactionInterfaceType = typeof(IEntityFrameworkTransaction<>);

        /// <summary>
        /// Represents the <see cref="EntityFrameworkTransaction{TContext}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EntityFrameworkTransactionType = typeof(EntityFrameworkTransaction<>);
    }
}