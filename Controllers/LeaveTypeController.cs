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

        public ActionResult Index()
        {
            var leaveType = _repo.FindAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveType);
            return View(model);
        }

       
        public ActionResult Details(int id)
        {
            if (!_repo.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                leaveType.DateCreated = DateTime.Now;

                var isSuccess= _repo.Create(leaveType);

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

        public ActionResult Edit(int id)
        {
            if (!_repo.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                //leaveType.DateCreated = DateTime.Now;

                var isSuccess = _repo.Update(leaveType);

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

        public ActionResult Delete(int id)
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
            var leaveType = _repo.FindById(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            var isSuccess = _repo.Delete(leaveType);

            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LeaveTypeViewModel model)
        {
            try
            {
                var leaveType = _repo.FindById(id);
                if (leaveType == null)
                {
                    return NotFound();
                }
                var isSuccess = _repo.Delete(leaveType);

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
