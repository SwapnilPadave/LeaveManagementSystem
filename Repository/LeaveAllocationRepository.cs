using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ChechAllocation(int leaveTypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.Where(x =>x.EmployeeId==employeeid && x.LeaveTypeId==leaveTypeid && x.Period==period).Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            var leaveTypes =await _db.LeaveAllocations
                .Include(x=>x.LeaveType)
                .Include(x => x.Employee)
                .ToListAsync();
            return leaveTypes;
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            var leaveType =await _db.LeaveAllocations
                .Include(x => x.LeaveType)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id==id);
            return leaveType;
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.Where(x => x.EmployeeId == id && x.Period == period).ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeType(string employeeId, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            var allocations = await FindAll();
            return allocations.FirstOrDefault(x => x.EmployeeId == employeeId && x.Period == period && x.LeaveTypeId == leaveTypeId);
        }

        public async Task<bool> IsExists(int id)
        {
            var exists =await _db.LeaveAllocations.AnyAsync(x => x.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes =await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
