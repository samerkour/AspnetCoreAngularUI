using System;
using System.Collections.Generic;
using System.Text;

namespace Is4.IdentityServer4.Shared.Configuration.Constants
{
    public class AuthorizationConsts
    {
        public const string AdministrationPolicy = "RequireAdministratorRole";
        public const string DeveloperPolicy = "RequireDeveloperRole";
        public const string UserPolicy = "RequireUserRole";
    }
}
