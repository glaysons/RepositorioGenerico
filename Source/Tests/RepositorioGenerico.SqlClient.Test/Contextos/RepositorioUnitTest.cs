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
			//config.CarregarSubPropriedade(o => o.Filhos.Select(f => f.Netos.Select(n => n.Filho)));

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

		[TestMethod]
		public void SeConsultarEstruturaDeFilhosDevePreencherPai()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var repositorio = contexto.Repositorio<FilhoDoObjetoDeTestes>();

			var config = repositorio.Buscar.CriarQuery();

			config
				.AdicionarCondicao(o => o.IdPai).Entre(1, 2)
				.CarregarPropriedade(o => o.Pai);

			var filhos = repositorio.Buscar.Varios(config).ToList();

			filhos
				.Should()
				.NotBeNull()
				.And
				.HaveCount(5);

			ValidarFilho(filhos[0], "Filho 1A", "Teste A");
			ValidarFilho(filhos[1], "Filho 2A", "Teste A");
			ValidarFilho(filhos[2], "Filho 3A", "Teste A");

			ValidarFilho(filhos[3], "Filho 1B", "Teste B");
			ValidarFilho(filhos[4], "Filho 2B", "Teste B");
		}

		private static void ValidarFilho(FilhoDoObjetoDeTestes filho, string nome, string nomePai)
		{
			filho.Nome
				.Should()
				.Be(nome);

			filho.Pai
				.Should()
				.NotBeNull();

			filho.Pai.Nome
				.Should()
				.Be(nomePai);
		}

		[TestMethod]
		public void SeConsultarViaQueryUmObjetoSemDicionarioDeveSerPossivelCarregarDados()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoSemDicionario>();
			var config = buscar.CriarQuery()
				.DefinirTabela("ObjetoVirtual")
				.AdicionarCondicao(c => c.Codigo).Igual(1);

			var objeto = buscar.Um(config);

			objeto
				.Should()
				.NotBeNull();

			objeto.Nome
				.Should()
				.Be("Teste A");

			objeto.Duplo
				.Should()
				.Be(123.45);
		}

		[TestMethod]
		public void SeConsultarViaProcedureUmObjetoSemDicionarioDeveSerPossivelCarregarDados()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoSemDicionario>();
			var config = buscar.CriarProcedure("spConsultarObjetoDeTestes");
			config.DefinirParametro("id").Valor(1);

			var objeto = buscar.Um(config);

			objeto
				.Should()
				.NotBeNull();

			objeto.Nome
				.Should()
				.Be("Teste A");

			objeto.Duplo
				.Should()
				.Be(123.45);
		}

		[TestMethod]
		public void SeConsultarViaQueryUmObjetoInexistenteDeveRetornarNulo()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoDeTestes>();
			var config = buscar.CriarQuery()
				.AdicionarCondicao(c => c.Codigo).Igual(-999);

			var objeto = buscar.Um(config);

			objeto
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void SeConsultarObjetoSemDicionarioViaQueryUmObjetoInexistenteDeveRetornarNulo()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoSemDicionario>();
			var config = buscar.CriarQuery()
				.DefinirTabela("ObjetoVirtual")
				.AdicionarCondicao(c => c.Codigo).Igual(-999);

			var objeto = buscar.Um(config);

			objeto
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void SeConsultarViaProcedureUmObjetoInexistenteDeveRetornarNulo()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoDeTestes>();
			var config = buscar.CriarProcedure("spConsultarObjetoDeTestes");
			config.DefinirParametro("id").Valor(-999);

			var objeto = buscar.Um(config);

			objeto
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void SeConsultarObjetoSemDicionarioViaProcedureUmObjetoInexistenteDeveRetornarNulo()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoSemDicionario>();
			var config = buscar.CriarProcedure("spConsultarObjetoDeTestes");
			config.DefinirParametro("id").Valor(-999);

			var objeto = buscar.Um(config);

			objeto
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void SeGerarConsultaComEstruturaDiferenteDoObjetoAtualDeveSerPossivelConsultar()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			var buscar = contexto.Buscar<ObjetoDeTestes>();
			var config = buscar.CriarQuery()
				.AdicionarResultado(o => o.Logico)
				.AdicionarResultadoAgregado(Pattern.Buscadores.Agregadores.Count)
				.AdicionarAgrupamento(o => o.Logico)
				.AdicionarOrdem(o => o.Logico);

			var valor = false;
			var registros = 0;
			foreach (var registro in buscar.Registros(config))
			{
				registro[0]
					.Should()
					.Be(valor);
				((int)registro[1])
					.Should()
					.BeGreaterThan(0);
				registros += 1;
				valor = !valor;
			}

			registros
				.Should()
				.Be(2);
		}

	}
}
