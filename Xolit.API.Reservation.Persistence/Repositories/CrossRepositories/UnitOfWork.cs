using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories;
using Xolit.API.Reservation.Domain.Reservation;
using Xolit.API.Reservation.Domain.Space;
using Xolit.API.Reservation.Domain.User;
using Xolit.API.Reservation.Persistence.DBContext;

namespace Xolit.API.Reservation.Persistence.Repositories.CrossRepositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReservationDbContext _context;

        public IGenericRepository<ReservationEntity> Reservations { get; private set; }
        public IGenericRepository<SpaceEntity> Space { get; private set; }
        public IGenericRepository<UserEntity> User { get; private set; }

        public UnitOfWork(ReservationDbContext context)
        {
            _context = context;

            // Instanciar repositorios genéricos
            Reservations = new GenericRepository<ReservationEntity>(_context);
            Space = new GenericRepository<SpaceEntity>(_context);
            User = new GenericRepository<UserEntity>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
