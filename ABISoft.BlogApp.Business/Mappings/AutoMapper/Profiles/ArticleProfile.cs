using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Entities.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Business.Mappings.AutoMapper.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleAddDto, Article>().ForMember(entity => entity.CreatedDate, dto => dto.MapFrom(x => DateTime.Now));
            CreateMap<ArticleUpdateDto, Article>().ForMember(entity => entity.ModifiedDate, dto => dto.MapFrom(x => DateTime.Now)); 

        }
    }
}
