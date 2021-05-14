using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Coronavirus_Information_v3.Models
{
    public class Article
    {
        int id;
        string titre;
        string auteur;
        DateTime date;
        string description;
        Illustration illustration;
        string objet;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Auteur { get => auteur; set => auteur = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Description { get => description; set => description = value; }
        public Illustration Illustration { get => illustration; set => illustration = value; }
        public string Objet { get => objet; set => objet = value; }

        public Article()
        {
        }

        public Article(string titre, string auteur, DateTime date, string description, Illustration illustration, string objet, int id = 0)
        {
            Id = id;
            Titre = titre ?? throw new ArgumentNullException(nameof(titre));
            Auteur = auteur ?? throw new ArgumentNullException(nameof(auteur));
            Date = date;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Illustration = illustration ?? throw new ArgumentNullException(nameof(illustration));
            Objet = objet ?? throw new ArgumentNullException(nameof(objet));
        }

        public static string ToUrl(string title)
        {
            string url = title.Replace(" ", "-");
            return url;
        }

        public int Save()
        {
            int ans = 0;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                SqlDataReader reader = null;
                string sql = "Insert into Article (titre, auteur, date, description, illustration, objet) values (@titre, @auteur, @date, @description, @illustration, @objet)";
                try
                {
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@titre", this.Titre);
                    command.Parameters.AddWithValue("@auteur", this.Auteur);
                    command.Parameters.AddWithValue("@date", this.Date);
                    command.Parameters.AddWithValue("@illustration", this.Illustration.Id);
                    command.Parameters.AddWithValue("@description", this.Description);
                    command.Parameters.AddWithValue("@objet", this.Objet);

                    command.ExecuteNonQuery();

                    sql = "select Top 1 Id from Article order by id desc";
                    command = new SqlCommand(sql, connection);

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ans = reader.GetInt32(0);
                    }
                }
                catch (Exception ex)
                {
                    ans = 0;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    connection.Close();
                }
            }
            return ans;
        }

        public static bool DeleteArticle(int idArticle)
        {
            bool ans = false;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                string sql = "Delete Article where Id = @idArticle";
                try
                {
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@idArticle", idArticle);
                    command.ExecuteNonQuery();
                    ans = true;
                }
                catch (Exception ex)
                {
                    ans = false;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    connection.Close();
                }
            }
            return ans;
        }

        public static int GetNbPages(double parPage)
        {
            double count = 0;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                SqlDataReader dataReader = null;
                string sql = "select count(*) from V_Article";
                try
                {
                    command = new SqlCommand(sql, connection);

                    dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        count = Convert.ToDouble(dataReader.GetInt32(0));
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
            double division = count / parPage;
            double divsionRound = Math.Round(division, 0);
            int ans = 0;
            if ((division - divsionRound) > 0)
            {
                ans = Convert.ToInt32(divsionRound + 1);
            }
            else
            {
                ans = Convert.ToInt32(divsionRound);
            }
            return ans;
        }

        static int[] GetIntervalle(int page, int parPage)
        {
            int[] ans = new int[2];
            ans[1] = page * parPage;
            ans[0] = ans[1] - parPage;
            return ans;
        }

        public static List<Article> GetArticles(int page = 1, int parPage = 100)
        {
            int[] intervalle = Article.GetIntervalle(page, parPage);
            List<Article> articles = new List<Article>();
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                SqlDataReader dataReader = null;
                string sql = "select * from V_Article where Row > @premier and Row <= @dernier";
                try
                {
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@premier", intervalle[0]);
                    command.Parameters.AddWithValue("@dernier", intervalle[1]);

                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Article article = new Article(dataReader.GetString(2), dataReader.GetString(3), dataReader.GetDateTime(4), dataReader.GetString(5), new Illustration(dataReader.GetString(7), dataReader.GetString(8)), dataReader.GetString(6), dataReader.GetInt32(1));
                        articles.Add(article);
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
            return articles;
        }

        public static Article GetArticleParId(int id)
        {
            Article article = null;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand command = null;
                SqlDataReader dataReader = null;
                string sql = "select * from V_Article where Id = @idArticle";
                try
                {
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@idArticle", id);

                    dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        article = new Article(dataReader.GetString(2), dataReader.GetString(3), dataReader.GetDateTime(4), dataReader.GetString(5), new Illustration(dataReader.GetString(7), dataReader.GetString(8)), dataReader.GetString(6), dataReader.GetInt32(1));
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
            return article;
        }

        // fonction pour updater un artilce
        public bool UpdateArticle()
        {
            bool ans = false;
            SqlConnection connection = DBConnection.Connect();
            connection.Open();
            try
            {
                if (connection != null)
                {
                    string sql = "Update from Article set titre = @titre, auteur = @auteur";
                    SqlCommand command = null;
                    command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ans;
        }
    }
}
