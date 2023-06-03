using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Is4.IdentityServer4.Shared.Authorization
{
    public class AuthorizedUserData
    {
        private readonly string _selectedFarmKey;
        private HttpContext _httpContext;
        private string _userid;
        private string _username;
        private string _role;
        private string _selectedFarmId;
        private string _locale;
        private string _zoneInfo;

        public AuthorizedUserData(HttpContext httpContext, string SelectedFarmKey)
        {
            _selectedFarmKey = SelectedFarmKey;
            _httpContext = httpContext;
        }

        public string UserId { get { return _userid ??= _httpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value.ToString(); } }
        public string UserName { get { return _username ??= _httpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value.ToString(); } }
        public string Role { get { return _role ??= _httpContext.Items.FirstOrDefault(x => x.Key.ToString() == "selected_role").Value.ToString(); } }  //accepted role from authorization process
        public bool hasAdminAccess { get { return (bool)_httpContext.Items.FirstOrDefault(x => x.Key.ToString() == "hasAdminAccess").Value; } } //is this user have a full access? we consider alwaysAllowedRoles have more access that other roles
        public bool hasAllTeritoriesAccess { get { return (bool)_httpContext.Items.FirstOrDefault(x => x.Key.ToString() == "hasAllTeritoriesAccess").Value; } }
        public string SelectedFarmId { get { return _selectedFarmId ??= _httpContext.Request.Headers.FirstOrDefault(x => x.Key == _selectedFarmKey).Value.ToString(); } }
        public string Locale { get { return _locale ??= _httpContext.User.Claims.FirstOrDefault(x => x.Type == "locale")?.Value.ToString(); } }
        public string ZoneInfo { get { return _zoneInfo ??= _httpContext.User.Claims.FirstOrDefault(x => x.Type == "zoneinfo")?.Value.ToString(); } }
        
    }
}
