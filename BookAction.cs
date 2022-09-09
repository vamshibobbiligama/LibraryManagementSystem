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
            System.Console.WriteLine("No Books available ,pls contact Dr.Avinash(Admin)");
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
        catch(Exception e)
        {
            System.Console.WriteLine("Enter valid input !");
            goto again;
        }
        /*
        if user enter book id whose quantity is = 0
        */
        
        try
        {   
            cmd.CommandText="select * from orders where bid="+choice+" and uid=(select uid from users where username = '"+username+"')"; 
            reader=cmd.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Close();
                System.Console.WriteLine("\nYou already have one");
                goto again;
            }
            reader.Close();
            cmd.CommandText="select * from books where bid="+choice+" and BooksCount>0;";
            reader=cmd.ExecuteReader();
            if(!reader.HasRows)
            {
                reader.Close();
                throw new Exception();
            }
            reader.Close();
            OrderConfirmed(choice, cmd, username);
            user.ChooseAction(cmd);
        }
        catch (System.Exception e)
        {
            System.Console.WriteLine("\nEnter Valid Book Id !");
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
            System.Console.WriteLine("\nYou have no books to return !");
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
            catch(Exception e)
            {
                System.Console.WriteLine("Enter valid input !");
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
            System.Console.WriteLine("Book returned successfully !");
            user.ChooseAction(cmd);
        }
        else 
        {
            reader.Close();
            System.Console.WriteLine("Please enter valid book ID");
            goto again;
        }
    }


    private void OrderConfirmed(int bookid, SqlCommand cmd, string username)
    {
        cmd.CommandText="update books set BooksCount = BooksCount-1 where bid="+bookid+";";
        cmd.ExecuteNonQuery();
        cmd.CommandText="insert into Orders values("+bookid+",(select uid from users where username='"+username+"'))";
        cmd.ExecuteNonQuery();
        System.Console.Write("\nBook issued successfully, Enjoy !!\n");
    }

    public void BooksWithMe(SqlCommand cmd, string username, User user, bool chk)
    {
        cmd.CommandText="select bid, bookName from books where bid in (select bid from orders where uid=(select uid from users where username='"+username+"'))";
        SqlDataReader reader=cmd.ExecuteReader();
        if(!reader.HasRows)
            Console.WriteLine("\nYou don't have any books to return !");
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
        user.ChooseAction(cmd);
    }


    public void AddNewBook(SqlCommand cmd, Admin admin)
    {
        string BookName = Console.ReadLine().Trim();
        int BookCount=0;
        try
        {
            BookCount = Convert.ToInt32(Console.ReadLine());
        }
        catch (System.Exception)
        {
            System.Console.WriteLine();
        }
        cmd.CommandText
    }

}