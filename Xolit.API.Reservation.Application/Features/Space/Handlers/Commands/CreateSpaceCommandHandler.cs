using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.DTOs.Space.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using Xolit.API.Reservation.Application.Response;
using Xolit.API.Reservation.Domain.Space;

namespace Xolit.API.Reservation.Application.Features.Space.Handlers.Commands
{
    public class CreateSpaceCommandHandler : IRequestHandler<CreateSpaceCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSpaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateSpaceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validar el DTO
                var validator = new CreateSpaceValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.SpaceDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Mapear el DTO a la entidad Space
                var space = _mapper.Map<SpaceEntity>(request.SpaceDto);

                // Establecer fechas de creación y modificación
                space.CreatedDate = DateTime.UtcNow;
                space.UpdatedDate = DateTime.UtcNow;

                // Agregar el espacio a la base de datos
                await _unitOfWork.Space.AddAsync(space);
                await _unitOfWork.CompleteAsync();

                // Configurar la respuesta exitosa
                response.Success = true;
                response.Message = "Space created successfully";
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error creating space";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
