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

        public bool ChechAllocation(int leaveTypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(x =>x.EmployeeId==employeeid && x.LeaveTypeId==leaveTypeid && x.Period==period).Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            var leaveTypes = _db.LeaveAllocations
                .Include(x=>x.LeaveType)
                .Include(x => x.Employee)
                .ToList();
            return leaveTypes;
        }

        public LeaveAllocation FindById(int id)
        {
            var leaveType = _db.LeaveAllocations
                .Include(x => x.LeaveType)
                .Include(x => x.Employee)
                .FirstOrDefault(x => x.Id==id);
            return leaveType;
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .Where(x => x.EmployeeId == id && x.Period == period).ToList();
        }

        public LeaveAllocation GetLeaveAllocationsByEmployeeType(string employeeId, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .FirstOrDefault(x => x.EmployeeId == employeeId && x.Period == period && x.LeaveTypeId == leaveTypeId);
        }

        public bool IsExists(int id)
        {
            var exists = _db.LeaveAllocations.Any(x => x.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
