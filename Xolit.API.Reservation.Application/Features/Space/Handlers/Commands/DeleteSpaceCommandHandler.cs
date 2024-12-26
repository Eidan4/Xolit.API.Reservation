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
using Xolit.API.Reservation.Application.DTOs.User.Validation;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Space.Handlers.Commands
{
    public class DeleteSpaceCommandHandler : IRequestHandler<DeleteSpaceCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteSpaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(DeleteSpaceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {

                var validator = new DeleteSpaceValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send: {firstError}");
                }

                // Buscar el espacio por ID
                var space = await _unitOfWork.Space.GetByIdAsync(request.Id);
                _ = space == null ? throw new Exception("Space not found") : true;

                // Eliminar el espacio
                _unitOfWork.Space.Delete(space);
                await _unitOfWork.CompleteAsync();

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Space deleted successfully";
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error deleting space";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
