﻿using Core.CrossCuttingConcerns.Logging.NLog;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
   public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            new LoggerServiceBase("Core").Fatal(e, e.Message, httpContext.Request.Host + httpContext.Request.Path);
            string message = "Internal Server Error";
            if(e.GetType()==typeof(ValidationException))
            {
                message = e.Message;
            }
#if DEBUG
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = e.Message
            }.ToString());
#else
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
#endif
        }
    }
}

