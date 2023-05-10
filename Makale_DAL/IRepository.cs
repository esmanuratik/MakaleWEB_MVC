using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Makale_DAL
{
    public interface IRepository<T>//buradaki metotları bir classa verirsek bunları kull. zorunda kalır.
    {
        int Insert(T nesne);//T tipte int döndüren nesne metotu oluşturduk.
        int Update(T nesne);
        int Delete(T nesne);
        List<T> Liste();
        IQueryable<T> ListQueryable();
        List<T> Liste(Expression<Func<T,bool>>kosul);//bir koşula bağlı listeleme yapsın.
        T Find(Expression<Func<T, bool>> kosul);
    }
}
