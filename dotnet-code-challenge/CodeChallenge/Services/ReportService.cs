using CodeChallenge.Data;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly ILogger<ReportService> _logger; 
        private int count = 0;

        public ReportService(ILogger<ReportService> logger, IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Recursive function which will return the requested employee hierarchy
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public ReportingStructure CalculateNumberOfDirectReports(string employeeId)
        {
            ReportingStructure reportingStructure = new ReportingStructure();
            // Get employee from Employee repository
            var employee = _reportsRepository.GetById(employeeId);
            // Check if there are direct report employees
            if (employee?.DirectReports != null)
            {
                foreach (var directReport in employee?.DirectReports)
                {
                    /* 
                     * If there is an employee send a recursive call to check
                       if employee has more direct report employees in it
                    */
                    reportingStructure = CalculateNumberOfDirectReports(directReport.EmployeeId);
                    if (reportingStructure != null)
                    {
                        // This counter will be updated based on the first call
                        count++;
                    }
                }
            }

            reportingStructure.Employee = employee;
            reportingStructure.NumberOfReports = count;
            // The first call employee will returned in this case
            // e.g. in case of 16a596ae-edd3-4847-99fe-c4518e82c86f it's count is 4
            // e.g. in case of 03aa1462-ffa9-4978-901b-7c001562cf6f it's count is 2
            return reportingStructure;
        }
    }
}
