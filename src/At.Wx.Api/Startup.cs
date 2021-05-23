using At.Wx.Api.Infrastructure;
using At.Wx.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using At.Wx.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace At.Wx.Api
{
    public class Startup
    {
        private const string AllowAllOriginsPolicyName = "AllowAllOriginsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var apiConfig = new ApiConfig();
            Configuration.Bind("ApiConfig", apiConfig);
            services.AddSingleton(apiConfig);
            services.AddTransient<ProductServices>();

            services.AddHttpClient<ProductClient>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["WoolliesTestBaseUrl"]);
                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                })
                .AddHttpMessageHandler(provider => new TokenEnricher(apiConfig));

            //services.AddControllers();
            services.AddControllers(options =>
                {
                    options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                    options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllOriginsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowAllOriginsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
