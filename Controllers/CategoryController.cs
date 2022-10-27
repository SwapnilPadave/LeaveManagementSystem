using AutoMapper;
using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        public CategoryController(
            ICategoryRepository categoryRepository
            ,IMapper mapper
            , UserManager<Employee> userManager)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            var categoryList = await _categoryRepository.FindAll();
            var model = _mapper.Map<List<CategoryMaster>, List<CategoryViewModel>>(categoryList.ToList());
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryViewModel master)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(master);
                }

                var category = _mapper.Map<CategoryMaster>(master);
                category.CreatedDate = DateTime.Now;
                var employee = _userManager.GetUserAsync(User).Result;
                category.CreatedBy = employee.Id;

                var categoryModel = new CategoryMaster
                {
                    CatId = category.CatId,
                    CatName = master.CatName,
                    CreatedDate=DateTime.Now,
                    CreatedBy=employee.Id,
                    IsActive=master.IsActive
                };                
                
                var isSuccess = await _categoryRepository.Create(categoryModel);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong..");
                    return View(master);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //throw ex;
                ModelState.AddModelError("", "Something Went Wrong..");
                return View(master);
            }            
        }

        public async Task<ActionResult> Details(int id)
        {
            var cat = await _categoryRepository.FindById(id);
            var model = _mapper.Map<CategoryViewModel>(cat);
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var IsExists = await _categoryRepository.IsExists(id);
            if (!IsExists)
            {
                return NotFound();
            }
            var cat = await _categoryRepository.FindById(id);
            var model = _mapper.Map<CategoryViewModel>(cat);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var cat = _mapper.Map<CategoryMaster>(model);                

                var isSuccess = await _categoryRepository.Update(cat);

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
            var cat = await _categoryRepository.FindById(id);
            if (cat == null)
            {
                return NotFound();
            }
            var isSuccess = await _categoryRepository.Delete(cat);

            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, CategoryViewModel model)
        {
            try
            {
                var cat = await _categoryRepository.FindById(id);
                if (cat == null)
                {
                    return NotFound();
                }
                var isSuccess = await _categoryRepository.Delete(cat);

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
