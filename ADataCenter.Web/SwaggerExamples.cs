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
        public class SingleIncident : IExamplesProvider<Incident>
        {
            public Incident GetExamples()
            {
                return new Incident
                {
                    incidentName = "Fire",
                    objtype = "CAM",
                    objid = "1",
                    action = "ALARM",
                    incidentTimestamp = Instant.FromDateTimeUtc(DateTime.UtcNow),
                    user_id = "1"
                };
            }
        }

        public class MultiIncident : IExamplesProvider<IEnumerable<Incident>>
        {
            public IEnumerable<Incident> GetExamples()
            {
                return new List<Incident>()
            {
                new Incident
                {
                    incidentName = "Smoke on the water",
                    objtype = "CAM",
                    objid = "1",
                    action = "ALARM",
                    incidentTimestamp = Instant.FromDateTimeUtc(DateTime.UtcNow),
                    user_id = "2"
                },
                new Incident
                {
                    incidentName = "Fire",
                    objtype = "CAM",
                    objid = "2",
                    action = "ALARM",
                    incidentTimestamp = Instant.FromDateTimeUtc(DateTime.UtcNow),
                    user_id = "1"
                }
            };
            }
        }

        public class IncidentList : IExamplesProvider<incident_handling_list>
        {
            public incident_handling_list GetExamples()
            {
                return new incident_handling_list()
                {
                    incident_id = Guid.NewGuid(),
                    data = new List<incident_handling>()
                    {
                        new incident_handling
                        {
                            id = Guid.NewGuid(),
                            line_action = "act1",
                            line_descr = "descr1",
                            lineDateTime = DateTime.UtcNow
                        },
                        new incident_handling
                        {
                            id = Guid.NewGuid(),
                            line_action = "act2",
                            line_descr = "descr2",
                            lineDateTime = DateTime.UtcNow
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
