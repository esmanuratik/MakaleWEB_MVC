using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities.ViewModel
{
    public class RegisterModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez "),StringLength(30)]
        public string KullaniciAdi { get; set; }
        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez "),StringLength(200),EmailAddress(ErrorMessage ="{0} İçin Geçerli Bir E-Posta Adresi Giriniz...")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez "),StringLength(20)]
        public string Sifre { get; set; }
        [DisplayName("Şifre(Tekrar)"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez "), StringLength(20),Compare(nameof(Sifre),ErrorMessage ="{0} ile {1} uyuşmuyor...")]
        public string Sifre2 { get; set; }

    }
}
