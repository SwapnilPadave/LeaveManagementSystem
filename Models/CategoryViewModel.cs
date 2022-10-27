using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class CategoryViewModel
    {
        [Display(Name = "Category Id")]
        public int CatId { get; set; }
        [Display(Name = "Category Name")]
        public string CatName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public EmployeeViewModel CreatedById { get; set; }
        public bool IsActive { get; set; }
    }    
}
