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
        public IEnumerable<Incident> Get()
        {
            return  _repo.GetAll().Result;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Route("GetById/{id}")]
        
        [HttpGet ("{incident_id:guid}", Name = "GetById")]
        public ActionResult<Incident> GetById(Guid Incident_id)
        {
            Incident temp = _repo.GetById(Incident_id).Result;
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
        public ActionResult<Incident> CreateIncident([FromBody] Incident inIncident)
        {
            if(inIncident.ID != Guid.Empty)
            {
                var tempIncident = _repo.GetById(inIncident.ID).Result;
                if(tempIncident != null)
                {
                    return BadRequest("id already exist");
                }
            }
            

            var retIncident = _repo.Create(inIncident).Result;
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
        public ActionResult UpdateIncident(Incident inIncident)
        {
            var tempIncident = _repo.GetById(inIncident.ID).Result;
            if (tempIncident == null)
            {
                return BadRequest("id not exist");
            }

            var res = _repo.Update(inIncident).Result;
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
        public ActionResult DeleteIncident(Guid idIncident)
        {
            var tempIncident = _repo.GetById(idIncident).Result;
            if (tempIncident == null)
            {
                return BadRequest("id not exist");
            }

            var res = _repo.Delete(idIncident).Result;
            if (res != EN_RETCODE.OK)
            {
                return BadRequest("delete failed");
            }

            return Ok();
        }
    }
}
