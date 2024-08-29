using Dapper;
using DapperPractice1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperPractice1.Data
{
    internal class Database:IDisposable
    {
        public SqlConnection Connection;
        public Database(string connectionString) {
            Connection = new SqlConnection(connectionString);
        }

        public Student Login(int studentId)
        {
            var query = "Select * from Students where Id=@studentId";
            var student= Connection.QueryFirstOrDefault<Student>(query, param: new{studentId=studentId });
            return student;
        }
        public List<Book> GetAllBooks()
        {
            var query = "select * from Books Where Quantity>0";
            var books = Connection.Query<Book>(query).ToList();
            return books;
        }
        public void TakeBook(int IdStudent, int IdBook)
        {
            var query = $"SELECT TOP 1 Id FROM dbo.S_Cards ORDER BY Id DESC;";
            SCard scard = new SCard ( Connection.ExecuteScalar<int>(query)+1, IdStudent, IdBook );
            var command = "Insert into S_Cards (Id,Id_Student, Id_Book, DateOut,DateIn,Id_Lib) VALUES(@Id,@Id_Student, @Id_Book, @DateOut,@DateIn, @Id_Lib)";
            Connection.Execute(command, param: scard);
        }

        public List<Book> BorrowedBooks(int IdStudent)
        {
            var borrowedBooks = Connection.Query<Book>("SELECT b.Id, b.Name FROM Books b JOIN S_Cards s ON b.Id = s.Id_Book WHERE s.Id_Student = @StudentId AND s.DateIn IS NULL", new { StudentId = IdStudent }).ToList();
            return borrowedBooks;
        }

        public void ReturnBook(int IdStudent, int IdBook)
        {
            var QueryForScardId = " Select Id FROM S_Cards WHERE Id_Student = @IdStudent AND Id_Book = @IdBook";
            int ScardId = Connection.ExecuteScalar<int>(QueryForScardId, param: new {IdStudent,IdBook});
            if (ScardId != 0) 
            {
                
                var command = "UPDATE S_Cards SET DateIn = GETDATE() WHERE Id = @ScardId";
                Connection.Execute(command, new { ScardId });
            }
            else
            {
                Console.WriteLine("No matching record found to return the book.");
            }
//id-ə uyğun scard-obyektini necə tapa bilərəm
        }



        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }

        ~Database()
        {
            Dispose();
        }
    }
}
