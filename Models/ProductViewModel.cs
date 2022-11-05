using LeaveManagementSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Product Id")]
        public int ProId { get; set; }
        [Display(Name = "Category Id")]
        public int CatId { get; set; }
        public IEnumerable<SelectListItem> CatTypes { get; set; }
        [Display(Name = "Category Name")]
        public string CatName { get; set; }
        public CategoryViewModel categoryByName { get; set; }
        [Display(Name = "Product Name")]
        public string ProName { get; set; }
        public Decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public EmployeeViewModel CreatedById { get; set; }
        public string ModifiedBy { get; set; }
        public EmployeeViewModel ModifiedById { get; set; }
        public int Quantity { get; set; }
    }

    public class EmployeeProductViewModel
    {
        
        [Display(Name = "Product Id")]
        public int ProId { get; set; }
        [Display(Name = "Product Name")]
        public string ProName { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set; }
        public CategoryViewModel categoryByName { get; set; }        
        public Decimal Price { get; set; }               
        public int Quantity { get; set; }
    }
    public class ProductCreateViewModel
    {
        [Display(Name = "Product Id")]
        public int ProId { get; set; }
        [Display(Name = "Product Name")]
        public string ProName { get; set; }        
        public int CatId { get; set; }
        [Display(Name = "Category Id")]
        public IEnumerable<SelectListItem> CatTypes { get; set; }
        public Decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public EmployeeViewModel CreatedById { get; set; }
        public string ModifiedBy { get; set; }
        public EmployeeViewModel ModifiedById { get; set; }
        public int Quantity { get; set; }
    }
}
