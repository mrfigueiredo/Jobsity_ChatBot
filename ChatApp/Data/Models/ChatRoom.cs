namespace ChatApp.Data.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
