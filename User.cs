using System.Data.SqlClient;

class User
{
    private string username;
    private string password;
    public User()
    {
        ChooseLoginType();
    }

    private void ChooseLoginType()
    {
        again:
            System.Console.WriteLine();
            Console.WriteLine("1.Login");
            Console.WriteLine("2.Register");
            Console.WriteLine("3.Exit");
            Console.Write("\nEnter your Choice : ");
            int choice=0;
            try{
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                System.Console.WriteLine("\n\t\t\t!!! Enter valid input !!!");
                goto again;
            }
            switch (choice)
            {
                case 1 : Login();
                         break;   
                case 2 : Register();
                         break;
                case 3 : Console.WriteLine("\n\t\t\t-----!!! Thank You !!!-----");
                         SQLConnection.sql.Close();
                         Environment.Exit(0);
                         break;
                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
    }


    private void Register()
    {
        SqlCommand cmd=SQLConnection.GetSQLCommand();
        again:
            System.Console.Write("Enter the Username : ");
            string uname=Console.ReadLine();
            cmd.CommandText="select * from users where username='"+uname+"';";
            SqlDataReader reader=cmd.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Close();
                System.Console.WriteLine("\n\t\t\t!!! Username already exits, Choose another username !!!");
                goto again;
            }
            reader.Close();
            System.Console.Write("Enter password : ");
            string pass=Console.ReadLine();
            cmd.CommandText="insert into users values('"+uname+"','"+pass+"');";
            cmd.ExecuteNonQuery();
            System.Console.WriteLine("\n\t\t\t!!! Registered Successfully :) !!!\n");
            SQLConnection.sql.Close();
            ChooseLoginType();
    }



    private void Login()
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
                Login();
            }
            reader.Close();
            ChooseAction(cmd);
        }
        else 
        {
            System.Console.WriteLine("Invalid username/password");
            Login();
        }
        
    }

    public void ChooseAction(SqlCommand cmd)
    {  
       again:
        System.Console.WriteLine("\n--------Menu-------\n");
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
            catch(Exception)
            {
                System.Console.WriteLine("Enter valid input !");
                goto again;
            }
            switch (choice)
            {
                case 1 : Act.LendingBook(cmd, username, this);
                         break;   

                case 2 : Act.ReturnBook(cmd, username, this);
                         break;

                case 3 : Act.BooksWithMe(cmd,username, this, true);
                         break;

                case 4 : SQLConnection.sql.Close();
                         System.Console.WriteLine("\nSuccessfully Logged out\n");
                         Program.Main();
                         break;

                case 5 : SQLConnection.sql.Close();
                         System.Console.WriteLine("\n\t\t\t!!! Thank You !!!\n");
                         Environment.Exit(0);
                         break;

                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
    }
}