using Makale_BLL;
using Makale_Entities;
using MakaleWEB_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace MakaleWEB_MVC.Controllers
{
    public class YorumController : Controller
    {
        // GET: Yorum
        public ActionResult YorumGoster(int? id)//bu id ile commetscripte ki id nin makalesini ver diyoruz
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MakaleYonet my = new MakaleYonet();
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialYorumlar",makale.Yorumlar);
        }
        [HttpPost]
        public ActionResult YorumGuncelle(int? id,string text)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            YorumYonet yy = new YorumYonet();
            Yorum yorum = yy.YorumBul(id.Value);

            if (yorum == null)
            {
                return HttpNotFound();
            }

            yorum.Text = text;

           if(yy.YorumUpdate(yorum) >0)
            {
                //hayatyı json  formatında dönücem
                return Json(new { hata = false },JsonRequestBehavior.AllowGet);
            }

            return Json(new { hata = true },JsonRequestBehavior.AllowGet);

        }
        public ActionResult YorumSil(int?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            YorumYonet yy = new YorumYonet();
            Yorum yorum = yy.YorumBul(id.Value);

            if (yorum == null)
            {
                return HttpNotFound();
            }

            if (yy.YorumSil(yorum) > 0)
            {
                //hatayı json  formatında dönücem
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult YorumEkle(Yorum nesne, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MakaleYonet my = new MakaleYonet();
            Makale makale = my.MakaleBul(id.Value);

            if (makale == null)
            {
                return HttpNotFound();
            }

            nesne.Makale = makale;
            nesne.Kullanici = SessionUser.Login;

            YorumYonet yy = new YorumYonet();

            if (yy.YorumEkle(nesne) > 0)
            {
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);
        }


    }
}