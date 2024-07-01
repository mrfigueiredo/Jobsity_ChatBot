using Microsoft.AspNetCore.SignalR;
using ChatApp.Data.Models;
using ChatApp.Data;
using ChatApp.Services;

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

        public async Task SendMessage(string user, string message)
        {
            if (message.StartsWith("/stock="))
            {
                var stockCode = message.Substring(7);
                _rabbitMQService.PublishStockRequest(stockCode);
            }
            else
            {
                var chatMessage = new ChatMessage
                {
                    UserId = user,
                    Content = message,
                    Timestamp = DateTime.Now,
                    ChatRoomId = 1 // assuming a default chatroom
                };

                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }
    }
}
