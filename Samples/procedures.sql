USE [RepositorioGenerico]
GO

if (object_id('spExistemClientesVinculados') is not null)
	drop procedure spExistemClientesVinculados

go

create procedure spExistemClientesVinculados
	@CodCidade int
as
	select	top 1 1
	from	Clientes
	where	(CodCidade = @CodCidade)

go

if (object_id('spExistemContatosVinculados') is not null)
	drop procedure spExistemContatosVinculados

go

create procedure spExistemContatosVinculados
	@CodTipoContato int
as
	select	top 1 1
	from	ContatosDosClientes
	where	(CodTipoContato = @CodTipoContato)
	union
	select	top 1 1
	from	ContatosDosFilhos
	where	(CodTipoContato = @CodTipoContato)
