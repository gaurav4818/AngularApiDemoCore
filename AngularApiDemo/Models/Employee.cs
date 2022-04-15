using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApiDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpPassword { get; set; }
        public int EmpSalary { get; set; }    
    }
}
