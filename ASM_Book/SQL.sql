CREATE DATABASE Books;
USE Books;

CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500) NULL
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(255) NOT NULL,
    CategoryID INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    StockQuantity INT NOT NULL CHECK (StockQuantity >= 0),
    ImageURL VARBINARY(MAX) NULL,
    Description NVARCHAR(500) NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    Position NVARCHAR(100),
    PhoneNumber NVARCHAR(20) NULL,
    HireDate DATE NULL,
    Salary DECIMAL(10,2) NULL CHECK (Salary >= 0)
);

CREATE TABLE UserAccounts (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT UNIQUE NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'Sales', 'Warehouse')) NOT NULL,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) UNIQUE NOT NULL,
    Email NVARCHAR(255) NULL,
    Address NVARCHAR(500) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Invoices (
    InvoiceID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    EmployeeID INT NULL,  
    TotalAmount DECIMAL(10,2) NOT NULL CHECK (TotalAmount >= 0),
    InvoiceDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) CHECK (Status IN ('Pending', 'Paid', 'Cancelled')) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE SET NULL
);

CREATE TABLE InvoiceDetails (
    InvoiceDetailID INT PRIMARY KEY IDENTITY(1,1),
    InvoiceID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0),
    Total AS (Quantity * UnitPrice) PERSISTED,
    FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Insert data into Categories
INSERT INTO Categories (CategoryName, Description) VALUES
('Fiction', 'Novels and short stories from various authors'),
('Science', 'Books related to scientific discoveries and knowledge'),
('Business', 'Books about entrepreneurship, management, and finance'),
('Technology', 'Latest trends in software, AI, and tech innovations'),
('Self-Help', 'Guides and books for personal development and success');

-- Insert data into Products (Books)
INSERT INTO Products (ProductName, CategoryID, Price, StockQuantity, ImageURL, Description) VALUES
('The Great Gatsby', 1, 15.99, 50, (SELECT * FROM OPENROWSET(BULK N'D:\ASM\Picture\1.png', SINGLE_BLOB) AS Image), 'A classic novel by F. Scott Fitzgerald'),
('A Brief History of Time', 2, 20.50, 40,  (SELECT * FROM OPENROWSET(BULK N'D:\ASM\Picture\2.png', SINGLE_BLOB) AS Image), 'Stephen Hawking''s book on cosmology'),
('Rich Dad Poor Dad', 3, 12.99, 30,  (SELECT * FROM OPENROWSET(BULK N'D:\ASM\Picture\3.png', SINGLE_BLOB) AS Image), 'Personal finance and investing strategies'),
('Artificial Intelligence: A Modern Approach', 4, 35.00, 25,  (SELECT * FROM OPENROWSET(BULK N'D:\ASM\Picture\4.png', SINGLE_BLOB) AS Image), 'Comprehensive book on AI and ML'),
('Atomic Habits', 5, 18.99, 60,  (SELECT * FROM OPENROWSET(BULK N'D:\ASM\Picture\5.png', SINGLE_BLOB) AS Image), 'Guide to building good habits and breaking bad ones');

-- Insert data into Employees
INSERT INTO Employees (FullName, Position, PhoneNumber, HireDate, Salary) VALUES
('John Doe', 'Sales Manager', '1234567890', '2022-01-15', 4500.00),
('Jane Smith', 'Cashier', '0987654321', '2021-06-20', 3000.00),
('Michael Brown', 'Warehouse Manager', '1122334455', '2023-03-10', 4000.00),
('Emma Johnson', 'Sales Assistant', '2233445566', '2020-09-05', 2800.00),
('David Wilson', 'Administrator', '3344556677', '2019-12-01', 5000.00);

-- Insert data into UserAccounts
INSERT INTO UserAccounts (EmployeeID, Username, Password, Role, IsActive) VALUES
(1, 'vu1', '111', 'Admin', 1),
(2, 'vu2', '111', 'Sales', 1),
(3, 'vu3', '111', 'Warehouse', 1);

-- Insert data into Customers
INSERT INTO Customers (FullName, PhoneNumber, Email, Address) VALUES
('Alice Cooper', '05551234567', 'alice@example.com', '123 Elm Street'),
('Bob Marley', '05559876543', 'bob@example.com', '456 Oak Avenue'),
('Charlie Chaplin', '05554567890', 'charlie@example.com', '789 Pine Road'),
('Diana Ross', '05552345678', 'diana@example.com', '101 Maple Drive'),
('Edward Norton', '05556789012', 'edward@example.com', '202 Birch Lane');

-- Insert data into Invoices
INSERT INTO Invoices (CustomerID, EmployeeID, TotalAmount, InvoiceDate, Status) VALUES
(1, 1, 45.98, '2024-03-10', 'Paid'),
(2, 2, 20.50, '2024-03-11', 'Pending'),
(3, 3, 70.00, '2024-03-12', 'Paid');

-- Insert data into InvoiceDetails
INSERT INTO InvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice) VALUES
(1, 1, 2, 15.99),
(2, 2, 1, 18.99),
(3, 3, 1, 20.50);

