using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xolit.API.Reservation.Application.DTOs.User
{
    public class UserLoginDto
    {
        public string Email { get; set; } // Email del usuario
        public string Password { get; set; } //Password del usuario
    }
}
