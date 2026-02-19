using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Services.ResultModel;
using Gotcha.Core.Entities;

namespace Gotcha.Core.Interfaces
{
    public interface IRepositoryService<T>
    {
        public Task<ResultModel<List<T>>> GetAllAsync();
        public Task<ResultModel<T>> GetByIdAsync(Guid id);
        public Task<ResultModel<T>> AddAsync(T entity);
        public Task<ResultModel<T>> UpdateAsync(T entity);
        public Task<ResultModel<T>> DeleteAsync(Guid id);
    }
}
