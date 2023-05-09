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
        MakaleBLL_Sonuc<Makale> sonuc = new MakaleBLL_Sonuc<Makale>();
        public List<Makale> Listele()
        {
           return rep_makale.Liste();
        }
        public Makale MakaleBul(int id)
        {
            return rep_makale.Find(x => x.Id == id);
        }

        public MakaleBLL_Sonuc<Makale> MakaleEkle(Makale makale)
        {
            sonuc.nesne = rep_makale.Find(x => x.Baslik == makale.Baslik && x.Kategori.Id == makale.Kategori.Id);
            if (sonuc.nesne != null)
            {
                sonuc.hatalar.Add("Bu makale kayıtlı");
            }
            else
            {
                if (rep_makale.Insert(makale) < 1)
                {
                    sonuc.hatalar.Add("Makaleler Eklenemdi!");
                }
            }
            return sonuc;

        }

        public void MakaleSil(int id)
        {
            throw new NotImplementedException();
        }

        public void MakaleUpdate(Makale makale)
        {
            throw new NotImplementedException();
        }
    }
}
