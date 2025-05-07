using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chatting_Service_Backend.Models;
using Chatting_Service_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatting_Service_Backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseConnector _db;

        public UserController(DatabaseConnector db)
        {
            _db = db;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Check if the username already exists
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            // Add the new user to the database
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            // Check if the user exists and the password matches
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(existingUser);
        }
        [HttpPut("chat/add/{id}")]
        public async Task<IActionResult> AddChatroom(int id, [FromBody] int chatroomId)
        {
            // Find the user by ID
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found."); 
            }

            // Add the chatroom ID to the user's list of chatrooms
            user.Chatrooms.Add(chatroomId);
            await _db.SaveChangesAsync();

            return Ok("Chatroom added successfully.");
        }

    }
}