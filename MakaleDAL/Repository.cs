﻿using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Makale_DAL
{
    public class Repository<T> : Singleton,IRepository<T> where T: class //T tipin class olduğunun koşulunu verdik T tip için başka birşey gönderemem çünkü dbset class dışında int vs kabul etmez.

    {      
        private DbSet<T>dbset;//dbSet e tekrar tekrar uzun yazarak ulaşmak yerine değişken tanımladım
        public Repository()
        {
            dbset=db.Set<T>();
        }
        public List<T> Liste()
        {
            /*return db.Set<T>().ToList();*///DatabaseContext deki Db<Set> ler gibi yazdık ve erişmeye çalıştık
            return dbset.ToList();
           
        }

        public List<T> Liste(Expression<Func<T, bool>> kosul)
        {
            return dbset.Where(kosul).ToList();
        }
        public int Insert(T nesne)
        {
            dbset.Add(nesne);
            //her class baseclass dan kalıtılmadığı için kontrolü yapılmalı yoksa hata alır

            if (nesne is BaseClass)
            {
                BaseClass obj = nesne as BaseClass; //nesne baseclass dan türetildiği için buna alındı.
                DateTime tarih=DateTime.Now;

                obj.KayitTarihi = tarih;
                obj.DegistirmeTarihi = tarih;
                obj.DegistirenKullanici = "system";
            } 

        

            return db.SaveChanges();
        }
        public int Delete(T nesne)
        {
            dbset.Remove(nesne);
            return db.SaveChanges();
        }

        public int Update(T nesne)
        {
            //her class baseclass dan kalıtılmadığı için kontrolü yapılmalı yoksa hata alır

            if (nesne is BaseClass)
            {
                BaseClass obj = nesne as BaseClass; //nesne baseclass dan türetildiği için buna alındı.
                
                obj.DegistirmeTarihi = DateTime.Now;
                obj.DegistirenKullanici = "system";
            }

            return db.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> kosul)
        {
           return dbset.FirstOrDefault(kosul);
        }

        public IQueryable<Begeni> ListQueryable()
        {
            return (IQueryable<Begeni>)dbset.AsQueryable();
        }
    }
}
