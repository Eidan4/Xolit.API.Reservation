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
using Xolit.API.Reservation.Application.DTOs.Recervation.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Queries;
using Xolit.API.Reservation.Application.Response;
using Xolit.API.Reservation.Domain.Reservation;

namespace Xolit.API.Reservation.Application.Features.Reservation.Handlers.Queries
{
    public class GetReservationCommandHandler : IRequestHandler<GetReservationByDayCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReservationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetReservationByDayCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Parsear la fecha en formato DD/MM/YYYY
                if (!DateTime.TryParseExact(request.Day, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                {
                    throw new Exception("Invalid date format. Use DD/MM/YYYY.");
                }

                // Rango de horario permitido
                var startOfDay = parsedDate.Date.AddHours(8); // 8:00 AM
                var endOfDay = parsedDate.Date.AddHours(20); // 8:00 PM

                // Obtener reservas del día seleccionado para el SpaceId proporcionado
                var reservations = await _unitOfWork.Reservations.FindAsync(r =>
                    r.StartTime.Date == parsedDate.Date && r.SpaceId == request.SpaceId);

                // Crear lista de intervalos de 2 horas
                var availableIntervals = new List<string>();
                for (var time = startOfDay; time < endOfDay; time = time.AddHours(2))
                {
                    var intervalStart = time;
                    var intervalEnd = time.AddHours(2);

                    // Verificar si el intervalo está reservado
                    var isReserved = reservations.Any(r =>
                        r.StartTime < intervalEnd && r.EndTime > intervalStart);

                    if (!isReserved)
                    {
                        availableIntervals.Add($"{intervalStart:HH:mm} - {intervalEnd:HH:mm}");
                    }
                }

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "AvailableIntervals successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "AvailableIntervals",
                        Value = JsonConvert.SerializeObject(availableIntervals)
                    }
                };
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
