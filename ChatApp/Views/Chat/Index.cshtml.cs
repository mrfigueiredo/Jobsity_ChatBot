using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data.Models;
using ChatApp.Data;

namespace ChatApp.Views.Chat
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ChatRoom> ChatRooms { get; set; }

        public async Task OnGetAsync()
        {
            ChatRooms = await _context.ChatRooms.ToListAsync();
        }
    }
}
