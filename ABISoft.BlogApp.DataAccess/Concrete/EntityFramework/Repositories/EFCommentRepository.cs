using ABISoft.BlogApp.DataAccess.Abstract.Repositories;
using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EFCommentRepository : EFEntityRepositoryBase<Comment>, ICommentRepository
    {
        public EFCommentRepository(DbContext context) : base(context)
        {
        }
    }
}
