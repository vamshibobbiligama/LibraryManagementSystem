using System.Data.SqlClient;
class BookAction
{
    //User 
    public void LendingBook(SqlCommand cmd, string username, User user)
    {
        again:
        cmd.CommandText="select bid,bookName from books where BooksCount>0";
        SqlDataReader reader = cmd.ExecuteReader();
        if(reader.HasRows)
        {   
            System.Console.WriteLine();
            Console.WriteLine("Books that are Available : ");
            System.Console.WriteLine();
            Console.WriteLine("Id   |  BookName");
            Console.WriteLine("----------------");
            while (reader.Read())
                System.Console.WriteLine(reader.GetInt32(0)+"  |  "+reader.GetString(1));   
        }
        else 
        {
            System.Console.WriteLine("\t\t\t!!! No Books available !!!");
            SQLConnection.sql.Close();
            Environment.Exit(0);
        }
        reader.Close();
        System.Console.WriteLine();
        System.Console.Write("Enter BookId : ");
        int choice=0;
        try{
            choice = Convert.ToInt32(Console.ReadLine());
        }
        catch(Exception)
        {
            System.Console.WriteLine("\t\t\t!!! Enter valid input !!!");
            goto again;
        }
        /*
        if user enter book id whose quantity is 0
        */
        try
        {   
            cmd.CommandText="select * from books where bid="+choice+" and BooksCount>0;";
            reader=cmd.ExecuteReader();
            if(!reader.HasRows)
            {
                reader.Close();
                throw new Exception();
            }
            reader.Close();
            cmd.CommandText="select * from orders where bid="+choice+" and uid=(select uid from users where username = '"+username+"')"; 
            reader=cmd.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Close();
                System.Console.WriteLine("\n\t\t\t!!! You already have one !!!");
                goto again;
            }
            reader.Close();
            OrderConfirmed(choice, cmd, username);
            user.ChooseAction(cmd);
        }
        catch (System.Exception)
        {
            System.Console.WriteLine("\n\t\t\t!!! Enter Valid Book Id !!!");
            goto again;
        }
    }



    public void ReturnBook(SqlCommand cmd, string username, User user)
    {
        again:
        /*
        @ comment
        If User has no books to return ,then the control shifts back to previous menu.
        */
        cmd.CommandText="select bid from orders where uid=(select uid from users where username = '"+username+"');";
        SqlDataReader reader = cmd.ExecuteReader();
        if(!reader.HasRows)
        {
            System.Console.WriteLine("\n\t\t\t!!! You have no books to return !!!");
            reader.Close();
            user.ChooseAction(cmd);
            return;
        }
        reader.Close();
        BooksWithMe(cmd,username,user,false);
        System.Console.WriteLine();
        System.Console.Write("Enter BookId : ");
        int bid=0;
            try{
                bid = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                System.Console.WriteLine("\t\t\t!!! Enter valid input !!!");
                goto again;
            }
        cmd.CommandText="select * from orders where bid="+bid+"and uid=(select uid from users where username='"+username+"');";
        reader = cmd.ExecuteReader();
        if(reader.HasRows)
        {   
            reader.Close();
            cmd.CommandText="update books set BooksCount = BooksCount+1 where bid="+bid+";";  
            cmd.ExecuteNonQuery();
            cmd.CommandText="delete from orders where bid="+bid+"and uid=(select uid from users where username='"+username+"');";
            cmd.ExecuteNonQuery();
            System.Console.WriteLine("\t\t\t!!! Book returned successfully !!!");
            System.Console.WriteLine("\nPress Enter to go back to Previous Menu....");
            // Console.ReadLine();
            user.ChooseAction(cmd);
        }
        else 
        {
            reader.Close();
            System.Console.WriteLine("\t\t\t!!! Please enter valid book ID !!!");
            goto again;
        }
    }



    private void OrderConfirmed(int bookid, SqlCommand cmd, string username)
    {
        cmd.CommandText="update books set BooksCount = BooksCount-1 where bid="+bookid+";";
        cmd.ExecuteNonQuery();
        cmd.CommandText="insert into Orders values("+bookid+",(select uid from users where username='"+username+"'))";
        cmd.ExecuteNonQuery();
        System.Console.Write("\n\t\t\t!!! Book issued successfully, Enjoy !!!\n");
        System.Console.WriteLine("\nPress Enter to go back to Previous Menu....");
        Console.ReadLine();
    }



    public void BooksWithMe(SqlCommand cmd, string username, User user, bool chk)
    {
        cmd.CommandText="select bid, bookName from books where bid in (select bid from orders where uid=(select uid from users where username='"+username+"'))";
        SqlDataReader reader=cmd.ExecuteReader();
        if(!reader.HasRows)
            Console.WriteLine("\n\t\t\t!!! You don't have any books to return !!!");
        else
        {
            System.Console.WriteLine("\nBookId    |    Book Name");
            System.Console.WriteLine("----------------------------");
            while (reader.Read())
            {
                System.Console.WriteLine("  "+reader.GetInt32(0)+"     |    "+reader.GetString(1));
            }
        }
        reader.Close();
        if(chk)
        {
            System.Console.WriteLine("\nPress Enter to go back to Previous Menu....");
            Console.ReadLine();
            user.ChooseAction(cmd);
        }
    }



    public void AddNewBook(SqlCommand cmd, Admin admin)
    {
        again:
            System.Console.Write("Enter Book Name : ");
            string BookName = Console.ReadLine().Trim();
            int BookCount=0;
            try
            {
                System.Console.Write("Enter Book Count : ");
                BookCount = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception )
            {
                System.Console.WriteLine("\t\t\tPlease enter valid number!!");
                goto again;
            }
            cmd.CommandText="select * from books where bid='"+BookName+"';";
            SqlDataReader reader=cmd.ExecuteReader();
            if(reader.HasRows)
            {
                System.Console.WriteLine("\n\t\t\t!!! Book already exists !!!");
                reader.Close();
                goto again;
            }
            reader.Close();
            cmd.CommandText="insert into books values('"+BookName+"',"+BookCount+");";
            cmd.ExecuteNonQuery();
            System.Console.WriteLine("\n\t\t\t!!! Book inserted successfully !!!\n");
            System.Console.WriteLine("\nPress Enter to go back to Previous Menu....");
            Console.ReadLine();
            admin.ChooseAdminAction(cmd);
    }



    public void AddExistingBook(SqlCommand cmd,Admin admin)
    {
        again:
            cmd.CommandText="select * from books";
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {   
                System.Console.WriteLine();
                Console.WriteLine("Books that are stored : ");
                System.Console.WriteLine();
                Console.WriteLine("Id   |  BookName (Book Count)");
                Console.WriteLine("---------------------------------------------");
                while (reader.Read())
                    System.Console.WriteLine(reader.GetInt32(0)+"  |  "+reader.GetString(1)+" ("+reader.GetInt32(2)+")");   
            }
            else 
            {
                System.Console.WriteLine("\t\t\t!!! No Books available !!!");
                SQLConnection.sql.Close();
                reader.Close();
                admin.ChooseAdminAction(cmd);
            }
            reader.Close();
            System.Console.WriteLine();
            System.Console.Write("Enter BookId : ");
            int BookId=0;
            try{
                BookId = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                System.Console.WriteLine("\n\t\t\t!!! Enter valid input !!!");
                goto again;
            }
            System.Console.Write("Enter number of books you want to add : ");
            int BookCount=0;
            try{
                BookCount = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                System.Console.WriteLine("\n\t\t\t !!! Enter valid input !!!");
                goto again;
            }
            cmd.CommandText="update books set BooksCount=BooksCount+"+BookCount+" where bid="+BookId+";";
            cmd.ExecuteNonQuery();
            System.Console.WriteLine("\n\t\t\t!!! Books Count Updated successfully !!!");
            System.Console.WriteLine("\nPress Enter to go back to Previous Menu....");
            Console.ReadLine();
            admin.ChooseAdminAction(cmd);
    }



    public void DeleteExistingBooks(SqlCommand cmd,Admin admin)
    {
        again:
            cmd.CommandText="select * from books";
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {   
                System.Console.WriteLine();
                Console.WriteLine("Books that are stored : ");
                System.Console.WriteLine();
                Console.WriteLine("Id   |  BookName (Book Count)");
                Console.WriteLine("---------------------------------------------");
                while (reader.Read())
                    System.Console.WriteLine(reader.GetInt32(0)+"  |  "+reader.GetString(1)+" ("+reader.GetInt32(2)+")");   
            }
            else 
            {
                System.Console.WriteLine("\t\t\t!!! No Books available !!!");
                admin.ChooseAdminAction(cmd);
            }
            reader.Close();
            System.Console.WriteLine();
            System.Console.Write("Enter BookId : ");
            int BookId=0;
            try{
                BookId = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                System.Console.WriteLine("Enter valid input !");
                goto again;
            }
            cmd.CommandText="select * from books where bid="+BookId+";";
            reader=cmd.ExecuteReader();
            if(reader.HasRows)
            {
                cmd.CommandText="delete from books where bid="+BookId+";";
                System.Console.WriteLine("\n\t\t\t!!! Book deleted successfully !!!");
                reader.Close();
                System.Console.WriteLine("\nPress Enter to go back to previous menu....");
                Console.ReadLine();
                admin.ChooseAdminAction(cmd);
            }
            else
            {
                System.Console.WriteLine("\n\t\t\t!!! Enter valid Book ID !!!");
                reader.Close();
                goto again;
            }
    }



    public void BookDetails(SqlCommand cmd,Admin admin)
    {
        cmd.CommandText="select * from books";
        SqlDataReader reader = cmd.ExecuteReader();
        if(reader.HasRows)
        {   
            System.Console.WriteLine();
            Console.WriteLine("Books that are stored : ");
            System.Console.WriteLine();
            Console.WriteLine("Id   |  BookName (Book Count)");
            Console.WriteLine("---------------------------------------------");
            while (reader.Read())
                System.Console.WriteLine(reader.GetInt32(0)+"  |  "+reader.GetString(1)+" ("+reader.GetInt32(2)+")");   
        }
        else 
        {
            System.Console.WriteLine("\t\t\t!!! No Books available !!!");
            SQLConnection.sql.Close();
            reader.Close();
            admin.ChooseAdminAction(cmd);
        }
        reader.Close();
        System.Console.WriteLine("\nPress Enter to go back to previous menu....");
        Console.ReadLine();
        admin.ChooseAdminAction(cmd);
    }



    public void UserDetails(SqlCommand cmd,Admin admin)
    {
        cmd.CommandText="select * from users";
        SqlDataReader reader = cmd.ExecuteReader();
        if(reader.HasRows)
        {   
            System.Console.WriteLine();
            Console.WriteLine("User Details : ");
            System.Console.WriteLine();
            Console.WriteLine("Id |  Users");
            Console.WriteLine("---------------------------------------------");
            while (reader.Read())
                System.Console.WriteLine(reader.GetInt32(0)+"  |  "+reader.GetString(1));   
        }
        else 
        {
            System.Console.WriteLine("\t\t\t!!! No Users found !!!");
        }
        reader.Close();
        System.Console.WriteLine("\nPress Enter to go back to previous Menu....");
        System.Console.ReadLine();
        admin.ChooseAdminAction(cmd);

    }




    public void Orders(SqlCommand cmd, Admin admin)
    {
        cmd.CommandText="select * from orders";
        SqlDataReader reader = cmd.ExecuteReader();
        if(reader.HasRows)
        {   
            System.Console.WriteLine();
            Console.WriteLine("Orders : ");
            System.Console.WriteLine();
            Console.WriteLine("Order Id  |  Book Id   |  User Id  ");
            Console.WriteLine("----------------------------------");
            while (reader.Read())
                System.Console.WriteLine(reader.GetInt32(0)+"     |     "+reader.GetInt32(1)+"    |     "+reader.GetInt32(2));   
        }
        else 
        {
            System.Console.WriteLine("\n\t\t\t!!! No Orders found !!!\n");
        }
        reader.Close();
        System.Console.WriteLine("\nPress Enter to go back to previous Menu....");
        System.Console.ReadLine();
        admin.ChooseAdminAction(cmd);
    }
}