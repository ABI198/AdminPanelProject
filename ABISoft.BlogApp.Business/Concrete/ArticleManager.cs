using ABISoft.BlogApp.Business.Abstract;
using ABISoft.BlogApp.DataAccess.Abstract.UnitOfWork;
using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Entities.Dtos;
using ABISoft.BlogApp.Shared.Utilities.Results.Abstract;
using ABISoft.BlogApp.Shared.Utilities.Results.ComplexTypes;
using ABISoft.BlogApp.Shared.Utilities.Results.Concrete;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Business.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ArticleManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1; 
            await _uow.ArticleRepository.AddAsync(article);
            await _uow.SaveChangesAsync();
            return new Result(ResultStatus.Success, $"{article.Title} makalesi {createdByName} tarafından başarılı bir şekilde eklendi.");
        }
        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await _uow.ArticleRepository.GetAllAsync(null, x => x.Comments, x => x.Category, x => x.User);
            if(articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Makaleler bulunamadı.", null);
        }
        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var categoryIdIsValid = await _uow.CategoryRepository.AnyAsync(x => x.Id == categoryId); //Check whether categoryId is valid or not
            if (categoryIdIsValid)
            {
                var selectedArticles = await _uow.ArticleRepository.
               GetAllAsync(x => x.CategoryId == categoryId && !x.IsDeleted && x.IsActive, x => x.Comments, x => x.Category, x => x.User);
                if (selectedArticles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
                    {
                        Articles = selectedArticles,
                        ResultStatus = ResultStatus.Success
                    });
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, "Makaleler bulunamadı.", null);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı.", null);
        }
        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var selectedArticles = await _uow.ArticleRepository.
                GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.Comments, x => x.Category, x => x.User);
            if (selectedArticles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
                {
                    Articles = selectedArticles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Makaleler bulunamadı.", null);
        }
        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync()
        {
            var selectedArticles = await _uow.ArticleRepository.
              GetAllAsync(x => !x.IsDeleted, x => x.Comments, x => x.Category, x => x.User);
            if(selectedArticles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto() { 
                    Articles = selectedArticles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Makaleler bulunamadı.", null);
        }
        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var article = await _uow.ArticleRepository.
                GetAsync(x => x.Id == articleId, x => x.Comments, x => x.Category, x => x.User);
            if(article != null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto()
                {
                    Article = article,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, "Böyle bir makale bulunamadı", null);
        }
        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var articleIdIsValid = await _uow.ArticleRepository.AnyAsync(x => x.Id == articleId);
            if (articleIdIsValid)
            {
                var hardDeletedArticle = await _uow.ArticleRepository.GetAsync(x => x.Id == articleId);
                await _uow.ArticleRepository.DeleteAsync(hardDeletedArticle);
                await _uow.SaveChangesAsync();
                return new Result(ResultStatus.Success, $"{hardDeletedArticle.Title} makalesi başarıyla veritabanından silindi.");
            }
            return new Result(ResultStatus.Error, $"{articleId} ID değerine sahip bir makale bulunamadı.");
        }
        public async Task<IResult> SoftDeleteAsync(int articleId, string modifiedByName)
        {
            var articleIdIsValid = await _uow.ArticleRepository.AnyAsync(x => x.Id == articleId);
            if (articleIdIsValid)
            {
                var deletedArticle = await _uow.ArticleRepository.GetAsync(x => x.Id == articleId);
                deletedArticle.IsDeleted = true;
                deletedArticle.ModifiedByName = modifiedByName;
                deletedArticle.ModifiedDate = DateTime.Now;
                await _uow.ArticleRepository.UpdateAsync(deletedArticle);
                await _uow.SaveChangesAsync();
                return new Result(ResultStatus.Success, $"{deletedArticle.Title} makalesi {modifiedByName} tarafından başarıyla silinmiştir(Soft).");
            }
            return new Result(ResultStatus.Error, $"{articleId} ID degerine sahip bir makale bulunamamıştır.");
        }
        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var updatedArticle = _mapper.Map<Article>(articleUpdateDto);
            updatedArticle.ModifiedByName = modifiedByName;
            await _uow.ArticleRepository.UpdateAsync(updatedArticle);
            await _uow.SaveChangesAsync();
            return new Result(ResultStatus.Success, $"{updatedArticle.Title} makalesi {modifiedByName} tarafından başarılı bir şekilde güncellendi.");
        }
    }
}
