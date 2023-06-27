using LeaveManagementSystem.Data;
using LeaveManagementSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    public class AssignRolesController : Controller
    {
        //private readonly UserManager<Employee> _userManager;
        private readonly EmployeeRepository _employeeRepository;
        public AssignRolesController(/*UserManager<Employee> userManager,*/ EmployeeRepository employeeRepository)
        {
            //_userManager = userManager;
            _employeeRepository = employeeRepository;
        }
        // GET: AssignRolesController
        public async Task<IActionResult> Index()
        {
            var employees = _employeeRepository.FindAll();
            return View(nameof(Index));
        }

        // GET: AssignRolesController/Details/5
        public async Task<ActionResult> AssignRole(string id)
        {
            var emp = _employeeRepository.FindById(id);
            //var role=
            return View();
        }

        // GET: AssignRolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssignRolesController/Create
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

        // GET: AssignRolesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssignRolesController/Edit/5
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
                return View();
            }
        }

        // GET: AssignRolesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AssignRolesController/Delete/5
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
