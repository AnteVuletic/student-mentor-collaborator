namespace StudentMentor.Domain.Models.ViewModels
{
    public class SendMessageModel
    {
        public int UserFromId { get; set; }
        public int UserToId { get; set; }
        public string Content { get; set; }
    }
}
