-- =====================================================
-- Tax Management Database - Seed Data
-- Realistic tax data for testing NL2SQL application
-- =====================================================

USE TaxManagementDB;
GO

PRINT 'Starting data seeding...';
GO

-- =====================================================
-- Seed: IncomeTypes
-- =====================================================
SET IDENTITY_INSERT dbo.IncomeTypes ON;

INSERT INTO dbo.IncomeTypes (IncomeTypeID, IncomeTypeCode, IncomeTypeName, Description, IsActive)
VALUES
    (1, 'W2', 'W-2 Wages', 'Wages, salaries, tips reported on Form W-2', 1),
    (2, '1099-INT', 'Interest Income', 'Interest income from banks and financial institutions', 1),
    (3, '1099-DIV', 'Dividend Income', 'Dividends and distributions from investments', 1),
    (4, '1099-MISC', 'Miscellaneous Income', 'Self-employment and contract work income', 1),
    (5, 'RENTAL', 'Rental Income', 'Income from rental properties', 1),
    (6, 'BUSINESS', 'Business Income', 'Income from business operations', 1),
    (7, 'CAPITAL', 'Capital Gains', 'Capital gains from sale of assets', 1),
    (8, 'PENSION', 'Retirement Income', 'Pension and retirement distributions', 1);

SET IDENTITY_INSERT dbo.IncomeTypes OFF;
GO

-- =====================================================
-- Seed: DeductionTypes
-- =====================================================
SET IDENTITY_INSERT dbo.DeductionTypes ON;

INSERT INTO dbo.DeductionTypes (DeductionTypeID, DeductionTypeCode, DeductionTypeName, Description, MaximumAmount, IsActive)
VALUES
    (1, 'STD-SINGLE', 'Standard Deduction - Single', 'Standard deduction for single filers', 13850.00, 1),
    (2, 'STD-MFJ', 'Standard Deduction - MFJ', 'Standard deduction for married filing jointly', 27700.00, 1),
    (3, 'MORTGAGE-INT', 'Mortgage Interest', 'Interest paid on mortgage loans', 750000.00, 1),
    (4, 'PROPERTY-TAX', 'Property Tax', 'State and local property taxes', 10000.00, 1),
    (5, 'CHARITY', 'Charitable Contributions', 'Donations to qualified charities', NULL, 1),
    (6, 'MEDICAL', 'Medical Expenses', 'Medical and dental expenses exceeding 7.5% AGI', NULL, 1),
    (7, 'STATE-TAX', 'State Income Tax', 'State and local income taxes', 10000.00, 1),
    (8, 'STUDENT-LOAN', 'Student Loan Interest', 'Interest paid on student loans', 2500.00, 1),
    (9, 'BUSINESS-EXP', 'Business Expenses', 'Ordinary and necessary business expenses', NULL, 1),
    (10, 'HOME-OFFICE', 'Home Office Deduction', 'Home office expenses for self-employed', NULL, 1);

SET IDENTITY_INSERT dbo.DeductionTypes OFF;
GO

-- =====================================================
-- Seed: TaxBrackets for 2024
-- =====================================================
-- Single Filers
INSERT INTO dbo.TaxBrackets (TaxYear, FilingStatus, MinIncome, MaxIncome, TaxRate, BaseAmount)
VALUES
    (2024, 'Single', 0.00, 11600.00, 10.00, 0.00),
    (2024, 'Single', 11600.01, 47150.00, 12.00, 1160.00),
    (2024, 'Single', 47150.01, 100525.00, 22.00, 5426.00),
    (2024, 'Single', 100525.01, 191950.00, 24.00, 17168.50),
    (2024, 'Single', 191950.01, 243725.00, 32.00, 39110.50),
    (2024, 'Single', 243725.01, 609350.00, 35.00, 55678.50),
    (2024, 'Single', 609350.01, NULL, 37.00, 183647.25);

-- Married Filing Jointly
INSERT INTO dbo.TaxBrackets (TaxYear, FilingStatus, MinIncome, MaxIncome, TaxRate, BaseAmount)
VALUES
    (2024, 'Married Filing Jointly', 0.00, 23200.00, 10.00, 0.00),
    (2024, 'Married Filing Jointly', 23200.01, 94300.00, 12.00, 2320.00),
    (2024, 'Married Filing Jointly', 94300.01, 201050.00, 22.00, 10852.00),
    (2024, 'Married Filing Jointly', 201050.01, 383900.00, 24.00, 34337.00),
    (2024, 'Married Filing Jointly', 383900.01, 487450.00, 32.00, 78221.00),
    (2024, 'Married Filing Jointly', 487450.01, 731200.00, 35.00, 111357.00),
    (2024, 'Married Filing Jointly', 731200.01, NULL, 37.00, 196669.50);

