using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    [Table("Yorum")]
    public class Yorum:BaseClass
    {
        [Required, StringLength(500)]
        public string Text { get; set; }
        public virtual Makale Makale { get; set; }  //yorum makaleye aittir.(ilişkilendirme)
        public virtual Kullanici Kullanici { get; set; }//yorumu yapan kullanıcılar vardır
    }
}
