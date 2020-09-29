// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Example.Domain.Commands.ModelState.User;
using RapidField.SolidInstruments.Example.Domain.Messages.Command.ModelState.User;
using RapidField.SolidInstruments.Example.Domain.Models.User;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;

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
            : base()
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{identifier:Guid}")]
        public ActionResult Delete([FromRoute] Guid identifier)
        {
            try
            {
                var model = Mediator.Process<DomainModel>(new FindDomainModelByIdentifierCommand(identifier));

                if (model is null)
                {
                    return NotFound(identifier);
                }

                _ = Mediator.Process(new DeleteDomainModelCommandMessage(new DeleteDomainModelCommand(model)));
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
        [ProducesResponseType(typeof(DomainModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{identifier:Guid}")]
        public ActionResult<DomainModel> Get([FromRoute] Guid identifier)
        {
            try
            {
                var model = Mediator.Process<DomainModel>(new FindDomainModelByIdentifierCommand(identifier));

                if (model is null)
                {
                    return NotFound(identifier);
                }

                var serializer = new JsonSerializer<DomainModel>();
                var response = Encoding.UTF8.GetString(serializer.Serialize(model));
                return Ok(response);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Post([FromBody] DomainModel model)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put([FromBody] DomainModel model)
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