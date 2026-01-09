using System;
using System.Data;
using System.Text;
using TaxNL2SQL.Data;

namespace TaxNL2SQL.Utils
{
    /// <summary>
    /// Utility class to format query results for display
    /// </summary>
    public class ResultFormatter
    {
        /// <summary>
        /// Formats a DataTable as a readable string
        /// </summary>
        public static string FormatDataTable(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return "No results found.";
            }

            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine($"Results: {dataTable.Rows.Count} row(s) returned");
            result.AppendLine(new string('=', 80));

            // Build column headers
            var headers = new StringBuilder();
            var columnWidths = new int[dataTable.Columns.Count];

            // Calculate column widths
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                string columnName = dataTable.Columns[i].ColumnName;
                int maxWidth = columnName.Length;

                foreach (DataRow row in dataTable.Rows)
                {
                    string value = row[i]?.ToString() ?? "NULL";
                    if (value.Length > maxWidth)
                    {
                        maxWidth = value.Length;
                    }
                }

                // Limit column width to 40 characters for readability
                columnWidths[i] = Math.Min(maxWidth + 2, 40);
            }

            // Print headers
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                string columnName = dataTable.Columns[i].ColumnName;
                headers.Append(columnName.PadRight(columnWidths[i]));
            }
            result.AppendLine(headers.ToString());
            result.AppendLine(new string('-', 80));

            // Print rows
            foreach (DataRow row in dataTable.Rows)
            {
                var rowBuilder = new StringBuilder();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string value = row[i]?.ToString() ?? "NULL";
                    
                    // Truncate if too long
                    if (value.Length > 38)
                    {
                        value = value.Substring(0, 35) + "...";
                    }

                    rowBuilder.Append(value.PadRight(columnWidths[i]));
                }
                result.AppendLine(rowBuilder.ToString());
            }

            result.AppendLine(new string('=', 80));
            return result.ToString();
        }

        /// <summary>
        /// Formats DataTable as CSV-style text for AI processing
        /// </summary>
        public static string FormatDataTableForAI(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return "No results found.";
            }

            var result = new StringBuilder();
            
            // Add column headers
            var headers = new string[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                headers[i] = dataTable.Columns[i].ColumnName;
            }
            result.AppendLine(string.Join(", ", headers));

            // Add rows
            foreach (DataRow row in dataTable.Rows)
            {
                var values = new string[dataTable.Columns.Count];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    values[i] = row[i]?.ToString() ?? "NULL";
                }
                result.AppendLine(string.Join(", ", values));
            }

            return result.ToString();
        }

        /// <summary>
        /// Formats an error message
        /// </summary>
        public static string FormatError(string errorMessage)
        {
            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine("? ERROR:");
            result.AppendLine(new string('=', 80));
            result.AppendLine(errorMessage);
            result.AppendLine(new string('=', 80));
            return result.ToString();
        }

        /// <summary>
        /// Formats a success message
        /// </summary>
        public static string FormatSuccess(string message)
        {
            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine("? SUCCESS:");
            result.AppendLine(new string('=', 80));
            result.AppendLine(message);
            result.AppendLine(new string('=', 80));
            return result.ToString();
        }

        /// <summary>
        /// Formats the SQL query for display
        /// </summary>
        public static string FormatSQLQuery(string sqlQuery)
        {
            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine("?? Generated SQL Query:");
            result.AppendLine(new string('-', 80));
            result.AppendLine(sqlQuery);
            result.AppendLine(new string('-', 80));
            return result.ToString();
        }
    }
}
