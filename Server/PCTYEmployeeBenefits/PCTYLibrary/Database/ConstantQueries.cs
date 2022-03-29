using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Database
{
    public static class ConstantQueries
    {
        public const string AllEmployees = "Select * from dbo.Employee e join dbo.[Dependent] as d on d.Fk_Id = e.id";
    }
}