-- Head of Household
INSERT INTO dbo.TaxBrackets (TaxYear, FilingStatus, MinIncome, MaxIncome, TaxRate, BaseAmount)
VALUES
    (2024, 'Head of Household', 0.00, 16550.00, 10.00, 0.00),
    (2024, 'Head of Household', 16550.01, 63100.00, 12.00, 1655.00),
    (2024, 'Head of Household', 63100.01, 100500.00, 22.00, 7241.00),
    (2024, 'Head of Household', 100500.01, 191950.00, 24.00, 15469.00),
    (2024, 'Head of Household', 191950.01, 243700.00, 32.00, 37417.00),
    (2024, 'Head of Household', 243700.01, 609350.00, 35.00, 53977.00),
    (2024, 'Head of Household', 609350.01, NULL, 37.00, 181954.50);
GO

-- =====================================================
-- Seed: TaxPayers (Realistic data)
-- =====================================================
SET IDENTITY_INSERT dbo.TaxPayers ON;

INSERT INTO dbo.TaxPayers (TaxPayerID, TaxPayerNumber, FirstName, LastName, DateOfBirth, SSN, Email, PhoneNumber, Address, City, State, ZipCode, FilingStatus, RegisteredDate, IsActive)
VALUES
    (1, 'TP2024001', 'Sarah', 'Johnson', '1985-03-15', '123-45-6789', 'sarah.johnson@email.com', '555-0101', '123 Maple Street', 'Seattle', 'WA', '98101', 'Single', '2020-01-15', 1),
    (2, 'TP2024002', 'Michael', 'Chen', '1978-07-22', '234-56-7890', 'michael.chen@email.com', '555-0102', '456 Oak Avenue', 'San Francisco', 'CA', '94102', 'Married Filing Jointly', '2019-03-20', 1),
    (3, 'TP2024003', 'Emily', 'Rodriguez', '1990-11-08', '345-67-8901', 'emily.rodriguez@email.com', '555-0103', '789 Pine Road', 'Austin', 'TX', '78701', 'Single', '2021-06-10', 1),
    (4, 'TP2024004', 'David', 'Williams', '1982-05-30', '456-78-9012', 'david.williams@email.com', '555-0104', '321 Elm Boulevard', 'New York', 'NY', '10001', 'Married Filing Jointly', '2018-09-05', 1),
    (5, 'TP2024005', 'Jennifer', 'Brown', '1975-09-12', '567-89-0123', 'jennifer.brown@email.com', '555-0105', '654 Cedar Lane', 'Boston', 'MA', '02108', 'Head of Household', '2020-04-18', 1),
    (6, 'TP2024006', 'Robert', 'Davis', '1988-02-25', '678-90-1234', 'robert.davis@email.com', '555-0106', '987 Birch Court', 'Denver', 'CO', '80202', 'Single', '2022-01-22', 1),
    (7, 'TP2024007', 'Lisa', 'Martinez', '1983-12-17', '789-01-2345', 'lisa.martinez@email.com', '555-0107', '147 Spruce Drive', 'Miami', 'FL', '33101', 'Married Filing Jointly', '2019-11-30', 1),
    (8, 'TP2024008', 'James', 'Anderson', '1992-06-03', '890-12-3456', 'james.anderson@email.com', '555-0108', '258 Willow Way', 'Chicago', 'IL', '60601', 'Single', '2021-08-14', 1),
    (9, 'TP2024009', 'Maria', 'Garcia', '1980-04-28', '901-23-4567', 'maria.garcia@email.com', '555-0109', '369 Ash Street', 'Phoenix', 'AZ', '85001', 'Head of Household', '2020-07-09', 1),
    (10, 'TP2024010', 'Christopher', 'Wilson', '1987-10-14', '012-34-5678', 'chris.wilson@email.com', '555-0110', '741 Poplar Avenue', 'Portland', 'OR', '97201', 'Married Filing Jointly', '2019-12-12', 1);

SET IDENTITY_INSERT dbo.TaxPayers OFF;
GO

-- =====================================================
-- Seed: TaxReturns for 2024
-- =====================================================
SET IDENTITY_INSERT dbo.TaxReturns ON;

