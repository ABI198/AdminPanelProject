using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Shared.Entities.Abstract;
using ABISoft.BlogApp.Shared.Utilities.Results.ComplexTypes;

namespace ABISoft.BlogApp.Entities.Dtos
{
    public class ArticleDto : DtoGetBase
    {
        public Article Article { get; set; }
    }
}
