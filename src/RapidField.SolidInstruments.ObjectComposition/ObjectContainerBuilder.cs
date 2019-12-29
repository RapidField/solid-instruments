// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents an object that configures and initializes new <see cref="IObjectContainer" /> instances.
    /// </summary>
    public sealed class ObjectContainerBuilder : ObjectBuilder<IObjectContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainerBuilder" /> class.
        /// </summary>
        public ObjectContainerBuilder()
            : base()
        {
            return;
        }

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" />.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container.
        /// </typeparam>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public ObjectContainerBuilder ConfigureProductType<TProduct>()
            where TProduct : class, new() => ConfigureProductType(() => new TProduct());

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" />.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container.
        /// </typeparam>
        /// <param name="productionFunction">
        /// A function that produces the specified type.
        /// </param>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productionFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public ObjectContainerBuilder ConfigureProductType<TProduct>(Func<TProduct> productionFunction)
            where TProduct : class => ConfigureRequestProductTypePair<TProduct, TProduct>(productionFunction);

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" /> in response to
        /// a request for <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">
        /// The request type that identifies the registration.
        /// </typeparam>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container as a result of a request for <typeparamref name="TRequest" />.
        /// </typeparam>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public ObjectContainerBuilder ConfigureRequestProductTypePair<TRequest, TProduct>()
            where TRequest : class
            where TProduct : class, TRequest, new() => ConfigureRequestProductTypePair<TRequest, TProduct>(() => new TProduct());

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" /> in response to
        /// a request for <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">
        /// The request type that identifies the registration.
        /// </typeparam>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container as a result of a request for <typeparamref name="TRequest" />.
        /// </typeparam>
        /// <param name="productionFunction">
        /// A function that produces the specified type.
        /// </param>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productionFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public ObjectContainerBuilder ConfigureRequestProductTypePair<TRequest, TProduct>(Func<TProduct> productionFunction)
            where TRequest : class
            where TProduct : class, TRequest
        {
            productionFunction.RejectIf().IsNull(nameof(productionFunction));
            var requestType = typeof(TRequest);
            var productType = typeof(TProduct);
            var definitionKey = new ObjectContainerDefinition(requestType, productType);

            try
            {
                if (DefinitionConfigurationActions.Keys.Any(key => key.RequestType == requestType))
                {
                    throw new ArgumentException($"The builder is already configured for the request type {requestType.FullName}.", nameof(TRequest));
                }

                if (FunctionConfigurationActions.ContainsKey(definitionKey))
                {
                    throw new ArgumentException($"The builder is already configured for the request-product type pair: {requestType.FullName}, {productType.FullName}.", nameof(TProduct));
                }

                DefinitionConfigurationActions.Add(definitionKey, new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
                {
                    definitions.Add<TRequest, TProduct>();
                }));

                if (FunctionConfigurationActions.Keys.Any(definition => definition.ProductType == productType))
                {
                    return this;
                }

                FunctionConfigurationActions.Add(definitionKey, new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
                {
                    functions.Add(productionFunction);
                }));
            }
            catch (ArgumentException exception)
            {
                throw new ObjectBuilderException(GetType(), exception);
            }

            return this;
        }

        /// <summary>
        /// Produces the configured <see cref="ObjectContainer" /> instance.
        /// </summary>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised during finalization of the builder.
        /// </exception>
        protected sealed override IObjectContainer ToResult(ConcurrencyControlToken controlToken)
        {
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                foreach (var action in FunctionConfigurationActions.Values)
                {
                    action(functions);
                }
            });

            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                foreach (var action in DefinitionConfigurationActions.Values)
                {
                    action(definitions);
                }
            });

            return new ObjectContainer(factoryConfigurator, definitionConfigurator);
        }

        /// <summary>
        /// Represents a collection of actions that configure the definitions that control the behavior of the resulting
        /// <see cref="IObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDictionary<ObjectContainerDefinition, Action<ObjectContainerConfigurationDefinitions>> DefinitionConfigurationActions = new Dictionary<ObjectContainerDefinition, Action<ObjectContainerConfigurationDefinitions>>();

        /// <summary>
        /// Represents a collection of definitions that control the behavior of the resulting <see cref="IObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObjectContainerConfigurationDefinitions Definitions = new ObjectContainerConfigurationDefinitions();

        /// <summary>
        /// Represents a collection of actions that configure the functions that produce new object instances for the resulting
        /// <see cref="IObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDictionary<ObjectContainerDefinition, Action<ObjectFactoryConfigurationProductionFunctions>> FunctionConfigurationActions = new Dictionary<ObjectContainerDefinition, Action<ObjectFactoryConfigurationProductionFunctions>>();

        /// <summary>
        /// Represents a collection of configured types and functions that produce them using the resulting
        /// <see cref="IObjectContainer" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObjectFactoryConfigurationProductionFunctions Functions = new ObjectFactoryConfigurationProductionFunctions();
    }
}