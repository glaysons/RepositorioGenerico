using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Fake.Contextos;
using RepositorioGenerico.Test.Objetos;
using System;

namespace RepositorioGenerico.Fake.Test.Contextos
{
	[TestClass]
	public class ContextoFakeUnitTest
	{

		[TestMethod]
		public void SeConsultarRepositorioDeFormasDiferentesDeveEncontrarMesmoRepositorio()
		{
			var contexto = new ContextoFake();

			var repositorioUm = contexto.Repositorio<ObjetoDeTestes>();

			var repositorioDois = contexto.Repositorio(typeof(ObjetoDeTestes));

			repositorioUm
				.Should()
				.BeSameAs(repositorioDois);
		}

		[TestMethod]
		public void SeAdicionarRegistrosNoContextoORepositorioNaoDeveSerModificadoMasDeveSerConsultado()
		{
			var contexto = new ContextoFake();

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should()
				.Be(0);

			contexto.AdicionarRegistro(new ObjetoDeTestes() { Codigo = 1, Nome = "Nome Valido Para Adicao" });

			repositorio.Quantidade
				.Should()
				.Be(0);

			var config = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Igual(1);

			var registro = repositorio.Buscar.Um(config);

			registro
				.Should()
				.NotBeNull();

			registro.Codigo
				.Should()
				.Be(1);

			registro.Nome
				.Should()
				.Be("Nome Valido Para Adicao");
		}

		[TestMethod]
		public void SeAdicionarUmaListaDeRegistrosNoContextoORepositorioCorretoDeveserModificado()
		{
			var contexto = new ContextoFake();

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should()
				.Be(0);

			var lista = CriarListaPadrao();

			contexto.AdicionarRegistros(lista);

			repositorio.Quantidade
				.Should()
				.Be(0);

			var indice = 0;
			foreach (var item in repositorio.Buscar.Todos())
			{
				ValidarItemDaListaPadrao(indice, item);
				indice++;
			}
		}

		private static IList<ObjetoDeTestes> CriarListaPadrao()
		{
			return new List<ObjetoDeTestes>
			{
				new ObjetoDeTestes() {Codigo = 1, Nome = "Nome Valido Para Adicao"},
				new ObjetoDeTestes() {Codigo = 2, Nome = "Novo Nome Valido Para Adicao"}
			};
		}

		private static void ValidarItemDaListaPadrao(int indice, ObjetoDeTestes item)
		{
			item.Codigo
				.Should().Be(indice + 1);
			if (indice == 0)
				item.Nome
					.Should().Be("Nome Valido Para Adicao");
			else
				item.Nome
					.Should().Be("Novo Nome Valido Para Adicao");
		}

		[TestMethod]
		public void SeDefinirResultadoProcedureSemProcedureDeveGerarErro()
		{
			var contexto = new ContextoFake();
			var lista = new List<ObjetoDeTestes>();

			Action nomeNulo = () => contexto.DefinirResultadoProcedure(null, lista);
			nomeNulo
				.ShouldThrow<ArgumentNullException>();

			Action nomeVazio = () => contexto.DefinirResultadoProcedure(string.Empty, lista);
			nomeVazio
				.ShouldThrow<ArgumentNullException>();
		}

		[TestMethod]
		public void SeDefinirResultadoProcedureOsMesmosDevemSerLocalizados()
		{
			var contexto = new ContextoFake();

			var lista = CriarListaPadrao();

			contexto.DefinirResultadoProcedure("proc", lista);

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var indice = 0;
			var config = repositorio.Buscar.CriarProcedure("proc");
			foreach (var item in repositorio.Buscar.Varios(config))
			{
				ValidarItemDaListaPadrao(indice, item);
				indice++;
			}
		}

		[TestMethod]
		public void SeDefinirResultadoProcedureVariasVezesApenasOUltimoDevemSerLocalizado()
		{
			var contexto = new ContextoFake();

			var lista = CriarListaPadrao();

			contexto.DefinirResultadoProcedure("proc", lista);
			contexto.DefinirResultadoProcedure("proc", lista);
			contexto.DefinirResultadoProcedure("proc", lista);
			contexto.DefinirResultadoProcedure("proc", lista);
			contexto.DefinirResultadoProcedure("proc", lista);
			contexto.DefinirResultadoProcedure("proc", lista);

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var indice = 0;
			var config = repositorio.Buscar.CriarProcedure("proc");
			foreach (var item in repositorio.Buscar.Varios(config))
			{
				ValidarItemDaListaPadrao(indice, item);
				indice++;
			}
		}


		[TestMethod]
		public void SeDefinirResultadoScalarProcedureOValorScalarDeveSerRetornado()
		{
			var contexto = new ContextoFake();

			double constante = 123.56;

			contexto.DefinirResultadoScalarProcedure("proc", constante);

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			var config = repositorio.Buscar.CriarProcedure("proc");
			var valor = repositorio.Buscar.Scalar(config);

			valor
				.Should()
				.BeOfType<double>()
				.And
				.Be(constante);
		}

		[TestMethod]
		public void SeDefinirResultadoNonQueryProcedureOValorNonQueryDeveSerRetornado()
		{
			var contexto = new ContextoFake();
			var procs = new[] { "procA", "procB", "procC" };
			var resultados = new[] { 11, 17, 23 };

			for (int n = 0; n < 3; n++)
				contexto.DefinirResultadoNonQueryProcedure(procs[n], resultados[n]);

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			for (int n = 0; n < 3; n++)
			{
				var config = repositorio.Buscar.CriarProcedure(procs[n]);
				var valor = repositorio.Buscar.NonQuery(config);

				valor
					.Should()
					.Be(resultados[n]);
			}
		}

		[TestMethod]
		public void SeDefinirResultadoScalarProcedureMultiplasVezesOValorScalarDeveSerRetornado()
		{
			var contexto = new ContextoFake();

			contexto.DefinirResultadoScalarProcedure("proc", 123);

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();
			var config = repositorio.Buscar.CriarProcedure("proc");

			var valor = repositorio.Buscar.Scalar(config);
			valor
				.Should()
				.Be(123);

			contexto.DefinirResultadoScalarProcedure("proc", 456);

			var novoValor = repositorio.Buscar.Scalar(config);
			novoValor
				.Should()
				.Be(456);
		}

		[TestMethod]
		public void SeDefinirResultadoNonQueryProcedureMultiplasVezesOValorScalarDeveSerRetornado()
		{
			var contexto = new ContextoFake();

			contexto.DefinirResultadoNonQueryProcedure("proc", 11);

			var repositorio = contexto.Repositorio<ObjetoDeTestes>();
			var config = repositorio.Buscar.CriarProcedure("proc");

			var valor = repositorio.Buscar.NonQuery(config);
			valor
				.Should()
				.Be(11);

			contexto.DefinirResultadoNonQueryProcedure("proc", 17);

			var novoValor = repositorio.Buscar.NonQuery(config);
			novoValor
				.Should()
				.Be(17);
		}

	}
}
