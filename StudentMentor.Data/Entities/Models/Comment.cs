namespace StudentMentor.Data.Entities.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
