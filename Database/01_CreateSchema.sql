-- =====================================================
-- Tax Management Database Schema
-- Created for NL2SQL Tax Application
-- =====================================================

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TaxManagementDB')
BEGIN
    CREATE DATABASE TaxManagementDB;
END
GO

USE TaxManagementDB;
GO

-- =====================================================
-- Table: TaxPayers
-- Description: Stores taxpayer information
-- =====================================================
IF OBJECT_ID('dbo.TaxPayers', 'U') IS NOT NULL
    DROP TABLE dbo.TaxPayers;
GO

CREATE TABLE dbo.TaxPayers (
    TaxPayerID          INT IDENTITY(1,1) PRIMARY KEY,
    TaxPayerNumber      NVARCHAR(50) NOT NULL UNIQUE,
    FirstName           NVARCHAR(100) NOT NULL,
    LastName            NVARCHAR(100) NOT NULL,
    DateOfBirth         DATE NOT NULL,
    SSN                 NVARCHAR(11) NOT NULL UNIQUE, -- Social Security Number
    Email               NVARCHAR(255),
    PhoneNumber         NVARCHAR(20),
    Address             NVARCHAR(500),
    City                NVARCHAR(100),
    State               NVARCHAR(50),
    ZipCode             NVARCHAR(10),
    FilingStatus        NVARCHAR(20) NOT NULL, -- Single, Married Filing Jointly, etc.
    RegisteredDate      DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive            BIT NOT NULL DEFAULT 1,
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate        DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_TaxPayers_FilingStatus CHECK (FilingStatus IN ('Single', 'Married Filing Jointly', 'Married Filing Separately', 'Head of Household', 'Qualifying Widow(er)'))
);
GO

-- =====================================================
-- Table: TaxReturns
-- Description: Stores tax return filings
-- =====================================================
IF OBJECT_ID('dbo.TaxReturns', 'U') IS NOT NULL
    DROP TABLE dbo.TaxReturns;
GO

CREATE TABLE dbo.TaxReturns (
    TaxReturnID         INT IDENTITY(1,1) PRIMARY KEY,
    TaxPayerID          INT NOT NULL,
    TaxYear             INT NOT NULL,
    ReturnNumber        NVARCHAR(50) NOT NULL UNIQUE,
    FilingDate          DATETIME NOT NULL,
    FilingStatus        NVARCHAR(20) NOT NULL,
    GrossIncome         DECIMAL(18,2) NOT NULL,
    TaxableIncome       DECIMAL(18,2) NOT NULL,
    TotalDeductions     DECIMAL(18,2) NOT NULL DEFAULT 0,
    TotalCredits        DECIMAL(18,2) NOT NULL DEFAULT 0,
    TaxLiability        DECIMAL(18,2) NOT NULL,
    TaxWithheld         DECIMAL(18,2) NOT NULL DEFAULT 0,
    RefundAmount        DECIMAL(18,2) NULL,
    AmountDue           DECIMAL(18,2) NULL,
    ReturnStatus        NVARCHAR(20) NOT NULL DEFAULT 'Filed',
    ProcessedDate       DATETIME NULL,
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate        DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TaxReturns_TaxPayers FOREIGN KEY (TaxPayerID) REFERENCES dbo.TaxPayers(TaxPayerID),
    CONSTRAINT CK_TaxReturns_Status CHECK (ReturnStatus IN ('Draft', 'Filed', 'Accepted', 'Rejected', 'Amended', 'Under Review')),
    CONSTRAINT CK_TaxReturns_FilingStatus CHECK (FilingStatus IN ('Single', 'Married Filing Jointly', 'Married Filing Separately', 'Head of Household', 'Qualifying Widow(er)'))
);
GO

-- =====================================================
-- Table: IncomeTypes
-- Description: Reference table for income categories
-- =====================================================
IF OBJECT_ID('dbo.IncomeTypes', 'U') IS NOT NULL
    DROP TABLE dbo.IncomeTypes;
GO

