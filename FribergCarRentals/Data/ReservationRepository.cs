using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ReservationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Reservation reservation)
        {
            await _applicationDbContext.Reservations.AddAsync(reservation);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Reservation? reservation = await GetByIdAsync(id);
            if (reservation == null)
                return;
            _applicationDbContext.Reservations.Remove(reservation);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _applicationDbContext.Reservations
                .Include(r => r.Customer)
                    .ThenInclude(c => c.IdentityUser)
                .Include(r => r.Car)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _applicationDbContext.Reservations.AnyAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Reservation t)
        {
            _applicationDbContext.Reservations.Update(t);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
