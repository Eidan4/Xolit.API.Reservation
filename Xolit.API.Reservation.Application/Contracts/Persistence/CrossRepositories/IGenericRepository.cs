using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xolit.API.Reservation.Domain.User;

namespace Xolit.API.Reservation.Application.Contracts.Persistence.CrossRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Obtener todos los elementos
        Task<IEnumerable<T>> GetAllAsync();

        // Obtener un elemento por su ID
        Task<T> GetByIdAsync(int id);

        // Obtener elementos con condiciones específicas (filtros)
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Agregar un nuevo elemento
        Task AddAsync(T entity);

        // Agregar varios elementos
        Task AddRangeAsync(IEnumerable<T> entities);

        // Actualizar un elemento existente
        void Update(T entity);

        // Eliminar un elemento
        void Delete(T entity);

        // Eliminar varios elementos
        void DeleteRange(IEnumerable<T> entities);
    }
}
