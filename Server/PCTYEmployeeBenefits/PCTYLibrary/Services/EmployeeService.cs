using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PCTYLibrary.Database;
using PCTYLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeeList();
        Task<EmployeeCosts> GetEmployeeBenefitsCost(int employeeId);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IConfiguration _configuration;
        private readonly IDiscountService _discountService;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IConfiguration configuration , IDiscountService discountService , IEmployeeRepository employeeRepository)
        {
            _configuration = configuration;
            _discountService = discountService;
            _employeeRepository = employeeRepository;
        }

        private EmployerConfigurationOptions GetEmployerConfigurationOptions()
        {
            return _configuration.GetSection("EmployerConfiguration").Get<EmployerConfigurationOptions>();
        }

        private bool IsDataLoadFromDb()
        {
            return _configuration.GetSection("DataLoadFromDB").Get<bool>();
        }
    
        public async Task<EmployeeCosts> GetEmployeeBenefitsCost(int employeeId)
        {
            var employerConfigurationOptions = GetEmployerConfigurationOptions();

            var employeeBenefitsCost = 0;
            var employeeBenefitsPerYear = employerConfigurationOptions.EmployeeCostOfBenefits;
            var depedentsCostPerYear = employerConfigurationOptions.DependentCostOfBenefits;
            var discount = _discountService.GetCurrentDiscount(employerConfigurationOptions);

            var discountPercentage = discount.DiscountPercentage;

            var allEmployees = await GetEmployeeList();
            var currentEmployee = allEmployees.FirstOrDefault(a => a.Id == employeeId);

            if (currentEmployee != null)
            {
                var dependentDiscountedAmount = 0;
                var employeeDiscountedAmount = 0;
                var dependentCost = 0;

                if (currentEmployee.Dependents != null)
                {
                    var dependents = currentEmployee.Dependents;
                    var dependentEligibleForDiscount = _discountService.GetEligibleDependendsForDiscount(discount, dependents);
                    if (dependentEligibleForDiscount > 0)
                    {
                        var percnetile = (discountPercentage / 100f);
                        var totalAMount = (depedentsCostPerYear * dependentEligibleForDiscount);
                        dependentDiscountedAmount = (int)(totalAMount - (totalAMount * percnetile));
                    }

                    var dependentsCostWithOutDiscount = (dependents.Count() - dependentEligibleForDiscount) * depedentsCostPerYear;
                     dependentCost = dependentsCostWithOutDiscount + dependentDiscountedAmount;

                }

                if (_discountService.GetEligibleEmployeeForDiscount(discount,currentEmployee))
                {
                    employeeDiscountedAmount = employeeBenefitsPerYear - ((int)((employeeBenefitsPerYear) * (discountPercentage / 100f)));
                }
                else
                {
                    employeeDiscountedAmount = employeeBenefitsPerYear;
                }

                employeeBenefitsCost = employeeDiscountedAmount + dependentCost;
            }          

            var employeeCosts = new EmployeeCosts()
            {
                TotalCostOfBenefitsPerYear = employeeBenefitsCost,
                SalaryPerPayCheck = employerConfigurationOptions.EmployeeSalaryPerPayCheck
            };

            employeeCosts.SalaryAfterBenefitsDeductionPerPayCheck = employerConfigurationOptions.EmployeeSalaryPerPayCheck -
                (employeeBenefitsCost / employerConfigurationOptions.TotalPayChecksPerYear);
            return employeeCosts;
        }


        private async Task<IEnumerable<Employee>> LoadJson()
        {
            IEnumerable<Employee> employees = new List<Employee>();

            var currentLocation = Assembly.GetExecutingAssembly().Location;
            if (currentLocation != null)
            {
                string? executableLocation = Path.GetDirectoryName(currentLocation);
                if (string.IsNullOrEmpty(executableLocation))
                {
                    return employees;
                }

                string xslLocation = Path.Combine(executableLocation, "TestData.json");

                using StreamReader r = new StreamReader(xslLocation);
                string? json = r.ReadToEnd();
                employees = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Employee>>(json));
            }

            return employees;
        }
        public async Task<IEnumerable<Employee>> GetEmployeeList()
        {
            return IsDataLoadFromDb() ? await _employeeRepository.GetAllEmployees() : await LoadJson();
           
            //return new List<Employee>()
            //{
            //    new Employee()
            //    {
            //        Id = 1, Name = "Moin",
            //        Dependents = new List<Dependent>()
            //        {
            //            new Dependent()
            //            {
            //                Name = "Rubeena",Relation = "Spouse"
            //            },
            //            new Dependent()
            //            {
            //               Name = "Shayaan" , Relation="Son"
            //            }
            //        }
            //    },
            //    new Employee() { Id = 2, Name = "Subhani" ,
            //        Dependents = new List<Dependent>()
            //        {
            //            new Dependent() { Name = "Sahil", Relation="Son" }
            //        }
            //    },
            //    new Employee() { Id = 3, Name = "AK" },
            //      new Employee() { Id = 4, Name = "Arifa", 
            //          Dependents = new List<Dependent>()
            //        {
            //            new Dependent()
            //            {
            //                Name = "Aani",Relation = "Spouse"
            //            },
            //            new Dependent()
            //            {
            //               Name = "Aahir" , Relation="Son"
            //            },
            //             new Dependent()
            //            {
            //               Name = "Arshita" , Relation="Daughter"
            //            }
            //        } }
            //};

        }
    }  
}
