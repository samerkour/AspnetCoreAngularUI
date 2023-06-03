using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.IdentityServer4AdminEntities
{
    public partial class ApiResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
