using Makale_DAL;
using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_BLL
{
    public class YorumYonet
    {
        Repository<Yorum> rep_yorum=new Repository<Yorum>();
        public Yorum YorumBul(int id)
        {
            return rep_yorum.Find(x => x.Id == id);
        }
        public int YorumUpdate(Yorum yorum)
        {
            return rep_yorum.Update(yorum);
        }
        public int YorumSil(Yorum yorum)
        {
            return rep_yorum.Delete(yorum);
        }

        public int YorumEkle(Yorum yorum)
        {
            return rep_yorum.Insert(yorum);
        }
    }
}
