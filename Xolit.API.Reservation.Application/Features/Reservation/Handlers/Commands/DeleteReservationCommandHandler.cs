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
using Xolit.API.Reservation.Application.DTOs.Recervation.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Reservation.Handlers.Commands
{
    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteReservationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Crear validador y validar el DTO
                var validator = new DeleteReservationValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send: {firstError}");
                }

                var reservation = await _unitOfWork.Reservations.GetByIdAsync(request.Id);
                _ = reservation == null ? throw new Exception("Reservation not found") : true;

                _unitOfWork.Reservations.Delete(reservation);
                await _unitOfWork.CompleteAsync();

                response.Success = true;
                response.Message = "Reservation Deleted successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "ReservationDelete",
                        Value = JsonConvert.SerializeObject(reservation)
                    }
                };

            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error al delete Reservation";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
