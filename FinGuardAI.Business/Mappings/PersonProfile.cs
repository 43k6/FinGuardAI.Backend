using AutoMapper;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.Business.Mappings
{
    public class PersonProfile :Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDto>().ReverseMap();

        }
    }


}
