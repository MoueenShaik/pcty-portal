using MediatR;
using PCTYLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Queries
{
    public class GetEmployeeBenefitsCostQuery : IRequest<EmployeeCosts>
    {
        public int EmployeeId { get; set; }
        public class GetEmployeeBenefitsCostQueryHandler : IRequestHandler<GetEmployeeBenefitsCostQuery, EmployeeCosts>
        {

            private readonly IEmployeeService _employeeService;

            public GetEmployeeBenefitsCostQueryHandler(IEmployeeService employeeService)
            {
                _employeeService = employeeService;
            }

            public async Task<EmployeeCosts> Handle(GetEmployeeBenefitsCostQuery query, CancellationToken cancellationToken)
            {
                return await _employeeService.GetEmployeeBenefitsCost(query.EmployeeId);
            }
        }
    }
}
