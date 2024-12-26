using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.DTOs.Common;
using Xolit.API.Reservation.Application.DTOs.Recervation;
using Xolit.API.Reservation.Application.DTOs.Recervation.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Queries;
using Xolit.API.Reservation.Application.Response;
using Xolit.API.Reservation.Domain.Reservation;

namespace Xolit.API.Reservation.Application.Features.Reservation.Handlers.Queries
{
    public class GetByUserIdReservationCommandHandler : IRequestHandler<GetByUserIdReservationCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetByUserIdReservationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetByUserIdReservationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Obtener todas las reservas del usuario
                var userReservations = await _unitOfWork.Reservations.FindAsync(r => r.UserId == request.UserId);

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "User reservations retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "UserReservations",
                        Value = JsonConvert.SerializeObject(userReservations)
                    }
                };
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error al obtener las reservas del usuario";
                response.Errors = new List<string>
                {
                    ex.InnerException?.Message ?? ex.Message
                };
            }

            return response;
        }
    }
}
