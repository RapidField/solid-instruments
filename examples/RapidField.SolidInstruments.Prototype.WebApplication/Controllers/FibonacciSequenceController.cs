// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Example.DatabaseModel;
using RapidField.SolidInstruments.Example.WebApplication.Models.FibonacciSequence;
using RapidField.SolidInstruments.Mathematics.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Example.WebApplication.Controllers
{
    /// <summary>
    /// Processes requests for the path ~/FibonacciSequence
    /// </summary>
    public sealed class FibonacciSequenceController : ApplicationController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FibonacciSequenceController" /> class.
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
        public FibonacciSequenceController(IConfiguration applicationConfiguration, ICommandMediator mediator)
            : base(applicationConfiguration, mediator)
        {
            return;
        }

        /// <summary>
        /// Processes GET requests for the path ~/FibonacciSequence
        /// </summary>
        /// <returns>
        /// A <see cref="ViewResult" />.
        /// </returns>
        [HttpGet]
        public IActionResult Index()
        {
            var fibonacciNumberValues = new List<Int64>(Mediator.Process(ExampleCommands.GetFibonacciNumberValues()));
            var fibonacciSequence = FibonacciSequence.Classic;

            if (fibonacciNumberValues.Any())
            {
                while (true)
                {
                    var value = (Int64)fibonacciSequence.CalculateNext();

                    if (fibonacciNumberValues.Contains(value))
                    {
                        continue;
                    }

                    fibonacciNumberValues.Add(value);
                    Mediator.Process(ExampleCommands.AddFibonacciNumber(value));
                    break;
                }
            }
            else
            {
                var initialTerms = fibonacciSequence.ToArray(0, fibonacciSequence.CalculatedTermCount);

                foreach (var term in initialTerms)
                {
                    var value = (Int64)term;
                    fibonacciNumberValues.Add(value);
                    Mediator.Process(ExampleCommands.AddFibonacciNumber(value));
                }
            }

            var model = new IndexModel(fibonacciNumberValues);
            return View(model);
        }
    }
}