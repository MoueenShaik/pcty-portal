using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly EmployerConfigurationOptions _employerConfigurationOptions;
        public EmployeeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private EmployerConfigurationOptions GetEmployerConfigurationOptions()
        {
            return _configuration.GetSection("EmployerConfiguration").Get<EmployerConfigurationOptions>();
        }

        private Discount GetCurrentDiscount(EmployerConfigurationOptions employerConfigurationOptions)
        {
            Discount discount = null;

            if(!employerConfigurationOptions.IsDiscountsApplicable)
            {
                return discount;
            }

            discount = employerConfigurationOptions?.Discount?.FirstOrDefault(a => a.Type == employerConfigurationOptions?.DiscountType);
            return discount;
        }

        private int GetEligibleDependendsForDiscount(Discount discount ,IList<Dependent> dependents)
        {
            if(discount.Type == "Name")
            {
              return  dependents.Where(a => a.Name.StartsWith(discount.Condition)).Count();
            }

            return 0;
        }

        private bool GetEligibleEmployeeForDiscount(Discount discount, Employee currentEmployee)
        {
            if (discount.Type == "Name")
            {
                return currentEmployee.Name.StartsWith(discount.Condition);
            }

            return false;
        }
        public async Task<EmployeeCosts> GetEmployeeBenefitsCost(int employeeId)
        {
            var employerConfigurationOptions = GetEmployerConfigurationOptions();

            var employeeBenefitsCost = 0;
            var employeeBenefitsPerYear = employerConfigurationOptions.EmployeeCostOfBenefits;
            var depedentsCostPerYear = employerConfigurationOptions.DependentCostOfBenefits;
            var discount = GetCurrentDiscount(employerConfigurationOptions);
            var discountPercentage = discount.DiscountPercentage;
            var allEmployees = await LoadJson();
            var currentEmployee = allEmployees.FirstOrDefault(a => a.Id == employeeId);

            if (currentEmployee != null)
            {
                var dependentDiscountedAmount = 0;
                var employeeDiscountedAmount = 0;
                var dependentCost = 0;

                if (currentEmployee.Dependents != null)
                {
                    var dependents = currentEmployee.Dependents;
                    var dependentEligibleForDiscount = GetEligibleDependendsForDiscount(discount, dependents);
                    if (dependentEligibleForDiscount > 0)
                    {
                        var percnetile = (discountPercentage / 100f);
                        var totalAMount = (depedentsCostPerYear * dependentEligibleForDiscount);
                        dependentDiscountedAmount = (int)(totalAMount - (totalAMount * percnetile));
                    }

                    var dependentsCostWithOutDiscount = (dependents.Count() - dependentEligibleForDiscount) * depedentsCostPerYear;
                     dependentCost = dependentsCostWithOutDiscount + dependentDiscountedAmount;

                }

                if (GetEligibleEmployeeForDiscount(discount,currentEmployee))
                {
                    employeeDiscountedAmount = employeeBenefitsPerYear - ((int)((employeeBenefitsPerYear) * (discountPercentage / 100f)));
                }
                else
                {
                    employeeDiscountedAmount = employeeBenefitsPerYear;
                }

                employeeBenefitsCost = employeeDiscountedAmount + dependentCost;
            }
            else
            {

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


        public async Task<IEnumerable<Employee>> LoadJson()
        {
            string executableLocation = Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().Location);
            string xslLocation = Path.Combine(executableLocation, "TestData.json");

            IEnumerable<Employee> employees = new List<Employee>();
            using (StreamReader r = new StreamReader(xslLocation))
            {
                string json = r.ReadToEnd();
                employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(json);
            }

            return employees;
        }
        public async Task<IEnumerable<Employee>> GetEmployeeList()
        {
            return await LoadJson();
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
