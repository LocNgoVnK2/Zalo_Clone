﻿using AutoMapper;
using Infrastructure.Entities;
using Zalo_Clone.Models;

namespace Zalo_Clone
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Reaction
            CreateMap<ReactionModel, Reaction>();
            CreateMap<Reaction, ReactionModel>();
            //UserData
            CreateMap<UserDataModel, UserData>();
            CreateMap<UserData, UserDataModel>();
        }
    }
}
