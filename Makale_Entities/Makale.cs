using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    [Table("Makale")]
    public class Makale:BaseClass
    {
        [Required,StringLength(50)]
        public string Baslik { get; set; }
        [Required, StringLength(1000)]
        public string Icerik { get; set; }
        public bool Taslak { get; set; } //yazdığım yazıyı taslak olarak saklayabilirim kullanıcı tarafından                                    görülmez bool yapıyoruz taslak=true ise gözükmesin false ise gözüksün
        public int BegeniSayisi { get; set; } 
        public virtual List<Yorum> Yorumlar { get; set; }    //bir makalenin n tane yorumu olabilir.
        public virtual Kullanici Kullanici { get; set; } //bir makalenin kullanıcısı var.
        public virtual Kategori Kategori { get; set; }   //bir makalenin kategorisi var
        public virtual List<Begeni> Begeniler { get; set; }

    }

}



