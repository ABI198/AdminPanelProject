using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Entities.Dtos;
using ABISoft.BlogApp.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Business.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync(); //Allows us to filter articles that are in draft mode
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId); 
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> SoftDeleteAsync(int articleId, string modifiedByName); //IsDeleted => True 
        Task<IResult> HardDeleteAsync(int articleId); 
    }
}
