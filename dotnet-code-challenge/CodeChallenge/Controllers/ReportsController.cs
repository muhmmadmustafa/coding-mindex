using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportService _reportService;

        public ReportsController(ILogger<EmployeeController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        [HttpGet("{id}", Name = "directReports")]
        public IActionResult DirectReports(String id)
        {
            var directReports = _reportService.CalculateNumberOfDirectReports(id);
            return Ok(directReports);
        }
    }
}
