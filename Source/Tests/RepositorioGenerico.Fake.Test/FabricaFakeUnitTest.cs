using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Fake.Test
{
	[TestClass]
	public class FabricaFakeUnitTest
	{

		private IContexto CriarContextoParaTestes()
		{
			var contexto = FabricaFake.CriarContexto();

			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 1, Nome = "Teste de cadastros do nome A" });
			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 2, Nome = "Teste de cadastros do nome B" });
			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 3, Nome = "Teste de cadastros do nome C" });
			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 4, Nome = "Teste de cadastros do nome D" });
			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 5, Nome = "Teste de cadastros do nome E" });

			return contexto;
		}

		[TestMethod]
		public void SeCriarUmContextoFakeNaoDeveGerarErro()
		{
			Action act = () => FabricaFake.CriarContexto();
			act.ShouldNotThrow();
		}

		[TestMethod]
		public void SeCriarUmContextoLegadoFakeNaoDeveGerarErro()
		{
			Action act = () => FabricaFake.CriarContextoLegado();
			act.ShouldNotThrow();
		}

		[TestMethod]
		public void SeCarregarContextoDeveConterRegistros()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();
			repositorio.Quantidade
				.Should().Be(5);
		}

		[TestMethod]
		public void SeConsultarPorCodigoUmRegistroDoPoolDeveEncontrar()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var query = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Igual(5);

			var registro = repositorio.Buscar.Um(query);

			registro.Should().NotBeNull();
			registro.Nome.Should().Be("Teste de cadastros do nome E");
		}

		[TestMethod]
		public void SeConsultarPorVariosRegistrosDoPoolDeveEncontrar()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();
			
			var query = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Seja(Operadores.MaiorOuIgual, 3)
				.AdicionarCondicao(c => c.Codigo).Seja(Operadores.MenorOuIgual, 5);

			var n = 3;

			foreach (var registro in repositorio.Buscar.Varios(query))
			{
				registro.Should().NotBeNull();
				registro.Codigo.Should().Be(n);
				registro.Nome.Should().Be("Teste de cadastros do nome " + char.ConvertFromUtf32(64 + n));
				n++;
			}

			n.Should().Be(6);
		}

		[TestMethod]
		public void SeConsultarPorNomeContendoDeveEncontrar()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var query = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Nome).Contenha("nome C");

			var registro = repositorio.Buscar.Um(query);

			registro.Should().NotBeNull();
			registro.Nome.Should().Be("Teste de cadastros do nome C");
		}
	
		[TestMethod]
		public void SeConsultarTodosDeveEncontrar()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var n = 0;
			foreach (var registro in repositorio.Buscar.Todos())
			{
				registro.Should().NotBeNull();
				registro.Codigo.Should().Be(n + 1);
				registro.Nome.Should().Be("Teste de cadastros do nome " + char.ConvertFromUtf32(65 + n));
				n++;
			}

			n.Should().Be(5);
		}

	}
}
