using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Channel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Message
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ChannelId { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Color { get; set; }
        public long Timestamp { get; set; }
    }
}
