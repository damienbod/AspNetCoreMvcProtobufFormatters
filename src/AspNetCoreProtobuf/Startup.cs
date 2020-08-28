using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using AspNetCoreProtobuf.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreProtobuf
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44318";
                    options.ApiName = "ProtoApiClient";
                    options.ApiSecret = "apiprotoSecret";
                });

            services.AddAuthorization(options =>
                options.AddPolicy("RequiredScope", policy =>
                {
                    policy.RequireClaim("scope", "apiproto");
                })
            );

            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true;
            //});

            services.AddControllers(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat("protobuf", MediaTypeHeaderValue.Parse("application/x-protobuf"));
            }).AddProtobufFormatters();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
