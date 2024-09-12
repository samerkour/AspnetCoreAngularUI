using STech.CrossCutting;
using STech.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Globalization;
using System.Net.Http;
using STech.Infrastructure.Context;
using Microsoft.Extensions.Caching.Distributed;

namespace STech.Presentation.Api.Controllers
{
    public abstract class BaseController: ControllerBase
    {
        protected long? userId;
        protected CultureInfo culture;
        protected readonly IHttpContextAccessor _httpContextAccessor; 
        protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        protected readonly IUnitOfWork<PortEmployeesDbContext> _unitOfWork;
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IDistributedCache _cache;

        public BaseController(
                IHttpContextAccessor httpContextAccessor,
                IStringLocalizer<SharedResource> sharedLocalizer,
                IUnitOfWork<PortEmployeesDbContext> unitOfWork,
                IHttpClientFactory httpClientFactory,
                IDistributedCache cache
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _sharedLocalizer = sharedLocalizer;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _cache = cache;

            var user = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            userId = user== null ? null : long.Parse(user.Value);
            culture = httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
        }
    }
}
