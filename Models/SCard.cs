

namespace DapperPractice1.Models
{
    internal class SCard
    {
        public SCard(int id, int id_Student, int id_Book)
        {
            Id = id;
            Id_Student = id_Student;
            Id_Book = id_Book;
            DateOut = DateTime.Now;
            Id_Lib = 1;
        }

        public int Id { get; set; }
        public int Id_Student { get; set; }
        public int Id_Book { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime? DateIn { get; set; }
        public int Id_Lib { get; set; }

    }
}
