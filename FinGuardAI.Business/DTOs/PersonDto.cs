using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.DTOs
{
    public class PersonDto
    {

        public int PersonID { get; set; }
        public string NationalID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public PersonDto(int PersonID, string NationalID,string FirstName,string LastName,string Address, string Phone, string Email)
        {
            this.PersonID = PersonID;
            this.NationalID = NationalID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
        }

    }
}
