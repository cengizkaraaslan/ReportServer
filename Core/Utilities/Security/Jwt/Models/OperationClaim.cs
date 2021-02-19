using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Jwt.Models
{
    public class OperationClaim
    {
        public Guid RolId { get; set; }
        public string RolAd { get; set; }
    }
}
