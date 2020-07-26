// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Manages object creation, storage, resolution and disposal for a related group of object instances.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectContainer" /> is the default implementation of <see cref="IObjectContainer" />.
    /// </remarks>
    public sealed class ObjectContainer : ConfigurableInstrument<ObjectContainerConfiguration>, IObjectContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainer" /> class.
        /// </summary>
        /// <param name="factoryConfigurator">
        /// An action that configures the object factory for the container.
        /// </param>
        /// <param name="definitionConfigurator">
        /// An action that configures the request-product type pair definitions for the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="factoryConfigurator" /> is <see langword="null" /> -or- <paramref name="definitionConfigurator" /> is
        /// <see langword="null" />.
        /// </exception>
        public ObjectContainer(Action<IObjectFactoryConfigurationProductionFunctions> factoryConfigurator, Action<IObjectContainerConfigurationDefinitions> definitionConfigurator)
            : this(InitializeFactory(DefaultConfiguration, factoryConfigurator.RejectIf().IsNull()), definitionConfigurator)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainer" /> class.
        /// </summary>
        /// <param name="factory">
        /// A factory that produces objects for the container.
        /// </param>
        /// <param name="definitionConfigurator">
        /// An action that configures the request-product type pair definitions for the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="definitionConfigurator" /> is <see langword="null" /> -or- <paramref name="factory" /> is
        /// <see langword="null" />.
        /// </exception>
        public ObjectContainer(IObjectFactory factory, Action<IObjectContainerConfigurationDefinitions> definitionConfigurator)
            : this(DefaultConfiguration, factory, definitionConfigurator, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainer" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factory">
        /// A factory that produces objects for the container.
        /// </param>
        /// <param name="definitionConfigurator">
        /// An action that configures the request-product type pair definitions for the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="definitionConfigurator" />
        /// is <see langword="null" /> -or- <paramref name="factory" /> is <see langword="null" />.
        /// </exception>
        public ObjectContainer(IConfiguration applicationConfiguration, IObjectFactory factory, Action<IObjectContainerConfigurationDefinitions> definitionConfigurator)
            : this(applicationConfiguration, factory, definitionConfigurator, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainer" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factory">
        /// A factory that produces objects for the container.
        /// </param>
        /// <param name="definitionConfigurator">
        /// An action that configures the request-product type pair definitions for the container.
        /// </param>
        /// <param name="managesFactory">
        /// A valued that indicates whether or not <paramref name="factory" /> is managed by the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="definitionConfigurator" />
        /// is <see langword="null" /> -or- <paramref name="factory" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private ObjectContainer(IConfiguration applicationConfiguration, IObjectFactory factory, Action<IObjectContainerConfigurationDefinitions> definitionConfigurator, Boolean managesFactory)
            : base(applicationConfiguration)
        {
            DefinitionConfigurator = definitionConfigurator.RejectIf().IsNull(nameof(definitionConfigurator));
            Factory = factory.RejectIf().IsNull(nameof(factory)).TargetArgument;
            LazyInstanceDictionary = new Lazy<IDictionary<Type, Object>>(InitializeInstanceDictionary, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyInstanceGroup = new Lazy<IFactoryProducedInstanceGroup>(InitializeInstanceGroup, LazyThreadSafetyMode.PublicationOnly);
            ManagesFactory = managesFactory;
        }

        /// <summary>
        /// Returns the instance of specified type that is managed by the current <see cref="ObjectContainer" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the instance to return.
        /// </typeparam>
        /// <returns>
        /// The instance of specified type that is managed by the current <see cref="ObjectContainer" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T" /> is not a supported type for the group.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        public T Get<T>()
            where T : class
        {
            var requestType = typeof(T);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                try
                {
                    if (Registrations.ContainsKey(requestType) == false)
                    {
                        throw new ArgumentException($"{requestType.FullName} is not a registered request type.", nameof(T));
                    }
                }
                catch (ObjectConfigurationException exception)
                {
                    throw new ObjectProductionException(requestType, exception);
                }

                var productType = Registrations[requestType].ProductType;

                if (Factory.SupportedProductTypes.Contains(productType) == false)
                {
                    throw new ArgumentException($"{productType.FullName} is not a registered product type.", nameof(T));
                }

                T lazyProduct;

                try
                {
                    var lazyInitializer = InstanceDictionary[requestType];
                    var lazyValueProperty = lazyInitializer.GetType().GetProperty(nameof(Lazy<T>.Value));
                    lazyProduct = lazyValueProperty.GetValue(lazyInitializer) as T;
                }
                catch (Exception exception)
                {
                    throw new ObjectProductionException(requestType, exception);
                }

                if (lazyProduct is null)
                {
                    throw new ArgumentException("The factory does not support the registered types.", nameof(T));
                }

                return lazyProduct;
            }
        }

        /// <summary>
        /// Returns a new instance of specified type that is managed by the current <see cref="ObjectContainer" />.
        /// </summary>
        /// <remarks>
        /// <see cref="GetNew{T}" /> differs from <see cref="Get{T}" /> in that it returns a distinct instance for every subsequent
        /// call.
        /// </remarks>
        /// <typeparam name="T">
        /// The type of the instance to return.
        /// </typeparam>
        /// <returns>
        /// The instance of specified type that is managed by the current <see cref="ObjectContainer" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T" /> is not a supported type for the group.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        public T GetNew<T>()
            where T : class
        {
            RejectIfDisposed();
            var requestType = typeof(T);

            try
            {
                if (Registrations.ContainsKey(requestType) == false)
                {
                    throw new ArgumentException($"{requestType.FullName} is not a registered request type.", nameof(T));
                }

                var productType = Registrations[requestType].ProductType;
                var getNewMethod = InstanceGroup.GetType().GetMethod(nameof(InstanceGroup.GetNew), Array.Empty<Type>()).MakeGenericMethod(productType);

                if (!(getNewMethod.Invoke(InstanceGroup, Array.Empty<Object>()) is T newProduct))
                {
                    throw new ArgumentException("The factory does not support the registered types.", nameof(T));
                }

                return newProduct;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (ObjectProductionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ObjectProductionException(requestType, exception);
            }
        }

        /// <summary>
        /// Configures the current <see cref="ObjectContainer" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="ObjectContainer" />.
        /// </param>
        protected sealed override void Configure(ObjectContainerConfiguration configuration) => DefinitionConfigurator(configuration.Definitions);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectContainer" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    LazyInstanceGroup?.Dispose();

                    if (ManagesFactory)
                    {
                        Factory?.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Initializes and configures a new <see cref="IObjectFactory" /> for the container.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factoryConfigurator">
        /// An action that configures the object factory for the container.
        /// </param>
        /// <returns>
        /// A new <see cref="IObjectFactory" /> for the container.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static IObjectFactory InitializeFactory(IConfiguration applicationConfiguration, Action<IObjectFactoryConfigurationProductionFunctions> factoryConfigurator) => new ContainerObjectFactory(applicationConfiguration, factoryConfigurator);

        /// <summary>
        /// Gets a collection of managed object instances.
        /// </summary>
        /// <returns>
        /// A collection of managed object instances.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The object container is not configured or the factory does not support the registered types.
        /// </exception>
        [DebuggerHidden]
        private IDictionary<Type, Object> InitializeInstanceDictionary()
        {
            var instanceDictionary = new ConcurrentDictionary<Type, Object>();

            try
            {
                foreach (var definition in Registrations.Values)
                {
                    var getLazyMethodInfo = InstanceGroup.GetType().GetMethod(nameof(InstanceGroup.GetLazy)).MakeGenericMethod(definition.ProductType);
                    var lazyInstance = getLazyMethodInfo.Invoke(InstanceGroup, Array.Empty<Object>());
                    instanceDictionary.AddOrUpdate(definition.RequestType, lazyInstance, (key, oldValue) => lazyInstance);
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"The object container is not configured or the factory does not support the registered types. See inner exception.", exception);
            }

            return instanceDictionary;
        }

        /// <summary>
        /// Initializes an object that manages creation and lifetime of instances for the current <see cref="ObjectContainer" />.
        /// </summary>
        /// <returns>
        /// An object that manages creation and lifetime of instances for the current <see cref="ObjectContainer" />.
        /// </returns>
        [DebuggerHidden]
        private IFactoryProducedInstanceGroup InitializeInstanceGroup() => new FactoryProducedInstanceGroup(Factory);

        /// <summary>
        /// Gets the types of the instances that are managed by the current <see cref="ObjectContainer" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<Type> InstanceTypes
        {
            get
            {
                foreach (var instanceType in Registrations.Keys)
                {
                    RejectIfDisposed();
                    yield return instanceType;
                }
            }
        }

        /// <summary>
        /// Gets a collection of request-product type pairs that constitute the configured definitions for the container.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<Type, IObjectContainerDefinition> Registrations => ((ObjectContainerConfigurationDefinitions)Configuration.Definitions).Registrations;

        /// <summary>
        /// Gets a collection of managed object instances.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The object container is not configured or the factory does not support the registered types.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<Type, Object> InstanceDictionary => LazyInstanceDictionary.Value;

        /// <summary>
        /// Gets an object that manages creation and lifetime of instances for the current <see cref="ObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IFactoryProducedInstanceGroup InstanceGroup => LazyInstanceGroup.Value;

        /// <summary>
        /// Represents an action that configures the request-product type pair definitions for the container.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<IObjectContainerConfigurationDefinitions> DefinitionConfigurator;

        /// <summary>
        /// Represents an factory that produces objects for the current <see cref="ObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IObjectFactory Factory;

        /// <summary>
        /// Represents a lazily-initialized collection of managed object instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDictionary<Type, Object>> LazyInstanceDictionary;

        /// <summary>
        /// Represents a lazily-initialized object that manages creation and lifetime of instances for the current
        /// <see cref="ObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IFactoryProducedInstanceGroup> LazyInstanceGroup;

        /// <summary>
        /// Represents a valued that indicates whether or not the current <see cref="ObjectContainer" /> manages
        /// <see cref="Factory" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean ManagesFactory;

        /// <summary>
        /// Represents the default <see cref="IObjectFactory" /> that is used to produce object instances for
        /// <see cref="ObjectContainer" /> instances.
        /// </summary>
        private sealed class ContainerObjectFactory : ObjectFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ContainerObjectFactory" /> class.
            /// </summary>
            /// <param name="applicationConfiguration">
            /// Configuration information for the application.
            /// </param>
            /// <param name="factoryConfigurator">
            /// An action that configures the object factory.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
            /// </exception>
            [DebuggerHidden]
            internal ContainerObjectFactory(IConfiguration applicationConfiguration, Action<IObjectFactoryConfigurationProductionFunctions> factoryConfigurator)
                : base(applicationConfiguration)
            {
                FactoryConfigurator = new Action<IObjectFactoryConfigurationProductionFunctions<Object>>((functions) =>
                {
                    factoryConfigurator(new ObjectFactoryConfigurationProductionFunctions(functions));
                });
            }

            /// <summary>
            /// Configures the current <see cref="ContainerObjectFactory" />.
            /// </summary>
            /// <param name="configuration">
            /// Configuration information for the current <see cref="ContainerObjectFactory" />.
            /// </param>
            protected sealed override void Configure(ObjectFactoryConfiguration configuration) => FactoryConfigurator(configuration.ProductionFunctions);

            /// <summary>
            /// Releases all resources consumed by the current <see cref="ContainerObjectFactory" />.
            /// </summary>
            /// <param name="disposing">
            /// A value indicating whether or not managed resources should be released.
            /// </param>
            protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

            /// <summary>
            /// Represents an action that configures the object factory.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly Action<IObjectFactoryConfigurationProductionFunctions<Object>> FactoryConfigurator;
        }
    }
}