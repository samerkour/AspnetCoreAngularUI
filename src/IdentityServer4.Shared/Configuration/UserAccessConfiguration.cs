namespace Is4.IdentityServer4.Shared.Configuration
{
    public class UserAccessConfiguration
    {
        public BackOfficeConfiguration BackOffice { get; set; }
        public ControllersConfituration[] Controllers { get; set; }
    }

    public class BackOfficeConfiguration
    {
        public string[] AllTeritories { get; set; }
        public string[] Roles { get; set; }
    }

    public class ControllersConfituration
    {
        public string Name { get; set; }
        public MethodsConfituration[] Methods { get; set; }
    }

    public class MethodsConfituration
    {
        public string Name { get; set; }
        public string[] Roles { get; set; }
    }
}
