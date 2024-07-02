using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data.Models;
using ChatApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<ChatRoom> ChatRooms { get; set; }

        public ChatModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            ChatRooms = _context.ChatRooms.ToList();
        }

        public IActionResult OnPost(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return BadRequest("Chat room name cannot be empty.");
            }

            var chatRoom = new ChatRoom { Name = roomName };
            _context.ChatRooms.Add(chatRoom);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteRoom(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return BadRequest("Chat room name cannot be empty.");
            }

            var chatRoom = new ChatRoom { Name = roomName };
            var deletedRoom = _context.ChatRooms.Remove(chatRoom);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}
