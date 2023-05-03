using Makale_Common;
using Makale_DAL;
using Makale_Entities;
using Makale_Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class KullaniciYonet
    {
        Repository<Kullanici>rep_kul=new Repository<Kullanici>();

        public MakaleBLL_Sonuc<Kullanici> ActivateUser(Guid id)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
            sonuc.nesne=rep_kul.Find(x => x.AktifGuid == id);
            if (sonuc.nesne!=null)
            {
                if (sonuc.nesne.Aktif)
                {
                    sonuc.hatalar.Add("Kullanıcı zaten daha önce aktif edilmişitir");
                }
                else 
                {
                    sonuc.nesne.Aktif = true;
                    rep_kul.Update(sonuc.nesne); 
                } 
            }
            else 
            {
                sonuc.hatalar.Add("Aktifleştirilecek kullanıcı bulunmadı.");
            }
            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciBul(int id)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc= new MakaleBLL_Sonuc<Kullanici>();

            sonuc.nesne = rep_kul.Find(x => x.Id == id);
            if (sonuc.nesne == null)
            {
                sonuc.hatalar.Add("Kullanıcı Bulunamadı");
            }
            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciKaydet(RegisterModel model)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc=new MakaleBLL_Sonuc<Kullanici>();

            //kullanici return edildi home controlle da if koşulu yazılmalı

            sonuc.nesne=rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email);

            if (sonuc.nesne != null)
            {
                if (sonuc.nesne.KullaniciAdi==model.KullaniciAdi)
                {
                    sonuc.hatalar.Add("Bu Kullanıcı Adı Sistemde Kayıtlı");
                }
                if (sonuc.nesne.Email==model.Email)
                {
                    sonuc.hatalar.Add("Bu E-Mail sistemde kayıtlı");
                }
                
            }
            else 
            {
               int islemsonuc= rep_kul.Insert(new Kullanici()
                    {
                   //her yere bunu yazmam gerekicek update ınsert vs büyün makale kategori vs için bu gerekiyor o halde repositoryde yazalım.
                       KullaniciAdi = model.KullaniciAdi,
                       Email = model.Email,
                       Sifre = model.Sifre,
                       AktifGuid=Guid.NewGuid()


                       //KayitTarihi=DateTime.Now,
                       //DegistirmeTarihi=DateTime.Now,
                       //DegistirenKullanici="system"

                    });

                //kullancı var ise sonucu öyle döndürsün 
                if (islemsonuc>0)
                {
                    sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email);

                    //aktivasyon maili gönderilmesi

                    string siteUrl=ConfigHelper.Get<string>("SiteRootUri");

                    string aktivateUrl = $"{siteUrl}/Home/HesapAktiflestir/{sonuc.nesne.AktifGuid}";

                    string body = $"Merhaba{sonuc.nesne.Adi}{sonuc.nesne.Soyad}<br/> Hesabınıız aktifleştirmek için  <a href='{aktivateUrl}' target='_blank'> tıklayınız </a>";
                    MailHelper.SendMail(body, sonuc.nesne.Email, "Hesap Aktifleştirme");

                } 
            }
            return sonuc;
        }
        public MakaleBLL_Sonuc<Kullanici> LoginKontrol(LoginModel model)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
           sonuc.nesne= rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre);

            if (sonuc.nesne==null)
            {
                sonuc.hatalar.Add("Kullanıcı adı ya da şifre hatalı!!!");
            } 
            else
            {
                if (!sonuc.nesne.Aktif)
                {
                    sonuc.hatalar.Add("Kullanıcı aktifleştirilmemiştir.Lütfen e-posta adresinizi kontrol ediniz.");
                } 
                
            }

            return sonuc;
        }

    }
}
