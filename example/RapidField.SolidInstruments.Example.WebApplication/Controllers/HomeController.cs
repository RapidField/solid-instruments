// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Example.WebApplication.Models;
using RapidField.SolidInstruments.Example.WebApplication.Models.Home;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.WebApplication.Controllers
{
    /// <summary>
    /// Processes requests for the path ~/
    /// </summary>
    public sealed class HomeController : ApplicationController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
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
        public HomeController(IConfiguration applicationConfiguration, ICommandMediator mediator)
            : base(applicationConfiguration, mediator)
        {
            return;
        }

        /// <summary>
        /// Processes GET requests for the path ~/Error
        /// </summary>
        /// <returns>
        /// A <see cref="ViewResult" />.
        /// </returns>
        [HttpGet]
        public IActionResult Error() => View(new ErrorModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));

        /// <summary>
        /// Processes GET requests for the path ~/
        /// </summary>
        /// <returns>
        /// A <see cref="ViewResult" />.
        /// </returns>
        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexModel();
            return View(model);
        }
    }
}