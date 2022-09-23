using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace ABISoft.BlogApp.Entities.Dtos
{
    public class ArticleListDto : DtoGetBase
    {
        public IList<Article> Articles { get; set; }
    }
}
