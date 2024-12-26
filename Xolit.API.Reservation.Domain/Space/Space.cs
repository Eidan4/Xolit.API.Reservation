using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xolit.API.Reservation.Domain.Base;
using Xolit.API.Reservation.Domain.Reservation;

namespace Xolit.API.Reservation.Domain.Space
{
    public class SpaceEntity : BaseEntity
    {
        public string Name { get; set; } // Nombre del espacio
        public string Description { get; set; } // Descripción del espacio
        public int Capacity { get; set; } // Capacidad máxima del espacio
        public bool? IsAvailable { get; set; } // Indica si el espacio está disponible

        // Relaciones
        public virtual ICollection<ReservationEntity> Reservations { get; set; }
    }
}
