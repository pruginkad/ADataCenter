using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public class UnitOfWorkIncidentHandlingList : IUnitOfWorkList<incident_handling_list>
    {
        private readonly IRepositoryList<incident_handling_list> _Repository;

        public UnitOfWorkIncidentHandlingList(IRepositoryList<incident_handling_list> inRepository)
        {
            this._Repository = inRepository;
        }
        public Task<EN_RETCODE> Create(incident_handling_list item)
        {
            return _Repository.Create(item);
        }

        public Task<EN_RETCODE> Delete(Guid id)
        {
            return _Repository.Delete(id);
        }

        public Task<incident_handling_list> GetById(Guid id)
        {
            return _Repository.GetById(id);
        }

        public Task<EN_RETCODE> Update(incident_handling_list item)
        {
            return _Repository.Update(item);
        }
        public Task<incident_handling_list> GetAll(IEnumerable<Guid> filter)
        {
            return _Repository.GetAll(filter);
        }
    }
}
