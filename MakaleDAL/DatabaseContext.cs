using Makale_Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Makale_DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Makale> Makaleler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Begeni> Begeniler { get; set; }

        public DatabaseContext()  //veritabamý oluþturucun tetiklenmesi ctor ile yaparýz
        {
            Database.SetInitializer(new VeriTabaniOluþturucu());
        }
    }
}