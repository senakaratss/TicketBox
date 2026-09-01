using Microsoft.AspNetCore.SignalR;

namespace TicketBox.WebUI.Hubs
{
    public class SeatHoldCleanupService:BackgroundService
    {
        private readonly IHubContext<SeatHub> _hubContext;

        public SeatHoldCleanupService(IHubContext<SeatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var seat in SeatHub.SelectedSeats)
                {
                    if (seat.Value.Expiration <= DateTime.UtcNow)
                    {
                        if (SeatHub.SelectedSeats.TryRemove(
                            seat.Key,
                            out _))
                        {
                            var parts = seat.Key.Split('-');

                            var eventId = int.Parse(parts[0]);
                            var seatNumber = parts[1];

                            await _hubContext.Clients.All.SendAsync(
                                "SeatReleased",
                                eventId,
                                seatNumber,
                                stoppingToken);
                        }
                    }
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    stoppingToken);
            }
        }
    }
}
