using AutoMapper;
using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypeController(ILeaveTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var leaveType =await _repo.FindAll();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveType.ToList());
            return View(model);
        }

       
        public async Task<ActionResult> Details(int id)
        {
            var isExists = await _repo.IsExists(id); 
            if (!isExists)
            {
                return NotFound();
            }
            var leaveType =await _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                leaveType.DateCreated = DateTime.Now;

                var isSuccess=await _repo.Create(leaveType);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong..");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong..");
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var IsExists = await _repo.IsExists(id);
            if (!IsExists)
            {
                return NotFound();
            }
            var leaveType =await _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                //leaveType.DateCreated = DateTime.Now;

                var isSuccess =await _repo.Update(leaveType);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong..");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong..");
                return View(model);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            #region Delete Code 1
            //if (!_repo.IsExists(id))
            //{
            //    return NotFound();
            //}
            //var leaveType = _repo.FindById(id);
            //var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            //return View(model);
            #endregion

            var leaveType =await _repo.FindById(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            var isSuccess =await _repo.Delete(leaveType);

            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, LeaveTypeViewModel model)
        {
            try
            {
                var leaveType =await _repo.FindById(id);
                if (leaveType == null)
                {
                    return NotFound();
                }
                var isSuccess =await _repo.Delete(leaveType);

                if (!isSuccess)
                {                   
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {               
                return View(model);
            }
        }
    }
}
