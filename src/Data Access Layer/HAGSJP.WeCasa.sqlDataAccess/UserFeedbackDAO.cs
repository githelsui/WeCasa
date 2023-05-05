using System.Text.Json;
using System.Windows.Markup;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class UserFeedbackDAO : AccountMariaDAO
    {
        private string _connectionString;
        private DAOResult result;

        public MySqlConnectionStringBuilder BuildConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = 3306,
                UserID = "HAGSJP.WeCasa.SqlUser",
                Password = "cecs491",
                Database = "HAGSJP.WeCasa"
            };
            return builder;
        }

        public DAOResult ValidateSqlStatement(int rows)
        {
            var result = new DAOResult();

            if (rows == 1)
            {
                result.IsSuccessful = true;
                result.Message = string.Empty;

                return result;
            }
            result.IsSuccessful = false;
            result.Message = $"Rows affected were not 1. It was {rows}";

            return result;
        }

        public DAOResult storeFeedbackTicket(Feedback feedback)
        {
            var result = new DAOResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"INSERT INTO feedback (submission_date, first_name, last_name, email, feedback_type, feedback_msg, feedback_rating, resolved_status, resolved_date)
                    VALUES (@submission_date, @first_name, @last_name, @email, @feedback_type, @feedback_msg, @feedback_rating, @resolved_status, @resolved_date)";
                    

                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;

                    command.Parameters.AddWithValue("@submission_date", feedback.SubmissionDate);
                    command.Parameters.AddWithValue("@first_name", feedback.FirstName);
                    command.Parameters.AddWithValue("@last_name", feedback.LastName);
                    command.Parameters.AddWithValue("@email", feedback.Email);
                    command.Parameters.AddWithValue("@feedback_type", feedback.FeedbackType);
                    command.Parameters.AddWithValue("@feedback_msg", feedback.FeedbackMessage);
                    command.Parameters.AddWithValue("@feedback_rating", feedback.FeedbackRating);
                    command.Parameters.AddWithValue("@resolved_status", feedback.ResolvedStatus);
                    command.Parameters.AddWithValue("@resolved_date", feedback.ResolvedDate);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }
    }
}