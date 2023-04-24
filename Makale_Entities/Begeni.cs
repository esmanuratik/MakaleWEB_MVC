using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    [Table("Beğeni")]
    public class Begeni
    {
        //beğeni vardır ya da yoktur baseclass tan miras almaya gerek yok fakat ıd si olacak çünklü beğeni bize ıd den geliyor
        [Key ,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Kullanici Kullanici { get; set; }//kim beğendi
        public virtual Makale Makale { get; set; } // hangi makaleyi beğendi
    }
}
