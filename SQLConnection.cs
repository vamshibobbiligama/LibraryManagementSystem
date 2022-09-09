using System.Data.SqlClient;
class SQLConnection
{
    public static SqlConnection sql;
    public static SqlCommand GetSQLCommand()
    {
        
        sql=new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Library;Trusted_Connection=True;");
        sql.Open();
        SqlCommand cmd=new SqlCommand();
        cmd.Connection=sql;
        return cmd;
    }
}