using AngularApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApiDemo.Repository
{
    public interface IEmployeeRepository
    {
        Task<bool> Create(Employee emp);
        Task<List<Employee>> GetEmployee(int? id);
        Task Edit(Employee emp);
        Task<bool> Delete(int id);
        Task<Employee> GetOneEmployee(int id);
        Task<bool> SignUp(User user);
        Task<UserViewModel> Login(User user);
        Task<User> FindUserByEmail(string Email);

    }
}
