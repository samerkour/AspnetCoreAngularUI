using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Is4.IdentityServer4.Shared.Totp
{
    public interface IOtp
    {
        Task<string> GenerateOtp();
        Task<bool> ValidateOtp(string inputCode);
    }
}
