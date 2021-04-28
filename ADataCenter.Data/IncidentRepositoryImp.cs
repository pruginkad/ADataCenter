using ADataCenter.Domain;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace ADataCenter.Data
{

    public class IncidentRepositoryImp : IRepository<IncidentFullData>
    {
        private readonly IncidentContext _IncidentContext;

        public IncidentRepositoryImp(IncidentContext ContextIn)
        {
            this._IncidentContext = ContextIn;
        }

        public async Task<EN_RETCODE> Create(IncidentFullData item)
        {
            var db_row = await GetById(item.incident.id);
            if (db_row != null)
            {
                return EN_RETCODE.FAILED;
            }
            var new_db_row = await _IncidentContext.Incidents.AddAsync(item.incident);
            foreach(var line in item.incident_hl)
            {
                if(line.incident_id == Guid.Empty)
                {
                    line.incident_id = item.incident.id;
                }
                
                await _IncidentContext.incidentHandling.AddAsync(line);
            }
            try
            {
                await _IncidentContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return EN_RETCODE.FAILED;
            }

            return EN_RETCODE.OK;
        }

        public async Task<EN_RETCODE> Delete(Guid id)
        {
            var db_row = await _IncidentContext.Incidents.FirstOrDefaultAsync(t => t.id == id);
            if (db_row == null)
            {
                return EN_RETCODE.FAILED;
            }
            _IncidentContext.Incidents.Remove(db_row);

            
            var list = await _IncidentContext.incidentHandling.Where(t => t.incident_id == id).ToListAsync();
            foreach (var db_list_row in list)
            {
                _IncidentContext.incidentHandling.Remove(db_list_row);
            }

            await _IncidentContext.SaveChangesAsync();
            return EN_RETCODE.OK;
        }

        public async Task<IncidentFullData> GetById(Guid id)
        {
            IncidentFullData full_data = new IncidentFullData();
            try
            {
                full_data.incident = await _IncidentContext.Incidents.FirstAsync(t => t.id == id);
            }
            catch(InvalidOperationException ex)
            {
                return null;
            }
            

            full_data.incident_hl = await _IncidentContext.incidentHandling.Where(t => t.incident_id == id).ToListAsync();

            return full_data;
        }

        public async Task<List<Incident_Handling> > GetHandlingListByGuids(IEnumerable<Guid> filter)
        {
            var list = await _IncidentContext.incidentHandling.
                Where(i => filter.Contains(i.incident_id)
                //&& i.line_timestamp > DateTime.MinValue && i.line_timestamp < DateTime.MaxValue
                )
                .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<IncidentFullData>> GetAll(Filter4Get filter)
        {
            List<IncidentFullData> ret = new List<IncidentFullData>();

            Instant time1 = Instant.FromDateTimeUtc(filter.time1);
            Instant time2 = Instant.FromDateTimeUtc(filter.time2);

            Dictionary<Guid, IncidentFullData> dic_of_inc = new Dictionary<Guid, IncidentFullData>();

            var ret1 = await _IncidentContext.Incidents.
                Where(i => i.timestamp >= time1 && i.timestamp <= time2)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToListAsync();

            List<Guid> id_list = new List<Guid>();

            foreach (var cur_inc in ret1)
            {
                IncidentFullData fd = new IncidentFullData()
                {
                    incident = cur_inc,
                    image_list = new List<ImageData>(),
                    incident_hl = new List<Incident_Handling>()
                };
                ret.Add(fd);
                dic_of_inc.TryAdd(cur_inc.id, fd);
                id_list.Add(cur_inc.id);
            }

            var temp = await GetHandlingListByGuids(id_list);
            foreach (var l in temp)
            {
                IncidentFullData fd;
                if (dic_of_inc.TryGetValue(l.incident_id, out fd))
                {
                    fd.incident_hl.Add(l);
                }
            }

            
            return ret;
        }
    }
}
