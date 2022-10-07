using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 15, ErrorMessage ="Please Enter Valid Number..")]
        [Display(Name ="Number Of Days")]
        public int DefaultDays { get; set; }

        [Display(Name ="Created Date")]
        public DateTime? DateCreated { get; set; }
    }
   
}
