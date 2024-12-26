using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.DTOs.Recervation.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Reservation.Handlers.Commands
{
    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateReservationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var validator = new UpdateReservationValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request.ReservationDto, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send Owner: {firstError}");
                }

                var reservation = await _unitOfWork.Reservations.GetByIdAsync((int)request.ReservationDto.Id);
                _ = reservation == null ? throw new Exception("Reservation not found") : true;

                // Validar solapamientos
                var overlappingReservations = await _unitOfWork.Reservations.FindAsync(r =>
                    r.SpaceId == request.ReservationDto.SpaceId &&
                    r.Id != reservation.Id && // Excluir la misma reserva
                    r.StartTime < request.ReservationDto.EndTime &&
                    r.EndTime > request.ReservationDto.StartTime);

                _ = overlappingReservations.Any() ? throw new Exception("The reservation overlaps with another reservation") : true;

                // Mapear los datos del DTO a la entidad
                _mapper.Map(request.ReservationDto, reservation);

                // Actualizar la fecha de modificación
                reservation.UpdatedDate = DateTime.UtcNow;

                // Actualizar la reserva en la base de datos
                _unitOfWork.Reservations.Update(reservation);
                await _unitOfWork.CompleteAsync();

                // Configurar la respuesta exitosa
                response.Success = true;
                response.Message = "Reservation updated successfully";

            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error al Update Reservation";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
