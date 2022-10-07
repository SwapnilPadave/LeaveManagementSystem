using LeaveManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Contracts
{
    public interface ILeaveAllocationRepository: IRepositoryBase<LeaveAllocation>
    {
        bool ChechAllocation(int leaveTypeid, string employeeid);
        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id); 
    }
}
