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
        private readonly IRepository<IncidentFullData> _RepositoryIncident;

        string _image_path = string.Empty;
        public UnitOfWorkReport(
            IRepository<IncidentFullData> RepositoryIncident,
            ServiceArgs args)
        {
            _image_path = args.ImagePath;
            this._RepositoryIncident = RepositoryIncident;
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

            var incidents = await _RepositoryIncident.GetAll(filter);
            foreach (var fd in incidents)
            {
                page.incidents.Add(fd);

                foreach (var l in fd.incident_hl)
                {
                    if (!string.IsNullOrEmpty(l.image_path))
                    {
                        try
                        {
                            if(fd.image_list == null)
                            {
                                fd.image_list = new List<ImageData>();
                            }
                            
                            var image_data = GetAsBase64(l.image_path);
                            fd.image_list.Add(image_data);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
            }


            return page;
        }
    }
}
