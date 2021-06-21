namespace StudentMentor.Data.Entities.Models
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public Message Message { get; set; }
        public Student Student { get; set; }
    }
}
