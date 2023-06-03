using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.IdentityServer4AdminEntities
{
    public partial class ClientIdPrestriction
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
