using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakaleWEB_MVC.Models
{
    public static class SessionUser
    {
        public static Kullanici Login
        {
            //sessiondan değer okuyacağım
            get
            {
                if (HttpContext.Current.Session["login"] != null)
                {
                    return HttpContext.Current.Session["login"] as Kullanici;
                }
                return null;
            }
            //sessiona değer atıcam
            set
            {
                HttpContext.Current.Session["login"] = value;
            }
        }
    }
}