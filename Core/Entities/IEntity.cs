using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
   public interface IEntity
    {
        Guid Id { get; set; }
        string Durum { get; set; }
        DateTime? EklemeTarihi { get; set; }
        Guid EkleyenKullanici { get; set; }
    }
}
