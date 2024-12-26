using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xolit.API.Reservation.Application.DTOs.Common;

namespace Xolit.API.Reservation.Application.Response
{
    public class BaseCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatsCode { get; set; }
        public List<string> Errors { get; set; }
        public List<ParameterDto> Parameters { get; set; }
    }
}
