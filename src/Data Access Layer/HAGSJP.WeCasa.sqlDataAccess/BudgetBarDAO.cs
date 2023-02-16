using HAGSJP.WeCasa.Models;
using MySqlConnector;
namespace HAGSJP.WeCasa.sqlDataAccess
{
   public class BudgetBarDAO : AccountMariaDAO
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

        public DAOResult PopulateResult(DAOResult result, MySqlException sqlex)
        {
            result.IsSuccessful = false;
            result.ErrorStatus = sqlex.ErrorCode.ToString();
            result.Message = sqlex.Message;
            result.SqlState = sqlex.SqlState;
            return result;
        }

        public DAOResult InsertBill(Bill bill)
        {       
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"INSERT INTO Bills (username, bill_id, date_submitted, bill_description, amount, bill_name, payment_status, percentage_owed, is_repeated, is_deleted, date_deleted, receipt_file_name)
                                    VALUES (@username, @bill_id, @date_submitted, @bill_description, @amount, @bill_name, @payment_status, @percentage_owed, @is_repeated, @is_deleted, @date_deleted, @receipt_file_name);";
                
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@username".ToLower(), bill.Username.ToLower());
                    command.Parameters.AddWithValue("@bill_id", bill.BillId);
                    command.Parameters.AddWithValue("@date_submitted", bill.DateEntered);
                    command.Parameters.AddWithValue("@bill_description", bill.BillDescription);
                    command.Parameters.AddWithValue("@amount", bill.Amount);
                    command.Parameters.AddWithValue("@bill_name", bill.BillName);
                    command.Parameters.AddWithValue("@payment_status", bill.PaymentStatus);
                    command.Parameters.AddWithValue("@percentage_owed", bill.PercentageOwed);
                    command.Parameters.AddWithValue("@is_repeated", bill.IsRepeated);
                    command.Parameters.AddWithValue("@is_deleted", bill.IsDeleted);
                    command.Parameters.AddWithValue("@date_deleted", bill.DateDeleted);
                    command.Parameters.AddWithValue("@receipt_file_name", bill.PhotoFileName);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                } 
                return result;
            }
        }

        

        public DAOResult UpdateBill(Bill bill)
        {    
            var result = new DAOResult();   
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"UPDATE Bills 
                                            SET 
                                                bill_description = @bill_description,
                                                amount = @amount,
                                                bill_name = @bill_name,
                                                payment_status = @payment_status,
                                                percentage_owed = @percentage_owed,
                                                is_repeated = @is_repeated,
                                                is_deleted = @is_deleted,
                                                date_deleted = @date_deleted,
                                                receipt_file_name = @receipt_file_name
                                            WHERE bill_id = @bill_id
                                            AND username = @username;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@bill_id", bill.BillId);
                    command.Parameters.AddWithValue("@bill_description", bill.BillDescription);
                    command.Parameters.AddWithValue("@amount", bill.Amount);
                    command.Parameters.AddWithValue("@bill_name", bill.BillName);
                    command.Parameters.AddWithValue("@payment_status", bill.PaymentStatus);
                    command.Parameters.AddWithValue("@percentage_owed", bill.PercentageOwed);
                    command.Parameters.AddWithValue("@is_repeated", bill.IsRepeated);
                    command.Parameters.AddWithValue("@is_deleted", bill.IsDeleted);
                    command.Parameters.AddWithValue("@date_deleted", bill.DateDeleted);
                    command.Parameters.AddWithValue("@receipt_file_name", bill.PhotoFileName);
                    command.Parameters.AddWithValue("@username", bill.Username);


                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResultMultiple(rows);
                                }
                catch (MySqlException sqlex)
                {

                    PopulateResult(result, sqlex);
                } 
                return result;
            }
        }

        // public DAOResult UpdateBill(string username, Boolean paymentStatus, Decimal PercentageOwed)
        // {    
        //     var result = new DAOResult();   
        //     _connectionString = BuildConnectionString().ConnectionString;
        //     using(var connection = new MySqlConnection(_connectionString))
        //     {
        //         try
        //         {
        //             connection.Open();

        //             var insertSql = @"UPDATE Bills 
        //                                     SET 
        //                                         bill_description = @bill_description,
        //                                         amount = @amount,
        //                                         bill_name = @bill_name,
        //                                         payment_status = @payment_status,
        //                                         percentage_owed = @percentage_owed,
        //                                         is_repeated = @is_repeated,
        //                                         is_deleted = @is_deleted,
        //                                         date_deleted = @date_deleted,
        //                                         receipt_file_name = @receipt_file_name
        //                                     WHERE bill_id = @bill_id;";
        //             var command = connection.CreateCommand();
        //             command.CommandText = insertSql;
        //             command.Parameters.AddWithValue("@bill_id", bill.BillId);
        //             command.Parameters.AddWithValue("@bill_description", bill.BillDescription);
        //             command.Parameters.AddWithValue("@amount", bill.Amount);
        //             command.Parameters.AddWithValue("@bill_name", bill.BillName);
        //             command.Parameters.AddWithValue("@payment_status", bill.PaymentStatus);
        //             command.Parameters.AddWithValue("@percentage_owed", bill.PercentageOwed);
        //             command.Parameters.AddWithValue("@is_repeated", bill.IsRepeated);
        //             command.Parameters.AddWithValue("@is_deleted", bill.IsDeleted);
        //             command.Parameters.AddWithValue("@date_deleted", bill.DateDeleted);
        //             command.Parameters.AddWithValue("@receipt_file_name", bill.PhotoFileName);
        //             command.Parameters.AddWithValue("@username", bill.Username);


        //             var rows = (command.ExecuteNonQuery());
        //             result = result.ValidateSqlResult(rows);
        //                         }
        //         catch (MySqlException sqlex)
        //         {

        //             PopulateResult(result, sqlex);
        //         } 
        //         return result;
        //     }
        // }
    
        public DAOResult DeleteBill(string billId, DateTime date)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"UPDATE Bills 
                                            SET 
                                                is_deleted = @is_deleted,
                                                date_deleted = @date_deleted
                                            WHERE bill_id = @bill_id;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@bill_id", billId);
                    command.Parameters.AddWithValue("@is_deleted", true);
                    command.Parameters.AddWithValue("@date_deleted", date);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResultMultiple(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                } 
                return result;
            }
        }

        public DAOResult RestoreDeletedBill(string billId)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var insertSql = @"UPDATE Bills 
                                            SET
                                                is_deleted = @is_deleted,
                                                date_deleted = @date_deleted
                                            WHERE bill_id = @bill_id;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@bill_id", billId);
                    command.Parameters.AddWithValue("@is_deleted", false);
                    command.Parameters.AddWithValue("@date_deleted", default);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResultMultiple(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                }
                return result;
            }
        }

        public DAOResult RefreshBillList()
       {
            var result = new DAOResult();
           _connectionString = BuildConnectionString().ConnectionString;
           using(var connection = new MySqlConnection(_connectionString))
           {
                try
                {
                    connection.Open();
                    var insertSql = @"DELETE from Bills WHERE (SELECT MONTH(date_submitted) AS Month) != MONTH(NOW()) OR (SELECT YEAR(date_submitted) AS Month) != YEAR(NOW());";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResultMultiple(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                } 
                return result;
           }
       }

       public DAOResult DeleteAllOutdatedBills()
       {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"DELETE from Bills WHERE date_deleted <= NOW() - INTERVAL 1 DAY;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResultMultiple(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                } 
                return result;
            }
       }

        public List<Bill> GetBills(string username, int isDeleted)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    List<Bill> activeBills = new List<Bill>();

                    var insertSql = @"SELECT * from BILLS 
                                            WHERE is_deleted = @isDeleted
                                            AND username = @username;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@isDeleted", isDeleted);
                    command.Parameters.AddWithValue("@username".ToLower(), username.ToLower());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bill activeBill = new Bill();
                            activeBill.Username = reader.GetString(reader.GetOrdinal("username"));
                            activeBill.BillId = reader.GetString(reader.GetOrdinal("bill_id"));
                            activeBill.DateEntered = reader.GetDateTime(reader.GetOrdinal("date_submitted"));
                            activeBill.BillName = reader.GetString(reader.GetOrdinal("bill_name"));
                            activeBill.BillDescription = reader.GetString(reader.GetOrdinal("bill_description"));
                            activeBill.Amount = reader.GetDecimal(reader.GetOrdinal("amount"));
                            activeBill.PercentageOwed = reader.GetDecimal(reader.GetOrdinal("percentage_owed"));
                            activeBill.PaymentStatus = reader.GetInt32(reader.GetOrdinal("payment_Status")) == 1 ? true : false;
                            activeBill.IsRepeated = reader.GetInt32(reader.GetOrdinal("is_repeated")) == 1 ? true : false;
                            activeBill.IsDeleted = reader.GetInt32(reader.GetOrdinal("is_deleted")) == 1 ? true : false;
                            activeBill.DateDeleted = reader.IsDBNull(reader.GetOrdinal("date_deleted")) ? null : reader.GetDateTime(reader.GetOrdinal("date_deleted"));
                            activeBill.PhotoFileName = reader.GetString(reader.GetOrdinal("receipt_file_name"));
                            activeBills.Add(activeBill);
                        }
                    }
                    return activeBills;
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                } 
            }
        }

        public decimal GetBudget(string groupId)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"SELECT group_budget from GROUPS
                                        WHERE group_id = @groupId;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@groupId".ToLower(), groupId.ToLower());
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDecimal(0);
                        }
                        return -1;
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                } 
            }
        }

        public DAOResult EditBudget(string groupID, decimal amount)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"UPDATE GROUPS 
                                            SET 
                                                group_budget = @amount
                                            WHERE group_id = @groupID;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@groupID", groupID);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
                }
                catch (MySqlException sqlex)
                {
                    PopulateResult(result, sqlex);
                } 
                return result;
            }
        }
        
   }
}
