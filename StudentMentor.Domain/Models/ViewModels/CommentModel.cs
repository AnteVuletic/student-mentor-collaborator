namespace StudentMentor.Domain.Models.ViewModels
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public UserModel UserFrom { get; set; }
        public int UserMessageToId { get; set; }
        public int MessageId { get; set; }
    }
}
