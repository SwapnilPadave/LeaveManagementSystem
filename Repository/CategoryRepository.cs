using LeaveManagementSystem.Contracts;
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(CategoryMaster entity)
        {
            await _db.Categories.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(CategoryMaster entity)
        {
            _db.Categories.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<CategoryMaster>> FindAll()
        {
            var category = await _db.Categories.ToListAsync();
            return category;
        }

        public async Task<CategoryMaster> FindById(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CatId == id);
            return category;
        }

        public async Task<bool> IsExists(int id)
        {
            var exists = await _db.Categories.AnyAsync(x => x.CatId == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(CategoryMaster entity)
        {
            _db.Categories.Update(entity);
            return await Save();
        }
    }
}
