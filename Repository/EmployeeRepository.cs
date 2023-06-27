using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Employee entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Employee entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Employee>> FindAll()
        {
            var employees = await _db.Employees.ToListAsync();
            return employees;
        }

        public async Task<Employee> FindById(string id)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return employee;
        }

        public Task<Employee> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}

