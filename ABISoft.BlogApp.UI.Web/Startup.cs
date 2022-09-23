using ABISoft.BlogApp.Business.Extensions;
using ABISoft.BlogApp.Business.Mappings.AutoMapper.Profiles;
using ABISoft.BlogApp.UI.Web.Mappings.AutoMapper.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.UI.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt => 
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            }); //MVC + Razor Compilation + JSON Converter With Enum + Include
            services.AddSession();
            //Business Layer'a erişmesinde sorun yok!! UI -> BLL , Bu kod sayesinde profile'lar otomatikman IMapper'a ekleniyor!!
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile), typeof(UserProfile)); 
            services.LoadServices();
            //ConfigureApplicationCookie, AddIdentity( )'den sonra ve Startup içinde eklenmeli. Bu sebeple LoadServices( )'den sonra ekleyerek bunu garanti ediyoruz!!
            services.ConfigureApplicationCookie(opt => 
            {
                opt.LoginPath = new PathString("/Admin/User/Login"); //User girişi yapmadan admin area'ya girmeye kalkarsak sistem bu action'a yönlendirmeli!!
                opt.LogoutPath = new PathString("/Admin/User/Logout");
                opt.AccessDeniedPath = new PathString("/Admin/User/AccessDenied"); // (403 Access Denied)
                
                opt.Cookie = new CookieBuilder
                {
                    Name = "BlogApp",
                    HttpOnly = true, //JS ile cookie elde edilemez
                    SameSite = SameSiteMode.Strict, 
                    /*
                     * URL HTTPS ise Cookie'ler HTTPS Requestler gelirse gönderilecek
                     * URL HTTP ise Cookie'ler hem HTTP hem HTTPS Request'ler gerlise gönderilebilecek
                     * Gerçek hayatta bunu Always olarak kullanmak lazım!!
                     */
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,      
                };
                opt.SlidingExpiration = true; //Kullanıcı üçüncü günden sonra tekrar sisteme giriş yaparsa expiration time 7 gün daha artar
                opt.ExpireTimeSpan = System.TimeSpan.FromDays(7);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); //Bu da bir middleware!!! => 'Status Code' hatasını daha sade ve beyaz bir görüntü olarak gösterir! 
            }
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=index}/{id?}"
                );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
