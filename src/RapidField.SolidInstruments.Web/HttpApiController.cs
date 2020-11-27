// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Mvc;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Net;

namespace RapidField.SolidInstruments.Web
{
    /// <summary>
    /// Processes HTTP requests for an API endpoint.
    /// </summary>
    public abstract class HttpApiController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiController" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected HttpApiController(ICommandMediator mediator)
            : base()
        {
            Mediator = mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument;
        }

        /// <summary>
        /// Creates a <see cref="BadRequestObjectResult" />.
        /// </summary>
        /// <param name="exception">
        /// The exception that was raised which represents the server error. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A <see cref="BadRequestObjectResult" />.
        /// </returns>
        protected BadRequestObjectResult BadRequest(Exception exception) => BadRequest(exception, null);

        /// <summary>
        /// Creates a <see cref="BadRequestObjectResult" />.
        /// </summary>
        /// <param name="message">
        /// A message to include with the HTTP response. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A <see cref="BadRequestObjectResult" />.
        /// </returns>
        protected BadRequestObjectResult BadRequest(String message) => BadRequest(null, message);

        /// <summary>
        /// Creates a <see cref="BadRequestObjectResult" />.
        /// </summary>
        /// <param name="exception">
        /// The exception that was raised which represents the server error. The default value is <see langword="null" />.
        /// </param>
        /// <param name="message">
        /// A message to include with the HTTP response. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A <see cref="BadRequestObjectResult" />.
        /// </returns>
        protected BadRequestObjectResult BadRequest(Exception exception, String message)
        {
            var processedMessage = (message.IsNullOrEmpty() ? exception?.Message : message) ?? DefaultBadRequestMessage;
            return BadRequest(error: ConveysStackTraceInFailureResponses ? $"{processedMessage} {exception?.StackTrace}" : processedMessage);
        }

        /// <summary>
        /// Creates an <see cref="ObjectResult" /> with HTTP status code 500.
        /// </summary>
        /// <returns>
        /// An <see cref="ObjectResult" /> with HTTP status code 500.
        /// </returns>
        protected ObjectResult InternalServerError() => InternalServerError(null, null);

        /// <summary>
        /// Creates an <see cref="ObjectResult" /> with HTTP status code 500.
        /// </summary>
        /// <param name="exception">
        /// The exception that was raised which represents the server error. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// An <see cref="ObjectResult" /> with HTTP status code 500.
        /// </returns>
        protected ObjectResult InternalServerError(Exception exception) => InternalServerError(exception, null);

        /// <summary>
        /// Creates an <see cref="ObjectResult" /> with HTTP status code 500.
        /// </summary>
        /// <param name="message">
        /// A message to include with the HTTP response. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// An <see cref="ObjectResult" /> with HTTP status code 500.
        /// </returns>
        protected ObjectResult InternalServerError(String message) => InternalServerError(null, message);

        /// <summary>
        /// Creates an <see cref="ObjectResult" /> with HTTP status code 500.
        /// </summary>
        /// <param name="exception">
        /// The exception that was raised which represents the server error. The default value is <see langword="null" />.
        /// </param>
        /// <param name="message">
        /// A message to include with the HTTP response. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// An <see cref="ObjectResult" /> with HTTP status code 500.
        /// </returns>
        protected ObjectResult InternalServerError(Exception exception, String message)
        {
            var processedMessage = (message.IsNullOrEmpty() ? exception?.Message : message) ?? DefaultInternalServerErrorMessage;
            return StatusCode((Int32)HttpStatusCode.InternalServerError, ConveysStackTraceInFailureResponses ? $"{processedMessage} {exception?.StackTrace}" : processedMessage);
        }

        /// <summary>
        /// Represents a value indicating whether or not stack trace information is included with failure responses.
        /// </summary>
        protected virtual Boolean ConveysStackTraceInFailureResponses => DefaultConveysStackTraceInFailureResponses;

        /// <summary>
        /// Represents the standard route for an HTTP API controller.
        /// </summary>
        protected const String StandardControllerRoute = "[controller]";

        /// <summary>
        /// Represents processing intermediary that is used to process sub-commands.
        /// </summary>
        protected readonly ICommandMediator Mediator;

        /// <summary>
        /// Represents the default error message that is provided by <see cref="BadRequest(Exception, String)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultBadRequestMessage = "The request was improperly formed or invalid.";

        /// <summary>
        /// Represents the default value indicating whether or not stack trace information is included with failure responses.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultConveysStackTraceInFailureResponses = false;

        /// <summary>
        /// Represents the default error message that is provided by <see cref="InternalServerError(Exception, String)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultInternalServerErrorMessage = "An error occurred on the server while processing the request.";
    }
}