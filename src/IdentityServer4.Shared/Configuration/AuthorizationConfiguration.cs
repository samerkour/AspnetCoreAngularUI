namespace Is4.IdentityServer4.Shared.Configuration
{
    public class AuthorizationConfiguration
    {
        public string RolesPrefix { get; set; }
        public string FarmRolesPrefix { get; set; }
        public string MainRolesPrefix { get; set; }
        public string DefaultUserRole { get; set; }
        public string SelectedFarmKey { get; set; }
        public string CustomUserClaims { get; set; }
        public RoleConfiguration[] Roles { get; set; }
    }

    public class RoleConfiguration
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
