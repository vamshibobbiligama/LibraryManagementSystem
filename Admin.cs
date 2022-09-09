using System.Data.SqlClient;

class Admin
{
    private string username;
    private string password;
    public Admin()
    {
        System.Console.WriteLine();
        System.Console.Write("Enter Username : ");
        username = Console.ReadLine().Trim();
        System.Console.Write("Enter password : ");
        password = Console.ReadLine();
        Validate();
    }
    private void Validate()
    {
        if(username.Equals("Harsha") && password.Equals("Harsh"))
        {
            SqlCommand cmd=SQLConnection.GetSQLCommand();
            ChooseAdminAction(cmd);
        }
        else 
        {
            System.Console.WriteLine("Invalid username/password");
            Environment.Exit(0);
        } 
    }
    private void ChooseAdminAction(SqlCommand cmd)
    {
        again:
        System.Console.WriteLine("--------Menu-------");
        System.Console.WriteLine("1.Add New Books");
        System.Console.WriteLine("2.Add Existing Books");
        System.Console.WriteLine("3.Delete Existing Books");
        System.Console.WriteLine("4.User details");
        System.Console.WriteLine("5.Orders");
        System.Console.WriteLine("6.Exit");
        int choice=0;
        BookAction Act;
            try{
                choice = Convert.ToInt32(Console.ReadLine());
                Act=new BookAction();
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Enter valid input !");
                goto again;
            }
            switch (choice)
            {
                case 1 : Act.AddNewBook(cmd, this);
                         break;   

                case 2 : Act.ReturnBook(cmd, username, this);
                         break;
                case 6 : SQLConnection.sql.Close();
                         System.Console.WriteLine("\nThank You !!\n");
                         Environment.Exit(0);
                         break;

                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
        }

}