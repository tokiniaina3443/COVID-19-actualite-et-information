using System;
using System.Data.SqlClient;

namespace Coronavirus_Information_v3.Models
{
    public class DBConnection
    {
        public static SqlConnection Connect()
        {
            SqlConnection cnn = null;
            try
            {
                string connetionString = @"Data Source=Tokiniaina-PC;Initial Catalog=Covid19Actu;Integrated Security=True;Pooling=False";
                //string connetionString = @"Data Source=Covid19ActuDB.mssql.somee.com;Initial Catalog=Covid19ActuDB;Persist Security Info=True;User ID=tokiniaina3443_SQLLogin_1;Password=7sseigfput";
                cnn = new SqlConnection(connetionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cnn;
        }
    }
}
