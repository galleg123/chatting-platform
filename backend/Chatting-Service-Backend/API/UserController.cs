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

        public class UpdateColorRequest{
            public required string Color { get; set; }
        }

        [HttpPut("color/{id}")]
        public async Task<IActionResult> UpdateColor(int id, [FromBody] UpdateColorRequest request)
        {
            // Find the user by ID
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update the user's color
            user.Color = request.Color;
            await _db.SaveChangesAsync();

            return Ok(user);
        }
    }
}