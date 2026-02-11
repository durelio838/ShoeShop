CREATE DATABASE ShoeShopDB;
GO
USE ShoeShopDB;
GO

CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Login NVARCHAR(100) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID)
);

CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100) NOT NULL
);

CREATE TABLE Manufacturers (
    ManufacturerID INT PRIMARY KEY IDENTITY(1,1),
    ManufacturerName NVARCHAR(100) NOT NULL
);

CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Article NVARCHAR(20) NOT NULL,
    ProductName NVARCHAR(100) NOT NULL,
    Unit NVARCHAR(20) DEFAULT N'шт.',
    Price DECIMAL(10,2) NOT NULL,
    SupplierID INT FOREIGN KEY REFERENCES Suppliers(SupplierID),
    ManufacturerID INT FOREIGN KEY REFERENCES Manufacturers(ManufacturerID),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    Discount INT DEFAULT 0,
    StockQuantity INT DEFAULT 0,
    Description NVARCHAR(500),
    Photo NVARCHAR(100)
);

CREATE TABLE PickupPoints (
    PointID INT PRIMARY KEY IDENTITY(1,1),
    Address NVARCHAR(200) NOT NULL
);

CREATE TABLE OrderStatuses (
    StatusID INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) NOT NULL
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber INT NOT NULL,
    OrderDate DATE,
    DeliveryDate DATE,
    PickupPointID INT FOREIGN KEY REFERENCES PickupPoints(PointID),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    PickupCode NVARCHAR(10),
    StatusID INT FOREIGN KEY REFERENCES OrderStatuses(StatusID)
);

CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL
);

-- Данные
INSERT INTO Roles VALUES (N'Администратор'), (N'Менеджер'), (N'Авторизированный клиент');

INSERT INTO Users VALUES
(N'Никифорова Весения Николаевна', '94d5ous@gmail.com', 'uzWC67', 1),
(N'Сазонов Руслан Германович', 'uth4iz@mail.com', '2L6KZG', 1),
(N'Одинцов Серафим Артёмович', 'yzls62@outlook.com', 'JlFRCZ', 1),
(N'Степанов Михаил Артёмович', '1diph5e@tutanota.com', '8ntwUp', 2),
(N'Ворсин Петр Евгеньевич', 'tjde7c@yahoo.com', 'YOyhfR', 2),
(N'Старикова Елена Павловна', 'wpmrc3do@tutanota.com', 'RSbvHv', 2),
(N'Михайлюк Анна Вячеславовна', '5d4zbu@tutanota.com', 'rwVDh9', 3),
(N'Ситдикова Елена Анатольевна', 'ptec8ym@yahoo.com', 'LdNyos', 3),
(N'Ворсин Петр Евгеньевич', '1qz4kw@mail.com', 'gynQMT', 3),
(N'Старикова Елена Павловна', '4np6se@mail.com', 'AtnDjr', 3);

INSERT INTO Suppliers VALUES (N'Kari'), (N'Обувь для вас');

INSERT INTO Manufacturers VALUES (N'Kari'), (N'Marco Tozzi'), (N'Рос'), (N'Rieker'), (N'Alessio Nesca'), (N'CROSBY');

INSERT INTO Categories VALUES (N'Женская обувь'), (N'Мужская обувь');

INSERT INTO OrderStatuses VALUES (N'Новый'), (N'Завершен');

INSERT INTO PickupPoints VALUES 
(N'420151, г. Лесной, ул. Вишневая, 32'),
(N'125061, г. Лесной, ул. Подгорная, 8'),
(N'630370, г. Лесной, ул. Шоссейная, 24'),
(N'400562, г. Лесной, ул. Зеленая, 32'),
(N'614510, г. Лесной, ул. Маяковского, 47');

INSERT INTO Products VALUES
(N'A112T4', N'Ботинки', N'шт.', 4990, 1, 1, 1, 3, 6, N'Женские Ботинки демисезонные kari', '1.jpg'),
(N'F635R4', N'Ботинки', N'шт.', 3244, 2, 2, 1, 2, 13, N'Ботинки Marco Tozzi женские демисезонные', '2.jpg'),
(N'H782T5', N'Туфли', N'шт.', 4499, 1, 1, 2, 4, 5, N'Туфли kari мужские классика', '3.jpg'),
(N'G783F5', N'Ботинки', N'шт.', 5900, 1, 3, 2, 2, 8, N'Мужские ботинки Рос-Обувь кожаные', '4.jpg'),
(N'J384T6', N'Ботинки', N'шт.', 3800, 2, 4, 2, 2, 16, N'Полуботинки мужские Rieker', '5.jpg'),
(N'D572U8', N'Кроссовки', N'шт.', 4100, 2, 3, 2, 3, 6, N'Кроссовки мужские', '6.jpg'),
(N'F572H7', N'Туфли', N'шт.', 2700, 1, 2, 1, 2, 14, N'Туфли Marco Tozzi женские летние', '7.jpg'),
(N'D329H3', N'Полуботинки', N'шт.', 1890, 2, 5, 1, 4, 4, N'Полуботинки Alessio Nesca женские', '8.jpg'),
(N'B320R5', N'Туфли', N'шт.', 4300, 1, 4, 1, 2, 6, N'Туфли Rieker женские демисезонные', '9.jpg'),
(N'G432E4', N'Туфли', N'шт.', 2800, 1, 1, 1, 3, 15, N'Туфли kari женские', '10.jpg'),
(N'P764G4', N'Туфли', N'шт.', 6800, 1, 6, 1, 16, 15, N'Туфли женские ARGO', NULL),
(N'C436G5', N'Ботинки', N'шт.', 10200, 1, 5, 1, 17, 9, N'Ботинки женские ARGO', NULL),
(N'M542T5', N'Кроссовки', N'шт.', 2800, 2, 4, 2, 18, 3, N'Кроссовки мужские TOFA', NULL);

INSERT INTO Orders VALUES
(1, '2025-02-27', '2025-04-20', 1, 4, '901', 2),
(2, '2022-09-28', '2025-04-21', 2, 1, '902', 2),
(3, '2025-03-21', '2025-04-22', 3, 2, '903', 2),
(4, '2025-04-02', '2025-04-28', 4, 4, '909', 1),
(5, '2025-04-03', '2025-04-29', 5, 4, '910', 1);

INSERT INTO OrderItems VALUES
(1, 1, 2), (1, 2, 2),
(2, 3, 1), (2, 4, 1),
(3, 5, 10), (3, 6, 10),
(4, 7, 5), (4, 8, 4),
(5, 9, 5), (5, 10, 1);
GO