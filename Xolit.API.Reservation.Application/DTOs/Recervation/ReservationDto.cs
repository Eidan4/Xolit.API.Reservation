using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xolit.API.Reservation.Application.DTOs.Recervation
{
    public class ReservationDto
    {
        public int? Id { get; set; }
        public int SpaceId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
