using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data.Models;
using ChatApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Pages
{
    [Authorize]
    public class ChatRoomInteractiveModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<ChatRoom> ChatRooms { get; set; }
        public ChatRoom? SelectedRoom { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public ChatRoomInteractiveModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            ChatRooms = _context.ChatRooms.ToList();
            SelectedRoom = _context.ChatRooms.FirstOrDefault();
            if (SelectedRoom != null)
            {
                Messages = _context.ChatMessages
                                         .Where(m => m.ChatRoomId == SelectedRoom.Id)
                                         .OrderByDescending(m => m.Timestamp)
                                         .Take(50)
                                         .ToList();
            }
            else
            {
                Messages = new List<ChatMessage>();
            }
        }

        public IActionResult OnPost(string messageContent, string userId)
        {
            var chatRoom = SelectedRoom;
            if (chatRoom != null)
            {
                var message = new ChatMessage()
                {
                    Content = messageContent,
                    ChatRoom = chatRoom,
                    ChatRoomId = chatRoom.Id,
                    Timestamp = DateTime.Now,
                    UserId = userId
                };
                _context.ChatMessages.Add(message);        
            }
            return RedirectToPage();
        }

    }
}
