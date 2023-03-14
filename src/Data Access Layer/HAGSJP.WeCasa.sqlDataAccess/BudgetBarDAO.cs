using System.Text.Json;
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
            string billsJSON = JsonSerializer.Serialize(bill.Usernames);

            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"INSERT INTO Bills (usernames, owner, group_id, date_submitted, bill_description, amount, bill_name, payment_status, is_repeated, receipt_file_name)
                                    VALUES (@usernames, @owner, @group_id, @date_submitted, @bill_description, @amount, @bill_name, @payment_status, @is_repeated, @receipt_file_name);
                                    SELECT LAST_INSERT_ID();";
                
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@usernames", billsJSON);
                    command.Parameters.AddWithValue("@owner", bill.Owner);
                    command.Parameters.AddWithValue("@group_id", bill.GroupId);
                    command.Parameters.AddWithValue("@date_submitted", bill.DateEntered != null? bill.DateEntered : DateTime.Now);
                    command.Parameters.AddWithValue("@bill_description", bill.BillDescription);
                    command.Parameters.AddWithValue("@amount", bill.Amount);
                    command.Parameters.AddWithValue("@bill_name", bill.BillName);
                    command.Parameters.AddWithValue("@payment_status", bill.PaymentStatus != null? bill.PaymentStatus : false);
                    command.Parameters.AddWithValue("@is_repeated", bill.IsRepeated == null ? bill.IsRepeated : false);
                    command.Parameters.AddWithValue("@receipt_file_name", bill.PhotoFileName == null ? bill.PhotoFileName : "");


                    var billId = Convert.ToInt32(command.ExecuteScalar());
                    if (billId == null)
                    {
                        result.IsSuccessful = false;
                        result.Message = "Add bill failure";
                    }
                    else
                    {
                        result.IsSuccessful = true;
                        result.Message = "Add bill success";
                        result.ReturnedObject = billId;
                    }
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

        public DAOResult UpdateBill(Bill bill)
        {    
            var result = new DAOResult();
            string billsJSON = JsonSerializer.Serialize(bill.Usernames);
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
                                                usernames = @usernames,
                                                bill_name = @bill_name,
                                                payment_status = @payment_status,
                                                is_repeated = @is_repeated,
                                                receipt_file_name = @receipt_file_name
                                            WHERE bill_id = @bill_id;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@bill_description", bill.BillDescription);
                    command.Parameters.AddWithValue("@amount", bill.Amount);
                    command.Parameters.AddWithValue("@bill_name", bill.BillName);
                    command.Parameters.AddWithValue("@payment_status", bill.PaymentStatus);
                    command.Parameters.AddWithValue("@is_repeated", bill.IsRepeated);
                    command.Parameters.AddWithValue("@receipt_file_name", bill.PhotoFileName);
                    command.Parameters.AddWithValue("@usernames", billsJSON);
                    command.Parameters.AddWithValue("@bill_id", bill.BillId);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResultMultiple(rows);
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
    
        public DAOResult DeleteBill(int billId, DateTime date)
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

        public DAOResult RestoreDeletedBill(int billId)
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

        public List<Bill> GetBills(int groupId)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    List<Bill> bills = new List<Bill>();

                    var insertSql = @"SELECT * from BILLS 
                                            WHERE group_id = @groupId;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@groupId", groupId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bill bill = new Bill();
                            List<string>? usernames = JsonSerializer.Deserialize<List<string>>(reader.GetString(reader.GetOrdinal("usernames")));
                            bill.Usernames = usernames == null ? new List<string>() : usernames;
                            bill.BillId = reader.GetInt32(reader.GetOrdinal("bill_id"));
                            bill.Owner = reader.GetString(reader.GetOrdinal("owner"));
                            bill.GroupId = reader.GetInt32(reader.GetOrdinal("group_id"));
                            bill.DateEntered = reader.GetDateTime(reader.GetOrdinal("date_submitted"));
                            bill.BillName = reader.GetString(reader.GetOrdinal("bill_name"));
                            bill.BillDescription = reader.GetString(reader.GetOrdinal("bill_description"));
                            bill.Amount = reader.GetDecimal(reader.GetOrdinal("amount"));
                            bill.PaymentStatus = reader.GetInt32(reader.GetOrdinal("payment_status")) == 1 ? true : false;
                            bill.IsRepeated = reader.GetInt32(reader.GetOrdinal("is_repeated")) == 1 ? true : false;
                            bill.IsDeleted = reader.GetInt32(reader.GetOrdinal("is_deleted")) == 1 ? true : false;
                            bill.DateDeleted = reader.IsDBNull(reader.GetOrdinal("date_deleted")) ? null : reader.GetDateTime(reader.GetOrdinal("date_deleted"));
                            bill.PhotoFileName = reader.IsDBNull(reader.GetOrdinal("receipt_file_name")) ? "" : reader.GetString(reader.GetOrdinal("receipt_file_name"));
                            bills.Add(bill);
                        }
                        return bills;
                    }
                    throw new Exception("Cannot find item for get bills");
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

        public decimal GetBudget(int groupId)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"SELECT * from GROUPS
                                        WHERE group_id = @groupId;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@groupId", groupId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDecimal(reader.GetOrdinal("budget")); 
                        }
                        return -1; 
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int GetGroupId(string username)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertSql = @"SELECT * from USERGROUPS
                                        WHERE username = @username;";
                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@username".ToLower(), username.ToLower());
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(reader.GetOrdinal("group_id")); 
                        }
                        return 0; 
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DAOResult EditBudget(int groupID, decimal amount)
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
                                                budget = @amount
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