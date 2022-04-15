using AngularApiDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AngularApiDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;

        public EmployeeRepository(AppDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task<bool> Create(Employee emp)
        {
            if (emp.Id == 0)
            {
                await context.Employees.AddAsync(emp);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<Employee>> GetEmployee(int? id)
        {
            if (id == null)
            {
                var allemployee = await context.Employees.ToListAsync();
                await context.SaveChangesAsync();
                return allemployee;
            }
            return await context.Employees.Where(x => x.Id == id).ToListAsync();
        }
        public async Task<Employee> GetOneEmployee(int id)
        {
           
            return await context.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task Edit(Employee emp)
        {
            context.Employees.Update(emp);
            await context.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> SignUp(User user)
        {

         
            if (user.Id==0 )
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UserViewModel> Login(User user)
        {
            var userexist = await FindUserByEmail(user.Email);
            if(userexist != null && userexist.Password==user.Password)
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
                var authsigninkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                   issuer: configuration["JWT:ValidIssuer"],
                   audience: configuration["JWT:ValidAudience"],
                   expires: DateTime.Now.AddDays(1),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authsigninkey, SecurityAlgorithms.HmacSha256Signature)
                   );

                 new JwtSecurityTokenHandler().WriteToken(token);

                UserViewModel model = new UserViewModel {
                    Id = userexist.Id,
                    Email = userexist.Email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
                return model;
            }
            return null;
        }

        public async Task<User> FindUserByEmail(string Email)
        {
            var finduser = await context.Users.FirstOrDefaultAsync(x => x.Email == Email);
            return finduser;
        }
    }
}
