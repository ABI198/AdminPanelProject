using ABISoft.BlogApp.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.UI.Web.Areas.Admin.Models
{
    public class UserAddAjaxViewModel
    {
        public UserAddDto UserAddDto { get; set; } 
        public string UserAddPartial { get; set; } 
        public UserDto UserDto { get; set; } 
    }
}