CREATE TABLE dbo.IncomeTypes (
    IncomeTypeID        INT IDENTITY(1,1) PRIMARY KEY,
    IncomeTypeCode      NVARCHAR(20) NOT NULL UNIQUE,
    IncomeTypeName      NVARCHAR(100) NOT NULL,
    Description         NVARCHAR(500),
    IsActive            BIT NOT NULL DEFAULT 1
);
GO

-- =====================================================
-- Table: Incomes
-- Description: Stores detailed income information
-- =====================================================
IF OBJECT_ID('dbo.Incomes', 'U') IS NOT NULL
    DROP TABLE dbo.Incomes;
GO

CREATE TABLE dbo.Incomes (
    IncomeID            INT IDENTITY(1,1) PRIMARY KEY,
    TaxReturnID         INT NOT NULL,
    IncomeTypeID        INT NOT NULL,
    SourceName          NVARCHAR(200) NOT NULL, -- Employer, Bank, etc.
    IncomeAmount        DECIMAL(18,2) NOT NULL,
    IncomeDate          DATE NOT NULL,
    TaxWithheld         DECIMAL(18,2) NOT NULL DEFAULT 0,
    Description         NVARCHAR(500),
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Incomes_TaxReturns FOREIGN KEY (TaxReturnID) REFERENCES dbo.TaxReturns(TaxReturnID),
    CONSTRAINT FK_Incomes_IncomeTypes FOREIGN KEY (IncomeTypeID) REFERENCES dbo.IncomeTypes(IncomeTypeID),
    CONSTRAINT CK_Incomes_Amount CHECK (IncomeAmount >= 0)
);
GO

-- =====================================================
-- Table: DeductionTypes
-- Description: Reference table for deduction categories
-- =====================================================
IF OBJECT_ID('dbo.DeductionTypes', 'U') IS NOT NULL
    DROP TABLE dbo.DeductionTypes;
GO

CREATE TABLE dbo.DeductionTypes (
    DeductionTypeID     INT IDENTITY(1,1) PRIMARY KEY,
    DeductionTypeCode   NVARCHAR(20) NOT NULL UNIQUE,
    DeductionTypeName   NVARCHAR(100) NOT NULL,
    Description         NVARCHAR(500),
    MaximumAmount       DECIMAL(18,2) NULL,
    IsActive            BIT NOT NULL DEFAULT 1
);
GO

-- =====================================================
-- Table: Deductions
-- Description: Stores tax deductions claimed
-- =====================================================
IF OBJECT_ID('dbo.Deductions', 'U') IS NOT NULL
    DROP TABLE dbo.Deductions;
GO

CREATE TABLE dbo.Deductions (
    DeductionID         INT IDENTITY(1,1) PRIMARY KEY,
    TaxReturnID         INT NOT NULL,
    DeductionTypeID     INT NOT NULL,
    DeductionAmount     DECIMAL(18,2) NOT NULL,
    Description         NVARCHAR(500),
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Deductions_TaxReturns FOREIGN KEY (TaxReturnID) REFERENCES dbo.TaxReturns(TaxReturnID),
    CONSTRAINT FK_Deductions_DeductionTypes FOREIGN KEY (DeductionTypeID) REFERENCES dbo.DeductionTypes(DeductionTypeID),
    CONSTRAINT CK_Deductions_Amount CHECK (DeductionAmount >= 0)
);
GO

-- =====================================================
-- Table: TaxCredits
-- Description: Stores tax credits claimed
-- =====================================================
IF OBJECT_ID('dbo.TaxCredits', 'U') IS NOT NULL
    DROP TABLE dbo.TaxCredits;
GO

CREATE TABLE dbo.TaxCredits (
    TaxCreditID         INT IDENTITY(1,1) PRIMARY KEY,
    TaxReturnID         INT NOT NULL,
    CreditType          NVARCHAR(100) NOT NULL,
    CreditAmount        DECIMAL(18,2) NOT NULL,
    Description         NVARCHAR(500),
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TaxCredits_TaxReturns FOREIGN KEY (TaxReturnID) REFERENCES dbo.TaxReturns(TaxReturnID),
    CONSTRAINT CK_TaxCredits_Amount CHECK (CreditAmount >= 0)
);
GO

