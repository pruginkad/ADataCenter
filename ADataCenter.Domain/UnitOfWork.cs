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
        public async Task<EN_RETCODE> Create(Incident item)
        {
            return await IncidentRepository.Create(item);
        }

        public async Task<EN_RETCODE> Delete(Guid id)
        {
            return await IncidentRepository.Delete(id);
        }

        public async Task<IEnumerable<Incident>> GetAll(Filter4Get filter)
        {
            return await IncidentRepository.GetAll(filter);
        }

        public async Task<Incident> GetById(Guid id)
        {
            return await IncidentRepository.GetById(id);
        }
    }
}
