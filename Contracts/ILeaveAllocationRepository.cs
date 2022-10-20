using LeaveManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Contracts
{
    public interface ILeaveAllocationRepository: IRepositoryBase<LeaveAllocation>
    {
        Task<bool> ChechAllocation(int leaveTypeid, string employeeid);
        Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id);
        Task<LeaveAllocation> GetLeaveAllocationsByEmployeeType(string employeeId, int leaveTypeId);
    }
}
