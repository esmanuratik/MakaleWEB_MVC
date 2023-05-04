using MakaleDAL;
using Makale_Entities;
using Makale_Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class KategoriYönet
    {
        Repository<Kategori> rep_kat = new Repository<Kategori>();
        public List<Kategori> Listele()
        {
           return rep_kat.Liste();
        }
        public Kategori KategoriBul(int id)
        {
            return rep_kat.Find(x => x.Id==id);
        }

        public void KullaniciBul(RegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
