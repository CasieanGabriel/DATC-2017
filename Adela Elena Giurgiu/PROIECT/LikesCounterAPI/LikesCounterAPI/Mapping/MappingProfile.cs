using AutoMapper;
using LikesCounterAPI.Controllers;
using LikesCounterAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikesCounterAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountResource>();
        }
    }
}
