namespace Chatting_Service_Backend.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Message> Messages { get; set; } = new();
    }
}