namespace Chatting_Service_Backend.Models{
    public class Message
    {
        public int Id { get; set; } // Primary key
        public string MessageText { get; set; } = null!; // Required
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public int ChatroomId { get; set; } // Foreign key
        public int SenderId { get; set; } // Foreign key
        public User? Sender { get; set; } = null!; // Navigation property
        public Chat? Chatroom { get; set; } = null!; // Nagivation property
    }
    public class MessageDTO
    {
        public int Id { get; set; }        
        public string MessageText { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public UserDTO? Sender { get; set; } = null!;
        public int ChatroomId { get; set; }
    }
}