using Core.Entities.Abstract;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CustomerDto:IDto
    {
        public string Name { get; set; }          // Kişi adı veya şirket adı
        public string CompanyName { get; set; }   // Firma adı (isteğe bağlı)
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Adres Bilgileri
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CustomerField { get; set; }

        // Müşteri Durumu
        public CustomerStatus Status { get; set; } // Aktif, Potansiyel, Eski (enum olarak)

        // Son işlem bilgisi
        public DateTime? LastActionDate { get; set; } // Son işlem zamanı (teklif, kurulum vs)

        // Sistemsel bilgiler
        public DateTime CreatedAt { get; set; }      // Ne zaman eklendi
        public DateTime? UpdatedAt { get; set; }     // En son ne zaman güncellendi
    }
}
