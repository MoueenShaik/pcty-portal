using MediatR;
using PCTYLibrary.Models;
using PCTYLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Queries
{
    public class GetAllEmployeeQuery : IRequest<IEnumerable<Employee>>
    {
        public class GetAllPlayersQueryHandler : IRequestHandler<GetAllEmployeeQuery, IEnumerable<Employee>>
        {         
            private readonly IEmployeeService _employeeService;

            public GetAllPlayersQueryHandler(IEmployeeService employeeService)
            {
                _employeeService = employeeService;
            }

            public async Task<IEnumerable<Employee>> Handle(GetAllEmployeeQuery query, CancellationToken cancellationToken)
            {
                return await _employeeService.GetEmployeeList();
            }
        }
    }   
}
