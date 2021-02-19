
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Jwt
{
   public interface ITokenHelper
    {
         AccessToken CreateToken(Kullanici user, List<OperationClaim>  rol);
    }
}
