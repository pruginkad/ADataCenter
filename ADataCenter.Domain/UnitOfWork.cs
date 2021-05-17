using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public class UnitOfWorkIncident : IUnitOfWork<IncidentFullData>
    {
        private readonly IRepository<IncidentFullData> IncidentRepository;
        string _image_path = string.Empty;
        public UnitOfWorkIncident(IRepository<IncidentFullData> inRepository,
            ServiceArgs args)
        {
            _image_path = args.ImagePath;
            this.IncidentRepository = inRepository;
        }
        public async Task<EN_RETCODE> Create(IncidentFullData item)
        {
            try
            {
                if(item.image_list.Count > 0)
                {
                    foreach(var value in item.image_list)
                    {
                        if(value == null)
                        {
                            continue;
                        }
                        string ipath = Path.Combine(_image_path, value.path);
                        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(ipath));
                        var bytes = Convert.FromBase64String(value.image_data);
                        using (var imageFile = new FileStream(ipath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                    }                    
                }
                
            }
            catch(Exception ex)
            {

            }
            

            return await IncidentRepository.Create(item);
        }

        public async Task<EN_RETCODE> Delete(Guid id)
        {
            var temp = await IncidentRepository.GetById(id);
            if (temp == null)
            {
                return EN_RETCODE.FAILED;
            }
            foreach(var el in temp?.incident_hl)
            {
                try
                {
                    string ipath = Path.Combine(_image_path, el.image_path);
                    System.IO.File.Delete(ipath);
                }
                catch(Exception ex)
                {

                }
            }
            

            return await IncidentRepository.Delete(id);
        }

        public async Task<IEnumerable<IncidentFullData>> GetAll(Filter4Get filter)
        {
            return await IncidentRepository.GetAll(filter);
        }

        public async Task<IncidentFullData> GetById(Guid id)
        {
            return await IncidentRepository.GetById(id);
        }
    }
}
