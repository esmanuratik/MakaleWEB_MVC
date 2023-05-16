using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale_BLL;
using Makale_Entities;
using MakaleWEB_MVC.Filter;
using MakaleWEB_MVC.Models;

namespace MakaleWeb_MVC.Controllers
{
    [Auth]
    [AuthAdmin]
    [ExcFilter]//kedni yazdığım hata sayfası için oluşturduğum filter
    public class KategoriController : Controller
    {
        KategoriYönet ky = new KategoriYönet();
        
        public ActionResult Index()
        {
            return View(ky.Listele());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                MakaleBLL_Sonuc<Kategori> sonuc = ky.KategoriEkle(kategori);
                sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                return View(kategori);
            }

            CacheHelper.CacheTemizle();         
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                MakaleBLL_Sonuc<Kategori> sonuc = ky.KategoriEkle(kategori);
                sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                return View(kategori);
            }
            CacheHelper.CacheTemizle();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: Kategoris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ky.KategoriSil(id);
            CacheHelper.CacheTemizle();
            return RedirectToAction("Index");
        }


    }
}
