﻿using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Database.Repository.IRepository
{
    public interface IImageManagerRepository : IRepository<ImageManager>
    {
        void Update(ImageManager obj);
    }
}
