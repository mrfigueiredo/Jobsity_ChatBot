using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data.Models;
using ChatApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Pages
{
    [Authorize]
    public class ChatRoomInteractiveModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<ChatRoom> ChatRooms { get; set; }

        [BindProperty]
        public string? SelectedRoom { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public ChatRoomInteractiveModel(ApplicationDbContext context)
        {
            _context = context;
            SelectedRoom = null;
        }

        public void OnGet()
        {
            ChatRooms = _context.ChatRooms.ToList();
            /*SelectedRoom = _context.ChatRooms.FirstOrDefault()?.Name;
            if (SelectedRoom != null)
            {
                var room = _context.ChatRooms.First(room => room.Name == SelectedRoom);
                Messages = _context.ChatMessages
                                         .Where(m => m.Id == room.Id)
                                         .OrderByDescending(m => m.Timestamp)
                                         .Take(50)
                                         .ToList();
            }
            else
            {
                Messages = new List<ChatMessage>();
            }*/
        }
    }
}
