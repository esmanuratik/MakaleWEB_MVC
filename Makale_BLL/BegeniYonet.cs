using Makale_Entities;
using Makale_DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class BegeniYonet
    {
        Repository<Begeni> rep_begen=new Repository<Begeni>();
        public IQueryable<Begeni> ListQueryable()//beğeni tablosunu veriyor.
        {
            return rep_begen.ListQueryable();

        }
        public List<Begeni> Liste()//listquarable da where kullanımına izin vermediği için  bunu yazdık. 
        {
            return rep_begen.Liste();
        }
    }
}
