using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Entities.Dtos
{
    public class UserListDto : DtoGetBase
    {
        public IList<User> Users { get; set; }
    }
}
