using MyShop.DataAccess.Data;
using MyShop.Entities;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implemention
{
    public class CatigoryRepository : GenericRepository<Catigory>, ICatigoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CatigoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(Catigory entity)
        {
            var entityForUpdate = _context.catigory.FirstOrDefault(c => c.Id == entity.Id);
            if (entityForUpdate != null)
            {
                entityForUpdate.Name = entity.Name;
                entityForUpdate.Description = entity.Description;
                entityForUpdate.DateCreate = entity.DateCreate;
            }
        }

    }
}
