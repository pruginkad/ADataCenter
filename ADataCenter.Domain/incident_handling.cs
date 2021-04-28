using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ADataCenter.Domain
{
    //    line_descr character varying COLLATE pg_catalog."default",
    //line_action character varying COLLATE pg_catalog."default",
    //incident_id uuid,
    //line_number integer
    [Table("incident_handling", Schema = "public")]
    public class Incident_Handling
    {
        [Key]
        public Guid id { get; set; }
        public string line_descr { get; set; }
        public string line_action { get; set; }
        public string image_path { get; set; }
        public Guid incident_id { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public Instant line_timestamp { get; set; } = new Instant();

        [NotMapped]
        public DateTime line_datetime
        {
            get
            {
                return line_timestamp.ToDateTimeUtc();
            }
            set
            {
                line_timestamp = Instant.FromDateTimeUtc(value);
            }
        } //in UTC.
    }
}
