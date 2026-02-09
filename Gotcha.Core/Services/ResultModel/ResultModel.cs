using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Services.ResultModel.Base;

namespace Gotcha.Core.Services.ResultModel
{
    public class ResultModel<T> : BaseResult
    {
        public T? Data { get; set; }
    }
}
