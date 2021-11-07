using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql;
using Templo_de_Momo.Models;

namespace Templo_de_Momo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>//la api web valida con token
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["TokenAuthentication:Issuer"],
                        ValidAudience = Configuration["TokenAuthentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                            Configuration["TokenAuthentication:SecretKey"])),
                    };
                });
            services.AddControllersWithViews();

            services.AddTransient<IRepositorio<Usuario>, RepositorioUsuario>();
            services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
            services.AddTransient<IRepositorio<Biblioteca>, RepositorioBiblioteca>();
            services.AddTransient<IRepositorioBiblioteca, RepositorioBiblioteca>();
            services.AddTransient<IRepositorio<Comentario>, RepositorioComentario>();
            services.AddTransient<IRepositorioComentario, RepositorioComentario>();
            services.AddTransient<IRepositorio<Creador>, RepositorioCreador>();
            services.AddTransient<IRepositorioCreador, RepositorioCreador>();
            services.AddTransient<IRepositorio<Juego>, RepositorioJuego>();
            services.AddTransient<IRepositorioJuego, RepositorioJuego>();
            services.AddTransient<IRepositorio<Noticia>, RepositorioNoticia>();
            services.AddTransient<IRepositorioNoticia, RepositorioNoticia>();
            
            //Entity Framework
            var connection = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<DataContext>(
                options => options.UseMySql(
                    connection, 
                    ServerVersion.AutoDetect(connection)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
