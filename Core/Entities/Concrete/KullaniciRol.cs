using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Concrete
{
    public class KullaniciRol : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid KullaniciId { get; set; }
        public Guid RolId { get; set; }
        public string Durum { get; set; }
        public DateTime? EklemeTarihi { get; set; }
        public Guid EkleyenKullanici { get; set; }

        [ForeignKey("EkleyenKullanici")]
        public virtual Kullanici Kullanici { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual Kullanici KullaniciRolId { get; set; }

        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; }
    }
}
