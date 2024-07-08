using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface ICatigoryRepository : IGenericRepository<Catigory>
    {
        public void Update(Catigory entity);
    }
}
