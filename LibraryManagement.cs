using System;
namespace LibraryManagement
{
    class LibraryManage
    {
        public void Start()
        {   
            again:
            Console.WriteLine("1.User");
            Console.WriteLine("2.Admin");
            Console.WriteLine("3.Exit");
            Console.Write("Enter your Choice : ");
            int choice=0;
            try{
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Enter valid input !");
                goto again;
            }
            switch (choice)
            {
                case 1 : new User();
                         break;   
                case 2 : break;
                case 3 : Console.WriteLine("Thank You!");
                         SQLConnection.sql.Close();
                         Environment.Exit(0);
                         break;
                default: Console.WriteLine("Please enter valid choice !");
                         goto again;
            }
        }

    }
}