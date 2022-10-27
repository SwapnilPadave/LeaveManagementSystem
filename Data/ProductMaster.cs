using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Data
{
    public class ProductMaster
    {
        [Key]
        public int ProId { get; set; }        
        public int CatId { get; set; }        
        public string ProName { get; set; }
        public Decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Quantity { get; set; }
    }
}
