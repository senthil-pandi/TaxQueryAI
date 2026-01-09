# Tax NL2SQL - Natural Language to SQL Query Assistant

A C# console application that converts natural language questions into SQL queries using Azure OpenAI, specifically designed for tax management databases.

## Features

- 🤖 Natural language to SQL conversion using Azure OpenAI (GPT-4)
- 💬 Interactive chat interface with conversation history
- 📊 Automatic query execution and formatted results
- 🔍 Database schema analysis and relationship mapping
- 🎨 Colorful console output with clear formatting
- ⚙️ Configurable settings for query display and history

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server or SQL Server LocalDB
- Azure OpenAI API access

## Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/NL2SQL.git
   cd NL2SQL
   ```

2. **Configure Azure OpenAI**
   - Copy `TaxNL2SQL/appsettings.example.json` to `TaxNL2SQL/appsettings.json`
   - Update with your Azure OpenAI credentials:
     ```json
     {
       "AzureOpenAI": {
         "Endpoint": "https://YOUR_RESOURCE_NAME.openai.azure.com/",
         "ApiKey": "YOUR_AZURE_OPENAI_API_KEY",
         "DeploymentName": "YOUR_DEPLOYMENT_NAME"
       }
     }
     ```

3. **Setup Database**
   - Ensure SQL Server or LocalDB is running
   - Run the database setup scripts in order:
     ```bash
     # From SSMS or sqlcmd
     Database/01_CreateSchema.sql
     Database/02_SeedData.sql
     ```

4. **Build and Run**
   ```bash
   cd TaxNL2SQL
   dotnet build
   dotnet run
   ```

## Project Structure

```
NL2SQL/
├── TaxNL2SQL/              # Main application
│   ├── Configuration/      # Configuration management
│   ├── Data/               # Database helpers and query strategies
│   ├── Services/           # Azure OpenAI service
│   ├── Utils/              # Utilities (schema fetcher, formatter)
│   └── appsettings.json    # Application configuration (not in repo)
├── Database/               # Database setup scripts
│   ├── 01_CreateSchema.sql # Schema creation
│   └── 02_SeedData.sql     # Sample data
└── TaxAssistantNL2SQL/     # Additional project components
```

## Usage Examples

Once running, you can ask natural language questions like:

- "Show me all taxpayers"
- "What is the total income for taxpayer TP001?"
- "List all tax returns filed in 2023"
- "Show deductions for married filing jointly status"

Type `help` for more commands, or press Enter to exit.

## Configuration

Edit `appsettings.json` to customize:

- `ShowSQLQuery`: Display generated SQL queries (true/false)
- `ShowRawResults`: Show raw query results (true/false)
- `EnableChatHistory`: Enable conversation history (true/false)
- `MaxHistoryMessages`: Maximum messages to keep in history

## Security Note

⚠️ **Never commit `appsettings.json` with real API keys!** This file is excluded in `.gitignore`. Always use the example template and add your credentials locally.

## License

This is a sample/POC project for demonstration purposes.

## Contributing

This is a personal project. Feel free to fork and modify for your own use.
