using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class Rol : IEntity
    {
        public Guid Id { get; set; }
        public string RolAd { get; set; }
        public Guid EkleyenKullanici { get; set; }

        public DateTime? EklemeTarihi { get; set; }
        public string Durum { get; set; }
    }
}
