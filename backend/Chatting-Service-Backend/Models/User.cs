namespace Chatting_Service_Backend.Models{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Color { get; set; } = "#007bff"; // Default color
        public List<int> Chatrooms { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }

    public class UserDTO
    {
        public string Username { get; set; } = null!;
        public string Color { get; set; } = null!;
    }
}