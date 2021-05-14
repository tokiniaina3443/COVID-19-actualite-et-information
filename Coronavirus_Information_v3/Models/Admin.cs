using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Coronavirus_Information_v3.Models
{
    public class Admin
    {
        string id;
        string username;
        string password;

        public string Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public Admin()
        {
        }

        public Admin(string username, string password, string id = "0")
        {
            Id = id;
            Username = username;
            Password = password;
        }

        //<meta name = "google-site-verification" content="GjL6T19Gq1iDbxsSf45sC-fqf8SSkGqslXz7VbvN424" />
        public static string GetKeyGoogle()
        {
            string ans = "";
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                SqlDataReader dataReader = null;
                string sql = "select Top 1 [key] from Googlevalidation order by [id] desc";
                try
                {
                    command = new SqlCommand(sql, connection);

                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ans = dataReader.GetString(0);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    connection.Close();
                }
            }
            return ans;
        }
    }
}
