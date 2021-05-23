using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using At.Wx.Api.ExceptionHandling;
using Microsoft.AspNetCore.Builder;

namespace At.Wx.Api.Extensions
{
    public static class ExtensionsClass
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
