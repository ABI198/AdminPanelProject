using ABISoft.BlogApp.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.UI.Web.Areas.Admin.Models
{
    public class CategoryAddAjaxViewModel
    {
        //public CategoryAddDto CategoryAddDto { get; set; } 
        public string CategoryAddPartial { get; set; } 
        public CategoryDto CategoryDto { get; set; } 
    }
}
