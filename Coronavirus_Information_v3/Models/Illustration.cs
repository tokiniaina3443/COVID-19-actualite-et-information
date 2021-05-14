using System;
using System.Data;
using System.Data.SqlClient;

namespace Coronavirus_Information_v3.Models
{
    public class Illustration
    {
        int id;
        string nom;
        string chemin;

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Chemin { get => chemin; set => chemin = value; }

        public Illustration()
        {
        }

        public Illustration(string nom, string chemin, int id = 0)
        {
            Id = id;
            Nom = nom ?? throw new ArgumentNullException(nameof(nom));
            Chemin = chemin ?? throw new ArgumentNullException(nameof(chemin));
        }

        public int Save()
        {
            int ans = 0;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            SqlDataReader dataReader = null;
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                string sql = "Insert into Illustration (nom, chemin) values (@nom, @chemin)";
                try
                {
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@nom", this.Nom);
                    command.Parameters.AddWithValue("@chemin", this.Chemin);

                    command.ExecuteNonQuery();

                    sql = "select Top 1 Id from Illustration order by Id desc";
                    command = new SqlCommand(sql, connection);
                    dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        ans = dataReader.GetInt32(0);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    connection.Close();
                }
            }
            return ans;
        }

        public static int GetLastId()
        {
            int ans = 0;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            SqlDataReader dataReader = null;
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                string sql = "select Top 1 id from Illustration order by id desc";
                try
                {
                    command = new SqlCommand(sql, connection);
                    dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        ans = dataReader.GetInt32(0);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    connection.Close();
                }
            }
            return ans;
        }
    }
}
