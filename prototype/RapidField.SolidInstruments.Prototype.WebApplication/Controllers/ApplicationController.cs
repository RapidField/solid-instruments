// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;

namespace RapidField.SolidInstruments.Prototype.WebApplication.Controllers
{
    /// <summary>
    /// Represents a base controller for the application.
    /// </summary>
    public abstract class ApplicationController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary for commands issued by the controller.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="mediator" /> is
        /// <see langword="null" />.
        /// </exception>
        protected ApplicationController(IConfiguration applicationConfiguration, ICommandMediator mediator)
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
            Mediator = mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument;
        }

        /// <summary>
        /// Gets configuration information for the application.
        /// </summary>
        protected IConfiguration ApplicationConfiguration
        {
            get;
        }

        /// <summary>
        /// Gets a processing intermediary for commands issued by the controller.
        /// </summary>
        protected ICommandMediator Mediator
        {
            get;
        }
    }
}