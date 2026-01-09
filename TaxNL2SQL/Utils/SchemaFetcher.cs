using System;
using System.Data;
using System.Text;
using TaxNL2SQL.Data;

namespace TaxNL2SQL.Utils
{
    /// <summary>
    /// Utility class to fetch and format database schema information
    /// </summary>
    public class SchemaFetcher
    {
        private readonly DatabaseHelper _databaseHelper;

        public SchemaFetcher(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
        }

        /// <summary>
        /// Fetches the complete database schema with detailed information
        /// </summary>
        public string FetchDatabaseSchema()
        {
            try
            {
                string schemaQuery = @"
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME, ORDINAL_POSITION;";

                DataTable schemaData = _databaseHelper.GetDataTable(schemaQuery);

                if (schemaData == null || schemaData.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Failed to fetch database schema or schema is empty.");
                }

                return FormatSchemaForAI(schemaData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching database schema: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Fetches database schema with foreign key relationships
        /// </summary>
        public string FetchSchemaWithRelationships()
        {
            try
            {
                string basicSchema = FetchDatabaseSchema();
                string relationships = FetchForeignKeyRelationships();

                return $"{basicSchema}\n\n{relationships}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching schema with relationships: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Formats schema data in a clear, AI-friendly format
        /// </summary>
        private string FormatSchemaForAI(DataTable schemaData)
        {
            var schemaBuilder = new StringBuilder();
            schemaBuilder.AppendLine("=== DATABASE SCHEMA ===");
            schemaBuilder.AppendLine();

            string currentTable = string.Empty;

            foreach (DataRow row in schemaData.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                string columnName = row["COLUMN_NAME"].ToString();
                string dataType = row["DATA_TYPE"].ToString();
                string maxLength = row["CHARACTER_MAXIMUM_LENGTH"]?.ToString() ?? "";
                string isNullable = row["IS_NULLABLE"].ToString();

                // New table section
                if (tableName != currentTable)
                {
                    if (!string.IsNullOrEmpty(currentTable))
                    {
                        schemaBuilder.AppendLine();
                    }

                    schemaBuilder.AppendLine($"Table: dbo.{tableName}");
                    schemaBuilder.AppendLine(new string('-', 60));
                    currentTable = tableName;
                }

                // Format data type with length
                string fullDataType = dataType.ToUpper();
                if (!string.IsNullOrEmpty(maxLength) && maxLength != "-1")
                {
                    fullDataType += $"({maxLength})";
                }
                else if (maxLength == "-1")
                {
                    fullDataType += "(MAX)";
                }

                // Build column info
                string nullability = isNullable == "YES" ? "NULL" : "NOT NULL";
                schemaBuilder.AppendLine($"  - {columnName}: {fullDataType} {nullability}");
            }

            return schemaBuilder.ToString();
        }

        /// <summary>
        /// Fetches foreign key relationships
        /// </summary>
        private string FetchForeignKeyRelationships()
        {
            try
            {
                string relationshipQuery = @"
SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS ParentTable,
    cp.name AS ParentColumn,
    tr.name AS ReferencedTable,
    cr.name AS ReferencedColumn
FROM sys.foreign_keys AS fk
INNER JOIN sys.tables AS tp ON fk.parent_object_id = tp.object_id
INNER JOIN sys.tables AS tr ON fk.referenced_object_id = tr.object_id
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.columns AS cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
INNER JOIN sys.columns AS cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
WHERE tp.schema_id = SCHEMA_ID('dbo')
ORDER BY tp.name, fk.name;";

                DataTable relationships = _databaseHelper.GetDataTable(relationshipQuery);

                if (relationships == null || relationships.Rows.Count == 0)
                {
                    return "=== RELATIONSHIPS ===\nNo foreign key relationships found.";
                }

                var relationshipBuilder = new StringBuilder();
                relationshipBuilder.AppendLine("=== RELATIONSHIPS ===");
                relationshipBuilder.AppendLine();

                foreach (DataRow row in relationships.Rows)
                {
                    string parentTable = row["ParentTable"].ToString();
                    string parentColumn = row["ParentColumn"].ToString();
                    string referencedTable = row["ReferencedTable"].ToString();
                    string referencedColumn = row["ReferencedColumn"].ToString();

                    relationshipBuilder.AppendLine($"  {parentTable}.{parentColumn} ? {referencedTable}.{referencedColumn}");
                }

                return relationshipBuilder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching relationships: {ex.Message}");
                return "=== RELATIONSHIPS ===\nUnable to fetch relationship information.";
            }
        }

        /// <summary>
        /// Gets a quick summary of table names
        /// </summary>
        public string GetTableSummary()
        {
            try
            {
                string tableQuery = @"
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;";

                DataTable tables = _databaseHelper.GetDataTable(tableQuery);

                if (tables == null || tables.Rows.Count == 0)
                {
                    return "No tables found in database.";
                }

                var summary = new StringBuilder();
                summary.AppendLine($"Database contains {tables.Rows.Count} tables:");

                foreach (DataRow row in tables.Rows)
                {
                    summary.AppendLine($"  - dbo.{row["TABLE_NAME"]}");
                }

                return summary.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching table summary: {ex.Message}");
                throw;
            }
        }
    }
}
