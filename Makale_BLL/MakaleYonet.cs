using MakaleDAL;
using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class MakaleYonet
    {
        Repository<Makale>rep_makale = new Repository<Makale>();
        public List<Makale> Listele()
        {
           return rep_makale.Liste();
        }
    }
}
