using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADataCenter.Domain
{
    [Table("incidents", Schema = "public")]
    public class Incident
    {
        [Key]
        public Guid ID { get; set; }
        public string IncidentName { get; set; }
        public string objtype { get; set; }
        public string objid { get; set; }
        public string action { get; set; }
        public string user_id { get; set; }
        public DateTime IncidentTimestamp { get; set; } //in UTC.
        

        public void CopyFrom(Incident copy_it)
        {
            IncidentName = copy_it.IncidentName;
            IncidentTimestamp = copy_it.IncidentTimestamp;
            objtype = copy_it.objtype;
            objid = copy_it.objid;
            action = copy_it.action;
        }
    }
}
