using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Data
{
    public class ProductMaster
    {
        [Key]
        public int ProId { get; set; }        
        public int CatId { get; set; }
        [ForeignKey("CatId")]
        public CategoryMaster CatType { get; set; }
        public string ProName { get; set; }
        public Decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public Employee CreatedById { get; set; }
        public string ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public Employee ModifiedById { get; set; }
        public int Quantity { get; set; }
    }
}
