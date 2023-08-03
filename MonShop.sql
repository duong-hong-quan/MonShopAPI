CREATE DATABASE MonShop
USE MonShop

CREATE TABLE Roles (
    RoleId INT PRIMARY KEY,
    RoleName NVARCHAR(255) NOT NULL
);
CREATE TABLE Account (
    AccountId INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    ImageUrl NVARCHAR(1000) NULL,
    FullName NVARCHAR(255) NULL,
    Address NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    IsDeleted BIT NULL,
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(255) NOT NULL
);

CREATE TABLE Products (
    ProductId INT IDENTITY (1,1)  PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    ImageUrl NVARCHAR(1000) NOT NULL,
    Price FLOAT NOT NULL,
    Quantity INT NOT NULL,
    Description TEXT,
    CategoryId INT,
    ProductStatusId INT,
    IsDeleted BIT,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
CREATE TABLE OrderStatuses (
    OrderStatusId INT PRIMARY KEY,
    OrderStatus NVARCHAR(255) NOT NULL
);
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NULL,
    OrderDate DATE NULL,
    Total FLOAT NULL,
    OrderStatusId INT NOT NULL,
	BuyerAccountId INT NOT NULL,

    FOREIGN KEY (OrderStatusId) REFERENCES OrderStatuses(OrderStatusId),
    FOREIGN KEY (BuyerAccountId) REFERENCES Account(AccountId)

);

CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    PricePerUnit FLOAT NOT NULL,
    Subtotal FLOAT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);





CREATE TABLE PayPalPaymentResponses (
    PaymentResponseId INT PRIMARY KEY,
    OrderId INT NOT NULL,
    OrderDescription NVARCHAR(255) NULL,
    TransactionId NVARCHAR(50) NULL,
    PaymentId NVARCHAR(50) NULL,
    Success BIT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

CREATE TABLE VNPayPaymentResponses (
    PaymentResponseId INT PRIMARY KEY,
    OrderId INT NOT NULL,
    OrderDescription NVARCHAR(255) NULL,
    TransactionId NVARCHAR(50) NULL,
    PaymentId NVARCHAR(50) NULL,
    Success BIT NULL,
    Token NVARCHAR(255) NULL,
    VnPayResponseCode NVARCHAR(50) NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

CREATE TABLE MomoPaymentResponses (
    PaymentResponseId INT PRIMARY KEY,
    OrderId INT NOT NULL,
    Amount NVARCHAR(50) NULL,
    OrderInfo NVARCHAR(255) NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);



