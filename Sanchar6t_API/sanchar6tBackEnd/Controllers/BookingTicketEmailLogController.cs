using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Repositories;
using sanchar6tBackEnd.Services;

[ApiController]
[Route("api/booking-ticket-email-log")]
public class BookingTicketEmailLogController : ControllerBase
{
    private readonly IBookingTicketEmailLogRepository _repository;
    private readonly EmailSenderService _emailService;

    public BookingTicketEmailLogController(
        IBookingTicketEmailLogRepository repository,
        EmailSenderService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookingTicketEmailLog model)
    {
        model.EmailStatus = "Pending";
        model.CreatedDate = DateTime.Now;

        await _repository.AddAsync(model);

        try
        {
            await _emailService.SendEmailAsync(
                model.EmailTo,
                "Your Ticket Booking Confirmation",
                $"Hello,<br/><br/>Your ticket <b>{model.TicketNumber}</b> has been booked successfully."
            );

            model.EmailStatus = "Sent";
        }
        catch (Exception ex)
        {
            model.EmailStatus = "Failed";
            model.ErrorMessage = ex.Message;
        }

        await _repository.UpdateAsync(model);

        return Ok("Email log created & email sent");
    }
}
