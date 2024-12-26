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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Buscar al usuario por email
                var user = await _unitOfWork.User.FindAsync(u => u.Email == request.UserLoginDto.Email);

                if (user == null || !VerifyPassword(request.UserLoginDto.Password, user.FirstOrDefault()?.Password))
                {
                    response.Success = false;
                    response.Message = "Invalid email or password";
                    response.Errors = new List<string> { "Invalid credentials provided." };
                    return response;
                }

                // Configurar la respuesta exitosa con los datos del usuario
                var userDto = _mapper.Map<UserDto>(user.FirstOrDefault());
                response.Success = true;
                response.Message = "Login successful";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "User",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(userDto)
                    }
                };
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error during login";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Aquí puedes implementar tu lógica de hash o comparación de contraseñas
            // Por ejemplo, si usas bcrypt o un hash personalizado
            return inputPassword == storedPassword;
        }
    }
}
