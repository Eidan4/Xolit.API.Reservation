using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Application.DTOs.Common;
using Xolit.API.Reservation.Application.DTOs.User;
using Xolit.API.Reservation.Application.DTOs.User.Validation;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Application.Response;
using Xolit.API.Reservation.Domain.User;

namespace Xolit.API.Reservation.Application.Features.User.Handlers.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validar el DTO
                var validator = new CreateUserValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request.UserDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Mapear el DTO a la entidad User
                var user = _mapper.Map<UserEntity>(request.UserDto);

                // Establecer fechas de creación y modificación
                user.CreatedDate = DateTime.UtcNow;
                user.UpdatedDate = DateTime.UtcNow;

                // Agregar el usuario a la base de datos
                await _unitOfWork.User.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                // Mapear el usuario recién creado a un DTO
                var createdUserDto = _mapper.Map<UserDto>(user);

                // Configurar la respuesta exitosa
                response.Success = true;
                response.Message = "User created successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "CreatedUser",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(createdUserDto)
                    }
                };
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error creating user";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
