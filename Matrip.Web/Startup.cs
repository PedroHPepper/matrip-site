using Matrip.Domain.Libraries.Email;
using Matrip.Web.Libraries.Login;
using Matrip.Web.Libraries.Management.Payment.PagarMe;
using Matrip.Web.Libraries.Middleware;
using Matrip.Web.Libraries.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace Matrip.Web
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
            services.AddHttpContextAccessor();

            #region Smtp
            services.AddScoped<SmtpClient>(options =>
            {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = Configuration.GetValue<string>("Email:ServerSMTP"),
                    Port = Configuration.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Configuration.GetValue<string>("Email:Username"), Configuration.GetValue<string>("Email:Password")),
                    EnableSsl = true
                };

                return smtp;
            });
            #endregion

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Configuração da sessão
            services.AddMemoryCache();
            services.AddSession(options =>
            {

            });

            services.AddHttpContextAccessor();
            services.AddScoped<Session>();
            services.AddScoped<Libraries.Cookie.Cookie>();
            services.AddScoped<Libraries.ChoosedTripCookie.ChoosedTripCookie>();
            services.AddScoped<Libraries.ChoosedSubTripSaleDateCookie.ChoosedSubTripSaleDateCookie>();
            services.AddScoped<Libraries.ShoppingCartCookie.ShoppingCartCookie>();
            services.AddScoped<EmailManagement>();
            services.AddScoped<UserLogin>();
            services.AddScoped<ManagePagarMe>();


            //Configuração do MVC
            services.AddMvc(
                options => { options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O campo deve ser preenchido!"); }
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddSessionStateTempDataProvider();

            services.AddSession(options => { options.Cookie.IsEssential = true; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
