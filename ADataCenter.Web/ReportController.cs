using ADataCenter.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADataCenter.Web
{
    [Route("IncidentReportApi")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IUnitOfWork<Incident> _repo;
        private readonly IUnitOfWorkList<incident_handling_list> _repoList;

        public ReportController(ILogger<ReportController> logger,
            IUnitOfWork<Incident> repo,
            IUnitOfWorkList<incident_handling_list> repoList
            )
        {
            _logger = logger;
            _repo = repo;
            _repoList = repoList;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // POST api/<ReportController>
        [HttpPost]
        [Route("GetFullData")]
        public async Task<ActionResult> GetFullData(Filter4Get filter)
        {
            await _repo.GetAll(filter);
            return Ok();
        }
    }
}
