using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.DTOs.Common;
using Xolit.API.Reservation.Application.DTOs.Recervation;
using Xolit.API.Reservation.Application.DTOs.Recervation.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Response;
using Xolit.API.Reservation.Domain.Reservation;

namespace Xolit.API.Reservation.Application.Features.Reservation.Handlers.Commands
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateReservationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Validar el DTO
                var validator = new CreateReservationValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request.ReservationDto, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Verificar claves foráneas
                var userExists = await _unitOfWork.User.GetByIdAsync(request.ReservationDto.UserId);
                _ = userExists == null ? throw new Exception("Invalid UserId") : true;

                var spaceExists = await _unitOfWork.Space.GetByIdAsync(request.ReservationDto.SpaceId);
                _ = spaceExists == null ? throw new Exception("Invalid SpaceId") : true;

                // Regla: Un usuario no puede reservar dos espacios al mismo tiempo
                var userOverlappingReservations = await _unitOfWork.Reservations.FindAsync(r =>
                    r.UserId == request.ReservationDto.UserId &&
                    r.StartTime < request.ReservationDto.EndTime &&
                    r.EndTime > request.ReservationDto.StartTime);

                if (userOverlappingReservations.Any())
                {
                    response.Success = false;
                    response.Message = "User has another overlapping reservation.";
                    response.Errors = new List<string> { "A user cannot reserve multiple spaces at the same time." };
                    return response;
                }

                // Mapear el DTO a la entidad
                var reservation = _mapper.Map<ReservationEntity>(request.ReservationDto);
                reservation.CreatedDate = DateTime.UtcNow;
                reservation.UpdatedDate = DateTime.UtcNow;

                // Guardar la reserva en la base de datos
                await _unitOfWork.Reservations.AddAsync(reservation);
                await _unitOfWork.CompleteAsync();

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Reservation created successfully";
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error al crear Reservation";
                response.Errors = new List<string>
                {
                    ex.InnerException?.Message ?? ex.Message
                };
            }

            return response;
        }
    }
}
