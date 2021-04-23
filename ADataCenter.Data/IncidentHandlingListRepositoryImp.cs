using ADataCenter.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Data
{
    public class IncidentHandlingListRepositoryImp : IRepositoryList<incident_handling_list>
    {
        private readonly IncidentContext _IncidentContext;

        public IncidentHandlingListRepositoryImp(IncidentContext ContextIn)
        {
            this._IncidentContext = ContextIn;
        }

        public async Task<EN_RETCODE> Create(incident_handling_list item_list)
        {
            foreach(var item in item_list.data)
            {
                if(item.incident_id == Guid.Empty)
                {
                    item.incident_id = item_list.incident_id;
                }
                if (item.id == Guid.Empty)
                {
                    item.id = Guid.NewGuid();
                }
                var new_db_row = await _IncidentContext.AddAsync(item);
            }
            
            await _IncidentContext.SaveChangesAsync();

            return EN_RETCODE.OK;
        }

        public async Task<EN_RETCODE> Delete(Guid id)
        {
            var list  = await GetById(id);

            if (list == null)
            {
                return EN_RETCODE.FAILED;
            }

            foreach (var db_row in list.data)
            {
                _IncidentContext.IncidentHandling.Remove(db_row);
            }
            
            await _IncidentContext.SaveChangesAsync();

            return EN_RETCODE.OK;
        }

        public async Task<incident_handling_list> GetById(Guid id)
        {
            incident_handling_list list = new incident_handling_list();
            list.data = await _IncidentContext.IncidentHandling.Where(t => t.incident_id == id).ToListAsync();
            return list;
        }

        public async Task<EN_RETCODE> Update(incident_handling_list item)
        {
           return await Task<EN_RETCODE>.FromResult(EN_RETCODE.FAILED);
        }

        public async Task<incident_handling_list> GetAll(IEnumerable<Guid> filter)
        {
            incident_handling_list list = new incident_handling_list();
            list.data = await _IncidentContext.IncidentHandling.
                Where(i => filter.Contains(i.incident_id)
                //&& i.line_timestamp > DateTime.MinValue && i.line_timestamp < DateTime.MaxValue
                )
                .ToListAsync();
            return list;
        }
    }
}

