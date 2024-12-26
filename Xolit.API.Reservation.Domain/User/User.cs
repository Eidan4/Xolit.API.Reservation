using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xolit.API.Reservation.Domain.Base;
using Xolit.API.Reservation.Domain.Reservation;

namespace Xolit.API.Reservation.Domain.User
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; } // Nombre del usuario
        public string Email { get; set; } // Email del usuario
        public string Password { get; set; } //Password del usuario
        public string Role { get; set; } //Password del usuario

        // Relaciones
        public virtual ICollection<ReservationEntity> Reservations { get; set; }
    }
}
