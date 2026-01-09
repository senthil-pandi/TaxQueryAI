# Tax NL2SQL - Quick Setup Guide

## Prerequisites Checklist

Before starting, ensure you have:
- [ ] .NET 8 SDK installed
- [ ] SQL Server Express installed and running
- [ ] Azure OpenAI service with a deployed model
- [ ] SQL Server Management Studio (SSMS) or Azure Data Studio

## Step-by-Step Setup

### 1. Setup Database (5 minutes)

#### Option A: Using SQL Server Management Studio
1. Open SSMS
2. Connect to `localhost\SQLEXPRESS` (or your SQL Server instance)
3. Open and execute `Database/01_CreateSchema.sql` (press F5)
4. Open and execute `Database/02_SeedData.sql` (press F5)
5. Verify: You should see "Data seeding completed successfully!"

#### Option B: Using Command Line
```bash
sqlcmd -S localhost\SQLEXPRESS -E -i Database/01_CreateSchema.sql
sqlcmd -S localhost\SQLEXPRESS -E -i Database/02_SeedData.sql
```

### 2. Configure Azure OpenAI (2 minutes)

1. Open `TaxNL2SQL/appsettings.json`
2. Replace the following placeholders:

```json
{
  "AzureOpenAI": {
    "Endpoint": "https://YOUR-RESOURCE-NAME.openai.azure.com/",
    "ApiKey": "YOUR-API-KEY-HERE",
    "DeploymentName": "YOUR-DEPLOYMENT-NAME"
  }
}
```

#### Where to find these values:
1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to your Azure OpenAI resource
3. **Endpoint**: Go to "Keys and Endpoint" ? Copy "Endpoint"
4. **ApiKey**: Go to "Keys and Endpoint" ? Copy "Key 1" or "Key 2"
5. **DeploymentName**: Go to "Model deployments" ? Copy the deployment name (e.g., "gpt-4", "gpt-35-turbo")

### 3. Build and Run (1 minute)

```bash
# Navigate to solution directory
cd C:\Path\To\TaxNL2SQL

# Build the solution
dotnet build

# Run the application
cd TaxNL2SQL
dotnet run
```

Or simply open `TaxNL2SQL.sln` in Visual Studio and press F5!

## Verify Installation

When you run the application, you should see:

```
????????????????????????????????????????????????????????????????????????????????
?                                                                              ?
?                    ?? TAX NL2SQL ASSISTANT ??                                ?
?                                                                              ?
?              Natural Language to SQL for Tax Management                      ?
?                   Powered by Azure OpenAI                                    ?
?                                                                              ?
????????????????????????????????????????????????????????????????????????????????

?? Loading configuration...
? Configuration loaded successfully

?? Connecting to database...
? Database connected successfully

?? Loading database schema...
? Schema loaded successfully
Database contains 10 tables:
  - dbo.Deductions
  - dbo.DeductionTypes
  - dbo.Incomes
  - dbo.IncomeTypes
  - dbo.TaxBrackets
  - dbo.TaxCredits
  - dbo.TaxPayers
  - dbo.TaxPayments
  - dbo.TaxReturns

?? Initializing Azure OpenAI service...
? Azure OpenAI service initialized
```

## Quick Test Queries

Try these questions to verify everything is working:

1. **Simple Query**:
   ```
   Show me all taxpayers
   ```

2. **Aggregate Query**:
   ```
   Who has the highest tax liability?
   ```

3. **Join Query**:
   ```
   What is the total income for Sarah Johnson?
   ```

4. **Complex Query**:
   ```
   Show taxpayers who received refunds over $2000
   ```

## Troubleshooting

### "Failed to connect to database"
**Solution**: 
- Verify SQL Server Express is running
- Check if the connection string in appsettings.json matches your SQL Server instance
- Try: `Server=localhost;Database=TaxManagementDB;Trusted_Connection=True;TrustServerCertificate=True;`

### "Configuration validation failed"
**Solution**:
- Ensure all Azure OpenAI settings are configured
- Remove "YOUR_" placeholder text
- Verify endpoint URL ends with `.openai.azure.com/`

### "Build failed" or "Package not found"
**Solution**:
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore

# Try building again
dotnet build
```

### Azure OpenAI API errors
**Solution**:
- Verify your API key is valid (not expired)
- Check deployment name exactly matches Azure portal
- Ensure you have available quota for your model
- Try regenerating the API key in Azure portal

## Database Schema Quick Reference

### Core Tables
- **TaxPayers**: 10 taxpayers with personal info
- **TaxReturns**: Tax returns for 2024
- **Incomes**: Income sources (W-2, 1099, etc.)
- **Deductions**: Tax deductions claimed
- **TaxCredits**: Tax credits applied
- **TaxPayments**: Payment records

### Sample Taxpayers
1. Sarah Johnson - Single, $75K income
2. Michael Chen - Married, $185K income
3. Emily Rodriguez - Single, $52K income
4. David Williams - Married, $245K income
5. Jennifer Brown - Head of Household, $95K income
6. Robert Davis - Single, $68K income
7. Lisa Martinez - Married, $165K income
8. James Anderson - Single, $45K income
9. Maria Garcia - Head of Household, $112K income
10. Christopher Wilson - Married, $198K income

## Connection String Examples

### Windows Authentication (Default):
```json
"Server=localhost\\SQLEXPRESS;Database=TaxManagementDB;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=30;"
```

### SQL Server Authentication:
```json
"Server=localhost\\SQLEXPRESS;Database=TaxManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;Connection Timeout=30;"
```

### Azure SQL Database:
```json
"Server=tcp:yourserver.database.windows.net,1433;Database=TaxManagementDB;User Id=yourusername;Password=yourpassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Support Resources

- **Azure OpenAI**: https://learn.microsoft.com/azure/ai-services/openai/
- **.NET 8**: https://dotnet.microsoft.com/download/dotnet/8.0
- **SQL Server Express**: https://www.microsoft.com/sql-server/sql-server-downloads
- **SSMS**: https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms

## Next Steps

Once setup is complete:
1. Try the sample queries above
2. Explore the database schema with the `schema` command
3. Ask your own tax-related questions
4. Review the README.md for more advanced usage

---

**Estimated Total Setup Time: 10-15 minutes**

?? **You're ready to use Tax NL2SQL Assistant!**
