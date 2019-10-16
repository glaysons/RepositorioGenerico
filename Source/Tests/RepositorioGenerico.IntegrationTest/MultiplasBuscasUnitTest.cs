using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Test.Objetos;
using FluentAssertions;

namespace RepositorioGenerico.IntegrationTest
{
	[TestClass]
	public class MultiplasBuscasUnitTest
	{

		[TestMethod]
		public void SeRealizarConsulta1000VezesUtilizandoUmDeveTrazerResultadoTodasAsVezes()
		{
			var contexto = ContextoHelper.Criar();
			var buscador = contexto.Buscar<ObjetoDeTestes>();

			for (int n = 0; n < 1000; n++)
			{
				var config = buscador.CriarQuery()
					.AdicionarCondicao(s => s.Nome).Igual("Teste A");
				var objeto = buscador.Um(config);
				objeto
					.Should()
					.NotBeNull();

				objeto.Codigo
					.Should()
					.Be(1);
			}
		}

		[TestMethod]
		public void SeRealizarConsulta1000VezesUtilizandoVarioDeveTrazerResultadoTodasAsVezes()
		{
			var contexto = ContextoHelper.Criar();
			var buscador = contexto.Buscar<ObjetoDeTestes>();

			for (int n = 0; n < 1000; n++)
			{
				var config = buscador.CriarQuery()
					.DefinirLimite(1)
					.AdicionarCondicao(s => s.Nome).Igual("Teste A");
				ObjetoDeTestes objeto = null;
				foreach (var item in buscador.Varios(config))
					objeto = item;

				objeto
					.Should()
					.NotBeNull();

				objeto.Codigo
					.Should()
					.Be(1);
			}
		}

		[TestMethod]
		public void SeRealizarConsulta1000VezesUtilizandoConsultarDeveTrazerResultadoTodasAsVezes()
		{
			var contexto = ContextoHelper.Criar();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			for (int n = 0; n < 1000; n++)
			{
				ObjetoDeTestes objeto = repositorio.Consultar(1);

				objeto
					.Should()
					.NotBeNull();

				objeto.Codigo
					.Should()
					.Be(1);
			}
		}

		[TestMethod]
		public void SeAcessarRepositorioDezMilhoesDeVezesDeveRetornarRepositorioValido()
		{
			var contexto = ContextoHelper.Criar();

			contexto.Repositorio<NetoDoObjetoDeTestes>();
			contexto.Repositorio<FilhoDoObjetoDeTestes>();
			contexto.Repositorio<FilhoMistoDoObjetoDeTestes>();
			var primeiraInstancia = contexto.Repositorio<ObjetoDeTestes>();

			for (int i = 0; i < 10_000_000; i++)
			{
				var repositorio = contexto.Repositorio<ObjetoDeTestes>();
				repositorio
					.Should()
					.Be(primeiraInstancia);
			}
		}

	}
}
