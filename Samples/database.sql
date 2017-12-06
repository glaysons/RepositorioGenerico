USE [RepositorioGenerico]
GO

/****** Object:  Table [dbo].[Cidades]    Script Date: 17/11/2017 20:13:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Cidades](
	[CodCidade] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Estado] [varchar](2) NOT NULL,
 CONSTRAINT [PK_Cidades] PRIMARY KEY CLUSTERED 
(
	[CodCidade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Clientes]    Script Date: 17/11/2017 20:13:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Clientes](
	[CodCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Idade] [int] NOT NULL,
	[Endereco] [varchar](250) NULL,
	[CreditoDisponivel] [decimal](18, 2) NULL,
	[Bairro] [varchar](250) NULL,
	[CodCidade] [int] NULL,
	[RetemImpostos] [bit] NOT NULL,
	[Vip] [bit] NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[CodCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ContatosDosClientes]    Script Date: 17/11/2017 20:13:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ContatosDosClientes](
	[CodContatoCliente] [int] IDENTITY(1,1) NOT NULL,
	[CodCliente] [int] NOT NULL,
	[CodTipoContato] [int] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Telefone] [varchar](50) NULL,
	[Email] [varchar](250) NULL,
 CONSTRAINT [PK_ContatosDosClientes] PRIMARY KEY CLUSTERED 
(
	[CodContatoCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ContatosDosFilhos]    Script Date: 17/11/2017 20:13:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ContatosDosFilhos](
	[CodContatoFilho] [int] IDENTITY(1,1) NOT NULL,
	[CodFilho] [int] NOT NULL,
	[CodTipoContato] [int] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Telefone] [varchar](50) NULL,
	[Email] [varchar](250) NULL,
 CONSTRAINT [PK_ContatosFilhos] PRIMARY KEY CLUSTERED 
(
	[CodContatoFilho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Filhos]    Script Date: 17/11/2017 20:13:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Filhos](
	[CodFilho] [int] IDENTITY(1,1) NOT NULL,
	[CodCliente] [int] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[MoraComOsPais] [bit] NOT NULL,
	[Idade] [int] NOT NULL,
	[DataDeNascimento] [datetime] NULL,
 CONSTRAINT [PK_Filhos] PRIMARY KEY CLUSTERED 
(
	[CodFilho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[TiposDosContatos]    Script Date: 17/11/2017 20:13:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TiposDosContatos](
	[CodTipoContato] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TiposContatos] PRIMARY KEY CLUSTERED 
(
	[CodTipoContato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Cidades] FOREIGN KEY([CodCidade])
REFERENCES [dbo].[Cidades] ([CodCidade])
GO

ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Cidades]
GO

ALTER TABLE [dbo].[ContatosDosClientes]  WITH CHECK ADD  CONSTRAINT [FK_ContatosDosClientes_Clientes] FOREIGN KEY([CodCliente])
REFERENCES [dbo].[Clientes] ([CodCliente])
GO

ALTER TABLE [dbo].[ContatosDosClientes] CHECK CONSTRAINT [FK_ContatosDosClientes_Clientes]
GO

ALTER TABLE [dbo].[ContatosDosClientes]  WITH CHECK ADD  CONSTRAINT [FK_ContatosDosClientes_TiposDosContatos] FOREIGN KEY([CodTipoContato])
REFERENCES [dbo].[TiposDosContatos] ([CodTipoContato])
GO

ALTER TABLE [dbo].[ContatosDosClientes] CHECK CONSTRAINT [FK_ContatosDosClientes_TiposDosContatos]
GO

ALTER TABLE [dbo].[ContatosDosFilhos]  WITH CHECK ADD  CONSTRAINT [FK_ContatosDosFilhos_TiposDosContatos] FOREIGN KEY([CodTipoContato])
REFERENCES [dbo].[TiposDosContatos] ([CodTipoContato])
GO

ALTER TABLE [dbo].[ContatosDosFilhos] CHECK CONSTRAINT [FK_ContatosDosFilhos_TiposDosContatos]
GO

ALTER TABLE [dbo].[ContatosDosFilhos]  WITH CHECK ADD  CONSTRAINT [FK_ContatosFilhos_Filhos] FOREIGN KEY([CodFilho])
REFERENCES [dbo].[Filhos] ([CodFilho])
GO

ALTER TABLE [dbo].[ContatosDosFilhos] CHECK CONSTRAINT [FK_ContatosFilhos_Filhos]
GO

ALTER TABLE [dbo].[Filhos]  WITH CHECK ADD  CONSTRAINT [FK_Filhos_Clientes] FOREIGN KEY([CodCliente])
REFERENCES [dbo].[Clientes] ([CodCliente])
GO

ALTER TABLE [dbo].[Filhos] CHECK CONSTRAINT [FK_Filhos_Clientes]
GO



/*

drop table [ContatosDosFilhos]
drop table [ContatosDosClientes]
drop table [TiposDosContatos]
drop table [Filhos]
drop table [Clientes]
drop table [Cidades]

*/