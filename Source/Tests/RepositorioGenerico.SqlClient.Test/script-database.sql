USE [master]
GO

/****** Object:  Database [RepositorioGenerico]    Script Date: 26/10/2017 18:19:47 ******/
CREATE DATABASE [RepositorioGenerico]

GO

USE [RepositorioGenerico]
GO

/****** Object:  Table [dbo].[NetoDoObjetoDeTestes]    Script Date: 26/10/2017 18:22:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[NetoDoObjetoDeTestes](
	[CodigoNeto] [int] IDENTITY(1,1) NOT NULL,
	[NomeNeto] [varchar](50) NOT NULL,
	[CodigoFilho] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ObjetoVirtual]    Script Date: 26/10/2017 18:22:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ObjetoVirtual](
	[Codigo] [int] IDENTITY(1,1) NOT NULL,
	[CodigoNulo] [int] NULL,
	[Nome] [varchar](50) NOT NULL,
	[Duplo] [float] NOT NULL,
	[DuploNulo] [float] NULL,
	[Decimal] [decimal](18, 2) NOT NULL,
	[DecimalNulo] [decimal](18, 2) NULL,
	[Logico] [bit] NOT NULL,
	[DataHora] [datetime] NOT NULL,
	[DataHoraNulo] [datetime] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ObjetoVirtualFilho]    Script Date: 26/10/2017 18:22:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ObjetoVirtualFilho](
	[CodigoFilho] [int] IDENTITY(1,1) NOT NULL,
	[NomeFilho] [varchar](50) NOT NULL,
	[CodigoPai] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

insert into [dbo].[ObjetoVirtual] ([Nome], [Duplo], [Decimal], [Logico], [DataHora])
values ('Teste A', 123.45, 234.56, 1, '20171001 11:15')

GO

insert into [dbo].[ObjetoVirtual] ([Nome], [Duplo], [Decimal], [Logico], [DataHora])
values ('Teste B', 234.56, 345.67, 1, '20171001 17:45')

