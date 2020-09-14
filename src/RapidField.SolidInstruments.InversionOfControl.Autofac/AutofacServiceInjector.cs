// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions;
using System;
using System.Diagnostics;
using System.Reflection;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac
{
    /// <summary>
    /// Facilitates injection of a collection of service descriptors into a <see cref="ContainerBuilder" />.
    /// </summary>
    public sealed class AutofacServiceInjector : ServiceInjector<ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceInjector" /> class.
        /// </summary>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors that are injected to a configurator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceDescriptors" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal AutofacServiceInjector(IServiceCollection serviceDescriptors)
            : base(serviceDescriptors)
        {
            return;
        }

        /// <summary>
        /// Adds a collection of service descriptors to a <see cref="ContainerBuilder" />.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures a container.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors that are added to the configurator.
        /// </param>
        protected sealed override void Inject(ContainerBuilder configurator, IServiceCollection serviceDescriptors)
        {
            configurator.RegisterType<ServiceProvider>().As<IServiceProvider>();
            configurator.RegisterType<ServiceScopeFactory>().As<IServiceScopeFactory>();

            foreach (var serviceDescriptor in serviceDescriptors)
            {
                if ((serviceDescriptor.ImplementationType is null) == false)
                {
                    if (serviceDescriptor.ServiceType.GetTypeInfo().IsGenericTypeDefinition)
                    {
                        configurator.RegisterGeneric(serviceDescriptor.ImplementationType).As(serviceDescriptor.ServiceType).WithLifetime(serviceDescriptor.Lifetime);
                        continue;
                    }

                    configurator.RegisterType(serviceDescriptor.ImplementationType).As(serviceDescriptor.ServiceType).WithLifetime(serviceDescriptor.Lifetime);
                }
                else if ((serviceDescriptor.ImplementationFactory is null) == false)
                {
                    configurator.RegisterComponent(RegistrationBuilder.ForDelegate(serviceDescriptor.ServiceType, (context, parameters) => serviceDescriptor.ImplementationFactory(context.Resolve<IServiceProvider>())).WithLifetime(serviceDescriptor.Lifetime).CreateRegistration());
                }
                else
                {
                    configurator.RegisterInstance(serviceDescriptor.ImplementationInstance).As(serviceDescriptor.ServiceType).WithLifetime(serviceDescriptor.Lifetime);
                }
            }
        }
    }
}