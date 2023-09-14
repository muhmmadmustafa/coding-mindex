using CodeChallenge.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CodeChallenge.Data
{
    public class CompensationDataSeeder
    {
        private CompensationContext _CompensationContext;
        private const String Compensation_SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public CompensationDataSeeder(CompensationContext CompensationContext)
        {
            _CompensationContext = CompensationContext;
        }

        public async Task Seed()
        {
            if (!_CompensationContext.Compensation.Any())
            {
                List<Compensation> Compensation = LoadCompensation();
                _CompensationContext.Compensation.AddRange(Compensation);
                await _CompensationContext.SaveChangesAsync();
            }
        }

        private List<Compensation> LoadCompensation()
        {
            using (FileStream fs = new FileStream(Compensation_SEED_DATA_FILE, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Compensation> Compensation = serializer.Deserialize<List<Compensation>>(jr);
                FixUpReferences(Compensation);

                return Compensation;
            }
        }

        //private void FixUpReferences(List<Compensation> compensations)
        //{
        //    var compensationIdRefMap = from compensation in compensations
        //                               select new
        //                               {
        //                                   Id = compensation.CompensationId,
        //                                   EmployeeId = compensation.Employee.EmployeeId,
        //                                   CompensationRef = compensation
        //                               };

        //    compensations.ForEach(employee =>
        //    {
        //        if (employee?.Employee.DirectReports != null)
        //        {
        //            var referencedEmployees = new List<Employee>(employee.Employee.DirectReports.Count);
        //            employee?.Employee?.DirectReports.ForEach(report =>
        //            {
        //                var referencedEmployee = compensationIdRefMap.First(e => e.Id == report.EmployeeId && e.EmployeeId == report.EmployeeId).CompensationRef;
        //                referencedEmployees.Add(referencedEmployee.Employee);
        //            });
        //            employee.Employee.DirectReports = referencedEmployees;
        //        }
        //    });
        //}

        private void FixUpReferences(List<Compensation> compensations)
        {
            var employeeMap = compensations.ToDictionary(c => c.Employee.EmployeeId);

            foreach (var compensation in compensations)
            {
                if (compensation.Employee != null && employeeMap.ContainsKey(compensation.Employee.EmployeeId))
                {
                    compensation.Employee = employeeMap[compensation.Employee.EmployeeId].Employee;
                }
            }
        }
    }
}
