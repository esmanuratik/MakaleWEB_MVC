using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    [Table("Kategori")]
    public class Kategori:BaseClass
    {
        [Required,StringLength(50),DisplayName("Başlık")]   
        public string Baslik { get; set; }
        [Required,StringLength(150), DisplayName("Açıklama")]
        public string Aciklama { get; set; }
        
        public virtual List<Makale> Makaleler { get; set; } //bir kategorinin birden fazla makalesi olabilir
       
        public Kategori()  //kullanıcı atmak için ctor oluşturup new ile örneklenmesi lazım yani bellekte bir yer                       ayırmış oldu.(veritabnıolusturucu.cs de makale eklediğimizde hata vermmesi için)
        {
            Makaleler = new List<Makale>();
        }

    }

}


