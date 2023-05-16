using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale_Entities;
using Makale_BLL;
using MakaleWEB_MVC.Models;
using System.Security.Cryptography;
using System.Web.Razor.Text;
using MakaleWEB_MVC.Filter;

namespace MakaleWEB_MVC.Controllers
{

    [ExcFilter]//kedni yazdığım hata sayfası için oluşturduğum filter
    public class MakaleController : Controller
    {
       MakaleYonet my=new MakaleYonet();
        KategoriYönet ky= new KategoriYönet();
        [Auth]
        public ActionResult Index()
        {
            if (SessionUser.Login!=null)
            {
                return View(my.Listele().Where(x => x.Kullanici.Id == SessionUser.Login.Id));
            }

            //Kullanici kullanici = Session["login"] as Kullanici ;   //sessiondan bilgi çekiyorum
            return View(my.Listele());  //select*from gibi bütün hepsi gelmesin diye koşul yazdık
        }
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale =my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }
        [Auth]
        public ActionResult Create()
        {
            ViewBag.Kategori = new SelectList(ky.Listele(),"Id","Baslik");//dropdownlisti doldurmak için
            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ModelState.Remove("Kategori.Aciklama");

            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", makale.Kategori.Id);//dropdownlisti doldurmak için
                                                    //makale.Kategori.Id sayfa post olduğunda boş gelmesin diye yazdık yoksa 

            if (ModelState.IsValid)
            {
               makale.Kullanici = SessionUser.Login;//makaleyi yazan kişi belli olmalı.
                makale.Kategori = ky.KategoriBul(makale.Kategori.Id);

               MakaleBLL_Sonuc<Makale> sonuc= my.MakaleEkle(makale);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            //session boşaldığı için tekrar dolması gerekir.
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik",makale.Kategori.Id);//dropdownlisti doldurmak için
            return View(makale);                                          //makale.Kategori.Id sayfa post olduğunda boş gelmesin diye yazdık yoksa dropdown sayfa post olduğunda tekrara boş gelecek ve seçmemi isteyebilir.
        }

        // GET: Makale/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            Makale makale = my.MakaleBul(id.Value);
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik", makale.Kategori.Id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            if (makale == null)
            {
                return HttpNotFound();
            }
           
            return View(makale);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Makale makale)
        {
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik", makale.Kategori.Id);//hiçbirşeye takılmada viewbag leri doldursun diye üste aldık.
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ModelState.Remove("Kategori.Aciklama");
           
            if (ModelState.IsValid)
            {
                makale.Kategori = ky.KategoriBul(makale.Kategori.Id);//update etmeden önce kategorinin değiştiğini belirtiyroum
                
                MakaleBLL_Sonuc<Makale> sonuc= my.MakaleUpdate(makale);
                
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            
            return View(makale);
        }

        // GET: Makale/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: Makale/Delete/5
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            my.MakaleSil(id);
            return RedirectToAction("Index");
        }
        BegeniYonet by = new BegeniYonet();

        [Auth]
        [HttpPost]
        public ActionResult MakaleGetir(int[] mid)
        {
           

            List<int> mliste = null;

            if (SessionUser.Login!=null && mid!=null) 
            {
                mliste = by.Liste().Where(x => x.Kullanici.Id == SessionUser.Login.Id && mid.Contains(x.Makale.Id)).Select(x => x.Makale.Id).ToList();
               
            }
            return Json(new { liste = mliste });

        }

        [Auth]
        [HttpPost]  
        public ActionResult MakaleBegen(int makaleid,bool begeni)
        {
            //daha önce begendim mi diye kontrol etmeliyim
           Begeni like= by.BegeniBul(makaleid, SessionUser.Login.Id);//buradan bana bir tane like dönecek ama dönmeyedebilir.Koşulla kontrol ediyoruz
            Makale makale = my.MakaleBul(makaleid);
            int sonuc = 0;
            if (like!=null&& begeni==false)
            {
                //begeniyi burada siliyoruz
                sonuc=by.BegeniSil(like);
            }
            else if (like==null&&begeni==true)//database de değer yok ilk kez beğenicem ya da begeniyi geri alıcam
            {
                sonuc = by.BegeniEkle(new Begeni()
                {
                    Kullanici = SessionUser.Login,
                    Makale = makale

                });
                               
            }
            if (sonuc>0)
            {
                //sonuc>0 ise ya ekledi ya sildi
                if (begeni) 
                {
                    makale.BegeniSayisi++;
                }
                else
                {
                    makale.BegeniSayisi--;
                }

                my.MakaleUpdate(makale);

                return Json(new { hata = false,begenisayisi=makale.BegeniSayisi });
            }
            else 
            {
                return Json(new {hata=true, begenisayisi = makale.BegeniSayisi });
            }
        }
        
        public ActionResult MakaleGoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialPageMakaleGoster", makale);
        }
       
        
    }
}
