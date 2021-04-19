using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public interface IUnitOfWork<T> where T : class
    {
        Task<EN_RETCODE> Create(T item);
        Task<T> GetById(Guid id);
        Task<EN_RETCODE> Delete(Guid id);
        Task<EN_RETCODE> Update(T item);
        Task<IEnumerable<T>> GetAll();
    }

    public interface IUnitOfWorkList<T> where T : class
    {
        Task<EN_RETCODE> Create(T item);
        Task<T> GetById(Guid id);
        Task<EN_RETCODE> Delete(Guid id);
        Task<EN_RETCODE> Update(T item);
    }
}
