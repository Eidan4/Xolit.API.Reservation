using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xolit.API.Reservation.Application.DTOs.User
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } // Nombre del usuario
        public string Email { get; set; } // Email del usuario
        public string Password { get; set; } //Password del usuario
        public string Role { get; set; } //Password del usuario
    }
}
