using AutoMapper;
using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository
            ,IMapper mapper
            ,UserManager<Employee> userManager
            , ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }
        public async Task<ActionResult> Index()
        {
            var catList = await _categoryRepository.FindAll();
            var productList = await _productRepository.FindAll();
            var model = _mapper.Map<List<ProductMaster>, List<ProductViewModel>>(productList.ToList());
           
            return View(model);
        }

        public async Task<ActionResult> EmployeeView()
        {
            var productList = await _productRepository.FindAll();
            var model = _mapper.Map<List<ProductMaster>, List<EmployeeProductViewModel>>(productList.ToList());
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var categoryType = await _categoryRepository.FindAll();
            var catTypesItems = categoryType.Select(x => new SelectListItem
            {
                Text = x.CatName,
                Value = x.CatId.ToString(),
            });
            var model = new ProductCreateViewModel
            {
                CatTypes = catTypesItems
            };
            return View(model);
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCreateViewModel model)
        {
            try
            {
                var categoryType = await _categoryRepository.FindAll();
                var catTypesItems = categoryType.Select(x => new SelectListItem
                {
                    Text = x.CatName,
                    Value = x.CatId.ToString(),
                });
                model.CatTypes = catTypesItems;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var emp = _userManager.GetUserAsync(User).Result;
                //var pro = _mapper.Map<ProductMaster>(model);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CreatedBy = emp.Id;
                model.ModifiedBy = emp.Id;

                var proCreateModel = new ProductMaster
                {
                    ProName = model.ProName,
                    CatId = model.CatId,
                    Price = model.Price,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = emp.Id,
                    ModifiedBy = emp.Id,
                    Quantity = model.Quantity,
                };
                var pro = _mapper.Map<ProductMaster>(proCreateModel);
                var isSuccess = await _productRepository.Create(pro);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wronge With Applying Your Request..");
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wronge...");
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var IsExists = await _productRepository.IsExists(id);
            if (!IsExists)
            {
                return NotFound();
            }
            var pro = await _productRepository.FindById(id);
            var model = _mapper.Map<ProductViewModel>(pro);

            var categoryType = await _categoryRepository.FindAll();
            var catTypesItems = categoryType.Select(x => new SelectListItem
            {
                Text = x.CatName,
                Value = x.CatId.ToString(),
            });
            model.CatTypes = catTypesItems;
            var model1 = new ProductViewModel
            {
                CatTypes = catTypesItems
            };
            //return View(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductViewModel model)
        {
            try
            {
                var categoryType = await _categoryRepository.FindAll();
                var catTypesItems = categoryType.Select(x => new SelectListItem
                {
                    Text = x.CatName,
                    Value = x.CatId.ToString(),
                });
                model.CatTypes = catTypesItems;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var emp = _userManager.GetUserAsync(User).Result;
                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = emp.Id;
                model.CreatedDate = model.CreatedDate;
                model.CreatedBy = model.CreatedBy;               

                var proEdit = _mapper.Map<ProductMaster>(model);

                var isSuccess = await _productRepository.Update(proEdit);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong..");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw ex;
                //ModelState.AddModelError("", "Something Went Wrong..");
                //return View(model);
            }
        }
        public async Task<ActionResult> Delete(int id)
        {
            var pro = await _productRepository.FindById(id);
            if (pro == null)
            {
                return NotFound();
            }
            var isSuccess = await _productRepository.Delete(pro);

            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Delete(int id, ProductViewModel model)
        {
            try
            {
                var pro = await _productRepository.FindById(id);
                if (pro == null)
                {
                    return NotFound();
                }
                var isSuccess = await _productRepository.Delete(pro);

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
