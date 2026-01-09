using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace TaxNL2SQL.Services
{
    /// <summary>
    /// Service for interacting with Azure OpenAI for SQL generation
    /// </summary>
    public class AzureOpenAIService
    {
        private readonly ChatClient _chatClient;
        private readonly string _systemPrompt;
        private readonly List<ChatMessage> _chatHistory;

        public AzureOpenAIService(string endpoint, string apiKey, string deploymentName)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));
            if (string.IsNullOrWhiteSpace(deploymentName))
                throw new ArgumentNullException(nameof(deploymentName));

            // Create the chat client with proper configuration
            var clientOptions = new AzureOpenAIClientOptions();
            var azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey), clientOptions);
            _chatClient = azureClient.GetChatClient(deploymentName);
            _chatHistory = new List<ChatMessage>();

            _systemPrompt = @"# ROLE:
            You are a highly intelligent T-SQL expert assistant specializing in tax management databases.
            Your task is to generate precise, efficient, and optimized T-SQL queries based on the provided database schema and user requirements.

            # INSTRUCTIONS:
            1. Return ONLY the T-SQL query without any explanation, comments, or additional text
            2. Do NOT include markdown code blocks, backticks, or 'sql' language identifiers
            3. Ensure queries are optimized and follow SQL Server best practices
            4. Use proper JOINs instead of subqueries when possible
            5. Always use table aliases for readability
            6. Include appropriate WHERE clauses to filter data efficiently
            7. Use aggregate functions (SUM, COUNT, AVG, etc.) when appropriate
            8. Format dates properly using SQL Server date functions
            9. For year-based queries, use the TaxYear column in TaxReturns table
            10. Always use schema prefix 'dbo.' before table names

            # OUTPUT FORMAT:
            - Return only the raw SQL query text
            - NO explanations before or after the query
            - NO code block markers
            - NO comments in the SQL

            # IMPORTANT DATABASE NOTES:
            - Tax returns are stored in dbo.TaxReturns table with TaxYear column
            - All monetary amounts are DECIMAL(18,2)
            - Use GETDATE() for current date comparisons
            - Primary tables: TaxPayers, TaxReturns, Incomes, Deductions, TaxCredits, TaxPayments
            - Always join through proper foreign key relationships

            If the user request is unclear or ambiguous, return only: 'CLARIFICATION NEEDED: [specific question]'";

            _chatHistory.Add(ChatMessage.CreateSystemMessage(_systemPrompt));
        }

        /// <summary>
        /// Generates SQL query from natural language question
        /// </summary>
        public async Task<string> GenerateSQLQueryAsync(string userQuestion, string databaseSchema)
        {
            if (string.IsNullOrWhiteSpace(userQuestion))
                throw new ArgumentNullException(nameof(userQuestion));
            if (string.IsNullOrWhiteSpace(databaseSchema))
                throw new ArgumentNullException(nameof(databaseSchema));

            try
            {
                // Build the user message with schema context
                string userMessage = $@"User Question: {userQuestion}

Database Schema:
{databaseSchema}

Generate the T-SQL query:";

                // Add user message to history
                _chatHistory.Add(ChatMessage.CreateUserMessage(userMessage));

                // Create chat completion options
                ChatCompletionOptions options = new ChatCompletionOptions
                {
                    Temperature = 0.1f, // Low temperature for more deterministic responses
                    MaxOutputTokenCount = 1000,
                    TopP = 0.95f
                };

                // Get completion
                ChatCompletion completion = await _chatClient.CompleteChatAsync(_chatHistory, options);

                if (completion?.Content == null || completion.Content.Count == 0)
                {
                    throw new InvalidOperationException("No response received from Azure OpenAI");
                }

                string sqlQuery = completion.Content[0].Text?.Trim() ?? string.Empty;

                // Clean up the response - remove any markdown or extra formatting
                sqlQuery = CleanSQLResponse(sqlQuery);

                // Add assistant response to history
                _chatHistory.Add(ChatMessage.CreateAssistantMessage(sqlQuery));

                return sqlQuery;
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Azure OpenAI API Error: {ex.Message}");
                Console.WriteLine($"Status: {ex.Status}");
                throw new InvalidOperationException($"Failed to generate SQL query: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating SQL query: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Generates a natural language answer based on query results
        /// </summary>
        public async Task<string> GenerateAnswerAsync(string userQuestion, string sqlQuery, string queryResults)
        {
            try
            {
                string answerPrompt = $@"The user asked: ""{userQuestion}""

The SQL query executed was:
{sqlQuery}

Query Results:
{queryResults}

Provide a clear, concise natural language answer to the user's question based on the results. 
Be specific with numbers and names. Keep the response conversational and easy to understand.";

                var messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage("You are a helpful tax assistant. Provide clear, accurate answers based on the data provided."),
                    ChatMessage.CreateUserMessage(answerPrompt)
                };

                ChatCompletionOptions options = new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    MaxOutputTokenCount = 500
                };

                ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);

                return completion.Content[0].Text?.Trim() ?? "Unable to generate answer.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating answer: {ex.Message}");
                return "Sorry, I couldn't generate a natural language answer, but the query results are displayed above.";
            }
        }

        /// <summary>
        /// Cleans SQL response by removing markdown and extra formatting
        /// </summary>
        private string CleanSQLResponse(string sqlResponse)
        {
            if (string.IsNullOrWhiteSpace(sqlResponse))
                return string.Empty;

            // Remove markdown code blocks
            sqlResponse = sqlResponse.Replace("```sql", "").Replace("```", "").Trim();

            // Remove any leading/trailing whitespace or newlines
            sqlResponse = sqlResponse.Trim();

            return sqlResponse;
        }

        /// <summary>
        /// Clears chat history (keeps only system prompt)
        /// </summary>
        public void ClearHistory()
        {
            _chatHistory.Clear();
            _chatHistory.Add(ChatMessage.CreateSystemMessage(_systemPrompt));
        }

        /// <summary>
        /// Gets the current conversation history count
        /// </summary>
        public int GetHistoryCount()
        {
            return _chatHistory.Count;
        }
    }
}
