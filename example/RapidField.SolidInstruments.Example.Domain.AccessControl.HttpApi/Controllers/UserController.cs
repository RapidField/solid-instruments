// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Mvc;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Example.Domain.Commands.ModelState.User;
using RapidField.SolidInstruments.Example.Domain.Messages.Command.ModelState.User;
using RapidField.SolidInstruments.Example.Domain.Models.User;
using System;
using System.Diagnostics;
using System.Net;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.HttpApi.Controllers
{
    /// <summary>
    /// Processes HTTP requests for the ~/User endpoint.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public sealed class UserController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public UserController(ICommandMediator mediator)
        {
            Mediator = mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument;
        }

        /// <summary>
        /// Handles DELETE requests for the endpoint.
        /// </summary>
        /// <param name="identifier">
        /// An identifier by which to find the <see cref="DomainModel" />.
        /// </param>
        /// <returns>
        /// A status code result.
        /// </returns>
        [HttpDelete]
        public IActionResult Delete([FromQuery] Guid identifier)
        {
            try
            {
                return Ok();
            }
            catch (ArgumentException exception)
            {
                return BadRequest($"{exception.Message} {exception.StackTrace}");
            }
            catch (Exception exception)
            {
                return StatusCode((Int32)HttpStatusCode.InternalServerError, $"{exception.Message} {exception.StackTrace}");
            }
        }

        /// <summary>
        /// Handles GET requests for the endpoint.
        /// </summary>
        /// <param name="identifier">
        /// An identifier by which to find the <see cref="DomainModel" />.
        /// </param>
        /// <returns>
        /// A <see cref="DomainModel" /> with matching <paramref name="identifier" />, or <see langword="null" /> if no matching
        /// model exists.
        /// </returns>
        [HttpGet]
        public IActionResult Get([FromQuery] Guid identifier)
        {
            try
            {
                var model = (DomainModel)null;
                return new JsonResult(model);
            }
            catch (ArgumentException exception)
            {
                return BadRequest($"{exception.Message} {exception.StackTrace}");
            }
            catch (Exception exception)
            {
                return StatusCode((Int32)HttpStatusCode.InternalServerError, $"{exception.Message} {exception.StackTrace}");
            }
        }

        /// <summary>
        /// Handles POST requests for the endpoint.
        /// </summary>
        /// <param name="model">
        /// The <see cref="DomainModel" /> to create.
        /// </param>
        /// <returns>
        /// A status code result.
        /// </returns>
        [HttpPost]
        public IActionResult Post([FromBody] DomainModel model)
        {
            try
            {
                _ = Mediator.Process(new CreateDomainModelCommandMessage(new CreateDomainModelCommand(model)));
                return Ok();
            }
            catch (ArgumentException exception)
            {
                return BadRequest($"{exception.Message} {exception.StackTrace}");
            }
            catch (Exception exception)
            {
                return StatusCode((Int32)HttpStatusCode.InternalServerError, $"{exception.Message} {exception.StackTrace}");
            }
        }

        /// <summary>
        /// Handles PUT requests for the endpoint.
        /// </summary>
        /// <param name="model">
        /// The <see cref="DomainModel" /> to create.
        /// </param>
        /// <returns>
        /// A status code result.
        /// </returns>
        [HttpPut]
        public IActionResult Put([FromBody] DomainModel model)
        {
            try
            {
                _ = Mediator.Process(new UpdateDomainModelCommandMessage(new UpdateDomainModelCommand(model)));
                return Ok();
            }
            catch (ArgumentException exception)
            {
                return BadRequest($"{exception.Message} {exception.StackTrace}");
            }
            catch (Exception exception)
            {
                return StatusCode((Int32)HttpStatusCode.InternalServerError, $"{exception.Message} {exception.StackTrace}");
            }
        }

        /// <summary>
        /// Represents processing intermediary that is used to process sub-commands.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICommandMediator Mediator;
    }
}