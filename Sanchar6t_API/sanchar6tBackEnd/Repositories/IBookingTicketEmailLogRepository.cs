using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Models;

namespace sanchar6tBackEnd.Repositories
{
    public class IBookingTicketEmailLogRepository
    {
        private readonly Sanchar6tDbContext _context;

        public IBookingTicketEmailLogRepository(Sanchar6tDbContext context)
        {
            _context = context;
        }

        // 🔹 Insert new email log
        public async Task AddAsync(BookingTicketEmailLog log)
        {
            _context.BookingTicketEmailLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        // 🔹 Update whole object (USED after sending email)
        public async Task UpdateAsync(BookingTicketEmailLog log)
        {
            _context.BookingTicketEmailLogs.Update(log);
            await _context.SaveChangesAsync();
        }

        // 🔹 Get pending emails
        public async Task<List<BookingTicketEmailLog>> GetPendingAsync()
        {
            return await _context.BookingTicketEmailLogs
                .Where(x => x.EmailStatus == "Pending")
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }

        // 🔹 Mark email as sent (by id)
        public async Task MarkSentAsync(int id)
        {
            var log = await _context.BookingTicketEmailLogs.FindAsync(id);
            if (log == null) return;

            log.EmailStatus = "Sent";
            log.LastSentDate = DateTime.Now;
            log.RetryCount++;

            await _context.SaveChangesAsync();
        }

        // 🔹 Mark email as failed (by id)
        public async Task MarkFailedAsync(int id, string errorMessage)
        {
            var log = await _context.BookingTicketEmailLogs.FindAsync(id);
            if (log == null) return;

            log.EmailStatus = "Failed";
            log.ErrorMessage = errorMessage;
            log.LastSentDate = DateTime.Now;
            log.RetryCount++;

            await _context.SaveChangesAsync();
        }
    }
}
