using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using WebApiRest.Data;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApiRest.Model.DTOs;
using Microsoft.Extensions.Options;

//[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace WebApiRest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutoMapperConfiguration.Configure();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Configurando el Middleware haciendolo con JWTBearer para proteger el acceso
            var secret = this.Configuration.GetValue<string>("SecretKey");
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            #endregion

            //services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            #region Esto es para la comunicación a la base de datos
            services.AddDbContext<WebApiRestContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("WebApiRestContext")));
            #endregion

            #region Este es el comportamiento que tendrá las validaciones para los modelos
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            #endregion

            #region Para que haga referencia a los controladores
            services.AddControllers();
            #endregion


            services.AddSignalR(options => options.EnableDetailedErrors = true);

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "WebApiRest",
                        Version = "v1",
                        Description = "Aprendiendo el manejo de .Net Core Web Api y Entity Framework",
                        Contact = new OpenApiContact
                        {
                            Name = "Jordan",
                            Email = string.Empty,
                            Url = new Uri("https://facebook.com/jordan")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Use unde MIT",
                            Url = new Uri("https://example.com/license")
                        }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            #region Esto creará un archivo nuevo en esta ruta donde estrán guardadas las veces que fue corrida la aplicación
            loggerFactory.AddFile("C:\\Jordan Vera\\Angular projects\\Sistema\\Logs\\WebApiRest-{Date}.txt");
            #endregion


            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiRest v1");
                    //c.RoutePrefix = string.Empty;
                    c.InjectStylesheet("/swagger-ui/custom.css");
                });
            }

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
