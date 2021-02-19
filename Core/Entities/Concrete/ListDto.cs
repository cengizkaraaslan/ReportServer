using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
   public class ListDto<T>
    {
        public IEnumerable<T> ModelList { get; set; }
        public int Toplam { get; set; }
    }
    public class ListDto
    {
        public IEnumerable<object> ModelList { get; set; }
        public int Toplam { get; set; }
    }
}
