using System.Data.SqlClient;

class User
{
    private string username;
    private string password;
    public User()
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
        SqlCommand cmd=SQLConnection.GetSQLCommand();
        cmd.CommandText="select password from users where username='"+username+"';";
        SqlDataReader reader = cmd.ExecuteReader();
        
        if (reader.HasRows)
        {
            reader.Read();
            if(!password.Equals(reader.GetString(0)))
            {
                System.Console.WriteLine("Invalid username/password");
                Environment.Exit(0);
            }
            reader.Close();
            ChooseAction(cmd);
        }
        else 
        {
            System.Console.WriteLine("Invalid username/password");
            Environment.Exit(0);
        }
        
    }

    public void ChooseAction(SqlCommand cmd)
    {  
       again:
        System.Console.WriteLine();
        System.Console.WriteLine("1. Lend a Book");
        System.Console.WriteLine("2. Return a Book");
        System.Console.WriteLine("3. Books With Me");
        System.Console.WriteLine("4. Logout");
        System.Console.WriteLine("5. Exit ");
        System.Console.WriteLine();
        System.Console.Write("Enter your choice : ");
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
                case 1 : Act.LendingBook(cmd, username);
                         break;   

                case 2 : Act.ReturnBook(cmd, username, this);
                         break;

                case 3 : Act.BooksWithMe(cmd,username);
                         break;

                case 4 : SQLConnection.sql.Close();
                         System.Console.WriteLine("\nSuccessfully Logged out\n");
                         Program.Main();
                         break;

                case 5 : SQLConnection.sql.Close();
                         System.Console.WriteLine("\nThank You !!\n");
                         Environment.Exit(0);
                         break;

                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
    }
}