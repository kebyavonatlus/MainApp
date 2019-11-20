CREATE TABLE Users
(
	UserId INT PRIMARY KEY IDENTITY(1,1),
	Login NVARCHAR(50) NOT NULL UNIQUE,
	Email NVARCHAR(300) NOT NULL,
	Password NVARCHAR(50) NOT NULL,
	FullName NVARCHAR(200) NOT NULL,
	IdentificationNumber NVARCHAR(14) NULL,
	BirthDate DATE NULL,
	Address NVARCHAR(300) NULL,
	IsCustomer BIT 
)

CREATE TABLE Roles 
(
	RoleId INT PRIMARY KEY IDENTITY(1,1),
	RoleName NVARCHAR(300) NOT NULL
)

CREATE TABLE UserRoles
(
	UserId INT NOT NULL,
	RoleId INT NOT NULL,
	PRIMARY KEY(UserId,RoleId),
	FOREIGN KEY (RoleId) REFERENCES dbo.Roles(RoleId),
	FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId)
)

CREATE TABLE Accounts
(
	AccountNumber INT PRIMARY KEY IDENTITY(1,1),
	AccountName NVARCHAR(300) NOT NULL,
	AccountOpenDate DATETIME NOT NULL,
	AccountCloseDate DATETIME NULL,
	Balance NUMERIC(15,2) NOT NULL,
	Currency CHAR(5) NOT NULL,
	UserId INT FOREIGN KEY REFERENCES dbo.Users(UserId) NOT NULL
)

CREATE TABLE dbo.Histories
(
	HistoryId INT PRIMARY KEY IDENTITY(1,1),
	DtAccount INT NOT NULL,
	CtAccount INT NOT NULL,
	[Sum] NUMERIC(15,2) NOT NULL,
	Comment NVARCHAR(300) NOT NULL,
	OperationDate DATETIME NOT NULL,
	UserId INT FOREIGN KEY REFERENCES dbo.Users(UserId) NOT NULL
)

CREATE TABLE dbo.Transfers
(
	TransferId INT PRIMARY KEY IDENTITY(1,1),
	AccountFrom INT NOT NULL,
	AccountTo INT NOT NULL,
	SenderUserId INT FOREIGN KEY REFERENCES dbo.Users(UserId) NOT NULL,
	ReceiverUserId INT FOREIGN KEY REFERENCES dbo.Users (UserId) NOT NULL,
	Comment NVARCHAR(300) NOT NULL,
	TransferSum NUMERIC(15,2) NOT NULL,
	TransferDate DATETIME NOT NULL,
	Comission NUMERIC(15,2) NULL
)

CREATE TABLE dbo.TransferHistories
(
	TransferId INT FOREIGN KEY REFERENCES dbo.Transfers(TransferId) NOT NULL,
	HistoryId INT FOREIGN KEY REFERENCES dbo.Histories(HistoryId) NOT NULL
	PRIMARY KEY(TransferId, HistoryId)
)


CREATE TABLE dbo.UtilityCategories
(
	UtilityCategoryId INT PRIMARY KEY IDENTITY(1,1),
	UtilityCategoryName NVARCHAR(300) NOT NULL,
	UtilityCategoryDescription NVARCHAR(300) NOT NULL
)

CREATE TABLE dbo.Utilities
(
	UtilityId INT PRIMARY KEY IDENTITY(1,1),
	UtilityName NVARCHAR(300) NOT NULL,
	UtilityDescription NVARCHAR(300) NULL,
	UtilityCategoryId INT FOREIGN KEY REFERENCES UtilityCategories(UtilityCategoryId) NOT NULL,
	UtilityAccountNumber INT FOREIGN KEY REFERENCES dbo.Accounts(AccountNumber) NOT NULL,
	UtilityImagePath NVARCHAR(MAX) NULL
)	


CREATE TABLE dbo.Payments
(
	PaymentId INT PRIMARY KEY IDENTITY(1,1),
	UtilityId INT FOREIGN KEY REFERENCES dbo.Utilities(UtilityId) NOT NULL,
	PaymentComission NUMERIC(15,2) NULL,
	PaymentSum NUMERIC(15,2) NOT NULL,
	PaymentComment NVARCHAR(200) NOT NULL,
	PaymentDate DATETIME NOT NULL,
	UserId INT FOREIGN KEY REFERENCES dbo.Users(UserId) NOT NULL
)

CREATE TABLE dbo.PaymentHistories
(
	PaymentId INT FOREIGN KEY REFERENCES dbo.Payments(PaymentId) NOT NULL,
	HistoryId INT FOREIGN KEY REFERENCES dbo.Histories(HistoryId) NOT NULL
	PRIMARY KEY(PaymentId, HistoryId)
)
