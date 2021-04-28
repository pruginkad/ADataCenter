using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Domain
{
    public class IncidentFullData
    {
        public Incident incident
        {
            get;
            set;
        }
        public List<Incident_Handling> incident_hl
        {
            get;
            set;
        }
        public List<ImageData> image_list
        {
            get;
            set;
        }
    }
}
