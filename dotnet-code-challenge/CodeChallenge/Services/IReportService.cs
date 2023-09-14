using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public interface IReportService
    {
        ReportingStructure CalculateNumberOfDirectReports(string employeeId);
    }
}