using Makale_BLL;
using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakaleWEB_MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Test test = new Test();
            //test.EkleTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.YorumTest();

            MakaleYonet my=new MakaleYonet();
            
            return View(my.Listele());
        }
        public PartialViewResult kategoriPartial()//bu kullanılmayacak ikinci bir alternatif böyle de 
        {
            KategoriYönet ky=new KategoriYönet();
            List<Kategori> liste = ky.Listele();
            return PartialView("_PartialPagekat2",liste);
        }
    }
}