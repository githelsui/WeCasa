using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class GroceryDAO : AccountMariaDAO
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

        public DAOResult AddGroceryItem(GroceryItem item)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertGroceryql = @"INSERT INTO Groceries (name, group_id, notes, assignments, is_purchased, created, created_by)
                                    VALUES (@name, @group_id, @notes, @assignments, @is_purchased, @created, @created_by);
                                    SELECT LAST_INSERT_ID();";

                    var command = connection.CreateCommand();
                    command.CommandText = insertGroceryql;
                    command.Parameters.AddWithValue("@name", item.Name);
                    command.Parameters.AddWithValue("@group_id", item.GroupId);
                    command.Parameters.AddWithValue("@created", item.Created);
                    command.Parameters.AddWithValue("@created_by", item.CreatedBy);
                    command.Parameters.AddWithValue("@notes", item.Notes != null ? item.Notes : null);
                    command.Parameters.AddWithValue("@is_purchased", item.IsPurchased == null ? item.IsPurchased : false);
                    string assignedToJSON = JsonSerializer.Serialize(item.Assignments);
                    command.Parameters.AddWithValue("@assignments", assignedToJSON);

                    // Execution of first query for item table
                    var groceryId = Convert.ToInt32(command.ExecuteScalar());
                    if (groceryId == null)
                    {
                        result.IsSuccessful = false;
                        result.Message = "Failure creating item.";
                    }
                    else // First query successful
                    {
                            item.GroceryId = groceryId;
                            result.IsSuccessful = true;
                            result.Message = "Creating grocery item success.";
                            result.ReturnedObject = item;
                            return result;
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }

        
        
    }
}
