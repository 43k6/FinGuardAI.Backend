using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.DTOs
{

    public class UserDTO
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

   
}
