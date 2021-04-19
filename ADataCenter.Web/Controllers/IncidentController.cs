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

namespace ADataCenter.Web.Controllers
{
    [ApiController]
    [Route("IncidentApi")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class IncidentController : ControllerBase
    {

        private readonly ILogger<IncidentController> _logger;
        private readonly IUnitOfWork<Incident> _repo;

        public IncidentController(ILogger<IncidentController> logger, IUnitOfWork<Incident> repo)
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
        [Route("GetAllIncidents")]
        public async Task<IEnumerable<Incident>> Get()
        {
            return  await _repo.GetAll();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Route("GetById/{id}")]
        
        [HttpGet ("{incident_id:guid}", Name = "GetById")]
        public async Task<ActionResult<Incident>> GetById(Guid Incident_id)
        {
            Incident temp = await _repo.GetById(Incident_id);
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
        public async Task<ActionResult<Incident>> CreateIncident([FromBody] Incident inIncident)
        {
            if(inIncident.ID != Guid.Empty)
            {
                var tempIncident = await _repo.GetById(inIncident.ID);
                if(tempIncident != null)
                {
                    return BadRequest("id already exist");
                }
            }
            

            var retIncident = await _repo.Create(inIncident);
            if(retIncident == null)
            {
                return BadRequest("unable to create");
            }
            
            return CreatedAtAction(@"GetById", new { Incident_id = retIncident.ID }, retIncident);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("UpdateIncident")]
        public async Task<ActionResult> UpdateIncident(Incident inIncident)
        {
            var tempIncident = await _repo.GetById(inIncident.ID);
            if (tempIncident == null)
            {
                return BadRequest("id not exist");
            }

            var res = await _repo.Update(inIncident);
            if (res != EN_RETCODE.OK)
            {
                return BadRequest("update failed");
            }

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("DeleteIncident/{idIncident}")]
        public async Task<ActionResult> DeleteIncident(Guid idIncident)
        {
            var tempIncident = await _repo.GetById(idIncident);
            if (tempIncident == null)
            {
                return BadRequest("id not exist");
            }

            var res = await _repo.Delete(idIncident);
            if (res != EN_RETCODE.OK)
            {
                return BadRequest("delete failed");
            }

            return Ok();
        }
    }
}
