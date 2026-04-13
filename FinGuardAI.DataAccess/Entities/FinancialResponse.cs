using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Entities
{
    public class FinancialResponse
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Decision { get; set; }
        public string DecisionClause { get; set; }
        public string Justification { get; set; }
        public decimal AcceptedAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public virtual FinancialRequest Request { get; set; }
        public virtual User Creator { get; set; }
    }
}
