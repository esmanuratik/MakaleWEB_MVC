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

namespace MakaleWEB_MVC.Controllers
{
    public class MakaleController : Controller
    {
       MakaleYonet my=new MakaleYonet();
        KategoriYönet ky= new KategoriYönet();
        public ActionResult Index()
        {
            Kullanici kullanici = Session["login"] as Kullanici ;   //sessiondan bilgi çekiyorum
            return View(my.Listele().Where(x=>x.Kullanici.Id==kullanici.Id));  //select*from gibi bütün hepsi gelmesin diye koşul yazdık
        }

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

        public ActionResult Create()
        {
            ViewBag.Kategori = new SelectList(ky.Listele(),"Id","Baslik");//dropdownlisti doldurmak için
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ModelState.Remove("Kategori.Aciklama");

            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik", makale.Kategori.Id);//dropdownlisti doldurmak için
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
                my.MakaleUpdate(makale);
                return RedirectToAction("Index");
            }
            
            return View(makale);
        }

        // GET: Makale/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            my.MakaleSil(id);
            return RedirectToAction("Index");
        }       
    }
}
