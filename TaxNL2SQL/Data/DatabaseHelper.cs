using System;
using System.Data;

namespace TaxNL2SQL.Data
{
    /// <summary>
    /// Database helper using Strategy pattern to abstract database operations
    /// </summary>
    public class DatabaseHelper
    {
        private readonly IQueryStrategy _queryStrategy;

        public DatabaseHelper(IQueryStrategy queryStrategy)
        {
            _queryStrategy = queryStrategy ?? throw new ArgumentNullException(nameof(queryStrategy));
        }

        /// <summary>
        /// Executes a query and returns results as DataTable
        /// </summary>
        public DataTable GetDataTable(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query cannot be null or empty.");
            }

            return _queryStrategy.ExecuteQuery(query);
        }

        /// <summary>
        /// Executes a non-query command and returns affected rows count
        /// </summary>
        public int ExecuteCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null or empty.");
            }

            return _queryStrategy.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Executes a scalar query and returns single value
        /// </summary>
        public object GetScalarValue(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query), "Query cannot be null or empty.");
            }

            return _queryStrategy.ExecuteScalar(query);
        }

        /// <summary>
        /// Tests database connection
        /// </summary>
        public bool TestConnection()
        {
            return _queryStrategy.TestConnection();
        }
    }
}
