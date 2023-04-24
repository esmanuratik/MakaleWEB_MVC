using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_DAL
{
    public class DatabaseContext:DbContext //class yerine adonet class oluşrurup empty codefirst oluştursaydım miras almak zorunda klmazdım fakat aynı şey tek farkı class oluşturduğunda miras almalısın
    {
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Makale> Makaleler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Yorum>Yorumlar { get; set; }
        public DbSet<Begeni>Begeniler { get; set; } 


    }
}
