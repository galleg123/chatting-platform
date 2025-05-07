namespace Chatting_Service_Backend.Models{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<int> Chatrooms { get; set; } = new();
    }
}