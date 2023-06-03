namespace Is4.IdentityServer4.Shared.Configuration.Identity
{
    public class LoginConfiguration
    {
        public LoginResolutionPolicy ResolutionPolicy { get; set; } = LoginResolutionPolicy.Username;
    }
}
