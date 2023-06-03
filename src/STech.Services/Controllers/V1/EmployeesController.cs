using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MassTransit;
using Is4.IdentityServer4.Shared.Configuration.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading;
using STech.CrossCutting;
using STech.Domain.Interfaces;
using STech.Infrastructure.Context;
using STech.Services.Employee.Api.Controllers;
using STech.Services.Employee.Api.Dtos.Employee.Api;
using STech.Services.Employee.Api.Validations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STech.Services.Employee.Controllers.V1
{
    //[Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    //[Authorize(Policy = AuthorizationConsts.UserPolicy)]
    public class EmployeesController : BaseController
    {

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected readonly IStringLocalizer<EmployeesController> _localizer;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly AccountNumberValidation _accountNumberValidation;


        public EmployeesController(
                IHttpContextAccessor httpContextAccessor,
                IStringLocalizer<EmployeesController> localizer,
                IStringLocalizer<SharedResource> sharedLocalizer,
                IUnitOfWork<PortEmployeesDbContext> unitOfWork,
                IHttpClientFactory httpClientFactory,
                IDistributedCache cache,
                IPublishEndpoint publishEndpoint,
                AccountNumberValidation accountNumberValidation

            ) : base(httpContextAccessor, sharedLocalizer, unitOfWork, httpClientFactory, cache)
        {
            _localizer = localizer;
            _publishEndpoint = publishEndpoint;
            _accountNumberValidation = accountNumberValidation;
        }



        /// <summary>
        /// Gets employees as list
        /// </summary>
        /// <returns></returns>
        // GET api/<EmployeesController>/5
        [HttpGet("GetAsListAsync")]
        public async Task<IActionResult> GetAsListAsync()
        {

            var values = await _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>().GetAsync(obj =>
                    new
                    {
                        obj.Id,
                        obj.FirstName,
                        obj.LastName,
                        obj.Age,
                        BirthDate = obj.BirthDate.Date.ToString(),
                        obj.PhoneNumber,
                        obj.Email,
                        obj.AccountNumber
                    },
                    null,
                    order => order.OrderBy(x => x.Id),
                    null
                );



            if (values == null)
                return BadRequest("No employee was found.");

            return Ok(values);
        }


        /// <summary>
        /// Gets employees as paged list
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        // GET api/<EmployeesController>/5
        [HttpGet("GetAsPagedListAsync")]
        public async Task<IActionResult> GetAsPagedListAsync(
                 int? pageSize,
                 int? pageIndex
            )
        {

            int pageScale = (pageSize ?? 5);
            int page = (pageIndex ?? 1);


            var values = await _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>().GetPagedListAsync(obj =>
                    new {
                        obj.Id,
                        obj.FirstName,
                        obj.LastName,
                        obj.Age,
                        obj.BirthDate,
                        obj.PhoneNumber,
                        obj.Email,
                        obj.AccountNumber
                    },
                    null,
                    order => order.OrderBy(x => x.Id),
                    null,
                    page,
                    pageScale
                );


            if (values == null)
                return BadRequest("No employee was found.");

            return Ok(values);
        }


        /// <summary>
        /// Gets an Employee by Id, the id will be extracted from jwt token if is not provided explicitly
        /// </summary>
        /// <param name="id">Employee Id, Optional</param>
        /// <returns></returns>
        [HttpGet("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            //Extract employee user id for jwt (json web token)
            if (this.userId.HasValue)
                id = this.userId.Value;

            var Employee = await _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>().GetFirstOrDefaultAsync(obj =>
                    new
                    {
                        obj.Id,
                        obj.FirstName,
                        obj.LastName,
                        obj.Age,
                        obj.BirthDate,
                        obj.PhoneNumber,
                        obj.Email,
                        obj.AccountNumber


                    },
                    filter => filter.Id == id
                   
                );


            if (Employee == null)
                return BadRequest("Employee is not found.");

            return Ok(Employee);
        }



        /// <summary>
        /// Creates new employee profile
        /// </summary>
        /// <returns></returns>
        // POST api/<EmployeesController>
        [HttpPost("PostAsync")]
        public async Task<IActionResult> PostAsync([FromBody] EmployeeDto EmployeeDto)
        {
                ////Enable this code in order to validate Account number format
                //var isvalid = _accountNumberValidation.IsAccountNumberValid(EmployeeDto.AccountNumber);
                //if (!isvalid)
                //    return BadRequest("Account number is not valid.");

                await _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>().InsertAsync( new STech.Domain.Entities.EmployeeEntities.Employee()
                    { 
                        FirstName=EmployeeDto.FirstName,
                        LastName=EmployeeDto.LastName,
                        Age=EmployeeDto.Age,
                        BirthDate=EmployeeDto.BirthDate,
                        PhoneNumber=EmployeeDto.PhoneNumber,
                        Email=EmployeeDto.Email,
                        AccountNumber=EmployeeDto.AccountNumber
                    }
                 );

               

                var saved = await _unitOfWork.SaveChangesAsync();



                if (saved <= 0)
                    return BadRequest("Unable to add employee.");


         

            return Ok();
        }



        /// <summary>
        /// Edits employee profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="EmployeeDto"></param>
        /// <returns></returns>
        // PUT api/<EmployeeController>/5
        [HttpPut("PutAsync/{id}")]
        public async Task<IActionResult> PutAsync(long id, [FromBody] EmployeeDto EmployeeDto)
        {

            ////Enable this code in order to validate Account number format
            //var isvalid = _accountNumberValidation.IsAccountNumberValid(EmployeeDto.AccountNumber);
            //if (!isvalid)
            //    return BadRequest("Account number is not valid.");


            _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>()
                .UpdateFields(
                 new STech.Domain.Entities.EmployeeEntities.Employee()
                 {
                     Id = id,
                     FirstName=EmployeeDto.FirstName,
                     LastName=EmployeeDto.LastName,
                     Age = EmployeeDto.Age,
                     BirthDate = EmployeeDto.BirthDate,
                     PhoneNumber=EmployeeDto.PhoneNumber,
                     Email=EmployeeDto.Email,
                     AccountNumber=EmployeeDto.AccountNumber,
                   
                 },
                 o => o.FirstName,
                 o => o.LastName,
                 o => o.BirthDate,
                 o => o.PhoneNumber,
                 o => o.Email,
                 o => o.AccountNumber
              );

            var saved = await _unitOfWork.SaveChangesAsync();


            if (saved <= 0)
                return BadRequest("Unable to update employee.");


            return Ok();
        }



      
        /// <summary>
        /// Deletes employee profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<EmployeeController>/5
        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {

            var emp = await _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>().GetFirstOrDefaultAsync(obj => obj.Id == id);

            if (emp == null)
                return NotFound("Employee is not found.");

            _unitOfWork.GetRepository<STech.Domain.Entities.EmployeeEntities.Employee>().Delete(id);


            var isSaved = await _unitOfWork.SaveChangesAsync();

            if (isSaved <= 0)
                return BadRequest("Unable to delete employee.");


            return Ok();
        }





    }
}
