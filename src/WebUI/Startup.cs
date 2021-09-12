using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddInfrastructure(Configuration);


            services.AddHttpContextAccessor();

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();


            services.AddControllersWithViews();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                });
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
           
                app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            var HostHTTPOptionsSection = Configuration.GetSection("HostHTTPOptions");
            var UseForwardedHeaders = bool.Parse(HostHTTPOptionsSection?.GetSection("UseForwardedHeaders")?.Value ?? "false");
            var UseHsts = bool.Parse(HostHTTPOptionsSection?.GetSection("UseHsts")?.Value ?? "false");
            var UseHttpsRedirection = bool.Parse(HostHTTPOptionsSection?.GetSection("UseHttpsRedirection")?.Value ?? "false");
            var AddXForwardedForAndProto = bool.Parse(HostHTTPOptionsSection?.GetSection("AddXForwardedForAndProto")?.Value ?? "false");


            if (UseForwardedHeaders)
            {
                if (AddXForwardedForAndProto)
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
                }
                else
                {
                    app.UseForwardedHeaders();
                }
            }

            if (UseHsts)
            {
                app.UseHsts();
            }

            if (UseHttpsRedirection)
            {
                app.UseHttpsRedirection();
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
