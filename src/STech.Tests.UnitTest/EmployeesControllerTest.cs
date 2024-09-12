using STech.Presentation.Controllers.V1;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Moq;
using STech.CrossCutting;
using STech.Domain.Interfaces;
using STech.Infrastructure.Context;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using STech.Infrastructure;
using STech.Presentation.Api.Dtos.Employee.Api;
using System;
using STech.Presentation.Api.Validations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using STech.Presentation.Controllers.V1;

namespace STech.Tests.UnitTest
{
    public class EmployeesControllerTest 
    {
        protected readonly IStringLocalizer<EmployeesController> _localizer;
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor; 
        private readonly Mock<IStringLocalizer<EmployeesController>> _mockLocalizer;
        private readonly Mock<IStringLocalizer<SharedResource>> _mockSharedLocalizer;
        private readonly Mock<IUnitOfWork<PortEmployeesDbContext>> _mockUnitOfWork;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IDistributedCache> _mockCashe;
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;


        private readonly EmployeesController _controller;

        public EmployeesControllerTest()
        {
            _validation = new AccountNumberValidation();
           
            var options = new DbContextOptionsBuilder<PortEmployeesDbContext>()
                .UseSqlServer("Data Source=DESKTOP-53A5H27\\LOCALSERVER;Initial Catalog=PortEmployeesDb;user id=sa;password=Admin@123;TrustServerCertificate=True").Options;

            var uow = new UnitOfWork<PortEmployeesDbContext>(new PortEmployeesDbContext(options));

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockLocalizer = new Mock<IStringLocalizer<EmployeesController>>();
            _mockSharedLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            _mockUnitOfWork = new Mock<IUnitOfWork<PortEmployeesDbContext>>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockCashe = new Mock<IDistributedCache>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();

            _controller = new EmployeesController(
                _mockHttpContextAccessor?.Object, 
                _mockLocalizer?.Object,
                _mockSharedLocalizer?.Object,
                 uow,
                _mockHttpClientFactory?.Object,
                _mockCashe?.Object,
                _mockPublishEndpoint?.Object,
                _validation);
        }


        private readonly AccountNumberValidation _validation;
       



        [Fact]
        public void IsValid_ValidAccountNumber_ReturnsTrue()
            => Assert.True(_validation.IsAccountNumberValid("123-4543234576-23"));

        //Theory and InlineData
        [Theory]
        [InlineData("1234-3454565676-23")]
        [InlineData("12-3454565676-23")]
        public void IsValid_AccountNumberFirstPartWrong_ReturnsFalse(string accountNumber)
            => Assert.False(_validation.IsAccountNumberValid(accountNumber));


        [Theory]
        [InlineData("123-345456567-23")]
        [InlineData("123-345456567633-23")]
        public void IsValid_AccountNumberMiddlePartWrong_ReturnsFalse(string accNumber)
             => Assert.False(_validation.IsAccountNumberValid(accNumber));

        [Theory]
        [InlineData("123-3434545656-2")]
        [InlineData("123-3454565676-233")]
        public void IsValid_AccountNumberLastPartWrong_ReturnsFalse(string accNumber)
            => Assert.False(_validation.IsAccountNumberValid(accNumber));


