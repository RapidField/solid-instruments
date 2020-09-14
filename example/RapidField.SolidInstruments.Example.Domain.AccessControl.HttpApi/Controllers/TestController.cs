// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Mvc;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.HttpApi.Controllers
{
    /// <summary>
    /// Processes HTTP requests for the ~/Test endpoint.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public sealed class TestController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        public TestController()
            : base()
        {
            return;
        }

        /// <summary>
        /// Handles GET requests for the endpoint.
        /// </summary>
        /// <returns>
        /// A status code result.
        /// </returns>
        [HttpGet]
        public IActionResult Get() => Ok();
    }
}