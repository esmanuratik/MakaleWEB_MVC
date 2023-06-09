﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale_Entities;
using Makale_BLL;
using MakaleWEB_MVC.Filter;

namespace MakaleWEB_MVC.Controllers
{
    [Auth]
    [AuthAdmin]
    [ExcFilter]//kedni yazdığım hata sayfası için oluşturduğum filter
    public class KullaniciController : Controller
    {
       KullaniciYonet ky=new KullaniciYonet();
        MakaleBLL_Sonuc<Kullanici> sonuc= new MakaleBLL_Sonuc<Kullanici> ();
       
        public ActionResult Index()
        {
            return View(ky.KullaniciListesi());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = ky.KullaniciBul(id.Value);
            if (sonuc.nesne== null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                ky.KullaniciKaydet(kullanici);
                return RedirectToAction("Index");
            }

            return View(kullanici);
        }

        // GET: Kullanici/Edit/5
        public ActionResult  Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = ky.KullaniciBul(id.Value);
            if (sonuc == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                sonuc = ky.KullaniciUpdate(kullanici);
                
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = ky.KullaniciBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // POST: Kullanici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sonuc = ky.KullaniciSil(id);
            return RedirectToAction("Index");
        }
    }
}
