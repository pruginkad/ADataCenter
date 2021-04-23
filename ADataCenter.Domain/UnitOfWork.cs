using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public class UnitOfWorkIncident : IUnitOfWork<Incident>
    {
        private readonly IRepository<Incident> IncidentRepository;

        public UnitOfWorkIncident(IRepository<Incident> inRepository)
        {
            this.IncidentRepository = inRepository;
        }
        public Task<EN_RETCODE> Create(Incident item)
        {
            return IncidentRepository.Create(item);
        }

        public Task<EN_RETCODE> Delete(Guid id)
        {
            return IncidentRepository.Delete(id);
        }

        public Task<IEnumerable<Incident>> GetAll(Filter4Get filter)
        {
            return IncidentRepository.GetAll(filter);
        }

        public Task<Incident> GetById(Guid id)
        {
            return IncidentRepository.GetById(id);
        }
    }
}
