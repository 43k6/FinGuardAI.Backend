using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int ActionId { get; set; } // قد يربط بـ RequestId أو ResponseId
        public enum TypesOfAction { Request, Response }
        public TypesOfAction ActionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public virtual User Creator { get; set; }
    }
}
