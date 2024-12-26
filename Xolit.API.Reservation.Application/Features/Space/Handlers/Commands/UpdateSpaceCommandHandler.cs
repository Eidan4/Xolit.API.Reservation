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
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Space.Handlers.Commands
{
    public class UpdateSpaceCommandHandler : IRequestHandler<UpdateSpaceCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSpaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateSpaceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validar el DTO
                var validator = new UpdateSpaceValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.SpaceDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Buscar el espacio existente
                var space = await _unitOfWork.Space.GetByIdAsync((int)request.SpaceDto.Id);
                _ = space == null ? throw new Exception("Space not found") : true;

                // Mapear los datos del DTO a la entidad Space
                _mapper.Map(request.SpaceDto, space);

                // Actualizar la fecha de modificación
                space.UpdatedDate = DateTime.UtcNow;

                // Guardar los cambios
                _unitOfWork.Space.Update(space);
                await _unitOfWork.CompleteAsync();

                // Configurar la respuesta exitosa
                response.Success = true;
                response.Message = "Space updated successfully";
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error updating space";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
