// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac.Builder;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="IRegistrationBuilder{TLimit, TActivatorData, TRegistrationStyle}" /> interface with inversion of
    /// control features.
    /// </summary>
    internal static class IRegistrationBuilderExtensions
    {
        /// <summary>
        /// Applies a service lifetime to a dependency registration.
        /// </summary>
        /// <typeparam name="TActivatorData">
        /// The type of the activator data.
        /// </typeparam>
        /// <typeparam name="TRegistrationStyle">
        /// The type of the registration style.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IRegistrationBuilder{TLimit, TActivatorData, TRegistrationStyle}" />.
        /// </param>
        /// <param name="lifetime">
        /// The service lifetime to apply.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IRegistrationBuilder{TLimit, TActivatorData, TRegistrationStyle}" />.
        /// </returns>
        [DebuggerHidden]
        internal static IRegistrationBuilder<Object, TActivatorData, TRegistrationStyle> WithLifetime<TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<Object, TActivatorData, TRegistrationStyle> target, ServiceLifetime lifetime) => lifetime switch
        {
            ServiceLifetime.Singleton => target.SingleInstance(),
            ServiceLifetime.Scoped => target.InstancePerLifetimeScope(),
            ServiceLifetime.Transient => target.InstancePerDependency(),
            _ => throw new UnsupportedSpecificationException($"The specified service lifetime, {lifetime}, is not supported.")
        };
    }
}