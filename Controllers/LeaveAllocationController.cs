﻿using AutoMapper;
using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveTypeRepository _leaveRepo;
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationController(ILeaveAllocationRepository leaveAllocationRepo, 
            ILeaveTypeRepository leaveRepo,
            IMapper mapper,
            UserManager<Employee> userManager
            )
        {
            _leaveAllocationRepo = leaveAllocationRepo;
            _leaveRepo = leaveRepo;
            _mapper = mapper;
            _userManager = userManager;
        }        
        public ActionResult Index()
        {
            var leaveType = _leaveRepo.FindAll().ToList();
            var mappedLeaveType = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveType);
            var model = new CreateLeaveAllocationViewModel
            {
                LeaveTypes = mappedLeaveType,
                NumberUpdated = 0
            };
            return View(model);
        }

        public ActionResult SetLeave(int id)
        {
            var leaveType = _leaveRepo.FindById(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var emp in employees)
            {
                if (_leaveAllocationRepo.ChechAllocation(id, emp.Id))
                    continue;
                var allocation = new LeaveAllocationViewModel
                {
                    DateCreated=DateTime.Now,
                    EmployeeId=emp.Id,
                    LeaveTypeId=id,
                    NumberOfDays=leaveType.DefaultDays,
                    Period=DateTime.Now.Year
                };
                var leaveallocation = _mapper.Map<LeaveAllocation>(allocation);
                _leaveAllocationRepo.Create(leaveallocation);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<EmployeeViewModel>>(employees);
            return View(model);
        }
        
        public ActionResult Details(string id)
        {
            var employee =_mapper.Map<EmployeeViewModel>(_userManager.FindByIdAsync(id).Result);            
            var allocations = _mapper.Map<List<LeaveAllocationViewModel>>(_leaveAllocationRepo.GetLeaveAllocationsByEmployee(id));
            var model = new ViewAllocationViewModel
            {
                Employee = employee,
                LeaveAllocations = allocations
            };
            return View(model);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
        
        public ActionResult Edit(int id)
        {
            var leaveAllocation = _leaveAllocationRepo.FindById(id);
            var model = _mapper.Map<EditLeaveAllocationViewModel>(leaveAllocation); ;
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = _leaveAllocationRepo.FindById(model.Id);
                record.NumberOfDays = model.NumberOfDays;
                var isSuccess=_leaveAllocationRepo.Update(record);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Error While Saving..");
                    return View(model);
                }
                return RedirectToAction(nameof(Details), new {id = model.EmployeeId });
            }
            catch
            {
                return View(model);
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
