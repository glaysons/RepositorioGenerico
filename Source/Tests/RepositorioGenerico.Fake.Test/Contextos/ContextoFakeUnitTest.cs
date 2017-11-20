using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Fake.Contextos;
using RepositorioGenerico.Test.Objetos;
using System;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Entities;

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
		public void SeCarregarRegistrosOContextoDeveConterRegistrosParaBuscaApenas()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should().Be(0);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(5);
		}

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
		public void SeConsultarPorCodigoDoContextoUmRegistroDoPoolDeveEncontrar()
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
		public void SeConsultarPorVariosRegistrosNoContextoOsMesmosDevemSerEncontrados()
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
		public void SeAdicionarUmaListaDeRegistrosNoContextoORepositorioNaoDeveSerModificadoEOsItensDevemSerEncontrados()
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

			indice
				.Should()
				.Be(2);
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

		[TestMethod]
		public void SeAdicionarUmRegistroNoContextoESalvarOMesmoDeveSerEncontrado()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should().Be(0);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(5);

			var objeto = new ObjetoDeTestes()
			{
				Codigo = 100,
				Nome = "Nome Que Pode Ser Inserido",
				Duplo = 123.56,
				Decimal = 234.67M,
				Logico = true,
				DataHora = DateTime.Now
			};

			repositorio.Inserir(objeto);

			objeto.EstadoEntidade
				.Should()
				.Be(EstadosEntidade.Novo);

			repositorio.Quantidade
				.Should()
				.Be(1);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(5);

			contexto.Salvar();

			objeto.EstadoEntidade
				.Should()
				.Be(EstadosEntidade.NaoModificado);

			objeto.Codigo
				.Should()
				.Be(6);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(6);

		}

		[TestMethod]
		public void SeAtualizarUmRegistroNoContextoOMesmoDeveSerAtualizado()
		{
			var contexto = CriarContextoParaTestes();
			var repositorio = contexto.Repositorio<ObjetoDeTestes>();

			repositorio.Quantidade
				.Should().Be(0);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(5);

			var configAtual = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Igual(3);

			var registroAtual = repositorio.Buscar.Um(configAtual);

			registroAtual
				.Should()
				.NotBeNull();

			registroAtual.Nome
				.Should()
				.Be("Teste de cadastros do nome C");

			registroAtual.Duplo
				.Should()
				.Be(0);

			registroAtual.Decimal
				.Should()
				.Be(0);

			registroAtual.Logico
				.Should()
				.BeFalse();


			var objetoEncontrado = repositorio.Consultar(3);
			objetoEncontrado.Nome = "Nome Que Pode Ser Atualizado";
			objetoEncontrado.Duplo = 123.56;
			objetoEncontrado.Decimal = 234.67M;
			objetoEncontrado.Logico = true;

			repositorio.Atualizar(objetoEncontrado);

			objetoEncontrado.EstadoEntidade
				.Should()
				.Be(EstadosEntidade.Modificado);

			repositorio.Quantidade
				.Should()
				.Be(1);

			repositorio.Buscar.Todos()
				.Should()
				.HaveCount(5);

			contexto.Salvar();

			objetoEncontrado.EstadoEntidade
				.Should()
				.Be(EstadosEntidade.NaoModificado);

			objetoEncontrado.Codigo
				.Should()
				.Be(3);

			var config = repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Igual(3);

			var registroAlterado = repositorio.Buscar.Um(config);

			registroAlterado
				.Should()
				.NotBeNull();

			registroAlterado.Nome
				.Should()
				.Be("Nome Que Pode Ser Atualizado");

			registroAlterado.Duplo
				.Should()
				.Be(123.56);

			registroAlterado.Decimal
				.Should()
				.Be(234.67M);

			registroAlterado.Logico
				.Should()
				.BeTrue();
		}

	}
}
