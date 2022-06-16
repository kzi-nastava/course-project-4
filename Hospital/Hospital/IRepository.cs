using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public interface IRepository<T>where T:class
    {
        List<T> Load();
        void Save(List<T> list);
    }
}
