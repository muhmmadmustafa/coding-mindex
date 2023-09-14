using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeRepository employeeRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public Compensation GetById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var compensationData = _compensationRepository.GetById(id);
                if (compensationData != null)
                {
                    var employeeData = _employeeRepository.GetById(id);
                    compensationData.Employee = employeeData;
                }
                return compensationData;
            }

            return null;

        }

        public Compensation Create(Compensation compensation)
        {
            if (compensation != null && !String.IsNullOrEmpty(compensation?.Employee?.EmployeeId))
            {
                var employeeData = _employeeRepository.GetById(compensation?.Employee?.EmployeeId);
                if (employeeData != null)
                {
                    _compensationRepository.Add(compensation);
                    _compensationRepository.SaveAsync().Wait();
                    return compensation;
                }
            }
            return null;
        }
    }
}
