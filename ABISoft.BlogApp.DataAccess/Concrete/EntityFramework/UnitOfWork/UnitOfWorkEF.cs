using ABISoft.BlogApp.DataAccess.Abstract.Repositories;
using ABISoft.BlogApp.DataAccess.Abstract.UnitOfWork;
using ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.Contexts;
using ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.Repositories;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.DataAccess.Concrete.EntityFramework.UnitOfWork
{
    public class UnitOfWorkEF : IUnitOfWork
    {
        private readonly BlogAppContext _blogAppContext;
        private EFArticleRepository articleRepository;
        private EFCategoryRepository categoryRepository;
        private EFCommentRepository commentRepository;

        public UnitOfWorkEF(BlogAppContext blogAppContext)
        {
            _blogAppContext = blogAppContext;
        }

        public IArticleRepository ArticleRepository => articleRepository ?? new EFArticleRepository(_blogAppContext);
        public ICategoryRepository CategoryRepository => categoryRepository ?? new EFCategoryRepository(_blogAppContext);
        public ICommentRepository CommentRepository => commentRepository ?? new EFCommentRepository(_blogAppContext);

        public async Task<int> SaveChangesAsync()
        {
            return await _blogAppContext.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await _blogAppContext.DisposeAsync();
        }
    }
}
