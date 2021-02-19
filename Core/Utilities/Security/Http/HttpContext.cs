using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;

namespace System.Web //Core.Utilities.Security.Http
{
    public class HttpContext
    {
        private  IHttpContextAccessor _httpContextAccessor;
        public  HttpContext()
        {
            _httpContextAccessor = (IHttpContextAccessor)ServiceTool.ServiceProvider.GetService(typeof(IHttpContextAccessor));
        }
     
        
        public  Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                return _httpContextAccessor.HttpContext;

            }
        }
        public  string GetUserId()
        {
            if (Current != null && Current.User != null && Current.User.Identity.IsAuthenticated)
            {
                return Current.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Guid.Empty.ToString();
        }
        public string GetUserName()
        {
            if (Current != null && Current.User != null && Current.User.Identity.IsAuthenticated)
            {
                return Current.User.FindFirst("username").Value;
            }
            return "";
        }
    }
}