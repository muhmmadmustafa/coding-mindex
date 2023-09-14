using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<EmployeeController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpGet("{id}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetEmployeeById(String id)
        {
            var compensationData = _compensationService.GetById(id);
            return Ok(compensationData);
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation?.Employee?.FirstName} {compensation?.Employee?.LastName}'");

            var result = _compensationService.Create(compensation);

            if (result == null)
                return NotFound();

            return CreatedAtRoute("getEmployeeById", new { id = compensation?.Employee?.EmployeeId }, compensation);
        }
    }
}
