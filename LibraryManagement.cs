using System;
namespace LibraryManagement
{
    class LibraryManage
    {
        public void Start()
        {   
            again:
            System.Console.WriteLine();
            Console.WriteLine("1.User");
            Console.WriteLine("2.Admin");
            Console.WriteLine("3.Exit");
            Console.Write("\nEnter your Choice : ");
            int choice=0;
            try{
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                System.Console.WriteLine("\nEnter valid input !");
                goto again;
            }
            switch (choice)
            {
                case 1 : new User();
                         break;   
                case 2 : new Admin();
                         break;
                case 3 : Console.WriteLine("\n\t\t\t!!! Thank You !!!\n");
                         try
                         {SQLConnection.sql.Close();}
                         catch (System.Exception)
                         {}
                         Environment.Exit(0);
                         break;
                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
        }
    }
}