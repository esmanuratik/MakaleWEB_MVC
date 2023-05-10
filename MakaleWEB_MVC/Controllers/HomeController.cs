using Makale_BLL;
using Makale_Common;
using Makale_Entities;
using Makale_Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleWEB_MVC.Models;

namespace MakaleWEB_MVC.Controllers
{
    public class HomeController : Controller
    {

        MakaleYonet my = new MakaleYonet();
        KategoriYönet ky = new KategoriYönet();
        KullaniciYonet kulky = new KullaniciYonet();

        public ActionResult Index()
        {
            //Test test = new Test();
            //test.EkleTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.YorumTest();

            
            return View(my.Listele());
        }
        public ActionResult Begendiklerim()
        {
            var liste=my.Listele();
            return View("Index",liste);
        }
        public ActionResult Kategori(int? id)//kategorilere bastığımda o kategoriye gitsin makaleyi getirsin.
        { //App_Start---routeconfing.cs da id old için burada id kullanıldı url ler buna göre oluşuyor.
            if (id==null)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kat = ky.KategoriBul(id.Value);
            
            return View("Index",kat.Makaleler);
        }

        public PartialViewResult kategoriPartial()//bu kullanılmayacak ikinci bir alternatif böyle de 
        {
           
            List<Kategori> liste = ky.Listele();
            return PartialView("_PartialPagekat2",liste);
        }
        public ActionResult EnBegenilenler()
        {
            return View("Index",my.Listele().OrderByDescending(x=>x.BegeniSayisi).ToList()); //beğeni sayısını sırala 
        }
        public ActionResult Hakkımızda()
        {
            return View();
        }
        public ActionResult SonYazilanlar()
        {
            return View("Index", my.Listele().OrderByDescending(x => x.DegistirmeTarihi).ToList()); //değiştirme sırasına göre sıralasın
        }
        [HttpGet]
        public ActionResult Giris()
        {          
            return View();
        }
        [HttpPost]
        public ActionResult Giris(LoginModel model) //kullancı adı ve şifre bulunuyor burada ve biz bunu modelle taşıdık onun için buraya onu verecceğim
        {
            if (ModelState.IsValid)
            {
                MakaleBLL_Sonuc<Kullanici> sonuc = kulky.LoginKontrol(model);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                //ındexe atmadan önce login oldum sessionda bu bilgiyi saklamalıyım.
                //Session["login"] =sonuc.nesne;//bulduğu kullanıcıyı atmış oldum.
                SessionUser.Login = sonuc.nesne;//session modelde oluşturuldu artık bu şelikde kullanılacak
                Uygulama.login = sonuc.nesne.KullaniciAdi;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult KayitOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KayitOl(RegisterModel model) 
        {
           
            //kayıt işlemi yapılacak
            //aktivasyon maili gönderilecek

            if (ModelState.IsValid)//model geçerliyse yukarıdaki işlmelere bak
            { 
                
                MakaleBLL_Sonuc<Kullanici> sonuc= kulky.KullaniciKaydet(model);
                //Kullancı adı ve email var mı kontrolü
              
                if (sonuc.hatalar.Count>0)
                {
                    //ModelState.AddModelError("","Bu Kullanıcı Adı ya da E-Mail Kayıtlı");
                    sonuc.hatalar.ForEach(x=> ModelState.AddModelError("",x));
                    return View(model);
                } 
                else 
                {
                    //database kaydet
                    return RedirectToAction("KayitBasarili");
                }
            }
            return View(model);
        }
        
        public ActionResult KayitBasarili()
        {
            return View();
        }
        public ActionResult HesapAktiflestir(Guid id) //routeconfigten dolayı guid id
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = kulky.ActivateUser(id); //aktivasyon sağlaması gerekiyor bunun için metot oluşturduk ActivateUser
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            } 

            return View();
        }
        public ActionResult Cikis()
        {
            Session.Clear();//bir tane session varsa bu kullanılabilir null atmakla aynı işlem.
            /*Session["login"] = null;   */      //cıkış yapınca session boşaltılması lazım.
            return RedirectToAction("Index");
        }
        public ActionResult Error()
        {
            List<string> errors = new List<string>();
            if (TempData["hatalar"]!=null)
            {
                ViewBag.hatalar = TempData["hatalar"];
            }
            else 
            {
                { ViewBag.hatalar = errors;}
            }
            //ViewBag.hatalar = TempData["hatalar"];
            return View();  
        }
        public ActionResult ProfilGoster()
        {
            //Kullanici kullanici= Session["login"] as Kullanici;
            //belki databseden slindi düşüncesiyle ilk kullanıcıyı hulduruyoruz:
            MakaleBLL_Sonuc<Kullanici> sonuc = kulky.KullaniciBul(SessionUser.Login.Id);

            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            } 
      
          
             return View(sonuc.nesne);  

        }
        [HttpGet]
        public ActionResult ProfiDegistir()
        {
            //kullanıcı bilgilerini sessiondan alıyoruz
           // Kullanici kullanici = Session["login"] as Kullanici;

            MakaleBLL_Sonuc<Kullanici> sonuc = kulky.KullaniciBul(SessionUser.Login.Id);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }
            


            return View(sonuc.nesne);
        }

        [HttpPost]
        public ActionResult ProfiDegistir(Kullanici model,HttpPostedFileBase profilresim)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                //resimi dosya seçe yükleme işlemi
                if (profilresim != null && (profilresim.ContentType == "image/jpg" || profilresim.ContentType == "image/jpeg" || profilresim.ContentType == "image/png"))
                {
                    //dosya kayıt olmuş oldu.
                    string dosya = $"user_{model.Id}.{profilresim.ContentType.Split('/')[1]}";
                    profilresim.SaveAs(Server.MapPath($"~/Resimler/{dosya}"));

                    model.ProfilResimDosyaAdı = dosya;//modelin resim dosya adı değişti ve kullanıcı yönette databaseden buldu resim attı
                }

                Uygulama.login = model.KullaniciAdi;

                MakaleBLL_Sonuc<Kullanici> sonuc = kulky.KullaniciUpdate(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                SessionUser.Login= sonuc.nesne;
                return RedirectToAction("ProfilGoster");
               

            }
            else 
            {

                return View(model);
            }
                     
        }

        public ActionResult ProfilSil()//profil sil indexe yönlednirecek
        {
            //Kullanici kullanici= Session["login"] as Kullanici;
            MakaleBLL_Sonuc<Kullanici> sonuc = kulky.KullaniciSil(SessionUser.Login.Id);
           // kulky.KullaniciSil(SessionUser.Login.Id);
            //hata var mı yok mu sildi mi silmedi mi kul.yönetten geldik
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }
            Session.Clear();//bu kişi artık yok ve login değil bundan dolayı session temizlenecek.
            return RedirectToAction("Index");

            //kullancının makalesi varsa kullanıcyı silemezsin 
        }


    }
}