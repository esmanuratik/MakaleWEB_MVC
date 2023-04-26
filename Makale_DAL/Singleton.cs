using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_DAL
{
    public class Singleton //bu işlemi repositoryde de yapabilirdik design pattern dikkat çeksin belli olsun diye burada yazdık 
    {
        public static DatabaseContext db;
        public Singleton() 
        {
            if (db == null)
            {
                db = new DatabaseContext();
            }
        }
    }
}
