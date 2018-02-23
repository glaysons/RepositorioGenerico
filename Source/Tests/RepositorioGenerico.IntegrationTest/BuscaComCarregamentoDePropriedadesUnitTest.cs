using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using RepositorioGenerico.Test.Objetos;
using System.Linq;

namespace RepositorioGenerico.IntegrationTest
{
	[TestClass]
	public class BuscaComCarregamentoDePropriedadesUnitTest
	{
		[TestMethod]
		public void SeConsultarVariosObjetosCarregandoPropriedadeFilhoDeveTrazerCorretamente()
		{
			var contexto = ContextoHelper.Criar();
			var buscador = contexto.Buscar<ObjetoDeTestes>();

			var config = buscador.CriarQuery()
				.CarregarPropriedade(o => o.Filhos)
				.AdicionarCondicao(o => o.Codigo).Entre(1, 2);

			var objetos = buscador.Varios(config).ToList();

			objetos.Count
				.Should()
				.Be(2);

			objetos[0].Filhos.Count
				.Should()
				.Be(3, "porque deve existir 3 filhos pro primeiro objeto!");

			objetos[1].Filhos.Count
				.Should()
				.Be(2, "porque deve existir 2 filhos pro segundo objeto!");

		}

		[TestMethod]
		public void SeConsultarVariosObjetosCarregandoPropriedadePaiDeveTrazerCorretamente()
		{
			var contexto = ContextoHelper.Criar();
			var buscador = contexto.Buscar<FilhoDoObjetoDeTestes>();

			var config = buscador.CriarQuery()
				.CarregarPropriedade(o => o.Netos)
				.CarregarPropriedade(o => o.Pai)
				.AdicionarCondicao(o => o.Id).Entre(4, 5);

			var objetos = buscador.Varios(config).ToList();

			objetos.Count
				.Should()
				.Be(2);

			objetos[0].Pai
				.Should()
				.NotBeNull("porque o pai de indice zero deve ser carregado!");

			objetos[0].Pai.Nome
				.Should()
				.Be("Teste B");

			objetos[1].Netos.Count
				.Should()
				.Be(1, "porque deve existir 1 filho pro segundo objeto!");
		}

		[TestMethod]
		public void SeConsultarOrdenandoECarregandoPropriedadesDeveCarregarPropriedadesCorretamente()
		{
			var contexto = ContextoHelper.Criar();
			var buscador = contexto.Buscar<ObjetoDeTestes>();

			var config = buscador.CriarQuery()
				.CarregarPropriedade(o => o.Filhos)
				.AdicionarCondicao(o => o.Codigo).Entre(1, 2)
				.AdicionarOrdem(o => o.Codigo);

			var objetos = buscador.Varios(config).ToList();

			objetos.Count
				.Should()
				.Be(2);

			objetos[0].Codigo
				.Should()
				.Be(1);

			objetos[0].Filhos.Count
				.Should()
				.Be(3, "porque deve existir 3 filhos pro primeiro objeto!");

			objetos[1].Codigo
				.Should()
				.Be(2);

			objetos[1].Filhos.Count
				.Should()
				.Be(2, "porque deve existir 2 filhos pro segundo objeto!");
		}


	}
}
