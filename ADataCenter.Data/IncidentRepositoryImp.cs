using ADataCenter.Domain;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ADataCenter.Data
{

    public class IncidentRepositoryImp : IRepository<Incident>
    {
        private readonly IncidentContext _IncidentContext;

        public IncidentRepositoryImp(IncidentContext ContextIn)
        {
            this._IncidentContext = ContextIn;
        }

        public async Task<Incident> Create(Incident item)
        {
            var db_row = await GetById(item.ID);
            if (db_row != null)
            {
                return default;
            }
            var new_db_row = await _IncidentContext.Incidents.AddAsync(item);
            try
            {
                await _IncidentContext.SaveChangesAsync();
            }
            catch(Exception)
            {
                return default;
            }
            
            return new_db_row.Entity;
        }

        public async Task<EN_RETCODE> Delete(Guid id)
        {
            var db_row = await GetById(id);
            if (db_row == null)
            {
                return EN_RETCODE.FAILED;
            }
            _IncidentContext.Incidents.Remove(db_row);
            await _IncidentContext.SaveChangesAsync();
            return EN_RETCODE.OK;
        }

        public Task<Incident> GetById(Guid id)
        {
            return  _IncidentContext.Incidents.FirstOrDefaultAsync(t => t.ID == id);
        }

        public async Task<EN_RETCODE> Update(Incident item)
        {
            var db_row = await GetById(item.ID);
            if (db_row == null)
            {
                return EN_RETCODE.FAILED;
            }
            
            db_row.CopyFrom(item);
            await _IncidentContext.SaveChangesAsync();
            return EN_RETCODE.OK;
        }
        public async Task<IEnumerable<Incident>> GetAll()
        {
            var ret = await _IncidentContext.Incidents.ToListAsync();
            return ret;
        }
    }
}
