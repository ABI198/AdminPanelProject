using ABISoft.BlogApp.DataAccess.Abstract.Repositories;
using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EFArticleRepository : EFEntityRepositoryBase<Article>, IArticleRepository
    {
        public EFArticleRepository(DbContext context) : base(context)
        {
        }
    }
}
