using System.Data;

namespace TaxNL2SQL.Data
{
    /// <summary>
    /// Interface for database query operations
    /// </summary>
    public interface IQueryStrategy
    {
        /// <summary>
        /// Executes a SQL query and returns the results as a DataTable
        /// </summary>
        /// <param name="query">The SQL query to execute</param>
        /// <returns>DataTable containing the query results</returns>
        DataTable ExecuteQuery(string query);

        /// <summary>
        /// Executes a SQL command (INSERT, UPDATE, DELETE) and returns the number of affected rows
        /// </summary>
        /// <param name="command">The SQL command to execute</param>
        /// <returns>Number of rows affected</returns>
        int ExecuteNonQuery(string command);

        /// <summary>
        /// Executes a SQL query and returns a single scalar value
        /// </summary>
        /// <param name="query">The SQL query to execute</param>
        /// <returns>The scalar value result</returns>
        object ExecuteScalar(string query);

        /// <summary>
        /// Tests the database connection
        /// </summary>
        /// <returns>True if connection is successful, false otherwise</returns>
        bool TestConnection();
    }
}
