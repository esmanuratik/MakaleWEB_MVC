﻿using Makale_BLL;
using Makale_Entities;
using Makale_Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
                Session["login"] =sonuc.nesne;//bulduğu kullanıcıyı atmış oldum.
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
                kulky.KullaniciKaydet(model);
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
        public ActionResult Profil()
        {
            Kullanici kullanici= Session["login"] as Kullanici;
            //belki databseden slindi düşüncesiyle ilk kullanıcıyı hulduruyoruz:
            MakaleBLL_Sonuc<Kullanici> sonuc = kulky.KullaniciBul(kullanici.Id);

            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            } 
      
          
             return View();  

        }


    }
}