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
        public async Task<bool> Create(LeaveRequest entity)
        {
            await _db.LeaveRequests.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            var leaveHistory =await _db.LeaveRequests
                .Include(x => x.RequestingEmployee)
                .Include(x => x.ApprovedBy)
                .Include(x => x.LeaveType)
                .ToListAsync();
            return leaveHistory;
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            var leaveHistory =await _db.LeaveRequests
                .Include(x => x.RequestingEmployee)
                .Include(x => x.ApprovedBy)
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id ==id);
            return leaveHistory;
        }

        public async Task<ICollection<LeaveRequest>> GetLeaveRequestByEmployee(string employeeId)
        {
            //var leaveRequests = _db.LeaveRequests
            //    .Include(x => x.RequestingEmployee)
            //    .Include(x => x.ApprovedBy)
            //    .Include(x => x.LeaveType)
            //    .Where(x => x.RequestingEmployeeId == employeeId)
            //    .ToList();

            var leaveRequests = await FindAll();                
            return leaveRequests.Where(x => x.RequestingEmployeeId == employeeId)
                .ToList();
        }

        public async Task<bool> IsExists(int id)
        {
            var exists =await _db.LeaveRequests.AnyAsync(x => x.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes =await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return await Save();
        }
    }
}
