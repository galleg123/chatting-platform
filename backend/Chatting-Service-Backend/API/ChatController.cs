using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chatting_Service_Backend.Data;
using Chatting_Service_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Chatting_Service_Backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly DatabaseConnector _db;

        public ChatController(DatabaseConnector db)
        {
            _db = db;
        }

        // Get all chatrooms
        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _db.Chats.ToListAsync();
            return Ok(chats);
        }

        // Get all chatrooms for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllChatsForUser(int userId)
        {
            // Find the user by ID
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Get the chatrooms the user is a member of
            var chatrooms = await _db.Chats
                .Where(c => user.Chatrooms.Contains(c.Id))
                .ToListAsync();

            return Ok(chatrooms);
        }

        // Get a specific chatroom by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {
            var chat = await _db.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound("Chatroom not found.");
            }
            return Ok(chat);
        }


        public class CreateChatRequest{
            public required Chat Chat { get; set; }
            public int UserId { get; set; }
        }
        // Create a new chatroom
        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            var chat = request.Chat;
            var userId = request.UserId;

            // Add the chat to the database and save changes to generate the ID
            _db.Chats.Add(chat);
            await _db.SaveChangesAsync();
            Console.WriteLine($"Chatroom created with ID: {chat.Id}");

            // Find the user by ID
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Add the generated chat ID to the user's chatrooms
            user.Chatrooms.Add(chat.Id);
            _db.Entry(user).State = EntityState.Modified; // Mark the user entity as modified

            // Save changes to update the user's chatrooms
            await _db.SaveChangesAsync();
            // Return the created chatroom
            return CreatedAtAction(nameof(GetChatById), new { id = chat.Id }, chat);
        }


        public class AddUserToChatRequest{
            public required string Name { get; set; }
            public int UserId {get; set;}
        }
        // Add a user to a chatroom
        [HttpPost("join")]
        public async Task<IActionResult> AddUserToChat([FromBody] AddUserToChatRequest request)
        {
            var userId = request.UserId;
            var name = request.Name;

            var chat = await _db.Chats.FirstOrDefaultAsync((c => c.Name == name));
            if (chat == null)
            {
                return NotFound("Chatroom not found.");
            }

            // Find the user by ID
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Add the chat ID to the user's chatrooms
            user.Chatrooms.Add(chat.Id);
            _db.Entry(user).State = EntityState.Modified; // Mark the user entity as modified

            // Save changes to update the user's chatrooms
            await _db.SaveChangesAsync();
            return Ok(chat);
        }


        // Update an existing chatroom
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChat(int id, [FromBody] Chat updatedChat)
        {
            var chat = await _db.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound("Chatroom not found.");
            }

            chat.Name = updatedChat.Name;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Delete a chatroom
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            var chat = await _db.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound("Chatroom not found.");
            }

            _db.Chats.Remove(chat);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
