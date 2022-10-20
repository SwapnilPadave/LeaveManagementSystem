using AutoMapper;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Contracts
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepository,
            IMapper mapper,
            UserManager<Employee> userManager,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveAllocationRepository leaveAllocationRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _userManager = userManager;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
        }
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Index()
        {
            var leaveRequests =await _leaveRequestRepository.FindAll();
            var leaveRequestModel = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests);
            var model = new AdminLeaveRequestViewModel
            {
                TotalRequests = leaveRequestModel.Count,
                ApprovedRequests = leaveRequestModel.Count(x => x.Approved == true),
                PendingRequests = leaveRequestModel.Count(x => x.Approved == null),
                RejectedRequests = leaveRequestModel.Count(x => x.Approved == false),
                LeaveRequestsView = leaveRequestModel
            };
            return View(model);
        }

        public async Task<ActionResult> MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeId = employee.Id;
            var employeeAllocation =await _leaveAllocationRepository.GetLeaveAllocationsByEmployee(employeeId);
            var employeeRequest =await _leaveRequestRepository.GetLeaveRequestByEmployee(employeeId);

            var employeeAllocationModel = _mapper.Map<List<LeaveAllocationViewModel>>(employeeAllocation);
            var employeeRequestModel = _mapper.Map<List<LeaveRequestViewModel>>(employeeRequest);

            var model = new EmployeeLeaveRequestViewModel
            {
                LeaveAllocations = employeeAllocationModel,
                LeaveRequests = employeeRequestModel
            };

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var leaveRequest =await _leaveRequestRepository.FindById(id);
            var model = _mapper.Map<LeaveRequestViewModel>(leaveRequest);
            return View(model);
        }

        public async Task<ActionResult> ApproveRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest =await _leaveRequestRepository.FindById(id);
                var employeeId = leaveRequest.RequestingEmployeeId;
                var leaveTypeId = leaveRequest.LeaveTypeId;
                var allocation =await _leaveAllocationRepository.GetLeaveAllocationsByEmployeeType(employeeId, leaveTypeId);
                int daysRequsted = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                //allocation.NumberOfDays = allocation.NumberOfDays - daysRequsted;
                allocation.NumberOfDays -= daysRequsted;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                await _leaveRequestRepository.Update(leaveRequest);
                await _leaveAllocationRepository.Update(allocation);

                return RedirectToAction(nameof(Index), "LeaveRequest");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "LeaveRequest");
            }
        }

        public async Task<ActionResult> RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest =await _leaveRequestRepository.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                await _leaveRequestRepository.Update(leaveRequest);
                return RedirectToAction(nameof(Index), "LeaveRequest");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "LeaveRequest");
            }
        }

        public async Task<ActionResult> CancelRequest(int id)
        {
            var leaveRequest =await _leaveRequestRepository.FindById(id);
            leaveRequest.Cancelled = true;
            await _leaveRequestRepository.Update(leaveRequest);
            return RedirectToAction("MyLeave");
        }

        public async Task<ActionResult> Create()
        {
            var leaveTypes =await _leaveTypeRepository.FindAll();
            var leaveTypesItems = leaveTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            var model = new CreateLeaveRequestViewModel
            {
                LeaveTypes = leaveTypesItems
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestViewModel model)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes =await _leaveTypeRepository.FindAll();
                var leaveTypesItems = leaveTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                model.LeaveTypes = leaveTypesItems;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start Date cannot be further in the futre than the End Date..");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                var allocation =await _leaveAllocationRepository.GetLeaveAllocationsByEmployeeType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int)(endDate - startDate).TotalDays;

                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You Do not Have Sufficient Days For This Request..");
                    return View(model);
                }

                var leaveRequestModel = new LeaveRequest
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId,
                    RequestComments=model.RequestComments
                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                var isSuccess =await _leaveRequestRepository.Create(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wronge With Applying Your Request..");
                    return View(model);
                }

                return RedirectToAction("MyLeave");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wronge...");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return  View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

