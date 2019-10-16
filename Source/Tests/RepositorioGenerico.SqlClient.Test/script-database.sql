USE [master]
GO

/****** Object:  Database [RepositorioGenerico]    Script Date: 26/10/2017 18:19:47 ******/
CREATE DATABASE [RepositorioGenerico]

GO

USE [RepositorioGenerico]
GO


/****** Object:  Table [dbo].[Atividades]    Script Date: 17/04/2018 07:59:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Atividades](
	[Codigo] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[CargaHoraria] [int] NOT NULL,
 CONSTRAINT [PK_Atividades] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
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
	[CodigoFilho] [int] NOT NULL,
	[CampoComOpcoesInteiras] [int] NULL,
	[CampoComOpcoesString] [varchar](1) NULL
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


/****** Object:  Table [dbo].[AtividadesPorObjeto]    Script Date: 17/04/2018 07:59:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AtividadesPorObjeto](
	[CodigoObjeto] [int] NOT NULL,
	[CodigoAtividade] [int] NOT NULL,
 CONSTRAINT [PK_AtividadesPorObjeto] PRIMARY KEY CLUSTERED 
(
	[CodigoObjeto] ASC,
	[CodigoAtividade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AtividadesPorObjeto]  WITH CHECK ADD  CONSTRAINT [FK_AtividadesPorObjeto_Atividades] FOREIGN KEY([CodigoAtividade])
REFERENCES [dbo].[Atividades] ([Codigo])
GO

ALTER TABLE [dbo].[AtividadesPorObjeto] CHECK CONSTRAINT [FK_AtividadesPorObjeto_Atividades]
GO

ALTER TABLE [dbo].[AtividadesPorObjeto]  WITH CHECK ADD  CONSTRAINT [FK_AtividadesPorObjeto_ObjetoVirtual] FOREIGN KEY([CodigoObjeto])
REFERENCES [dbo].[ObjetoVirtual] ([Codigo])
GO

ALTER TABLE [dbo].[AtividadesPorObjeto] CHECK CONSTRAINT [FK_AtividadesPorObjeto_ObjetoVirtual]
GO

/**************     DADOS       *************/


insert into [dbo].[ObjetoVirtual] ([Nome], [Duplo], [Decimal], [Logico], [DataHora])
values ('Teste A', 123.45, 234.56, 1, '20171001 11:15')

GO

insert into [dbo].[ObjetoVirtual] ([Nome], [Duplo], [Decimal], [Logico], [DataHora])
values ('Teste B', 234.56, 345.67, 1, '20171001 17:45')

GO

insert into [dbo].[ObjetoVirtualFilho] ([NomeFilho], [CodigoPai])
values ('Filho 1A', 1)

GO

insert into [dbo].[NetoDoObjetoDeTestes] ([NomeNeto], [CodigoFilho])
values ('1o Neto Filho 1A', 1)

GO

insert into [dbo].[NetoDoObjetoDeTestes] ([NomeNeto], [CodigoFilho])
values ('2o Neto Filho 1A', 1)

GO

insert into [dbo].[ObjetoVirtualFilho] ([NomeFilho], [CodigoPai])
values ('Filho 2A', 1)

GO

insert into [dbo].[NetoDoObjetoDeTestes] ([NomeNeto], [CodigoFilho])
values ('1o Neto Filho 2A', 2)

GO

insert into [dbo].[ObjetoVirtualFilho] ([NomeFilho], [CodigoPai])
values ('Filho 3A', 1)

GO

insert into [dbo].[ObjetoVirtualFilho] ([NomeFilho], [CodigoPai])
values ('Filho 1B', 2)

GO
insert into [dbo].[ObjetoVirtualFilho] ([NomeFilho], [CodigoPai])
values ('Filho 2B', 2)

GO

insert into [dbo].[NetoDoObjetoDeTestes] ([NomeNeto], [CodigoFilho])
values ('1o Neto Filho 2B', 5)

GO


insert into [dbo].[Atividades] ([Nome], [CargaHoraria])
values ('Caminhar', 1)

GO

insert into [dbo].[Atividades] ([Nome], [CargaHoraria])
values ('Correr', 2)

GO


insert into [dbo].[Atividades] ([Nome], [CargaHoraria])
values ('Andar de bicicleta', 3)

GO


insert into [dbo].[Atividades] ([Nome], [CargaHoraria])
values ('Programar', 8)

GO

insert into [dbo].[AtividadesPorObjeto] ([CodigoObjeto], [CodigoAtividade])
values (1, 1)

GO

insert into [dbo].[AtividadesPorObjeto] ([CodigoObjeto], [CodigoAtividade])
values (1, 2)

GO

insert into [dbo].[AtividadesPorObjeto] ([CodigoObjeto], [CodigoAtividade])
values (1, 4)

GO

insert into [dbo].[AtividadesPorObjeto] ([CodigoObjeto], [CodigoAtividade])
values (2, 3)


/**************     PROCEDURES       *************/




create procedure spConsultarObjetoDeTestes
	@id int
as
	select	*
	from	[ObjetoVirtual]
	where	(Codigo = @id)
