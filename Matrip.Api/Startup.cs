using Matrip.Domain.Libraries.Email;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Domain.Models;
using Matrip.Web.Repositories;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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
            #region Banco de Dados
            //Conexão com banco de dados
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region Identity
            //Usando o usuário como identity user
            services.AddIdentity<ma01user, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Tirando a obrigação do password ter caracteres obrigatórios
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.AllowedUserNameCharacters = null;
            });
            #endregion

            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Security-Api-Key-Matrip-JWT"))
                };
            });
            #endregion

            #region Authorization
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                            .RequireAuthenticatedUser()
                                            .Build()
                    );
            });
            #endregion

            #region ConfigureCookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            #endregion

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

            #region Injeção de Dependência
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<Ima01UserRepository, ma01UserRepository>();
            services.AddScoped<Ima04GuideRepository, ma04GuideRepository>();
            services.AddScoped<Ima05TripRepository, ma05TripRepository>();
            services.AddScoped<Ima06TripCategoryRepository, ma06TripCategoryRepository>();
            services.AddScoped<Ima08UFRepository, ma08UFRepository>();
            services.AddScoped<Ima09CityRepository, ma09CityRepository>();
            services.AddScoped<Ima11ServiceRepository, ma11ServiceRepository>();
            services.AddScoped<Ima12SubtripGuideRepository, ma12SubtripGuideRepository>();
            services.AddScoped<Ima13TripPhotoRepository, ma13TripPhotoRepository>();
            services.AddScoped<Ima14SubTripRepository, ma14SubTripRepository>();
            services.AddScoped<Ima15SubTripPhotoRepository, ma15SubTripPhotoRepository>();
            services.AddScoped<Ima16SubTripScheduleRepository, ma16SubTripScheduleRepository>();
            services.AddScoped<Ima17SubTripValueRepository, ma17SubTripValueRepository>();
            services.AddScoped<Ima18TripItemShoppingCartRepository, ma18TripItemShoppingCartRepository>();
            services.AddScoped<Ima19SubTripItemShoppingCartRepository, ma19SubTripItemShoppingCartRepository>();
            services.AddScoped<Ima20ServiceItemShoppingCartRepository, ma20ServiceItemShoppingCartRepository>();
            services.AddScoped<Ima21SaleTripRepository, ma21SaleTripRepository>();
            services.AddScoped<Ima22SubTripSaleRepository, ma22SubTripSaleRepository>();
            services.AddScoped<Ima23ServiceSaleRepository, ma23ServiceSaleRepository>();
            services.AddScoped<Ima24PaymentRepository, ma24PaymentRepository>();
            services.AddScoped<Ima25PartnerRepository, ma25PartnerRepository>();
            services.AddScoped<Ima26PartnerGuideRepository, ma26PartnerGuideRepository>();
            services.AddScoped<Ima27AgeDiscountRepository, ma27AgeDiscountRepository>();
            services.AddScoped<Ima28SaleTouristRepository, ma28SaleTouristRepository>();
            services.AddScoped<Ima29TouristShoppingCartRepository, ma29TouristShoppingCartRepository>();
            services.AddScoped<Ima30GuideSubtripShoppingCartRepository, ma30GuideSubtripShoppingCartRepository>();
            services.AddScoped<Ima31SubtripSaleGuideRepository, ma31SubtripSaleGuideRepository>();
            services.AddScoped<Ima32saleRepository, ma32saleRepository>();
            services.AddScoped<Ima33UserAddressRepository, ma33UserAddressRepository>();
            services.AddScoped<Ima34TransferencePendenciesRepository, ma34TransferencePendenciesRepository>();
            services.AddScoped<Ima35cityphotoRepository, ma35cityphotoRepository>();
            services.AddScoped<Ima36SubtripGroupRepository, ma36SubtripGroupRepository>();
            services.AddScoped<Ima39tripEvaluationRepository, ma39tripEvaluationRepository>();
            services.AddScoped<EmailManagement>();
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).
                    AddJsonOptions(options => (options.SerializerSettings.ContractResolver as DefaultContractResolver).IgnoreSerializableAttribute = true).
                    AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                //Request
                await next.Invoke();
                //Response
                var unitOfWork = (IUnitOfWork)context.RequestServices.GetService(typeof(IUnitOfWork));
                await unitOfWork.Commit();
                unitOfWork.Dispose();
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseAuthentication();

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
