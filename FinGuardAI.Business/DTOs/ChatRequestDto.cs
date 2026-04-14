using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.Business.DTOs
{
    public class ChatRequestDto
    {
        public string Message { get; set; } = string.Empty;
        public string? SessionId { get; set; }
    }
}
