USE [master]

GO
/****** Object:  Database [MonShop]    Script Date: 8/4/2023 2:30:39 PM ******/
CREATE DATABASE [MonShop]

GO
ALTER DATABASE [MonShop] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MonShop] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MonShop] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MonShop] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MonShop] SET ARITHABORT OFF 
GO
ALTER DATABASE [MonShop] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MonShop] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MonShop] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MonShop] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MonShop] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MonShop] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MonShop] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MonShop] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MonShop] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MonShop] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MonShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MonShop] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MonShop] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MonShop] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MonShop] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MonShop] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MonShop] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MonShop] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MonShop] SET  MULTI_USER 
GO
ALTER DATABASE [MonShop] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MonShop] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MonShop] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MonShop] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MonShop] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MonShop] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MonShop] SET QUERY_STORE = OFF
GO
USE [MonShop]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[ImageUrl] [nvarchar](1000) NULL,
	[FullName] [nvarchar](255) NULL,
	[Address] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[IsDeleted] [bit] NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MomoPaymentResponses]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MomoPaymentResponses](
	[PaymentResponseId] [BIGINT] NOT NULL,
	[OrderId] [int] NOT NULL,
	[Amount] [nvarchar](50) NULL,
	[OrderInfo] [nvarchar](255) NULL,
	[Success] BIT NOT NULL
PRIMARY KEY CLUSTERED 
(
	[PaymentResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[PricePerUnit] [float] NOT NULL,
	[Subtotal] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int]  NOT NULL,
	[OrderDate] [DateTime] NULL,
	[Total] [float] NULL,
	[OrderStatusId] [int] NOT NULL,
	[BuyerAccountId] [int] NOT NULL,

PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatuses]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatuses](
	[OrderStatusId] [int] NOT NULL,
	[OrderStatus] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayPalPaymentResponses]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayPalPaymentResponses](
	[PaymentResponseId] nvarchar(255) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Amount] [nvarchar](50) NULL,
	[OrderInfo] [nvarchar](255) NULL,
	[Success] BIT NOT NULL

