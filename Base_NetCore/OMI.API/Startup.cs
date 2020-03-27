using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OMI.BUS;
using OMI.Core.Datalayer;
using OMI.Core.ExceptionHandler;
using OMI.Core.Helper;
using OMI.Core.Services;

namespace OMI.API
{
  public class Startup
  {
    private IServiceCollection _services;
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<SetupOptions>(Configuration.GetSection("Setup"));
      services.AddScoped<ICurrentUserService, CurrentUserService>();
      services.AddTransient<IDateTime, MachineDateTime>();
            
      services.AddBussiness();
      services.AddScoped<IOmiDbContext, OmiDbContext>();

      services.AddHttpContextAccessor();

      services.AddControllers()
              .AddNewtonsoftJson();

      // Register the Swagger generator, defining 1 or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "OMI API", Version = "v1" });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });

      _services = services;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseHttpStatusCodeExceptionMiddleware();
        RegisteredServicesPage(app);
      }
      else
      {
        // TODO: Rewrite Exception page
        app.UseHttpStatusCodeExceptionMiddleware();
        app.UseExceptionHandler(errorApp =>
        {
          errorApp.Run(async context =>
          {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
            await context.Response.WriteAsync("ERROR!<br><br>\r\n");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
              await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
            }

            await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");
            await context.Response.WriteAsync(new string(' ', 512)); // IE padding
          });
        });
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OMI API V1");
        c.RoutePrefix = string.Empty;
      });

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");
        endpoints.MapControllers();
      });
    }

    private void RegisteredServicesPage(IApplicationBuilder app)
    {
      app.Map("/services", builder => builder.Run(async context =>
      {
        var sb = new StringBuilder();
        sb.Append("<h1>Registered Services</h1>");
        sb.Append("<table><thead>");
        sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
        sb.Append("</thead><tbody>");
        foreach (var svc in _services)
        {
          sb.Append("<tr>");
          sb.Append($"<td>{svc.ServiceType.FullName}</td>");
          sb.Append($"<td>{svc.Lifetime}</td>");
          sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
          sb.Append("</tr>");
        }
        sb.Append("</tbody></table>");
        await context.Response.WriteAsync(sb.ToString());
      }));
    }
  }
}