        //Testing Exceptions with xUnit
        [Theory]
        [InlineData("123-345456567633=23")]
        [InlineData("123+345456567633-23")]
        [InlineData("123+345456567633=23")]
        public void IsValid_InvalidDelimiters_ThrowsArgumentException(string accNumber)
            => Assert.Throws<ArgumentException>(() => _validation.IsAccountNumberValid(accNumber));



        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {

            // Act
            var actionResult = await _controller.GetAsListAsync() as OkObjectResult;

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public async void Get_WhenCalled_ReturnsAllItems()
        {
            //var repositoryMock = new Mock<IRepository<Domain.Entities.EmployeeEntities.Employee>>();
            //repositoryMock.Setup(m => m.Get(null, null, null, true))
            //    .Returns(new List<Domain.Entities.EmployeeEntities.Employee>() {
            //        new Domain.Entities.EmployeeEntities.Employee(),
            //        new Domain.Entities.EmployeeEntities.Employee(),
            //        new Domain.Entities.EmployeeEntities.Employee()
            //    })
            //    .Verifiable();


            //var cxt = _mockUnitOfWork.Setup(m => m.DbContext)
            //    .Returns(new PortEmployeesDbContext());

            //_mockUnitOfWork.Setup(m => m.GetRepository<Domain.Entities.EmployeeEntities.Employee>(false))
            //    .Returns(repositoryMock.Object);

            // Act
            var actionResult = await _controller.GetAsListAsync() as OkObjectResult;

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            var value = okResult.Value as IEnumerable<object>;
            Assert.NotNull(value);

            Assert.NotEqual(0, value.ToList().Count);
        }


        [Fact]
        public async void GetById_UnknownIdPassed_ReturnsBadRequestResult()
        {
            // Act
            var badRequestResult = await _controller.GetByIdAsync(0);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        //[Fact]
        //public void GetById_ExistingIdPassed_ReturnsOkResult() 
        //{
        //    // Arrange
        //    var testGuid = 1;
        //    // Act
        //    var okResult = _controller.GetByIdAsync(testGuid);
        //    // Assert
        //    Assert.IsType<Task<IActionResult>>(okResult as Task<IActionResult>);
        //}



        //[Fact]
        //public async void Add_InvalidObjectPassed_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var emailMissingItem = new EmployeeDto()
        //    {
        //        FirstName = "Test",
        //        LastName = "Test",
        //        Age = 0,
        //        BirthDate = DateTimeOffset.UtcNow,
        //        PhoneNumber = "000000000",
        //        //Email="sa@gmail.com",
        //        AccountNumber = "123-23444444-3444"

        //    };

        //    // Act
        //    var badResponse = await _controller.PostAsync(emailMissingItem);
        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(badResponse);
        //}


        [Fact]
        public async void Add_ValidObjectPassed_ReturnsOkResultResponse() 
        {
           
            // Arrange
            var testItem = new EmployeeDto()
            {
                FirstName = "Test",
                LastName = "Test",
                Age = 0,
                BirthDate = DateTime.Now,
                PhoneNumber = "000000000",
                Email = "sa@gmail.com",
                AccountNumber = "123-4543234576-23"

            };

            // Act
            var okResponse = await _controller.PostAsync(testItem);
            // Assert
            Assert.IsType<OkResult>(okResponse);
        }





        [Fact]
        public async void Remove_NotExistingIdPassed_ReturnsNotFoundResultResponse()  
        {
            // Arrange
            var id = 0; 

            // Act
            var badResponse = await _controller.DeleteAsync(id);
            // Assert
            Assert.IsType<NotFoundObjectResult>(badResponse);
        }

        //[Theory]
        //[InlineData("AL35202111090000000001234567")]
        //[InlineData("BL35202111090000000001234568")]
        //public void Create_ActionExecutes_ReturnsViewForCreate(string bankaAccountNumber)
        //{
        //    var result = _controller.Post(new CustomerViewModel() { 
        //        Id= Guid.NewGuid(), 
        //        FirstName= "Samer", 
        //        LastName="Kour", 
        //        BirthDate= DateTime.UtcNow.AddYears(-30), 
        //        PhoneNumber="+989100000000",
        //        Email="koursamer@gmail.com",
        //        BankAccountNumber= bankaAccountNumber
        //    });

        //    Assert.IsType<IActionResult>(result);
        //}




        //[Fact]
        //public void Create_InvalidModelState_ReturnsView()
        //{
        //    _controller.ModelState.AddModelError("Name", "Name is required");

        //    var customer = new CustomerViewModel() { PhoneNumber="", Email="", BankAccountNumber = "AL35202111090000000001234567" };

        //    var result = _controller.Post(customer);

        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var testEmployee = Assert.IsType<CustomerViewModel>(viewResult.Model);

        //    Assert.Equal(customer.BankAccountNumber, testEmployee.BankAccountNumber);
        //    Assert.Equal(customer.PhoneNumber, testEmployee.PhoneNumber);
        //    Assert.Equal(customer.Email, testEmployee.Email); 
        //}






    }
}