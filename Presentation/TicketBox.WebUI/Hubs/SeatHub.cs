using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Drawing.Text;

namespace TicketBox.WebUI.Hubs
{
    public class SeatHub:Hub
    {
        public static readonly ConcurrentDictionary<string, SeatHold> SelectedSeats = new(); //temporary memory
        public async Task SelectSeat(int eventId,string seatNumber)
        {
            var key = $"{eventId}-{seatNumber}";

            var seatHold = new SeatHold
            {
                ConnectionId = Context.ConnectionId,
                Expiration = DateTime.UtcNow.AddMinutes(10)
            };

            var added= SelectedSeats.TryAdd(key,seatHold);
            if (!added)
            {
                await Clients.Caller.SendAsync("SeatAlreadySelected", eventId, seatNumber);
                return;
            }

            await Clients.Others.SendAsync("SeatSelected", eventId, seatNumber);
        }

        public async Task ReleaseSeat(int eventId,string seatNumber)
        {
            var key = $"{eventId}-{seatNumber}";

            if(SelectedSeats.TryGetValue(key,out var seatHold))
            {
                if (seatHold.ConnectionId == Context.ConnectionId)
                {
                    SelectedSeats.Remove(key, out _);

                    await Clients.Others.SendAsync("SeatReleased", eventId, seatNumber);
                }
            }
        }
    }
}
