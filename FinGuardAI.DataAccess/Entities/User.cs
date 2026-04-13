using System;
using System.Collections.Generic;

namespace FinGuardAI.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<FinancialRequest> FinancialRequests { get; set; }
        public virtual ICollection<FinancialResponse> FinancialResponses { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
    }
}
