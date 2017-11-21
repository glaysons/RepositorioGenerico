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
