using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xolit.API.Reservation.Domain.Base;
using Xolit.API.Reservation.Domain.Space;
using Xolit.API.Reservation.Domain.User;

namespace Xolit.API.Reservation.Domain.Reservation
{
    public class ReservationEntity : BaseEntity
    {
        public int SpaceId { get; set; }
        public int UserId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? IsActive { get; set; }

        // Relaciones
        public virtual SpaceEntity Space { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
