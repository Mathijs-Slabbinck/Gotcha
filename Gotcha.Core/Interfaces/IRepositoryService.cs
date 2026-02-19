using Gotcha.Core.Services.ResultModel;

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
