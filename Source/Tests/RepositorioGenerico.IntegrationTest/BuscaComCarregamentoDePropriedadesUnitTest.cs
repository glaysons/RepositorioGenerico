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
		public void SeConsultarVariosObjetosCarregandoPropriedadeDeveTrazerCorretamente()
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
	}
}
