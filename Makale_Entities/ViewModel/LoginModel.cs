using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities.ViewModel
{
    public class LoginModel
    {
        [DisplayName ("Kullanıcı Adı"),Required(ErrorMessage ="{0} Alanı Boş Geçilemez ")]
        public string KullaniciAdi { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez ")]
        public string Sifre { get; set; }
    }
}