INSERT INTO dbo.TaxReturns (TaxReturnID, TaxPayerID, TaxYear, ReturnNumber, FilingDate, FilingStatus, GrossIncome, TaxableIncome, TotalDeductions, TotalCredits, TaxLiability, TaxWithheld, RefundAmount, AmountDue, ReturnStatus, ProcessedDate)
VALUES
    (1, 1, 2024, 'RET2024-001', '2024-04-10', 'Single', 75000.00, 61150.00, 13850.00, 0.00, 9798.00, 11250.00, 1452.00, NULL, 'Accepted', '2024-04-15'),
    (2, 2, 2024, 'RET2024-002', '2024-03-28', 'Married Filing Jointly', 185000.00, 157300.00, 27700.00, 2000.00, 24929.00, 23000.00, NULL, 1929.00, 'Accepted', '2024-04-02'),
    (3, 3, 2024, 'RET2024-003', '2024-04-05', 'Single', 52000.00, 38150.00, 13850.00, 1000.00, 4183.00, 6500.00, 2317.00, NULL, 'Accepted', '2024-04-12'),
    (4, 4, 2024, 'RET2024-004', '2024-04-15', 'Married Filing Jointly', 245000.00, 217300.00, 27700.00, 4000.00, 39365.00, 38000.00, NULL, 1365.00, 'Filed', NULL),
    (5, 5, 2024, 'RET2024-005', '2024-03-15', 'Head of Household', 95000.00, 73900.00, 21100.00, 3000.00, 11083.00, 14000.00, 2917.00, NULL, 'Accepted', '2024-03-22'),
    (6, 6, 2024, 'RET2024-006', '2024-04-08', 'Single', 68000.00, 54150.00, 13850.00, 0.00, 8258.00, 10200.00, 1942.00, NULL, 'Accepted', '2024-04-14'),
    (7, 7, 2024, 'RET2024-007', '2024-03-30', 'Married Filing Jointly', 165000.00, 137300.00, 27700.00, 2500.00, 19669.00, 24000.00, 4331.00, NULL, 'Accepted', '2024-04-05'),
    (8, 8, 2024, 'RET2024-008', '2024-04-12', 'Single', 45000.00, 31150.00, 13850.00, 500.00, 3181.00, 5800.00, 2619.00, NULL, 'Accepted', '2024-04-16'),
    (9, 9, 2024, 'RET2024-009', '2024-03-20', 'Head of Household', 112000.00, 90900.00, 21100.00, 4000.00, 13653.00, 16000.00, 2347.00, NULL, 'Accepted', '2024-03-28'),
    (10, 10, 2024, 'RET2024-010', '2024-04-01', 'Married Filing Jointly', 198000.00, 170300.00, 27700.00, 3000.00, 28051.00, 30000.00, 1949.00, NULL, 'Accepted', '2024-04-08');

SET IDENTITY_INSERT dbo.TaxReturns OFF;
GO

-- =====================================================
-- Seed: Incomes (Detailed income for each return)
-- =====================================================
-- Sarah Johnson (TaxReturnID 1)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (1, 1, 'Tech Solutions Inc', 72000.00, '2024-12-31', 10800.00),
    (1, 2, 'First National Bank', 2500.00, '2024-12-31', 450.00),
    (1, 3, 'Investment Portfolio', 500.00, '2024-12-31', 0.00);

-- Michael Chen (TaxReturnID 2)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (2, 1, 'Global Finance Corp', 125000.00, '2024-12-31', 18750.00),
    (2, 1, 'Partner Spouse Income', 55000.00, '2024-12-31', 8250.00),
    (2, 3, 'Stock Dividends', 5000.00, '2024-12-31', 0.00);

-- Emily Rodriguez (TaxReturnID 3)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (3, 1, 'Marketing Agency LLC', 48000.00, '2024-12-31', 7200.00),
    (3, 4, 'Freelance Consulting', 4000.00, '2024-12-31', 0.00);

-- David Williams (TaxReturnID 4)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (4, 1, 'Law Firm Partners', 180000.00, '2024-12-31', 27000.00),
    (4, 1, 'Spouse Income', 60000.00, '2024-12-31', 9000.00),
    (4, 5, 'Rental Property', 5000.00, '2024-12-31', 0.00);

-- Jennifer Brown (TaxReturnID 5)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (5, 1, 'Education Institute', 85000.00, '2024-12-31', 12750.00),
    (5, 2, 'Savings Account Interest', 1500.00, '2024-12-31', 0.00),
    (5, 3, 'Mutual Funds', 8500.00, '2024-12-31', 1250.00);

-- Robert Davis (TaxReturnID 6)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (6, 1, 'Software Development Co', 65000.00, '2024-12-31', 9750.00),
    (6, 4, 'Contract Work', 3000.00, '2024-12-31', 0.00);

