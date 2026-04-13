using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Entities
{
    public class FinancialRequest
    {
        public int Id { get; set; }
        public string RequestName { get; set; }
        public decimal Amount { get; set; }
        public enum Categories {

            Travel,          // السفر
            Accommodation,   // الإقامة
            Software,        // البرمجيات
            Hardware,        // الأجهزة
            Marketing,       // التسويق
            OfficeSupplies,  // أدوات مكتبية
            ClientEntertainment, // ضيافة العملاء
            Other
        };
        public Categories RequestCategory { get; set; }
        public string Description { get; set; }
        public string Files { get; set; } // يمكن تخزين مسارات الملفات كنص أو JSON
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string State { get; set; }

        public virtual User Creator { get; set; }
        public virtual ICollection<FinancialResponse> Responses { get; set; }
    }
}
