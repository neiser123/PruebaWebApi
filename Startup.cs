using AutoMapper;
using logica.DTOs;
using logica.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace PruebaWebApi
{

    public class Startup
    {
        internal static MapperConfiguration MapperConfiguration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
              services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Prueba Empresa API", Version = "v1" });
                  // Set the comments path for the Swagger JSON and UI.
                  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                  c.IncludeXmlComments(xmlPath);


                  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                  {
                      Name = "Authorization",
                      Type = SecuritySchemeType.ApiKey,
                      Scheme = "Bearer",
                      BearerFormat = "JWT",
                      In = ParameterLocation.Header,
                      Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                  });
                  c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                         {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                       }
                 });

              });
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));
              services.AddAuthentication(x => {
                  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                  x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(x => {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;

                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false,
                      ValidateAudience = false,

                  };
              });

            /* var connection = @"Data source=DESKTOP-68CSEVF; Initial Catalog=PruebasSatelites; user id=sa; password=1234;";
             services.AddDbContext<PruebasSatelitesContext>
             (options => options.UseSqlServer(connection));*/
            services.AddDbContext<PruebasSatelitesContext>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MapperConfiguration = MapperConfig.MapperConfiguration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
         /*   app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();*/
            //        //automapper



            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prueba Empresa API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    //public  partial class Startup
    //{
    //    internal static MapperConfiguration MapperConfiguration { get; set; }
    //    public void ConfigureAuth(IApplicationBuilder app)
    //    {

    //        //automapper
    //        MapperConfiguration = MapperConfig.MapperConfiguration();
    //    }
    //}
}
