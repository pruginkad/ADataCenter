using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public class ServiceArgs
    {
        public string ImagePath { get; set; }
    }

    public class UnitOfWorkReport : IUnitOfWorkReport<ReportPage>
    {
        private readonly IRepository<Incident> _RepositoryIncident;
        private readonly IRepositoryList<incident_handling_list> _RepositoryList;

        string _image_path = string.Empty;
        public UnitOfWorkReport(
            IRepository<Incident> RepositoryIncident, 
            IRepositoryList<incident_handling_list> RepositoryList,
            ServiceArgs args)
        {
            _image_path = args.ImagePath;
            this._RepositoryIncident = RepositoryIncident;
            this._RepositoryList = RepositoryList;
        }

        public ImageData GetAsBase64(string path)
        {
            string ipath = Path.Combine(_image_path, path);
            Byte[] b;
            b = System.IO.File.ReadAllBytes(ipath);
            ImageData temp = new ImageData()
            {
                path = path,
                image_data = Convert.ToBase64String(b)
            };
            return temp;
        }
        public async Task<ReportPage> GetAll(Filter4Get filter)
        {
            ReportPage page = new ReportPage()
            {
                incidents = new List<IncidentFullData>()
            };

            Dictionary<Guid, IncidentFullData> dic_of_inc = new Dictionary<Guid, IncidentFullData>();
            var incidents = await _RepositoryIncident.GetAll(filter);
            List<Guid> id_list = new List<Guid>();
            foreach (var cur_inc in incidents)
            {
                IncidentFullData fd = new IncidentFullData()
                {
                    incident = cur_inc,
                    image_list = new List<ImageData>(),
                    incident_hl = new List<incident_handling>()
                };
                page.incidents.Add(fd);
                
                dic_of_inc.TryAdd(cur_inc.ID, fd);

                id_list.Add(cur_inc.ID);
            }

            var temp = await _RepositoryList.GetAll(id_list);
            foreach(var l in temp.data)
            {
                IncidentFullData fd;
                if (dic_of_inc.TryGetValue(l.incident_id, out fd))
                {
                    fd.incident_hl.Add(l);
                    if(!string.IsNullOrEmpty(l.image_path))
                    {
                        try
                        {
                            var image_data = GetAsBase64(l.image_path);
                            fd.image_list.Add(image_data);
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                }
            }

            return page;
        }
    }
}
