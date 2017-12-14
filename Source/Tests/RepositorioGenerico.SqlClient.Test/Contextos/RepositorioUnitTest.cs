using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.SqlClient.Contextos;
using RepositorioGenerico.Test.Objetos;
using FluentAssertions;
using System.Linq;

namespace RepositorioGenerico.SqlClient.Test.Contextos
{
	[TestClass]
	public class RepositorioUnitTest
	{
		[TestMethod]
		public void SeConsultarEstruturaDeObjetosDevePreencherPaiFilhosENetos()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var config = repositorio.Buscar.CriarQuery();

			config
				.AdicionarCondicao(o => o.Codigo).Igual(1)
				.CarregarPropriedade(o => o.Filhos);
				//.CarregarPropriedade(o => o.Filhos.Select(f => f.Netos));

			var objeto = repositorio.Buscar.Um(config);

			objeto
				.Should()
				.NotBeNull();

			objeto.Nome
				.Should()
				.Be("Teste A");

			objeto.Filhos
				.Should()
				.NotBeNull()
				.And
				.HaveCount(3);

			var filhos = objeto.Filhos.ToList();

			filhos[0].Nome
				.Should()
				.Be("Filho 1A");

			filhos[1].Nome
				.Should()
				.Be("Filho 2A");

			filhos[2].Nome
				.Should()
				.Be("Filho 3A");

			//filhos[0].Netos
			//	.Should()
			//	.NotBeNull()
			//	.And
			//	.HaveCount(2);

			//var netosFilho1 = filhos[0].Netos.ToList();

			//netosFilho1[0].NomeNeto
			//	.Should()
			//	.Be("1o Neto Filho 1A");

			//netosFilho1[0].NomeNeto
			//	.Should()
			//	.Be("2o Neto Filho 1A");

			//filhos[1].Netos
			//	.Should()
			//	.NotBeNull()
			//	.And
			//	.HaveCount(1);

			//var netosFilho2 = filhos[1].Netos.ToList();

			//netosFilho1[0].NomeNeto
			//	.Should()
			//	.Be("1o Neto Filho 2A");

			//filhos[3].Netos
			//	.Should()
			//	.NotBeNull()
			//	.And
			//	.HaveCount(0);
		}
	}
}
