using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.DTOs.User.Validation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.User.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validar los datos del DTO
                var validator = new UpdateUserValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.UserDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Buscar el usuario existente
                var user = await _unitOfWork.User.GetByIdAsync((int)request.UserDto.Id);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    response.Errors = new List<string> { "The specified user does not exist." };
                    return response;
                }

                // Mapear los datos del DTO a la entidad User
                _mapper.Map(request.UserDto, user);

                // Actualizar la fecha de modificación
                user.UpdatedDate = DateTime.UtcNow;

                // Guardar los cambios en la base de datos
                _unitOfWork.User.Update(user);
                await _unitOfWork.CompleteAsync();

                // Configurar la respuesta exitosa
                response.Success = true;
                response.Message = "User updated successfully";
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error updating user";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
