using AutoMapper;
using Xolit.API.Reservation.Application.DTOs.Recervation;
using Xolit.API.Reservation.Application.DTOs.Space;
using Xolit.API.Reservation.Application.DTOs.User;
using Xolit.API.Reservation.Domain.Reservation;
using Xolit.API.Reservation.Domain.Space;
using Xolit.API.Reservation.Domain.User;

namespace Xolit.API.Reservation.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, UserEntity>().ReverseMap();
            CreateMap<ReservationDto, ReservationEntity>().ReverseMap();
            CreateMap<SpaceDto, SpaceEntity>().ReverseMap();
        }
    }
}