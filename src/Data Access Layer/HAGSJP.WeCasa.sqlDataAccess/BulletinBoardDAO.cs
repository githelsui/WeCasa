using HAGSJP.WeCasa.Models;
using MySqlConnector;
// using HAGSJP.WeCasa.Logging.Implementations;

namespace HAGSJP.WeCasa.sqlDataAccess
{
   public class BulletinBoardDAO : AccountMariaDAO
   {
        private string _connectionString;
        private DAOResult result;
        // private Logger _logger;


        public MySqlConnectionStringBuilder BuildConnectionString()
        {
            Console.WriteLine("MySqlConnectionStringBuilder");
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = "localhost",
                    Port = 3306,
                    UserID = "HAGSJP.WeCasa.SqlUser",
                    Password = "cecs491",
                    Database = "HAGSJP.WeCasa"
                };
            
            Console.WriteLine("MySqlConnectionStringBuilder after" + builder);
            return builder;
        }

        public DAOResult PopulateResult(DAOResult result, MySqlException sqlex)
        {
            result.IsSuccessful = false;
            result.ErrorStatus = sqlex.ErrorCode.ToString();
            result.Message = sqlex.Message;
            result.SqlState = sqlex.SqlState;
            return result;
        }

        public DAOResult AddNote(Note note)
        {
            Console.Write("ADD NOTE DAO INIT" + note.LastModifiedUser);
            var result = new DAOResult();

            _connectionString = BuildConnectionString().ConnectionString;
            Console.Write("ADD NOTE CONNECTION STRING" + _connectionString);
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.Write("CONNECT OPEN");
                    connection.Open();
                    Console.Write("CONNECT OPEN AFTER");

                    var insertSql = @"INSERT INTO Notes (last_modified_user, group_id, date_entered, date_modified, message, is_deleted, photo_file_name, color, x_coor, y_coor)
                                    VALUES (@lastModifiedUser, @groupId, @dateSubmitted, @dateModified, @message, @isDeleted, @photoFileName, @color, @xCoor, @yCoor);
                                    SELECT LAST_INSERT_ID();";
                
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@lastModifiedUser", note.LastModifiedUser);
                    command.Parameters.AddWithValue("@groupId", note.GroupId);
                    command.Parameters.AddWithValue("@dateSubmitted", DateTime.Now);
                    command.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    command.Parameters.AddWithValue("@isDeleted", 0);
                    command.Parameters.AddWithValue("@message", note.Message);
                    command.Parameters.AddWithValue("@photoFileName", note.PhotoFileName == null ? " " : note.PhotoFileName);
                    command.Parameters.AddWithValue("@color", note.Color);
                    command.Parameters.AddWithValue("@xCoor", note.X);
                    command.Parameters.AddWithValue("@yCoor", note.Y);

                    var noteId = Convert.ToInt32(command.ExecuteScalar());
                    result.IsSuccessful = true;
                    result.Message = "Add note success";
                    result.ReturnedObject = noteId;
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

        public DAOResult UpdateNote(Note note)
        {    
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"UPDATE Notes 
                                            SET 
                                                message = @message,
                                                last_modified_user = @lastModifiedUser,
                                                date_modified = @dateModified,
                                                photo_file_name = @photoFileName,
                                                color = @color,
                                                x_coor = @xCoor,
                                                y_coor = @yCoor
                                            WHERE note_id = @noteId;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@lastModifiedUser", note.LastModifiedUser);
                    command.Parameters.AddWithValue("@dateModified", DateTime.Now);
                    command.Parameters.AddWithValue("@message", note.Message);
                    command.Parameters.AddWithValue("@photoFileName", note.PhotoFileName == null ? note.PhotoFileName : "");
                    command.Parameters.AddWithValue("@color", note.Color);
                    command.Parameters.AddWithValue("@xCoor", note.X);
                    command.Parameters.AddWithValue("@yCoor", note.Y);
                    command.Parameters.AddWithValue("@noteId", note.NoteId);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                    throw sqlex;
                } 
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }
    
        public DAOResult DeleteNote(int noteId)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"UPDATE Notes 
                                            SET 
                                                is_deleted = @isDeleted,
                                                date_deleted = @dateDeleted
                                            WHERE note_id = @noteId;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@noteId", noteId);
                    command.Parameters.AddWithValue("@isDeleted", true);
                    command.Parameters.AddWithValue("@dateDeleted", DateTime.UtcNow);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
                }
                catch (MySqlException sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = "Delete Note failed in data access";
                } 
                return result;
            }
        }

        public DAOResult GetNotes(int groupId)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                var result = new DAOResult();
                try
                {
                    connection.Open();
                    List<Note> notes = new List<Note>();

                    var insertSql = @"SELECT last_modified_user, group_id, note_id, date_entered, date_modified, message, photo_file_name, color, x_coor, y_coor from Notes
                                            WHERE group_id = @groupId AND is_deleted = 0;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@groupId", groupId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Note note = new Note();
                            note.NoteId = reader.GetInt32(reader.GetOrdinal("note_id"));
                            note.LastModifiedUser = reader.GetString(reader.GetOrdinal("last_modified_user"));
                            note.GroupId = reader.GetInt32(reader.GetOrdinal("group_id"));
                            note.DateEntered = reader.GetDateTime(reader.GetOrdinal("date_entered"));
                            note.DateModified = reader.GetDateTime(reader.GetOrdinal("date_modified"));
                            note.Message = reader.GetString(reader.GetOrdinal("message"));
                            note.PhotoFileName = reader.GetString(reader.GetOrdinal("photo_file_name"));
                            note.Color = reader.GetString(reader.GetOrdinal("color"));
                            note.X = reader.GetInt32(reader.GetOrdinal("x_coor"));
                            note.Y = reader.GetInt32(reader.GetOrdinal("y_coor"));
                            notes.Add(note);
                        }
                        result.IsSuccessful = true;
                        result.Message = "Get notes successful from database";
                        result.ReturnedObject = notes;
                        return result;
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                } 
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
   }

}
