USE [master]
GO

--drop database, if exists
DROP DATABASE [OZPXMLImport]
GO

--create database
CREATE DATABASE [OZPXMLImport]
GO




USE [OZPXMLImport]
GO

--***************************************************** [Pojistovna] ********************************************************
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pojistovna](
	[Id] [int] NOT NULL IDENTITY(1,1),
	[Nazev] [nvarchar](250) NOT NULL,
	[Zkratka] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--***************************************************** [PoskytovatelZdravotnichSluzeb] ********************************************************
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PoskytovatelZdravotnichSluzeb](
	[Id] [int] NOT NULL IDENTITY(1,1),  
	[Nazev] [nvarchar](250) NOT NULL,
	[TypPZSId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--***************************************************** [Smlouva] ********************************************************
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Smlouva](
	[Id] [int] NOT NULL IDENTITY(1,1),  
	[PoskytovatelZdravotnichSluzebId] [int] NOT NULL,
	[PojistovnaId] [int] NOT NULL,
	[DatumOd] [date] NOT NULL,
	[DatumDo] [date] NULL,
	[TypSmlouvyId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--***************************************************** [TypPZS] ********************************************************
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypPZS](
	[Id] [int] NOT NULL IDENTITY(1,1),  
	[Kod] [nvarchar](10) NOT NULL,
	[Nazev] [nvarchar](250) NOT NULL,
	[DatumOd] [date] NOT NULL,
	[DatumDo] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--***************************************************** [TypSmlouvy] ********************************************************
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypSmlouvy](
	[Id] [int] NOT NULL IDENTITY(1,1),  
	[Kod] [nvarchar](10) NOT NULL,
	[Nazev] [nvarchar](250) NOT NULL,
	[DatumOd] [date] NOT NULL,
	[DatumDo] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[PoskytovatelZdravotnichSluzeb]  WITH CHECK ADD  CONSTRAINT [FK_PoskytovatelZdravotnichSluzeb_TypPZS] FOREIGN KEY([TypPZSId])
REFERENCES [dbo].[TypPZS] ([Id])
GO
ALTER TABLE [dbo].[PoskytovatelZdravotnichSluzeb] CHECK CONSTRAINT [FK_PoskytovatelZdravotnichSluzeb_TypPZS]
GO
ALTER TABLE [dbo].[Smlouva]  WITH CHECK ADD  CONSTRAINT [FK_Smlouva_Pojistovna] FOREIGN KEY([PojistovnaId])
REFERENCES [dbo].[Pojistovna] ([Id])
GO
ALTER TABLE [dbo].[Smlouva] CHECK CONSTRAINT [FK_Smlouva_Pojistovna]
GO
ALTER TABLE [dbo].[Smlouva]  WITH CHECK ADD  CONSTRAINT [FK_Smlouva_PoskytovatelZdravotnichSluzeb] FOREIGN KEY([PoskytovatelZdravotnichSluzebId])
REFERENCES [dbo].[PoskytovatelZdravotnichSluzeb] ([Id])
GO
ALTER TABLE [dbo].[Smlouva] CHECK CONSTRAINT [FK_Smlouva_PoskytovatelZdravotnichSluzeb]
GO
ALTER TABLE [dbo].[Smlouva]  WITH CHECK ADD  CONSTRAINT [FK_Smlouva_TypSmlouvy] FOREIGN KEY([TypSmlouvyId])
REFERENCES [dbo].[TypSmlouvy] ([Id])
GO
ALTER TABLE [dbo].[Smlouva] CHECK CONSTRAINT [FK_Smlouva_TypSmlouvy]
GO



CREATE PROCEDURE GetListOfSmlouva 
@datum date = getdate

AS
BEGIN
	SET NOCOUNT ON;

	SELECT S.Id AS IdSmlouvy
	      ,PZS.Nazev AS NazevPZS
		  ,TPZS.Kod AS KodTypuPZS
		  ,P.Nazev AS PojistovnaNazev
		  ,P.Zkratka AS PojistovnaZkratka
		  ,T.Kod AS TypSmlouvyKod
		  ,T.Nazev AS TypSmlouvyNazev
		  ,S.DatumOd AS DatumOd
		  ,S.DatumDo AS DatumDo
	FROM Smlouva S
		LEFT JOIN PoskytovatelZdravotnichSluzeb PZS ON S.PoskytovatelZdravotnichSluzebId = PZS.Id
		LEFT JOIN Pojistovna P ON S.PojistovnaId = P.Id
		LEFT JOIN TypSmlouvy T ON S.TypSmlouvyId = T.Id
		LEFT JOIN TypPZS TPZS ON PZS.TypPZSId = TPZS.Id
	WHERE S.DatumOd <= @datum
	  AND (S.DatumDo >= @datum OR S.DatumDo IS NULL)
END
GO
