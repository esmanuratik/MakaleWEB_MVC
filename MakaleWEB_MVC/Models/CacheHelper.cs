using Makale_BLL;
using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MakaleWEB_MVC.Models
{
    public class CacheHelper
    {
        //cache helper kütüphanesi var onunla ekleyeceğiz.
        public static List<Kategori> KategoriCache()//örneklemeyelim diye static
        {
           var kategoriler= WebCache.Get("kat-cache"); //get ile cache okuyoruz set ile cache ediyoruz.kat-cache isminde bir cache oluşturmuş oluyoruz.
           
            if (kategoriler == null)
            {
                KategoriYönet ky=new KategoriYönet();
                kategoriler = ky.Listele();
                WebCache.Set("kat-cache",kategoriler,20,true);//20 dakikada bir öteler.
            }

            return kategoriler;

        }
        public static void CacheTemizle()
        {
            WebCache.Remove("kat-cache");
        }
    }
}