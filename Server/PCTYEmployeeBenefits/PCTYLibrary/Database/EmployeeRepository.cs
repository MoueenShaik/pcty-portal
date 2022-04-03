using Dapper;
using PCTYLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Database
{
    public interface IEmployeeRepository
    {

        public Task<IEnumerable<Employee>> GetAllEmployees();
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;
        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()   
        {
            var employees = new List<Employee>();

            var query = ConstantQueries.AllEmployees;
            using (var connection = _context.CreateConnection())
            {   
                
                var companies = await connection.QueryAsync<Employee, Dependent, Employee>(
                    query, (employee, dependent) =>
                    {                       
                        if(employee.Dependents == null)
                        {
                            employee.Dependents = new List<Dependent>();
                        }

                        employee.Dependents.Add(dependent);
                        if (!employees.Any(a => a.Id == employee.Id))
                        {
                            employees.Add(employee);
                        }
                        else
                        {
                            if (employees.First(a => a.Id == employee.Id).Dependents == null)
                            {
                                employees.First(a => a.Id == employee.Id).Dependents = new List<Dependent>() { dependent};
                            }
                            else
                            {
                                employees.First(a => a.Id == employee.Id).Dependents.Add(dependent);
                            }
                        }

                        return employee;
                    }
                );
            }

            return employees.DistinctBy(a => a.Id).ToList();
        }
    }
}
