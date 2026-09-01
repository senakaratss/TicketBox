using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Application.Features.Bookings.Commands;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;
using TicketBox.Domain.Enums;

namespace TicketBox.Application.Features.Bookings.Handlers
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IIdentityService _identityService;
        private readonly IQrCodeService _qrCodeService;
        private readonly ITicketImageService _ticketImageService;
        private readonly IEmailService _emailService;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingEmailTemplate _bookingEmailTemplate;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, IEventRepository eventRepository, IIdentityService identityService, IQrCodeService qrCodeService, ITicketImageService ticketImageService, IEmailService emailService, ITicketRepository ticketRepository, IBookingEmailTemplate bookingEmailTemplate)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _identityService = identityService;
            _qrCodeService = qrCodeService;
            _ticketImageService = ticketImageService;
            _emailService = emailService;
            _ticketRepository = ticketRepository;
            _bookingEmailTemplate = bookingEmailTemplate;
        }

        public async Task Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(request.EventId);
            var userId = await _identityService.GetCurrentUserIdAsync();
            var userInfo = await _identityService.GetUserInfoAsync(userId);

            if (request.TicketQuantity <= 0)
            {
                throw new Exception("En az 1 bilet seçmelisiniz.");
            }
            var soldTicketCount = eventEntity.Bookings.Sum(x => x.TicketQuantity);
            var remaningCapacity = eventEntity.Capacity - soldTicketCount;
            if (request.TicketQuantity > remaningCapacity)
            {
                throw new Exception("Yeterli bilet bulunmamaktadır.");
            }

            var hasSeatSelection = request.SeatNumbers != null && request.SeatNumbers.Any();
            if (hasSeatSelection)
            {
                if (request.SeatNumbers.Count != request.TicketQuantity)
                {
                    throw new Exception("Seçilen koltuk sayısı ile bilet sayısı eşleşmiyor.");
                }
                var takenSeats = await _ticketRepository.GetTakenSeatsByEventIdAsync(request.EventId);
                var alreadyTaken = request.SeatNumbers.Intersect(takenSeats).ToList();
                if (alreadyTaken.Any())
                {
                    throw new Exception($"Seçilen koltuklar zaten alınmış: {string.Join(", ", alreadyTaken)}");
                }
            }

            var booking = new Booking
            {
                EventId = request.EventId,
                TicketQuantity = request.TicketQuantity,
                TotalPrice = request.TicketQuantity * eventEntity.Price,
                BookingDate = DateTime.Now,
                UserId = userId
            };
            for (int i = 0; i < request.TicketQuantity; i++)
            {
                var serialNumber = $"TK-{Guid.NewGuid().ToString("N")[..10].ToUpper()}";
                var qrCode = _qrCodeService.GenerateQrCode(serialNumber);
                string? seatNumber = null;
                if (hasSeatSelection)
                {
                    seatNumber = request.SeatNumbers[i];
                }
                booking.Tickets.Add(new Ticket
                {
                    SerialNumber = serialNumber,
                    QRCode = qrCode,
                    CreatedDate = DateTime.Now,
                    Status = TicketStatus.Active,
                    SeatNumber = seatNumber
                });
            }
            await _bookingRepository.CreateBookingAsync(booking);

            //Mail attachments
            var attachments = new List<EmailAttachmentDto>();
            foreach (var ticket in booking.Tickets)
            {
                var ticketImageDto = new TicketImageDto
                {
                    EventName = eventEntity.Title,
                    EventLocation = eventEntity.Location,
                    EventImageUrl = eventEntity.ImageUrl,
                    EventDate = eventEntity.EventDate,
                    Holder = $"{userInfo.Name} {userInfo.Surname}",
                    SerialNumber = ticket.SerialNumber,
                    SeatNumber = ticket.SeatNumber,
                    QRCode = ticket.QRCode,
                    Status = ticket.Status
                };
                var imageBytes = await _ticketImageService.GenerateTicketImage(ticketImageDto);
                attachments.Add(new EmailAttachmentDto { FileName = $"{ticket.SerialNumber}.png", Content = imageBytes });
            }

            //send booking confirmation email
            var emailBody = _bookingEmailTemplate.CreateConfirmationEmail(userInfo.Name, eventEntity.Title, eventEntity.EventDate,
                eventEntity.Location, booking.BookingId, booking.TicketQuantity, booking.TotalPrice);

            await _emailService.SendEmailAsync(userInfo.Email, "TicketBox - Booking Confirmation", emailBody, attachments);
        }
    }
}
