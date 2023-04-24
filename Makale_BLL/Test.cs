using Makale_DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class Test
    {
        public Test() //test classında iş yaptırmak için ctor yaptık öteki türlü test. vs diyerek metot çağırmak             gerekirdi
        { 
            DatabaseContext db= new DatabaseContext();
            //db.Kullanicilar.ToList();
            db.Database.CreateIfNotExists();//database ilk kez create edilirken kullanabilirsin fakat bu database                                varken oluşmaz  


        }
    }
}
