using System.Data.SqlClient;
using LibraryManagement;

class Admin
{
    private string username="";
    private string password="";
    public Admin()
    {
        System.Console.WriteLine();
        System.Console.Write("Enter Username : ");
        this.username = Console.ReadLine().Trim();
        System.Console.Write("Enter password : ");
        password = Console.ReadLine();
        Validate();
    }
    private void Validate()
    {
        if(username.Equals("Admin") && password.Equals("Admin"))
        {
            SqlCommand cmd=SQLConnection.GetSQLCommand();
            ChooseAdminAction(cmd);
        }
        else 
        {
            System.Console.WriteLine("\nInvalid username/password");
            LibraryManage library=new LibraryManage();
            library.Start();
        } 
    }
    public void ChooseAdminAction(SqlCommand cmd)
    {
        again:
        System.Console.WriteLine("\n--------Menu-------\n");
        System.Console.WriteLine("1.Add New Books");
        System.Console.WriteLine("2.Add Existing Books");
        System.Console.WriteLine("3.Delete Existing Books");
        System.Console.WriteLine("4.Books Details");
        System.Console.WriteLine("5.User details");
        System.Console.WriteLine("6.Orders");
        System.Console.WriteLine("7.Logout");
        System.Console.WriteLine("8.Exit");
        System.Console.Write("\nEnter your choice : ");
        int choice=0;
        BookAction Act;
            try{
                choice = Convert.ToInt32(Console.ReadLine());
                Act=new BookAction();
            }
            catch(Exception)
            {
                System.Console.WriteLine("Enter valid input !");
                goto again;
            }
            switch (choice)
            {
                case 1 : Act.AddNewBook(cmd, this);
                         break;   

                case 2 : Act.AddExistingBook(cmd, this);
                         break;
                case 3 : Act.DeleteExistingBooks(cmd,this);
                         break;
                case 4 : Act.BookDetails(cmd,this);
                         break;
                case 5 : Act.UserDetails(cmd,this);
                         break;
                case 6 : Act.Orders(cmd,this);
                         break;
                case 7 : SQLConnection.sql.Close();
                         new LibraryManagement.LibraryManage().Start();
                         break;
                case 8 : SQLConnection.sql.Close();
                         System.Console.WriteLine("\nThank You !!\n");
                         Environment.Exit(0);
                         break;

                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
        }

}