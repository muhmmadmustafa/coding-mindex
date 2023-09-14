using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CodeChallenge.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IReportsRepository> _logger;

        public ReportsRepository(ILogger<IReportsRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee GetById(string id)
        {
            // Lazy load to include direct reports
            _employeeContext.Employees.Include(x => x.DirectReports).ToListAsync();
            var result = _employeeContext.Employees.FirstOrDefault(e => e.EmployeeId == id);
            return result;
        }
    }
}
