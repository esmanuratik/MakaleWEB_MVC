using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class MakaleBLL_Sonuc<T> where T : class
    {
        //hepsi için kullancağımdan bir tipe ihtiyacım var.
        public T nesne { get; set; }
        public List<string> hatalar { get; set; }
        public MakaleBLL_Sonuc()//hataları new le örneklemek için ctor açtık.
        {
            hatalar = new List<string>();
        } 
    }
} 
