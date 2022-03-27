using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTYLibrary.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Dependent> Dependents { get; set; }

    }
}
