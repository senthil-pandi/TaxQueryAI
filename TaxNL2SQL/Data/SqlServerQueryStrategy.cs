using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TaxNL2SQL.Data
{
    /// <summary>
    /// SQL Server query strategy implementation
    /// </summary>
    public class SqlServerQueryStrategy : IQueryStrategy
    {
        private readonly string _connectionString;

        public SqlServerQueryStrategy(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes a SQL query and returns results as a DataTable
        /// </summary>
        public DataTable ExecuteQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query cannot be null or empty.");
            }

            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandTimeout = 60; // 60 seconds timeout
                    connection.Open();

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                return dataTable;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error executing query: {ex.Message}");
                Console.WriteLine($"Error Number: {ex.Number}");
                Console.WriteLine($"Query: {query}");
                throw new InvalidOperationException($"Database query failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Executes a non-query SQL command (INSERT, UPDATE, DELETE)
        /// </summary>
        public int ExecuteNonQuery(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null or empty.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var sqlCommand = new SqlCommand(command, connection))
                {
                    sqlCommand.CommandTimeout = 60;
                    connection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error executing command: {ex.Message}");
                Console.WriteLine($"Error Number: {ex.Number}");
                Console.WriteLine($"Command: {command}");
                throw new InvalidOperationException($"Database command failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Executes a scalar query and returns a single value
        /// </summary>
        public object ExecuteScalar(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query cannot be null or empty.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandTimeout = 60;
                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error executing scalar query: {ex.Message}");
                Console.WriteLine($"Error Number: {ex.Number}");
                Console.WriteLine($"Query: {query}");
                throw new InvalidOperationException($"Database scalar query failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing scalar query: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Connection test failed: {ex.Message}");
                Console.WriteLine($"Error Number: {ex.Number}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
        }
    }
}
