using ABISoft.BlogApp.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.UI.Web.Areas.Admin.Models
{
    public class CategoryUpdateAjaxViewModel
    {
        public string CategoryUpdatePartial { get; set; } 
        public CategoryDto CategoryDto { get; set; } 
    }
}
