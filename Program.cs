using System.Data;
using System.Data.SqlClient;
using System.Net;
using Dapper;
using DapperPractice1.Data;
using DapperPractice1.Models;
using static System.Reflection.Metadata.BlobBuilder;

//var conStr = $"Data Source=LAPTOP-BG3587VM\\MSSQLSERVER01;Initial Catalog=Library;Integrated Security=true";
//using var sqlConnection = new SqlConnection(conStr);


using var db = new Database($"Data Source=LAPTOP-BG3587VM\\MSSQLSERVER01;Initial Catalog=Library;Integrated Security=true");

Console.Write("Enter your Student ID: ");
int studentId = int.Parse(Console.ReadLine());

if (IsValidStudent(studentId, db))
{
    ShowMenu(studentId, db);
}
else
{
    Console.WriteLine("Invalid Student ID. Exiting...");
}

static bool IsValidStudent(int studentId, Database db)
{
    var studentCount = db.Connection.QuerySingleOrDefault<int>("SELECT COUNT(*) FROM Students WHERE Id = @StudentId", new { StudentId = studentId });
    return studentCount > 0;
}
//static void SeeBorrowedBooks(int studentId, Database db)
//{
//    var borrowedBooks = db.Connection.Query<Book>("SELECT b.Id, b.Name FROM Books b JOIN S_Cards s ON b.Id = s.Id_Book WHERE s.Id_Student = @StudentId AND s.DateIn IS NULL", new { StudentId = studentId }).ToList();

//    if (borrowedBooks.Any())
//    {
//        Console.WriteLine("\nBooks You Borrowed:");
//        foreach (var book in borrowedBooks)
//        {
//            Console.WriteLine($"ID: {book.Id}, Name: {book.Name}");
//        }
//    }
//    else
//    {
//        Console.WriteLine("You haven't borrowed any books.");
//    }
//}
static void ShowMenu(int studentId,Database db)
{
    while (true)
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. See All Books");
        Console.WriteLine("2. See the Books You Borrowed");
        Console.WriteLine("3. Exit");

        Console.Write("Choose an option: ");
        string option = Console.ReadLine();
        switch (option)
        {
            case "1":
                    List<Book> books = db.GetAllBooks();
                    foreach(Book book in books)
                    {
                        Console.WriteLine($"ID:{book.Id},Name:{book.Name}");
                    }
                Console.WriteLine("Enter the ID of the book which you want to borrow:");
                int borrowOption = int.Parse(Console.ReadLine());
                db.TakeBook(studentId, borrowOption);
                break;
            case "2":
                
                List<Book> borrowedBooks=db.BorrowedBooks(studentId);
                foreach(Book book in borrowedBooks)
                {
                    Console.WriteLine($"ID:{book.Id},Name:{book.Name},Quantity:{book.Quantity}");
                }
                Console.WriteLine("Enter the ID of the book which you want to return:");
                int ReturnOption = int.Parse(Console.ReadLine());
                db.ReturnBook(studentId, ReturnOption);
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }
}

  