PRIMARY KEY CLUSTERED 
(
	[PaymentResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[ImageUrl] [nvarchar](1000) ,
	[Price] [float] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Description] [text] NULL,
	[CategoryId] [int] NULL,
	[ProductStatusId] [int] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VNPayPaymentResponses]    Script Date: 8/4/2023 2:30:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VNPayPaymentResponses](
	[PaymentResponseId] [BIGINT] NOT NULL,
	[OrderId] [int] NOT NULL,
	[Amount] [nvarchar](50) NULL,
	[OrderInfo] [nvarchar](255) NULL,
	[Success] BIT NOT NULL
PRIMARY KEY CLUSTERED 
(
	[PaymentResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([AccountId], [Email], [Password], [ImageUrl], [FullName], [Address], [PhoneNumber], [IsDeleted], [RoleId]) VALUES (1, N'mon@gmail.com', N'123456', NULL, N'Hong Quan', NULL, N'0999999999', 0, 1)
INSERT [dbo].[Account] ([AccountId], [Email], [Password], [ImageUrl], [FullName], [Address], [PhoneNumber], [IsDeleted], [RoleId]) VALUES (2, N'mon2@gmail.com', N'123456', NULL, N'Toi la mon', NULL, N'0909223123', 1, 1)
INSERT INTO [dbo].[Account] ([AccountId], [Email], [Password], [ImageUrl], [FullName], [Address], [PhoneNumber], [IsDeleted], [RoleId]) VALUES
(3, 'johndoe@gmail.com', '123456', NULL, 'John Doe', NULL, '0999999999', 0, 2),
(4, 'janedoe2@gmail.com', '123456', NULL, 'Jane Doe', NULL, '0999999999', 0, 2),
(5, 'admin@gmail.com', 'admin', NULL, 'Admin', NULL, NULL, 0, 1),
(6, 'user1@gmail.com', 'user1', NULL, 'User 1', NULL, NULL, 0, 2),
(7, 'user2@gmail.com', 'user2', NULL, 'User 2', NULL, NULL, 0, 2),
(8, 'user3@gmail.com', 'user3', NULL, 'User 3', NULL, NULL, 0, 2),
(9, 'user4@gmail.com', 'user4', NULL, 'User 4', NULL, NULL, 0, 2),
(10, 'user5@gmail.com', 'user5', NULL, 'User 5', NULL, NULL, 0, 2),
(11, 'user6@gmail.com', 'user6', NULL, 'User 6', NULL, NULL, 0, 2),
(12, 'user7@gmail.com', 'user7', NULL, 'User 7', NULL, NULL, 0, 2),
(13, 'user8@gmail.com', 'user8', NULL, 'User 8', NULL, NULL, 0, 2),
(14, 'user9@gmail.com', 'user9', NULL, 'User 9', NULL, NULL, 0, 2),
(15, 'user10@gmail.com', 'user10', NULL, 'User 10', NULL, NULL, 0, 2),
(16, 'user11@gmail.com', 'user11', NULL, 'User 11', NULL, NULL, 0, 2),
(17, 'user12@gmail.com', 'user12', NULL, 'User 12', NULL, NULL, 0, 2),
(18, 'user13@gmail.com', 'user13', NULL, 'User 13', NULL, NULL, 0, 2),
(19, 'user14@gmail.com', 'user14', NULL, 'User 14', NULL, NULL, 0, 2),
(20, 'user15@gmail.com', 'user15', NULL, 'User 15', NULL, NULL, 0, 2),
(21, 'user16@gmail.com', 'user16', NULL, 'User 16', NULL, NULL, 0, 2),
(22, 'user17@gmail.com', 'user17', NULL, 'User 17', NULL, NULL, 0, 2),
(23, 'user18@gmail.com', 'user18', NULL, 'User 18', NULL, NULL, 0, 2),
(24, 'user19@gmail.com', 'user19', NULL, 'User 19', NULL, NULL, 0, 2),
(25, 'user20@gmail.com', 'user20', NULL, 'User 20', NULL, NULL, 0, 2);
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT INTO [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES
(1, 'Pants'),
(2, 'Shirts'),
(3, 'Shoes'),
(4, 'Accessories');
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO

INSERT [dbo].[OrderStatuses] ([OrderStatusId], [OrderStatus]) VALUES (1, N'Pending')
INSERT [dbo].[OrderStatuses] ([OrderStatusId], [OrderStatus]) VALUES (2, N'Sucess Pay')
INSERT [dbo].[OrderStatuses] ([OrderStatusId], [OrderStatus]) VALUES (3, N'Fail Pay')
INSERT [dbo].[OrderStatuses] ([OrderStatusId], [OrderStatus]) VALUES (4, 'Shipped')
INSERT [dbo].[OrderStatuses] ([OrderStatusId], [OrderStatus]) VALUES (5, 'Delivered')
INSERT [dbo].[OrderStatuses] ([OrderStatusId], [OrderStatus]) VALUES (6, 'Cancelled')
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [ImageUrl], [Price], [Quantity], [Description], [CategoryId], [ProductStatusId], [IsDeleted]) VALUES
(1, 'T-Shirt Black', NULL, 100000, 10, 'T-shirt black with a white logo.', 1, 1, 0),
(2, 'T-Shirt White', NULL, 100000, 10, 'T-shirt white with a black logo.', 1, 1, 0),
(3, 'Pants Black', NULL, 200000, 10, 'Pants black with a white stripe.', 2, 1, 0),
(4, 'Pants White', NULL, 200000, 10, 'Pants white with a black stripe.', 2, 1, 0),
(5, 'Shoes Black', NULL, 300000, 10, 'Shoes black with a white logo.', 4, 1, 0),
(6, 'Shoes White', NULL, 300000, 10, 'Shoes white with a black logo.', 4, 1, 0),
(7, 'Bag Black', NULL, 400000, 10, 'Bag black with a white logo.', 4, 1, 0),
(8, 'Bag White', NULL, 400000, 10, 'Bag white with a black logo.', 4, 1, 0),
(9, 'Jeans Black', NULL, 250000, 10, 'Jeans black with a white logo.', 2, 1, 0),
(10, 'Jeans White', NULL, 250000, 10, 'Jeans white with a black logo.', 2, 1, 0),
(11, 'Dress Black', NULL, 300000, 10, 'Dress black with a white logo.', 3, 1, 0),
(12, 'Dress White', NULL, 300000, 10, 'Dress white with a black logo.', 3, 1, 0),
(13, 'Shoes Sneakers', NULL, 200000, 10, 'Shoes sneakers black with a white logo.', 4, 1, 0),
(14, 'Shoes Boots', NULL, 300000, 10, 'Shoes boots black with a white logo.', 4, 1, 0),
(15, 'Bag Backpack', NULL, 400000, 10, 'Bag backpack black with a white logo.', 4, 1, 0),
(16, 'Bag Handbag', NULL, 500000, 10, 'Bag handbag black with a white logo.', 4, 1, 0),
(17, 'Belt Black', NULL, 100000, 10,'Bag handbag black with a white logo.', 4, 1, 0)

SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, N'Staff')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (3, N'User')
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[MomoPaymentResponses]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([BuyerAccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([OrderStatusId])
REFERENCES [dbo].[OrderStatuses] ([OrderStatusId])
GO
ALTER TABLE [dbo].[PayPalPaymentResponses]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[VNPayPaymentResponses]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
USE [master]
GO
ALTER DATABASE [MonShop] SET  READ_WRITE 
GO
