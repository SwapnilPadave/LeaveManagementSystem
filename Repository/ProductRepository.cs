using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(ProductMaster entity)
        {
            await _db.Products.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(ProductMaster entity)
        {
            _db.Products.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<ProductMaster>> FindAll()
        {
            var product = await _db.Products.ToListAsync();
            return product;
        }

        public async Task<ProductMaster> FindById(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.ProId == id);
            return product;
        }

        public async Task<bool> IsExists(int id)
        {
            var exists = await _db.Products.AnyAsync(x => x.ProId == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(ProductMaster entity)
        {
            _db.Products.Update(entity);
            return await Save();
        }
    }
}
