using MakaleWEB_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MakaleWEB_MVC.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter//Login olmuş mu olmamış mı ve aynı zamanda da amin mi değil mi onu kontrol ediyoruz.
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionUser.Login != null && SessionUser.Login.Admin==false)
            {
                filterContext.Result =new  RedirectResult("/Home/YetkisizErisim");
            }
        }
    }
}