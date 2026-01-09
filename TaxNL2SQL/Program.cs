using System;
using System.Data;
using System.Threading.Tasks;
using TaxNL2SQL.Configuration;
using TaxNL2SQL.Data;
using TaxNL2SQL.Services;
using TaxNL2SQL.Utils;

namespace TaxNL2SQL
{
    /// <summary>
    /// Main program for Tax NL2SQL Application
    /// Natural Language to SQL for Tax Management Database
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            PrintWelcomeBanner();

            try
            {
                // Load configuration
                Console.WriteLine("📋 Loading configuration...");
                var configManager = new ConfigurationManager();
                configManager.ValidateConfiguration();
                Console.WriteLine("✓ Configuration loaded successfully\n");

                // Initialize database connection
                Console.WriteLine("🔌 Connecting to database...");
                string connectionString = configManager.GetConnectionString();
                var queryStrategy = new SqlServerQueryStrategy(connectionString);
                var databaseHelper = new DatabaseHelper(queryStrategy);

                // Test database connection
                if (!databaseHelper.TestConnection())
                {
                    Console.WriteLine(ResultFormatter.FormatError("Failed to connect to database. Please ensure SQL Server is running and connection string is correct."));
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("✓ Database connected successfully\n");

                // Fetch database schema
                Console.WriteLine("📚 Loading database schema...");
                var schemaFetcher = new SchemaFetcher(databaseHelper);
                string databaseSchema = schemaFetcher.FetchSchemaWithRelationships();
                Console.WriteLine("✓ Schema loaded successfully");
                Console.WriteLine(schemaFetcher.GetTableSummary());
                Console.WriteLine();

                // Initialize Azure OpenAI service
                Console.WriteLine("🤖 Initializing Azure OpenAI service...");
                string endpoint = configManager.GetAzureOpenAIEndpoint();
                string apiKey = configManager.GetAzureOpenAIApiKey();
                string deploymentName = configManager.GetDeploymentName();
                
                var aiService = new AzureOpenAIService(endpoint, apiKey, deploymentName);
                Console.WriteLine("✓ Azure OpenAI service initialized\n");

                // Get application settings
                bool showSQL = configManager.ShowSQLQuery();
                bool showRawResults = configManager.ShowRawResults();

                PrintInstructions();

                // Main chat loop
                while (true)
                {
                    Console.Write("\n💬 Your Question: ");
                    string userQuestion = Console.ReadLine()?.Trim();

                    // Handle empty input
                    if (string.IsNullOrWhiteSpace(userQuestion))
                    {
                        Console.WriteLine("\n👋 Thank you for using Tax NL2SQL Assistant! Goodbye!");
                        break;
                    }

                    // Handle special commands
                    if (userQuestion.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                        userQuestion.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("\n👋 Thank you for using Tax NL2SQL Assistant! Goodbye!");
                        break;
                    }

                    if (userQuestion.Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintInstructions();
                        continue;
                    }

                    if (userQuestion.Equals("clear", StringComparison.OrdinalIgnoreCase))
                    {
                        aiService.ClearHistory();
                        Console.Clear();
                        PrintWelcomeBanner();
                        PrintInstructions();
                        Console.WriteLine("✓ Chat history cleared.");
                        continue;
                    }

                    if (userQuestion.Equals("schema", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("\n" + databaseSchema);
                        continue;
                    }

                    try
                    {
                        Console.WriteLine("\n⏳ Processing your question...");

                        // Generate SQL query using Azure OpenAI
                        string sqlQuery = await aiService.GenerateSQLQueryAsync(userQuestion, databaseSchema);

                        // Check if clarification is needed
                        if (sqlQuery.StartsWith("CLARIFICATION NEEDED", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"\n❓ {sqlQuery}");
                            continue;
                        }

                        // Show generated SQL query if configured
                        if (showSQL)
                        {
                            Console.WriteLine(ResultFormatter.FormatSQLQuery(sqlQuery));
                        }

                        // Execute the SQL query
                        Console.WriteLine("⚙️  Executing query...");
                        DataTable results = databaseHelper.GetDataTable(sqlQuery);

                        // Display raw results if configured
                        if (showRawResults)
                        {
                            Console.WriteLine(ResultFormatter.FormatDataTable(results));
                        }

                        // Generate natural language answer
                        Console.WriteLine("💭 Generating answer...");
                        string queryResultsText = ResultFormatter.FormatDataTableForAI(results);
                        string answer = await aiService.GenerateAnswerAsync(userQuestion, sqlQuery, queryResultsText);

                        // Display the answer
                        Console.WriteLine("\n" + new string('=', 80));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("🎯 Answer:");
                        Console.ResetColor();
                        Console.WriteLine(new string('-', 80));
                        Console.WriteLine(answer);
                        Console.WriteLine(new string('=', 80));
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ResultFormatter.FormatError($"Query execution failed: {ex.Message}"));
                        Console.WriteLine("\n💡 Tip: Try rephrasing your question or type 'help' for examples.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ResultFormatter.FormatError($"An unexpected error occurred: {ex.Message}"));
                        Console.WriteLine("\n💡 Tip: Type 'help' for assistance or 'exit' to quit.");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ResultFormatter.FormatError($"Configuration Error: {ex.Message}"));
                Console.WriteLine("\n📝 Please update your appsettings.json file with valid Azure OpenAI credentials.");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ResultFormatter.FormatError($"Fatal Error: {ex.Message}"));
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Prints the welcome banner
        /// </summary>
        static void PrintWelcomeBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║                    💼 TAX NL2SQL ASSISTANT 💼                                ║
║                                                                              ║
║              Natural Language to SQL for Tax Management                      ║
║                   Powered by Azure OpenAI                                    ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
            Console.ResetColor();
        }

        /// <summary>
        /// Prints usage instructions
        /// </summary>
        static void PrintInstructions()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("📖 INSTRUCTIONS:");
            Console.ResetColor();
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("  • Ask questions about taxpayers, returns, income, deductions, etc.");
            Console.WriteLine("  • Examples:");
            Console.WriteLine("     - 'Show me all taxpayers who received a refund in 2024'");
            Console.WriteLine("     - 'What is the total income for Michael Chen?'");
            Console.WriteLine("     - 'Who has the highest tax liability?'");
            Console.WriteLine("     - 'List all single filers with income over $70,000'");
            Console.WriteLine("     - 'Show total deductions claimed by filing status'");
            Console.WriteLine();
            Console.WriteLine("  • Commands:");
            Console.WriteLine("     - 'help'   : Show this help message");
            Console.WriteLine("     - 'schema' : Display database schema");
            Console.WriteLine("     - 'clear'  : Clear chat history");
            Console.WriteLine("     - 'exit'   : Exit the application");
            Console.WriteLine("     - [Enter]  : Exit the application");
            Console.WriteLine(new string('-', 80));
        }
    }
}
