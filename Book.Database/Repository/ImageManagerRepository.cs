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
    public class ImageManagerRepository : Repository<ImageManager>, IImageManagerRepository
    {
        private ApplicationDbContext _db;
        public ImageManagerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ImageManager obj)
        {
            _db.ImageManagers.Update(obj);
        }
    }
}
