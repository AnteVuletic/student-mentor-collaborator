using System;
using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Data.Entities
{
    public class StudentFile
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int FileId { get; set; }
        public File File { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
    }
}
