CREATE DATABASE Shop;
USE Shop;

-- Bảng Nhà Cung Cấp (Suppliers)
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(255) NOT NULL,
    ContactName NVARCHAR(255),
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    Email NVARCHAR(255),
    Website NVARCHAR(255)
);

-- Bảng Khách Hàng (Customers)
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    Email NVARCHAR(255),
    DateOfBirth DATE
);

-- Bảng Nhân Viên (Employees)
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeName NVARCHAR(255) NOT NULL,
    Position NVARCHAR(100),
    Authority NVARCHAR(50),
    Email NVARCHAR(255) UNIQUE,
    HireDate DATE
);

-- Bảng Người Dùng (Users) (Liên kết với cả Employees và Customers)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL, 
    EmployeeID INT UNIQUE, 
    CustomerID INT UNIQUE,
    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'Sales', 'Warehouse')) NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE
);

-- Bảng Sản Phẩm (Products)
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductCode NVARCHAR(50) UNIQUE NOT NULL,
    ProductName NVARCHAR(255) NOT NULL,
    SellingPrice DECIMAL(18,2) NOT NULL,
    InventoryQuantity INT NOT NULL CHECK (InventoryQuantity >= 0),
    SupplierID INT,
    Image VARBINARY(MAX) NULL,
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE SET NULL
);

-- Bảng Hóa Đơn (Invoices)
CREATE TABLE Invoices (
    InvoiceID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceDate DATETIME DEFAULT GETDATE(),
    CustomerID INT,
    EmployeeID INT,
    TotalAmount DECIMAL(18,2) NOT NULL CHECK (TotalAmount >= 0),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE SET NULL
);

-- Bảng Chi Tiết Hóa Đơn (InvoiceDetails)
CREATE TABLE InvoiceDetails (
    InvoiceDetailID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceID INT,
    ProductID INT,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice >= 0),
    SubTotal AS (Quantity * UnitPrice) PERSISTED,
    FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE SET NULL
);

-- Bảng Lịch Sử Mua Hàng của Khách Hàng (Customer Purchase History)
CREATE TABLE CustomerPurchaseHistory (
    HistoryID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NULL,
    InvoiceID INT NULL,
    PurchaseDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE SET NULL,
    FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID) ON DELETE NO ACTION --
);

-- Insert dữ liệu vào bảng Suppliers (Nhà Cung Cấp)
INSERT INTO Suppliers (SupplierName, ContactName, Phone, Address, Email, Website) VALUES
('Hoa Tuoi ABC', 'Nguyen Van A', '0987654321', '123 Le Loi, Quan 1, TP HCM', 'hoaabc@gmail.com', 'www.hoaabc.com'),
('Shop Hoa Dep', 'Tran Thi B', '0977123456', '45 Tran Hung Dao, Ha Noi', 'shophoadep@gmail.com', 'www.shophoadep.com'),
('Dien Hoa 24h', 'Le Van C', '0903456789', '99 Nguyen Hue, Da Nang', 'dienhoa24h@gmail.com', 'www.dienhoa24h.com'),
('Hoa Hong Xanh', 'Pham Thi D', '0912345678', '78 Phan Chu Trinh, Hue', 'hoahongxanh@gmail.com', 'www.hoahongxanh.com'),
('Hoa tuoi Sai Gon', 'Bui Van E', '0934567890', '23 Ly Thuong Kiet, Can Tho', 'hoatuisg@gmail.com', 'www.hoatuisg.com');

-- Insert dữ liệu vào bảng Customers (Khách Hàng)
INSERT INTO Customers (CustomerName, Phone, Address, Email, DateOfBirth) VALUES
('Nguyen Van An', '0987123456', '10 Tran Phu, TP HCM', 'an.nguyen@gmail.com', '1990-05-20'),
('Tran Thi Binh', '0978654321', '22 Hoang Hoa Tham, Ha Noi', 'binh.tran@gmail.com', '1985-08-12'),
('Le Van Chau', '0963456789', '35 Le Loi, Da Nang', 'chau.le@gmail.com', '1992-10-25'),
('Pham Thi Dao', '0945678901', '56 Nguyen Trai, Hue', 'dao.pham@gmail.com', '1988-03-15'),
('Bui Van Em', '0936789012', '77 Ly Thuong Kiet, Can Tho', 'em.bui@gmail.com', '1995-07-08');

-- Insert dữ liệu vào bảng Employees (Nhân Viên)
INSERT INTO Employees (EmployeeName, Position, Authority, Email, HireDate) VALUES
('Nguyen Thanh H', 'Nhan vien ban hang', 'User', 'h.nguyen@gmail.com', '2021-06-01'),
('Tran Hoang K', 'Quan ly', 'Admin', 'k.tran@gmail.com', '2019-09-15'),
('Le Minh L', 'Nhan vien kho', 'User', 'l.le@gmail.com', '2020-12-10'),
('Pham Van M', 'Nhan vien giao hang', 'User', 'm.pham@gmail.com', '2022-03-05'),
('Bui Thi N', 'Nhan vien cham soc khach hang', 'User', 'n.bui@gmail.com', '2021-11-20');

-- Insert dữ liệu vào bảng Users (Người Dùng)
INSERT INTO Users (Username, Password, EmployeeID, CustomerID, Role) VALUES
('admin', 'qqq', 1, 1, 'Admin'),
('sales', 'qqq', 2, 2, 'Sales'),
('warehouse', 'qqq', 3, 3, 'Warehouse');

-- Insert dữ liệu vào bảng Products (Sản Phẩm)
INSERT INTO Products (ProductCode, ProductName, SellingPrice, InventoryQuantity, SupplierID, Image ) VALUES
('P001', 'Hoa Hong Do', 50000, 100, 1, (SELECT * FROM OPENROWSET(BULK N'D:\Deadline\Image\anh1.png', SINGLE_BLOB) AS Image)),
('P002', 'Hoa Cuc Vang', 30000, 150, 2, (SELECT * FROM OPENROWSET(BULK N'D:\Deadline\Image\anh2.png', SINGLE_BLOB) AS Image)),
('P003', 'Hoa Lan Tim', 70000, 80, 3, (SELECT * FROM OPENROWSET(BULK N'D:\Deadline\Image\anh3.png', SINGLE_BLOB) AS Image)),
('P004', 'Hoa Ly Trang', 60000, 90, 4, (SELECT * FROM OPENROWSET(BULK N'D:\Deadline\Image\anh4.png', SINGLE_BLOB) AS Image)),
('P005', 'Hoa Huong Duong', 40000, 120, 5, (SELECT * FROM OPENROWSET(BULK N'D:\Deadline\Image\anh5.png', SINGLE_BLOB) AS Image));

-- Insert dữ liệu vào bảng Invoices (Hóa Đơn)
INSERT INTO Invoices (CustomerID, EmployeeID, TotalAmount) VALUES
(1, 1, 150000),
(2, 2, 210000),
(3, 3, 140000);

-- Insert dữ liệu vào bảng InvoiceDetails (Chi Tiết Hóa Đơn)
INSERT INTO InvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice) VALUES
(1, 1, 2, 50000),
(1, 3, 1, 70000),
(2, 2, 3, 30000);

-- Insert dữ liệu vào bảng CustomerPurchaseHistory (Lịch Sử Mua Hàng của Khách Hàng)
INSERT INTO CustomerPurchaseHistory (CustomerID, InvoiceID, PurchaseDate) VALUES
(1, 1, '2024-03-10'),
(2, 2, '2024-03-12'),
(3, 3, '2024-03-15');

