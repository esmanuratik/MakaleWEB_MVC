using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakaleWEB_MVC.Models;

namespace MakaleWEB_MVC.Filter
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    //Authorization olmuş mu onun kontrolünü yapacağız.Login oldu mu olmadı mı//bu classı attirubute olarak kullanmam için filter attribute oldunu söylemlıyım ve ınteface implement etmeliyim.
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //Sessiona bakıyorum Login olundu mu olunmadı eğer olunmadıysa indexe atmalı
            if (SessionUser.Login == null)
            {
                filterContext.Result = new RedirectResult("/Home/Giris");
            }//hangi sayfalar login gerektiriyorsa ona auth classını vermeliyim

        }
    }
}