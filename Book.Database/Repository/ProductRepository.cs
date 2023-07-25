using Book.Database.Repository.IRepository;
using Book.Models;
using BookSite.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.Database.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.ProductId == obj.ProductId);
            // Manual mapping
            if (objFromDb != null)
            {
                obj.Title = objFromDb.Title;
                obj.Description = objFromDb.Description;
                obj.ISBN = objFromDb.ISBN;
                obj.Price = objFromDb.Price;
                obj.ListPrice = objFromDb.ListPrice;
                obj.Price50 = objFromDb.Price50;
                obj.Price100 = objFromDb.Price100;
                obj.CategoryId = objFromDb.CategoryId;
                obj.Author = objFromDb.Author;
                if(obj.ImageUrl != null)
                {
                    obj.ImageUrl = objFromDb.ImageUrl;
                }
            }
        }
    }
}
