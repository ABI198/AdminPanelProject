using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Shared.Utilities.Results.Abstract
{
    public interface IDataResult<out T> : IResult
    {
        //new DataResult<Category>(ResultStatus.Success, category);
        //new DataResult<IList<Category>>(ResultStatus.Success, categoryList);
        public T Data { get; } 
    }
}
