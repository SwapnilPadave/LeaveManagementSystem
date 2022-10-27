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

        public ActionResult Create()
        {
            return View();
        }        
    }
}
