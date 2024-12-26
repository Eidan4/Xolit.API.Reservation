using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xolit.API.Reservation.Domain.Reservation;
using Xolit.API.Reservation.Domain.Space;
using Xolit.API.Reservation.Domain.User;

namespace Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Ejemplo: Un repositorio para cada entidad
        IGenericRepository<ReservationEntity> Reservations { get; }
        IGenericRepository<SpaceEntity> Space { get; }
        IGenericRepository<UserEntity> User { get; }

        // Método para guardar cambios en la base de datos
        Task<int> CompleteAsync();
    }
}
