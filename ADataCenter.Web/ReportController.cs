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
        private readonly IUnitOfWorkReport<ReportPage> _repo;

        public ReportController(ILogger<ReportController> logger,
            IUnitOfWorkReport<ReportPage> repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // POST api/<ReportController>
        [HttpPost]
        [Route("GetFullData")]
        public async Task<ActionResult<ReportPage>> GetFullData(Filter4Get filter)
        {
            
            var page = await _repo.GetAll(filter);
            
            return Ok(page);
        }
    }
}
