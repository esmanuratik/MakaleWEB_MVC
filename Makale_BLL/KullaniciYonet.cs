using Makale_Common;
using MakaleDAL;
using Makale_Entities;
using Makale_Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace Makale_BLL
{
    public class KullaniciYonet
    {
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();

        public MakaleBLL_Sonuc<Kullanici> ActivateUser(Guid id)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
            sonuc.nesne = rep_kul.Find(x => x.AktifGuid == id);
            if (sonuc.nesne != null)
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
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();

            sonuc.nesne = rep_kul.Find(x => x.Id == id);
            if (sonuc.nesne == null)
            {
                sonuc.hatalar.Add("Kullanıcı Bulunamadı");
            }
            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciKaydet(RegisterModel model)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();



            //kullanici return edildi home controlle da if koşulu yazılmalı

            Kullanici nesne = new Kullanici();
            nesne.Email = model.Email;
            nesne.KullaniciAdi = model.KullaniciAdi;

            sonuc = KullaniciKontrol(nesne);
            if (sonuc.hatalar.Count > 0)
            {
                sonuc.nesne = nesne;
                return sonuc;
            }           
            else
            {
                int islemsonuc = rep_kul.Insert(new Kullanici()
                {
                    //her yere bunu yazmam gerekicek update ınsert vs büyün makale kategori vs için bu gerekiyor o halde repositoryde yazalım.
                    KullaniciAdi = model.KullaniciAdi,
                    Email = model.Email,
                    Sifre = model.Sifre,
                    AktifGuid = Guid.NewGuid(),
                    ProfilResimDosyaAdı="user1.jpg"


                    //KayitTarihi=DateTime.Now,
                    //DegistirmeTarihi=DateTime.Now,
                    //DegistirenKullanici="system"

                });

                //kullancı var ise sonucu öyle döndürsün 
                if (islemsonuc > 0)
                {
                    sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Email == model.Email);

                    //aktivasyon maili gönderilmesi

                    string siteUrl = ConfigHelper.Get<string>("SiteRootUri");

                    string aktivateUrl = $"{siteUrl}/Home/HesapAktiflestir/{sonuc.nesne.AktifGuid}";

                    string body = $"Merhaba{sonuc.nesne.Adi}{sonuc.nesne.Soyad}<br/> Hesabınıız aktifleştirmek için  <a href='{aktivateUrl}' target='_blank'> tıklayınız </a>";
                    MailHelper.SendMail(body, sonuc.nesne.Email, "Hesap Aktifleştirme");

                }
                return sonuc;
            }
            
        }
        public void KullaniciKaydet(Kullanici kullanici)
        {
            //yazılacak
        }


        MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
        //profil resmi için oluşturulan metot
        public MakaleBLL_Sonuc<Kullanici> KullaniciUpdate(Kullanici model)
        {

            //kullanıcıyı bul ve update et fakat aynı isimde olmaması lazım bunları kontrol edip izin vermemeliyiz.

            sonuc = KullaniciKontrol(model);//bu metot bana bir sonuc dönücek hata varsa hatayı return edicek yoksa update edicek


            if (sonuc.hatalar.Count > 0)
            {
                sonuc.nesne = model;
                return sonuc;
            }
            else
            {
                sonuc.nesne = rep_kul.Find(x => x.Id == model.Id);

                sonuc.nesne.Adi = model.Adi;
                sonuc.nesne.Soyad = model.Soyad;
                sonuc.nesne.Email = model.Email;
                sonuc.nesne.KullaniciAdi = model.KullaniciAdi;
                sonuc.nesne.Sifre = model.Sifre;
                sonuc.nesne.ProfilResimDosyaAdı = model.ProfilResimDosyaAdı;


                //kullanıcı update oldu  mu olmadı mı bakmak için(databasede sıkıntı var mı yok mu diye)
                if (rep_kul.Update(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("Profil bilgileri güncellenemedi");
                }
                return sonuc;

            }


        }
        //bu metot bana bir sonuc dönücek hata varsa hatayı return edicek yoksa update edicek yeni bilgileri
        public MakaleBLL_Sonuc<Kullanici> KullaniciKontrol(Kullanici kullanici)
        {
            Kullanici k1 = rep_kul.Find(x => x.Email == kullanici.Email);
            Kullanici k2 = rep_kul.Find(x => x.KullaniciAdi == kullanici.KullaniciAdi);

            if (k1 != null && k1.Id != kullanici.Id)
            {
                sonuc.hatalar.Add("Bu e-mail adresi kayıtlı");
            }
            if (k2 != null && k2.Id != kullanici.Id)
            {
                sonuc.hatalar.Add("Bu kullanıcı adı kayıtlı");
            }
            return sonuc;
        }

       public MakaleBLL_Sonuc<Kullanici> LoginKontrol(LoginModel model)
            {
                MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
                sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre);

                if (sonuc.nesne == null)
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

        public MakaleBLL_Sonuc<Kullanici> KullaniciSil(int id)
        {
            Kullanici kullanici = rep_kul.Find(x => x.Id == id);
            if (kullanici != null)
            {
                if (rep_kul.Delete(kullanici)<1)//delete update işlemleri1ei gerçeklşirse 1 dönder gerçekleşmezse 0 yua da eksi döner.
                {
                    sonuc.hatalar.Add("Kullancı silinemedi");
                }
            }
            else 
            {
                sonuc.hatalar.Add("Kullanıcı bulunamadı");
            }

            return sonuc;
        }
        public List<Kullanici> KullaniciListesi()
        {
            return rep_kul.Liste();
        }


    }
    }
