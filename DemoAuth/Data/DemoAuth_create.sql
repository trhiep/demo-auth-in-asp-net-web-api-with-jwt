-- Created by github.com/trhiep
-- Last modification date: 2024-10-22 10:37:39.961
USE [DemoAuth]
GO
-- tables
-- Table: Product
CREATE TABLE [Product] (
    [ProductID] int  NOT NULL IDENTITY(1, 1),
    [ProductName] nvarchar(255)  NOT NULL,
    [Price] decimal(10,2)  NOT NULL,
    CONSTRAINT Product_PK PRIMARY KEY  (ProductID)
);
GO
-- Table: UserInfo
CREATE TABLE [UserInfo] (
    [UserId] int  NOT NULL IDENTITY(1, 1),
    [UserName] varchar(100)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [DisplayName] nvarchar(255)  NOT NULL,
    [Role] varchar(50)  NOT NULL,
    CONSTRAINT UserInfo_PK PRIMARY KEY  (UserId)
);
GO
CREATE TABLE [UserToken] (
	[UserTokenId] int NOT NULL IDENTITY(1, 1),
    [UserId] int NOT NULL,
    [RefreshToken] varchar(100)  NOT NULL,
    [ExpiredAt] datetime  NOT NULL,
    CONSTRAINT UserToken_PK PRIMARY KEY  (UserTokenId),
	CONSTRAINT User_UserToken_FK FOREIGN KEY (UserId) REFERENCES "UserInfo" (UserId)
);
GO
-- Insert sample data into Product table
INSERT INTO [Product] ([ProductName], [Price]) 
VALUES 
    ('Laptop', 999.99),
    ('Smartphone', 699.99),
    ('Tablet', 299.99),
    ('Headphones', 49.99),
    ('Smartwatch', 199.99);
GO
-- Insert sample data into UserInfo table
INSERT INTO [UserInfo] ([UserName], [Password], [DisplayName], [Role])
VALUES 
    ('admin', 'admin', 'Admin User', 'ADMIN'),
    ('john_doe', 'john_doe', 'John Doe', 'NORMAL_USER'),
    ('jane_smith', 'jane_smith', 'Jane Smith', 'NORMAL_USER'),
    ('tech_admin', 'tech_admin', 'Tech Admin', 'ADMIN');

-- End of file.

