using ADataCenter.Domain;
using NodaTime;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADataCenter.Web
{
    namespace swag_examples
    {
        public class SingleIncident : IExamplesProvider<IncidentFullData>
        {
            public IncidentFullData GetExamples()
            {
                var guid = Guid.NewGuid();
                return new IncidentFullData
                {
                    incident = new Incident
                    {
                        id = guid,
                        name = "Fire_"+DateTime.UtcNow.ToShortTimeString(),
                        objtype = "CAM",
                        objid = "1",
                        action = "ALARM",
                        timestamp = Instant.FromDateTimeUtc(DateTime.UtcNow),
                        user_id = "1"
                    },
                    incident_hl = new List<Incident_Handling>()
                    {
                        new Incident_Handling
                        {
                            id = Guid.NewGuid(),
                            line_action = "act1",
                            line_descr = "descr1",
                            line_datetime = DateTime.UtcNow
                        },
                        new Incident_Handling
                        {
                            id = Guid.NewGuid(),
                            line_action = "act2",
                            line_descr = "descr2",
                            line_datetime = DateTime.UtcNow,
                            image_path = @$"test/{guid}.png"
                        }
                    },
                    image_list = new List<ImageData>()
                    {
                        new ImageData()
                        {
                            path = @$"test/{guid}.png",
                            image_data = "iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAAAfSURBVBhXY3gro8LAwLD1mSwQQTlQAORAEJQPBQwMAEfWCVLBLOaRAAAAAElFTkSuQmCC"
                        }
                    }
                };
                           
            }
        }

        ////////////////////////////////////
        public class SingleImage : IExamplesProvider<ImageData>
        {
            public ImageData GetExamples()
            {
                return new ImageData
                {
                    path = @"test/3.png",
                    image_data = "iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAAAfSURBVBhXY3gro8LAwLD1mSwQQTlQAORAEJQPBQwMAEfWCVLBLOaRAAAAAElFTkSuQmCC"
                };
            }
        }

        public class SingleFilter : IExamplesProvider<Filter4Get>
        {
            public Filter4Get GetExamples()
            {
                return new Filter4Get
                {
                    time1 = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    time2 = DateTime.UtcNow
                };
            }
        }
        
    }
}
