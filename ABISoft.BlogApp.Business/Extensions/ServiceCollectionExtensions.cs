using ABISoft.BlogApp.Business.Abstract;
using ABISoft.BlogApp.Business.Concrete;
using ABISoft.BlogApp.DataAccess.Abstract.UnitOfWork;
using ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.Contexts;
using ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.UnitOfWork;
using ABISoft.BlogApp.Entities.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BlogAppContext>(); 
            serviceCollection.AddIdentity<User, Role>(opt => {
                //User Password Options
                opt.Password.RequireDigit = false; 
                opt.Password.RequiredLength = 5;
                opt.Password.RequireNonAlphanumeric = false; 
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                //User Username and Email Options
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; 
                opt.User.RequireUniqueEmail = true; 
            }).AddEntityFrameworkStores<BlogAppContext>(); 

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWorkEF>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            return serviceCollection;
        }
    }
}
