using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xolit.API.Reservation.Application.DTOs.Space
{
    public class SpaceDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } // Nombre del espacio
        public string Description { get; set; } // Descripción del espacio
        public int Capacity { get; set; } // Capacidad máxima del espacio
        public bool? IsAvailable { get; set; } // Indica si el espacio está disponible
    }
}
