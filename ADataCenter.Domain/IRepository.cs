using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public enum EN_RETCODE
    {
        OK = 0,
        FAILED
    }
    public interface IRepository<T> where T : class
    {
        Task<EN_RETCODE> Create(T item);
        Task<T> GetById(Guid id);
        Task<EN_RETCODE> Delete(Guid id);
        Task<EN_RETCODE> Update(T item);
        Task<IEnumerable<T>> GetAll(Filter4Get filter);

    }

    public interface IRepositoryList<T> where T : class
    {
        Task<EN_RETCODE> Create(T item);
        Task<T> GetById(Guid id);
        Task<EN_RETCODE> Delete(Guid id);
        Task<EN_RETCODE> Update(T item);
    }
}
