using ABISoft.BlogApp.DataAccess.Abstract.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.DataAccess.Abstract.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable 
    {
        IArticleRepository ArticleRepository { get; } //_uow.ArticleRepository.
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        Task<int> SaveChangesAsync(); // Return number of affected records
    }
}
