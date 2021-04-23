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
        private readonly IUnitOfWork<Incident> _repo;
        private readonly IUnitOfWorkList<incident_handling_list> _repoList;

        public IncidentController(ILogger<IncidentController> logger, 
            IUnitOfWork<Incident> repo,
            IUnitOfWorkList<incident_handling_list> repoList
            )
        {
            _logger = logger;
            _repo = repo;
            _repoList = repoList;
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

        [HttpPost]
        [Route("GetAllIncidents")]
        public async Task<IEnumerable<Incident>> GetAllIncidents(Filter4Get filter)
        {
            return  await _repo.GetAll(filter);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpGet ("{incident_id:guid}", Name = "GetById")]
        [Route("GetById/{Incident_id}")]
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
        public async Task<ActionResult> CreateIncident(Incident inIncident)
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

            var res1 = await DeleteHandlingList(idIncident);
            if (res != EN_RETCODE.OK)
            {
                return BadRequest("DeleteHandlingList Failed");
            }
            return Ok();
        }
        /////////////////////////////////List

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("HandlingList/CreateIncidentHandling")]
        public async Task<ActionResult> CreateIncidentHandling([FromBody] incident_handling_list inIncident)
        {
            var retIncident = await _repoList.Create(inIncident);
            if (retIncident != EN_RETCODE.OK)
            {
                return BadRequest("unable to create");
            }

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [HttpGet("HandlingList/{idIncident:guid}", Name = "GetHandlingList")]
        public async Task<ActionResult<incident_handling_list>> GetHandlingList(Guid idIncident)
        {
            var temp = await _repoList.GetById(idIncident);
            if (temp == null)
            {
                return NotFound();
            }
            return Ok(temp);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("HandlingList/DeleteHandlingList/{idIncident}")]
        public async Task<ActionResult> DeleteHandlingList(Guid idIncident)
        {
            var res = await _repoList.Delete(idIncident);
            if (res != EN_RETCODE.OK)
            {
                return BadRequest("delete failed");
            }

            return Ok();
        }
    }
}
