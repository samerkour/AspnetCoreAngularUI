using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.IdentityServer4AdminEntities
{
    public partial class ApiResourceScope
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
