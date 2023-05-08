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
        MakaleBLL_Sonuc<Kategori> sonuc=new MakaleBLL_Sonuc<Kategori> ();
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
        public MakaleBLL_Sonuc<Kategori> KategoriEkle(Kategori model)
        {
            sonuc.nesne = rep_kat.Find(x =>x.Baslik==model.Baslik);
            if (sonuc.nesne != null)
            {
                sonuc.hatalar.Add("Bu kategori kayıtlıdır!!");
            }
            else 
            {
                if (rep_kat.Insert(model)<1) 
                {
                    sonuc.hatalar.Add("Kategori kaydedilmedi!!");   
                }
            }
            return sonuc;
        }
        public MakaleBLL_Sonuc<Kategori> KategoriUpdate(Kategori model)
        {

            sonuc.nesne = rep_kat.Find(x=>x.Id==model.Id);
            Kategori kategori = rep_kat.Find(x=>x.Baslik==model.Baslik && x.Id!=model.Id);
            
            if (sonuc.nesne != null&& kategori==null) 
            {
                sonuc.nesne.Baslik = model.Baslik;
                sonuc.nesne.Aciklama = model.Aciklama;
                if (rep_kat.Update(sonuc.nesne)<1)
                {
                    sonuc.hatalar.Add("Kategori Güncellenemedi");
                }
            }
            else 
            {
                if (kategori!=null) 
                {
                    sonuc.hatalar.Add("Bu kategori zaten kayıtlı");
                }
                else
                {
                    sonuc.hatalar.Add("Kategori Bulunamadı");
                }
               
            }
           
            return sonuc;
        }
        public MakaleBLL_Sonuc<Kategori> KategoriSil(int id)
        {
            Kategori kategori = rep_kat.Find(x => x.Id == id);
            Repository<Makale> rep_makale = new Repository<Makale>();
            Repository<Yorum> rep_yorum=new Repository<Yorum>();    
            Repository<Begeni> rep_begen=new Repository<Begeni>(); 
            //böyle siliyor olmamızın sebebi tabloların birbirine bağlı olması

            //1)KAtegorinin Makalelerini sil for ile silmeliyim 
            foreach(Makale item in kategori.Makaleler.ToList())
            {
                //2)Makalenin yorumlarını sil
                foreach(Yorum yorum in item.Yorumlar.ToList())
                {
                    rep_yorum.Delete(yorum);
                }

                //3)Makalenin beğenilerini sil
                foreach(Begeni begen in item.Begeniler.ToList())
                {
                    rep_begen.Delete(begen);  
                }
                rep_makale.Delete(item);
            }

            rep_kat.Delete(kategori);

            return sonuc;
        }

    }
}