-- Lisa Martinez (TaxReturnID 7)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (7, 1, 'Healthcare Provider', 98000.00, '2024-12-31', 14700.00),
    (7, 1, 'Spouse Income', 62000.00, '2024-12-31', 9300.00),
    (7, 2, 'Bank Interest', 5000.00, '2024-12-31', 0.00);

-- James Anderson (TaxReturnID 8)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (8, 1, 'Retail Management', 42000.00, '2024-12-31', 6300.00),
    (8, 4, 'Side Business', 3000.00, '2024-12-31', 0.00);

-- Maria Garcia (TaxReturnID 9)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (9, 1, 'Engineering Firm', 105000.00, '2024-12-31', 15750.00),
    (9, 5, 'Rental Income', 7000.00, '2024-12-31', 0.00);

-- Christopher Wilson (TaxReturnID 10)
INSERT INTO dbo.Incomes (TaxReturnID, IncomeTypeID, SourceName, IncomeAmount, IncomeDate, TaxWithheld)
VALUES
    (10, 1, 'Construction Company', 115000.00, '2024-12-31', 17250.00),
    (10, 1, 'Spouse Income', 78000.00, '2024-12-31', 11700.00),
    (10, 3, 'Investment Income', 5000.00, '2024-12-31', 1050.00);
GO

-- =====================================================
-- Seed: Deductions
-- =====================================================
-- TaxReturn 1 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (1, 1, 13850.00, 'Standard deduction for single filer');

-- TaxReturn 2 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (2, 2, 27700.00, 'Standard deduction for married filing jointly');

-- TaxReturn 3 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (3, 1, 13850.00, 'Standard deduction for single filer');

-- TaxReturn 4 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (4, 2, 27700.00, 'Standard deduction for married filing jointly');

-- TaxReturn 5 - Itemized Deductions
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES
    (5, 3, 12000.00, 'Mortgage interest on primary residence'),
    (5, 4, 5000.00, 'Property taxes'),
    (5, 5, 4100.00, 'Charitable contributions');

-- TaxReturn 6 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (6, 1, 13850.00, 'Standard deduction for single filer');

-- TaxReturn 7 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (7, 2, 27700.00, 'Standard deduction for married filing jointly');

-- TaxReturn 8 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (8, 1, 13850.00, 'Standard deduction for single filer');

-- TaxReturn 9 - Itemized Deductions
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES
    (9, 3, 15000.00, 'Mortgage interest'),
    (9, 4, 6100.00, 'Property and state taxes');

-- TaxReturn 10 - Standard Deduction
INSERT INTO dbo.Deductions (TaxReturnID, DeductionTypeID, DeductionAmount, Description)
VALUES (10, 2, 27700.00, 'Standard deduction for married filing jointly');
GO

-- =====================================================
-- Seed: TaxCredits
-- =====================================================
INSERT INTO dbo.TaxCredits (TaxReturnID, CreditType, CreditAmount, Description)
VALUES
    (2, 'Child Tax Credit', 2000.00, 'Credit for one qualifying child'),
    (3, 'Saver''s Credit', 1000.00, 'Retirement savings contribution credit'),
    (4, 'Child Tax Credit', 4000.00, 'Credit for two qualifying children'),
    (5, 'Earned Income Credit', 2000.00, 'EITC for head of household'),
    (5, 'Child Tax Credit', 1000.00, 'Credit for one qualifying child'),
    (7, 'Child Tax Credit', 2500.00, 'Credit for qualifying children'),
    (8, 'Education Credit', 500.00, 'Lifetime Learning Credit'),
    (9, 'Child Tax Credit', 4000.00, 'Credit for two qualifying children'),
    (10, 'Child Tax Credit', 3000.00, 'Credit for qualifying children');
GO

-- =====================================================
-- Seed: TaxPayments
-- =====================================================
INSERT INTO dbo.TaxPayments (TaxReturnID, PaymentDate, PaymentAmount, PaymentMethod, PaymentReference, PaymentStatus)
VALUES
    (2, '2024-04-15', 1929.00, 'Bank Transfer', 'PYMT-2024-002-001', 'Completed'),
    (4, '2024-04-15', 1365.00, 'Credit Card', 'PYMT-2024-004-001', 'Completed');
GO

PRINT 'Data seeding completed successfully!';
PRINT '-------------------------------------------';
PRINT 'Summary:';
PRINT '  - 8 Income Types';
PRINT '  - 10 Deduction Types';
PRINT '  - 10 TaxPayers';
PRINT '  - 10 TaxReturns for 2024';
PRINT '  - Multiple Incomes per return';
PRINT '  - Deductions and Credits';
PRINT '  - 2024 Tax Brackets';
PRINT '-------------------------------------------';
GO
