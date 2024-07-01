using ChatApp.Data;
using ChatApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Chat/ChatRooms
        [HttpGet("ChatRooms")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetChatRooms()
        {
            return await _context.ChatRooms.ToListAsync();
        }

        // POST: api/Chat/ChatRooms
        [HttpPost("ChatRooms")]
        public async Task<ActionResult<ChatRoom>> CreateChatRoom([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Chat room name cannot be empty.");
            }

            var chatRoom = new ChatRoom { Name = name };
            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChatRooms), new { id = chatRoom.Id }, chatRoom);
        }

        // GET: api/Chat/Messages/{chatRoomId}
        [HttpGet("Messages/{chatRoomId}")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetRecentMessages(int chatRoomId)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom == null)
            {
                return NotFound("Chat room not found.");
            }

            var messages = await _context.ChatMessages
                                         .Where(m => m.ChatRoomId == chatRoomId)
                                         .OrderByDescending(m => m.Timestamp)
                                         .Take(50)
                                         .ToListAsync();

            return messages;
        }

        // POST: api/Chat/Messages
        [HttpPost("Messages")]
        public async Task<ActionResult<ChatMessage>> PostMessage([FromBody] ChatMessage message)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(message.ChatRoomId);
            if (chatRoom == null)
            {
                return NotFound("Chat room not found.");
            }

            message.Timestamp = DateTime.Now;
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecentMessages), new { chatRoomId = message.ChatRoomId }, message);
        }
    }
}
