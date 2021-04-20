using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ADataCenter.Domain
{
    //    line_descr character varying COLLATE pg_catalog."default",
    //line_action character varying COLLATE pg_catalog."default",
    //incident_id uuid,
    //line_number integer
    [Table("incident_handling", Schema = "public")]
    public class incident_handling
    {
        [Key]
        public Guid id { get; set; }
        public string line_descr { get; set; }
        public string line_action { get; set; }
        public string image_path { get; set; }
        public Guid incident_id { get; set; }
        public DateTime line_timestamp { get; set; }
    }
    public class incident_handling_list
    {
        public Guid incident_id { get; set; }
        public List<incident_handling>  data { get; set; }
    }
}
