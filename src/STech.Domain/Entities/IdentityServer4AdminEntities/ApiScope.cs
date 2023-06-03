using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.IdentityServer4AdminEntities
{
    public partial class ApiScope
    {
        public ApiScope()
        {
            ApiScopeClaims = new HashSet<ApiScopeClaim>();
            ApiScopeProperties = new HashSet<ApiScopeProperty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<ApiScopeClaim> ApiScopeClaims { get; set; }
        public virtual ICollection<ApiScopeProperty> ApiScopeProperties { get; set; }
    }
}
