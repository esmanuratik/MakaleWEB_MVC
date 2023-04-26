using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_DAL
{
    public class VeriTabaniOluşturucu:CreateDatabaseIfNotExists<DatabaseContext>
        //bu database yoksa create et amaç seed metotunu kullanarak fake data atmak.
        //database oluşturulduğunda içine data atmak istiyorsak hep seed methodu kullanılır.
    {
        protected override void Seed(DatabaseContext context)//database create edildikten sonra seed metotu çalışacak
        {
            Kullanici admin = new Kullanici()//bir tane admin girişim olsun öteki kullanıcılar fakedata dan çekilecek
            {
                Adi = "Esmanur",
                 Soyad = "Atik",
                 Email = "esmanur@gmail.com",
                 Admin=true,
                 Aktif=true,
                 KullaniciAdi="esmanur",
                 Sifre="12345",
                 AktifGuid=Guid.NewGuid(),
                 KayitTarihi=DateTime.Now,
                 DegistirmeTarihi=DateTime.Now.AddMinutes(5),
                 DegistirenKullanici="esmanur"

            };
            context.Kullanicilar.Add(admin);//database e kayıt ekleme işlemi gerçekleşti

            for (int i = 1; i < 6; i++)//içeriye 5 tane kullanıcı atıyorum.
            {
                Kullanici k = new Kullanici() 
                {
                    Adi=FakeData.NameData.GetFirstName(),
                    Soyad=FakeData.NameData.GetSurname(),
                    Admin=false,
                    Aktif=true,
                    Email=FakeData.NetworkData.GetEmail(),
                    AktifGuid = Guid.NewGuid(),
                    KullaniciAdi=$"user{i}",//"user"+i.Tostring() şeklinde yazdığımızla aynı.bu string format yazımı
                    Sifre="123",
                    KayitTarihi = DateTime.Now,
                    DegistirmeTarihi = DateTime.Now.AddDays(-1),//1 gün öncesi 
                    DegistirenKullanici= $"user{i}"

                };

                context.Kullanicilar.Add(k);
            }
            context.SaveChanges();

            //kategori ekle kategori ekledikten sonra bir tane daha for açılacak her kategoride makale ekleyeceğiz makale içine de yorum ekleyeceğiz iç içe 3 tane for yazmış oluruz.

            //select * from kullanicilar gibi kullaniciları çekmek istiyorum.K
            List<Kullanici> kullanicilar = context.Kullanicilar.ToList();//makaleyi yazan kullanıcı belli değil onun için buraya aldım.

            //kategori ekle
            for (int i = 1; i < 6; i++)
            {
                Kategori kat = new Kategori()
                {
                    Baslik=FakeData.PlaceData.GetStreetName(),
                    Aciklama=FakeData.PlaceData.GetAddress(),
                    KayitTarihi=DateTime.Now,
                    DegistirmeTarihi=DateTime.Now,
                    DegistirenKullanici=admin.KullaniciAdi
                    
                };
                context.Kategoriler.Add(kat);

                //makale ekle
                for (int j = 0; j < 6; j++) //ekrana 6 tane div yerleştirdiğim için 6 tane dedim.
                {
                    Kullanici kullanici=kullanicilar[FakeData.NumberData.GetNumber(0, 5)];//degistiren kullanıcıya da aynı kişiyi atayım diye böyle yaptım.
                    Makale makale = new Makale()
                    {
                        Baslik = FakeData.TextData.GetAlphabetical(5),//5 tane harf getirir
                        Icerik = FakeData.TextData.GetSentences(2),
                        Taslak = false,
                        BegeniSayisi = FakeData.NumberData.GetNumber(2, 6),
                        KayitTarihi = DateTime.Now.AddDays(-2),
                        DegistirmeTarihi = DateTime.Now,
                        DegistirenKullanici = admin.KullaniciAdi,
                        Kullanici = kullanici//makaleyi yazan kulllanıcı belli olsun diye 

                    };

                    //hangi kategıride old belli değil bunu eklemek için bir kategori içinde makale ekliyoruz.

                    kat.Makaleler.Add(makale);//bir kategoriye 5 makale eklenmiş olacak.bu şekilde hata verir nesne tanımlanmassı gerekiyor.kategori classda, ctor da new ile örneklenmeli.bunu bütün classlar için yapacağız.

                    //makale.Kategori = kat;
                    //context.Makaleler.Add(makale) de diyebilirsin  yine aynı şeye denk gelir.Örneklemek istemezsen

                    //yorum ekle
                    for (int z = 0; z < 3; z++)
                    {
                        Kullanici yorum_kullanici= kullanicilar[FakeData.NumberData.GetNumber(0, 5)];//rastgele bir kişi seçelim
                        Yorum yorum = new Yorum()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            KayitTarihi = DateTime.Now.AddDays(- 1),
                            DegistirmeTarihi = DateTime.Now,
                            DegistirenKullanici = yorum_kullanici.KullaniciAdi,
                            Kullanici= yorum_kullanici
                            //yorumun kullanıcısı belli değil onu yine değistiren kullancıya atmalıyım
                        };

                        makale.Yorumlar.Add(yorum);//yorumları da örneklemek gerekir ctor da

                        //yorum.Makale = makale;
                        //context.Yorumlar.Add(yorum);  ----- tek satırda yazıp örneklediğimiz kodu böyle de yapabiliriz. 
                        //Bu yorumun makalesi belli olmalı ,makalenin de kullancısı belli olmalı.
                    }//for yorum

                   

                    //makalenin beğenileri var
                    for (int x = 0; x <makale.BegeniSayisi ; x++)
                    {
                        Kullanici begenen_kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                        //makaleyi kimler begenmiş onun için bunu yapıyoruz

                        Begeni begeni = new Begeni()
                        {
                            Kullanici = begenen_kullanici

                        };
                        makale.Begeniler.Add(begeni);
                    }//for begeni

                }//for makale


            }//for kategori
            context.SaveChanges();

        }

    }
}
