using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.IdentityServer4AdminEntities
{
    public partial class DeviceCode
    {
        public string UserCode { get; set; }
        public string DeviceCode1 { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Expiration { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }
        public string SessionId { get; set; }
    }
}
