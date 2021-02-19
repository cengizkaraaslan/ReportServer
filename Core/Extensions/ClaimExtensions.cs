
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim("name", name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim("nameid", nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, List<OperationClaim> rol)
        {
            rol.ToList().ForEach(role => claims.Add(new Claim("role", role.RolId.ToString() + "," + role.RolAd)));
        }
    }
}
