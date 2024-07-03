using Microsoft.AspNetCore.SignalR;
using ChatApp.Data.Models;
using ChatApp.Data;
using ChatApp.Services;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly RabbitMQService _rabbitMQService;

        public ChatHub(ApplicationDbContext context, RabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        public async Task ChangeRoom(string oldRoom, string NewRoom)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldRoom);
            await Groups.AddToGroupAsync(Context.ConnectionId, NewRoom);
        }

        public async Task SendMessage(string user, string message, string roomName)
        {
            var room = _context.ChatRooms.FirstOrDefault(room => room.Name == roomName);
            if (room != null)
            {
                if (message.StartsWith("/stock="))
                {
                    var stockCode = message.Substring(7);
                    _rabbitMQService.PublishStockRequest(stockCode, room.Name);
                }
                else
                {

                    var chatMessage = new ChatMessage
                    {
                        UserId = user,
                        Content = message,
                        Timestamp = DateTime.Now,
                        ChatRoomId = room.Id
                    };

                    _context.ChatMessages.Add(chatMessage);
                    await _context.SaveChangesAsync();

                    await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
                }
            }
        }
    }
}
