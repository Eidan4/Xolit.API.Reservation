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
using Xolit.API.Reservation.Application.DTOs.User.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.User.Handlers.Commands
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new DeleteUserValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send: {firstError}");
                }

                // Buscar el usuario por ID
                var user = await _unitOfWork.User.GetByIdAsync(request.Id);
                _ = user == null ? throw new Exception("User not found") : true;

                // Eliminar el usuario
                _unitOfWork.User.Delete(user);
                await _unitOfWork.CompleteAsync();

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "User deleted successfully";
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error deleting user";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
