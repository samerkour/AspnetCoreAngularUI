using STech.CrossCutting;
using STech.Domain.Interfaces;
using STech.Infrastructure.Context;
using IdentityModel.OidcClient;
using Is4.IdentityServer4.Shared.Configuration.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STech.Presentation.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    public class ValuesController : BaseController
    {
        private readonly IStringLocalizer<ValuesController> _localizer;

        public ValuesController(
              ILogger<ValuesController> logger
            , IHttpContextAccessor httpContextAccessor
            , IStringLocalizer<ValuesController> localizer
            , IHttpClientFactory httpClientFactory
            , IDistributedCache cache
            , IConfiguration configuration
            , IStringLocalizer<SharedResource> sharedLocalizer
            , IUnitOfWork<PortEmployeesDbContext> unitOfWork)
            : base(httpContextAccessor, sharedLocalizer, unitOfWork, httpClientFactory, cache)
        {
            _localizer = localizer;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return BadRequest("error test ...");
            return Ok();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
