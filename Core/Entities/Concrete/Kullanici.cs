using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class Kullanici : IEntity
    {
        public Guid Id { get; set; }
        public Guid SirketId { get; set; }
        public Guid ParametreId { get; set; }
        public string Ad { get; set; }

        public string Soyad { get; set; }
        public string Sifre { get; set; }
        public string KullaniciAdi { get; set; }
        public string Unvan { get; set; }
        public string TelefonNo { get; set; }
        public string Aciklama { get; set; }
        public bool Aktif { get; set; }
        public Guid EkleyenKullanici { get; set; }
        public DateTime? EklemeTarihi { get; set; }
        public string Durum { get; set; }

    }
}
