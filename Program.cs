using LibraryManagement;
class Program
{   public static void Main()
    {
        System.Console.WriteLine("\t\t----- Welcome to the Library -----");
        LibraryManage library=new LibraryManage();
        library.Start();
        SQLConnection.sql.Close();
    }
}