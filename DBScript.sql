USE [master]
GO
/****** Object:  Database [PICK_TO_COLOR]    Script Date: 2022-12-04 6:10:27 PM ******/
CREATE DATABASE [PICK_TO_COLOR]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PICK_TO_COLOR', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PICK_TO_COLOR.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PICK_TO_COLOR_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PICK_TO_COLOR_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PICK_TO_COLOR] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PICK_TO_COLOR].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PICK_TO_COLOR] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET ARITHABORT OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PICK_TO_COLOR] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PICK_TO_COLOR] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PICK_TO_COLOR] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PICK_TO_COLOR] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PICK_TO_COLOR] SET  MULTI_USER 
GO
ALTER DATABASE [PICK_TO_COLOR] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PICK_TO_COLOR] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PICK_TO_COLOR] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PICK_TO_COLOR] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PICK_TO_COLOR] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PICK_TO_COLOR] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PICK_TO_COLOR] SET QUERY_STORE = OFF
GO
USE [PICK_TO_COLOR]
GO
/****** Object:  Table [dbo].[Boxes]    Script Date: 2022-12-04 6:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boxes](
	[BoxTypeID] [int] IDENTITY(1,1) NOT NULL,
	[BoxTypeCode] [nvarchar](20) NULL,
	[BoxTypeDescription] [nvarchar](50) NULL,
	[TotalCBM] [decimal](10, 2) NULL,
	[CustomerID] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Boxes] PRIMARY KEY CLUSTERED 
(
	[BoxTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerCode] [nvarchar](20) NULL,
	[CustomerName] [nvarchar](50) NULL,
	[Description] [nvarchar](200) NULL,
	[IsEnabledOneScanning] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[LocationMasterID] [int] IDENTITY(1,1) NOT NULL,
	[LocationID] [nvarchar](50) NULL,
	[LocationCode] [nvarchar](10) NULL,
	[LocationSoundFile] [nvarchar](260) NULL,
	[BackgroundColorCode] [varchar](20) NULL,
	[ColorSoundFile] [nvarchar](260) NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[StationID] [int] NULL,
	[PickingPriority] [int] NULL,
	[FontColorCode] [varchar](20) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[LocationMasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[CustomerID] [int] NULL,
	[OrderCD] [nvarchar](20) NULL,
	[SKU] [nvarchar](50) NULL,
	[Quantity] [decimal](10, 2) NULL,
	[OrderTypeID] [int] NULL,
	[CreatedDateTime] [datetime] NULL,
	[OrderCompletedStatus] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderTypes]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderTypes](
	[OrderTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OrderTypeName] [nvarchar](20) NULL,
 CONSTRAINT [PK_OrderTypes] PRIMARY KEY CLUSTERED 
(
	[OrderTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductPicking]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductPicking](
	[PickingID] [int] IDENTITY(1,1) NOT NULL,
	[OrderCD] [nvarchar](20) NULL,
	[OperatorUserName] [nvarchar](50) NULL,
	[PickingStartTime] [datetime] NULL,
	[PickingEndTime] [datetime] NULL,
	[OrderType] [int] NULL,
	[BoxTypeID] [int] NULL,
	[PickStatus] [int] NULL,
 CONSTRAINT [PK_ProductPicking] PRIMARY KEY CLUSTERED 
(
	[PickingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductPickingLineItems]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductPickingLineItems](
	[PickingID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[SerialNo] [nvarchar](20) NOT NULL,
	[ScannedOn] [datetime] NULL,
	[ProductPickingLineItemID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProductPickingLineItems] PRIMARY KEY CLUSTERED 
(
	[ProductPickingLineItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[Barcode] [nvarchar](20) NULL,
	[SKU] [nvarchar](20) NULL,
	[Description] [nvarchar](200) NULL,
	[AssociatedPickingQty] [decimal](10, 2) NULL,
	[CheckSN] [bit] NULL,
	[CBM] [decimal](10, 2) NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LocationMasterID] [int] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stations]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stations](
	[StationID] [int] IDENTITY(1,1) NOT NULL,
	[StationName] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Stations] PRIMARY KEY CLUSTERED 
(
	[StationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionAudit]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionAudit](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditType] [int] NULL,
	[AuditTypeID] [int] NULL,
	[UserID] [int] NULL,
	[TransactionDescription] [nvarchar](2000) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_TransactionAudit] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[Name] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[IsManager] [bit] NULL,
	[IsOperator] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[ImagePath] [nvarchar](260) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_OrderCompletedStatus]  DEFAULT ((0)) FOR [OrderCompletedStatus]
GO
/****** Object:  StoredProcedure [dbo].[spGetOrderDetails]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetOrderDetails]
@ORDER_CD NVARCHAR(20),
@STATION_ID INT
AS
BEGIN

DECLARE @OrderType AS NVARCHAR(10)
DECLARE @CustomerID AS INT
DECLARE @CustomerName AS NVARCHAR(50)
DECLARE @OneScanningEnabled AS BIT
DECLARE @BoxTypeID AS INT
DECLARE @BoxTypeName AS NVARCHAR(20)
DECLARE @TotalOrderCBM AS INT
DECLARE @TotalItems AS INT
DECLARE @AccessibleItems AS INT
DECLARE @AllItemsInCurrentStation BIT
DECLARE @PickingID AS INT
DECLARE @TotalQuantity AS INT

	 --Get the customer ID
	 SELECT TOP 1 @CustomerID = C.CustomerID, @CustomerName=C.CustomerCode, @OneScanningEnabled = IsEnabledOneScanning
	 FROM Orders O LEFT OUTER JOIN Customers C on O.CustomerID = C.CustomerID
	 WHERE C.IsActive = 1 AND O.OrderCD = @ORDER_CD
	 
	 SELECT @TotalItems = COUNT(OrderCd) FROM Orders WHERE OrderCD = @ORDER_CD
	 
	 -- Get the TotalOrderCBM
	 SELECT @TotalOrderCBM = SUM(P.CBM * O.Quantity/P.AssociatedPickingQty),@AccessibleItems = COUNT(P.SKU) 
	 FROM Orders O INNER JOIN Products P on O.CustomerID = P.CustomerID
	 AND O.SKU = P.SKU
	 WHERE P.IsActive = 1 AND O.OrderCD = @ORDER_CD
	
	 --Get the box capable of holding that CBM
     SELECT TOP 1 @BoxTypeID = BoxTypeID, @BoxTypeName = BoxTypeCode FROM 
     Boxes WHERE TotalCBM > @TotalOrderCBM AND CustomerID = @CustomerID
     ORDER BY TotalCBM
     
     SELECT @OrderType = CASE WHEN @TotalItems = @AccessibleItems THEN 'PTC'
							  WHEN @TotalItems > @AccessibleItems AND @AccessibleItems <> 0 THEN 'MIX'
							  WHEN @TotalItems > @AccessibleItems AND @AccessibleItems = 0 THEN 'WH'
							  END
	 
	 --SELECT @AllItemsInCurrentStation = CASE WHEN COUNT(L.StationID)= 0 THEN 1 ELSE 0 END -- If there are items that belong to other station
	 --FROM Orders O INNER JOIN Products P ON O.CustomerID = P.CustomerID AND O.SKU = P.SKU
	 --INNER JOIN Locations L ON P.LocationMasterID = L.LocationMasterID 
	 --WHERE P.IsActive = 1 AND L.IsActive = 1 AND O.OrderCD =  @ORDER_CD AND 
	 --L.StationID <> @STATION_ID
	 
	 SELECT @AllItemsInCurrentStation=1;

	 -- Get Picking ID if it is completed or started but not cancelled.
	 SELECT @PickingID = PickingID FROM ProductPicking WHERE OrderCD = @ORDER_CD AND PickStatus <> 3 

	 -- Get Totaol Qty
	 SELECT @TotalQuantity=SUM(Quantity) From Orders WHERE OrderCD = @ORDER_CD 
	 	 
	 SELECT @OrderType AS 'OrderType',@CustomerID AS 'CustomerID',@CustomerName AS 'CustomerName',@OneScanningEnabled as 'OneScanningEnabled',
	 @BoxTypeID 'BoxTypeID',@BoxTypeName 'BoxTypeName',@TotalOrderCBM 'TotalOrderCBM',
	 @AllItemsInCurrentStation 'AllItemsInCurrentStation', @ORDER_CD as 'OrderCD',
	 @PickingID 'PickingID',@TotalQuantity 'TotalQuantity'
	
END
GO
/****** Object:  StoredProcedure [dbo].[spGetOrderItemDetails]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Description:	Get the order details
-- =============================================
CREATE PROCEDURE [dbo].[spGetOrderItemDetails]
	@ORDER_CD NVARCHAR(20),
	@STATION_ID INT
AS
BEGIN
	
	SELECT P.ProductID,P.Barcode,P.SKU,P.[Description],P.AssociatedPickingQty,P.CheckSN,P.CBM,
	P.LocationMasterID,L.LocationID,L.LocationCode,L.LocationSoundFile, L.BackgroundColorCode,
	L.FontColorCode, L.ColorSoundFile, L.PickingPriority, O.Quantity AS 'OrderQty',
	(SELECT CASE WHEN COUNT(PickingID) = CAST ((O.Quantity / P.AssociatedPickingQty) AS INT) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
	FROM ProductPickingLineItems WHERE PickingID = PP.PickingID AND ProductID = P.ProductID AND PP.PickStatus <> 3) AS 'IsItemPicked'
	FROM Orders O 
	INNER JOIN Products P ON O.CustomerID = P.CustomerID AND O.SKU = P.SKU
	INNER JOIN Locations L ON L.LocationMasterID = P.LocationMasterID
	LEFT OUTER JOIN ProductPicking PP ON O.OrderCD = PP.OrderCD
	WHERE P.IsActive = 1 AND O.OrderCD = @ORDER_CD AND L.IsActive = 1 AND (PP.PickStatus IS NULL OR PP.PickStatus <> 3)
	AND L.StationID=@STATION_ID
	ORDER BY L.PickingPriority
END
GO
/****** Object:  StoredProcedure [dbo].[spGetWarehouseSKUs]    Script Date: 2022-12-04 6:10:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Description:	Gets the Warehouse SKUs present in an order
-- =============================================
CREATE PROCEDURE [dbo].[spGetWarehouseSKUs]
	@ORDER_CD NVARCHAR(20)
AS
BEGIN
	
	SELECT O.SKU FROM 
	Orders O LEFT OUTER JOIN Products P ON O.SKU = P.SKU AND O.CustomerID = P.CustomerID
	WHERE O.OrderCD = @ORDER_CD AND P.ProductID IS NULL
	
END
GO
USE [master]
GO
ALTER DATABASE [PICK_TO_COLOR] SET  READ_WRITE 
GO

USE [PICK_TO_COLOR]
GO

INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Password]
           ,[Name]
           ,[IsActive]
           ,[IsManager]
           ,[IsOperator]
           ,[CreatedOn]
           ,[ImagePath])
     VALUES
           ('admin','TLK7nC8kRC9qUs0dqlJVW9lbmY09bHOGWSYur975WL4Qj1A3','admin',1,1,1,'2022-11-26 00:16:31.893',null)
GO
