using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Repository
{    
    public class LeaveRequestRepository : ILeaveRequestRepository
    {        
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            var leaveHistory = _db.LeaveRequests
                .Include(x => x.RequestingEmployee)
                .Include(x => x.ApprovedBy)
                .Include(x => x.LeaveType)
                .ToList();
            return leaveHistory;
        }

        public LeaveRequest FindById(int id)
        {
            var leaveHistory = _db.LeaveRequests
                .Include(x => x.RequestingEmployee)
                .Include(x => x.ApprovedBy)
                .Include(x => x.LeaveType)
                .FirstOrDefault(x => x.Id ==id);
            return leaveHistory;
        }

        public ICollection<LeaveRequest> GetLeaveRequestByEmployee(string employeeId)
        {
            //var leaveRequests = _db.LeaveRequests
            //    .Include(x => x.RequestingEmployee)
            //    .Include(x => x.ApprovedBy)
            //    .Include(x => x.LeaveType)
            //    .Where(x => x.RequestingEmployeeId == employeeId)
            //    .ToList();

            var leaveRequests = FindAll()
                .Where(x => x.RequestingEmployeeId == employeeId)
                .ToList();
            return leaveRequests;
        }

        public bool IsExists(int id)
        {
            var exists = _db.LeaveRequests.Any(x => x.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }
    }
}
