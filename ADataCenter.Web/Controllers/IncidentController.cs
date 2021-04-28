using ADataCenter.Data;
using ADataCenter.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ADataCenter.Web
{
    [ApiController]
    [Route("IncidentApi")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class IncidentController : ControllerBase
    {

        private readonly ILogger<IncidentController> _logger;
        private readonly IUnitOfWork<IncidentFullData> _repo;

        public IncidentController(ILogger<IncidentController> logger, 
            IUnitOfWork<IncidentFullData> repo
            )
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        [Route("GetOSs")]
        public string GetOperatingSystem()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Linux";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "Windows";
            }

            return ("Cannot determine operating system!");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpGet ("{incident_id:guid}", Name = "GetById")]
        [Route("GetById/{Incident_id}")]
        public async Task<ActionResult<IncidentFullData>> GetById(Guid Incident_id)
        {
            var temp = await _repo.GetById(Incident_id);
            if(temp == null)
            {
                return NotFound();
            }
            return Ok(temp);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CreateIncident")]
        public async Task<ActionResult> CreateIncident(IncidentFullData inIncident)
        {
            //if(inIncident.incident.ID != Guid.Empty)
            //{
            //    var tempIncident = await _repo.GetById(inIncident.incident.ID);
            //    if(tempIncident != null)
            //    {
            //        return BadRequest("id already exist");
            //    }
            //}
            

            var retIncident = await _repo.Create(inIncident);
            if(retIncident != EN_RETCODE.OK)
            {
                return BadRequest("unable to create");
            }

            return Ok();
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("DeleteIncident/{idIncident}")]
        public async Task<ActionResult> DeleteIncident(Guid idIncident)
        {
            var res = await _repo.Delete(idIncident);
            if (res != EN_RETCODE.OK)
            {
                return BadRequest("delete failed");
            }

            return Ok();
        }
    }
}
