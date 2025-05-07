using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chatting_Service_Backend.Data;
using Chatting_Service_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Chatting_Service_Backend.Hubs;

namespace Chatting_Service_Backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly DatabaseConnector _db;

        public MessageController(DatabaseConnector db)
        {
            _db = db;
        }

        // Get all messages for a specific chatroom
        [HttpGet("chatroom/{chatroomId}")]
        public async Task<IActionResult> GetMessagesForChatroom(int chatroomId)
        {
            var messages = await _db.Messages
                .Where(m => m.ChatroomId == chatroomId)
                .OrderBy(m => m.Timestamp) // Order messages by timestamp
                .Include(m => m.Sender) // Include sender information
                .ToListAsync();

            var messageDTOs = messages.Select(m => new MessageDTO
            {
                Id = m.Id,
                MessageText = m.MessageText,
                Timestamp = m.Timestamp,
                ChatroomId = m.ChatroomId,
                Sender = m.Sender != null ? new UserDTO{
                    Username = m.Sender.Username,
                    Color = m.Sender.Color,
                } : null
            }).ToList();
            return Ok(messageDTOs);
        }

        // Add a new message to a chatroom
        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] Message message, [FromServices] IHubContext<ChatHub> chatHub)
        {
            if (message.SenderId <= 0 || string.IsNullOrWhiteSpace(message.MessageText))
            {
                return BadRequest("SenderId and MessageText are required.");
            }

            var chatroom = await _db.Chats.FindAsync(message.ChatroomId);
            if (chatroom == null)
            {
                return NotFound("Chatroom not found.");
            }

            var sender = await _db.Users.FindAsync(message.SenderId);
            if (sender == null)
            {
                return NotFound("Sender not found.");
            }

            // Add the message to the database
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            // Notify all clients in the chatroom about the new message

            // Convert the Message object to a MessageDTO
            var messageDto = new MessageDTO
            {
                Id = message.Id,
                MessageText = message.MessageText,
                Timestamp = message.Timestamp,
                Sender = new UserDTO{
                    Username = sender.Username,
                    Color = sender.Color,
                },
                ChatroomId = message.ChatroomId
            };
            await chatHub.Clients.Group(message.ChatroomId.ToString())
                .SendAsync("ReceiveMessage", messageDto);

            return CreatedAtAction(nameof(GetMessagesForChatroom), new { chatroomId = message.ChatroomId }, messageDto);
        }

        // Delete a specific message by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _db.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound("Message not found.");
            }

            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