-- =====================================================
-- Table: TaxPayments
-- Description: Stores tax payments made by taxpayers
-- =====================================================
IF OBJECT_ID('dbo.TaxPayments', 'U') IS NOT NULL
    DROP TABLE dbo.TaxPayments;
GO

CREATE TABLE dbo.TaxPayments (
    PaymentID           INT IDENTITY(1,1) PRIMARY KEY,
    TaxReturnID         INT NOT NULL,
    PaymentDate         DATETIME NOT NULL,
    PaymentAmount       DECIMAL(18,2) NOT NULL,
    PaymentMethod       NVARCHAR(50) NOT NULL,
    PaymentReference    NVARCHAR(100),
    PaymentStatus       NVARCHAR(20) NOT NULL DEFAULT 'Completed',
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TaxPayments_TaxReturns FOREIGN KEY (TaxReturnID) REFERENCES dbo.TaxReturns(TaxReturnID),
    CONSTRAINT CK_TaxPayments_Amount CHECK (PaymentAmount > 0),
    CONSTRAINT CK_TaxPayments_Method CHECK (PaymentMethod IN ('Credit Card', 'Bank Transfer', 'Check', 'Cash', 'Direct Debit')),
    CONSTRAINT CK_TaxPayments_Status CHECK (PaymentStatus IN ('Pending', 'Completed', 'Failed', 'Refunded'))
);
GO

-- =====================================================
-- Table: TaxBrackets
-- Description: Stores tax bracket information by year
-- =====================================================
IF OBJECT_ID('dbo.TaxBrackets', 'U') IS NOT NULL
    DROP TABLE dbo.TaxBrackets;
GO

CREATE TABLE dbo.TaxBrackets (
    BracketID           INT IDENTITY(1,1) PRIMARY KEY,
    TaxYear             INT NOT NULL,
    FilingStatus        NVARCHAR(20) NOT NULL,
    MinIncome           DECIMAL(18,2) NOT NULL,
    MaxIncome           DECIMAL(18,2) NULL,
    TaxRate             DECIMAL(5,2) NOT NULL,
    BaseAmount          DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedDate         DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_TaxBrackets_FilingStatus CHECK (FilingStatus IN ('Single', 'Married Filing Jointly', 'Married Filing Separately', 'Head of Household', 'Qualifying Widow(er)'))
);
GO

-- =====================================================
-- Create Indexes for Performance
-- =====================================================
CREATE NONCLUSTERED INDEX IX_TaxPayers_TaxPayerNumber ON dbo.TaxPayers(TaxPayerNumber);
CREATE NONCLUSTERED INDEX IX_TaxPayers_SSN ON dbo.TaxPayers(SSN);
CREATE NONCLUSTERED INDEX IX_TaxReturns_TaxPayerID ON dbo.TaxReturns(TaxPayerID);
CREATE NONCLUSTERED INDEX IX_TaxReturns_TaxYear ON dbo.TaxReturns(TaxYear);
CREATE NONCLUSTERED INDEX IX_TaxReturns_ReturnStatus ON dbo.TaxReturns(ReturnStatus);
CREATE NONCLUSTERED INDEX IX_Incomes_TaxReturnID ON dbo.Incomes(TaxReturnID);
CREATE NONCLUSTERED INDEX IX_Deductions_TaxReturnID ON dbo.Deductions(TaxReturnID);
CREATE NONCLUSTERED INDEX IX_TaxCredits_TaxReturnID ON dbo.TaxCredits(TaxReturnID);
CREATE NONCLUSTERED INDEX IX_TaxPayments_TaxReturnID ON dbo.TaxPayments(TaxReturnID);
CREATE NONCLUSTERED INDEX IX_TaxBrackets_TaxYear_FilingStatus ON dbo.TaxBrackets(TaxYear, FilingStatus);
GO

PRINT 'Database schema created successfully!';
GO
