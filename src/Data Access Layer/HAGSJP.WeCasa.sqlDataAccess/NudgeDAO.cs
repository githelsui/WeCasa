using System;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class NudgeDAO : AccountMariaDAO
    {
        private string _connectionString;
        private DAOResult _result;

        public NudgeDAO()
        {
        }

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

        public DateTime? GetLastNudgeSent(int choreId, string senderEmail, string receiverEmail)
        {
            DateTime? lastNudgeSent = null;
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var selectSql = "SELECT last_sent FROM Nudges WHERE chore_id = @ChoreId AND sender_email = @SenderEmail AND recipient_email = @RecipientEmail ORDER BY last_sent DESC LIMIT 1";

                    var command = connection.CreateCommand();
                    command.CommandText = selectSql;
                    command.Parameters.AddWithValue("@ChoreId", choreId);
                    command.Parameters.AddWithValue("@SenderEmail", senderEmail);
                    command.Parameters.AddWithValue("@RecipientEmail", receiverEmail);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastNudgeSent = reader.GetDateTime(reader.GetOrdinal("last_sent"));
                        }
                    }
                }
                catch (Exception sqlex)
                {
                    Console.Write("SQEX MESSAGE: " + sqlex.Message);
                }
            }

            return lastNudgeSent;
        }





        public DAOResult SendNudge(Nudge nudge)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = "INSERT INTO Nudges (group_id, chore_id, sender_email, recipient_email, message, is_read, last_sent) " +
                    "VALUES (@GroupId, @ChoreId, @SenderEmail, @RecipientEmail, @Message, @IsComplete, @LastSent); " +
                   "SELECT LAST_INSERT_ID();";

                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@GroupId", nudge.GroupId);
                    command.Parameters.AddWithValue("@ChoreId", nudge.ChoreId);
                    command.Parameters.AddWithValue("@SenderEmail", nudge.SenderUsername);
                    command.Parameters.AddWithValue("@RecipientEmail", nudge.ReceiverUsername);
                    command.Parameters.AddWithValue("@Message", nudge.Message);
                    command.Parameters.AddWithValue("@IsComplete", nudge.IsComplete);
                    command.Parameters.AddWithValue("@LastSent", DateTime.UtcNow);

                    var NudgeId = Convert.ToInt32(command.ExecuteScalar());
                    result.IsSuccessful = true;
                    result.Message = "Add note success";
                    result.ReturnedObject = NudgeId;
                }
                catch (Exception sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = "Unable to add note to the database " + sqlex.Message;
                    Console.Write("SQEX MESSAAE" + sqlex.Message);
                }
                return result;
            }


        }
    }
}

