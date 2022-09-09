using LibraryManagement;
class Program
{   public static void Main()
    {
        LibraryManage library=new LibraryManage();
        library.Start();
        SQLConnection.sql.Close();
    }
}