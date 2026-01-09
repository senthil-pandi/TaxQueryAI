using System;
using Microsoft.Extensions.Configuration;

namespace TaxNL2SQL.Configuration
{
    /// <summary>
    /// Configuration manager for application settings
    /// </summary>
    public class ConfigurationManager
    {
        private readonly IConfiguration _configuration;

        public ConfigurationManager()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        public string GetConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("TaxManagementDB");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured in appsettings.json");
            }
            return connectionString;
        }

        /// <summary>
        /// Gets Azure OpenAI endpoint
        /// </summary>
        public string GetAzureOpenAIEndpoint()
        {
            var endpoint = _configuration["AzureOpenAI:Endpoint"];
            if (string.IsNullOrWhiteSpace(endpoint) || endpoint.Contains("YOUR_"))
            {
                throw new InvalidOperationException("Azure OpenAI endpoint is not configured. Please update appsettings.json with your Azure OpenAI endpoint.");
            }
            return endpoint;
        }

        /// <summary>
        /// Gets Azure OpenAI API key
        /// </summary>
        public string GetAzureOpenAIApiKey()
        {
            var apiKey = _configuration["AzureOpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("YOUR_"))
            {
                throw new InvalidOperationException("Azure OpenAI API key is not configured. Please update appsettings.json with your API key.");
            }
            return apiKey;
        }

        /// <summary>
        /// Gets Azure OpenAI deployment name
        /// </summary>
        public string GetDeploymentName()
        {
            var deploymentName = _configuration["AzureOpenAI:DeploymentName"];
            if (string.IsNullOrWhiteSpace(deploymentName) || deploymentName.Contains("YOUR_"))
            {
                throw new InvalidOperationException("Azure OpenAI deployment name is not configured. Please update appsettings.json with your deployment name.");
            }
            return deploymentName;
        }

        /// <summary>
        /// Gets whether to show SQL queries in console
        /// </summary>
        public bool ShowSQLQuery()
        {
            return _configuration.GetValue<bool>("Application:ShowSQLQuery", true);
        }

        /// <summary>
        /// Gets whether to show raw query results
        /// </summary>
        public bool ShowRawResults()
        {
            return _configuration.GetValue<bool>("Application:ShowRawResults", false);
        }

        /// <summary>
        /// Gets whether chat history is enabled
        /// </summary>
        public bool EnableChatHistory()
        {
            return _configuration.GetValue<bool>("Application:EnableChatHistory", true);
        }

        /// <summary>
        /// Gets maximum chat history messages
        /// </summary>
        public int GetMaxHistoryMessages()
        {
            return _configuration.GetValue<int>("Application:MaxHistoryMessages", 10);
        }

        /// <summary>
        /// Validates all required configuration settings
        /// </summary>
        public void ValidateConfiguration()
        {
            try
            {
                GetConnectionString();
                GetAzureOpenAIEndpoint();
                GetAzureOpenAIApiKey();
                GetDeploymentName();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Configuration validation failed: {ex.Message}", ex);
            }
        }
    }
}
