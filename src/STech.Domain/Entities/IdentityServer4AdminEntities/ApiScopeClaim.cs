using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.IdentityServer4AdminEntities
{
    public partial class ApiScopeClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ScopeId { get; set; }

        public virtual ApiScope Scope { get; set; }
    }
}
