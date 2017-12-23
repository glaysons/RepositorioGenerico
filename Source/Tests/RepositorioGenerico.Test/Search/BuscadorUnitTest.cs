using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Search;
using Moq;
using RepositorioGenerico.Pattern;
using FluentAssertions;
using RepositorioGenerico.Test.Objetos;
using System.Linq;
using System.Data;

namespace RepositorioGenerico.Test.Search
{
	[TestClass]
	public class BuscadorUnitTest
	{

		public class ObjetoQualquer
		{

			public int Um { get; set; }

			public int Dois { get; set; }

			public int Tres { get; set; }

		}

		[TestMethod]
		public void SeCriarProcedureUmObjetoConfiguradorProcedureDeveSerGerado()
		{
			var buscador = CriarBuscador();
			var proc = buscador.CriarProcedure("abc");
			proc
				.Should()
				.NotBeNull()
				.And
				.BeOfType<ConfiguradorProcedure>();
		}

		private Buscador CriarBuscador()
		{
			var comando = Mock.Of<IComando>();
			return new Buscador(comando, new QueryBuilderParaTeste());
		}

		[TestMethod]
		public void SeCriarQueryUmObjetoConfiguradorQueryDeveSerGerado()
		{
			var buscador = CriarBuscador();
			var query = buscador.CriarQuery("abc");
			query
				.Should()
				.NotBeNull()
				.And
				.BeOfType<ConfiguradorQuery>();
		}

		[TestMethod]
		public void SeConsultarTodosDeveTrazer()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);

			DefinirScriptQualquer(query);

			ValidarTabelaDeUmObjetoQualquer(buscador);
		}

		private static void DefinirScriptQualquer(QueryBuilderParaTeste query)
		{
			query.DefinirScriptParaTeste(
				"select 1 as Um, 2 as Dois, 3 as Tres union " + 
				"select 10, 20, 30");
		}

		private void ValidarTabelaDeUmObjetoQualquer(Buscador buscador)
		{
			var tabela = buscador.Todos("abc");

			tabela
				.Should()
				.NotBeNull();

			tabela.Columns.Count
				.Should()
				.Be(3, "porque deve ter 3 colunas!");

			tabela.Rows.Count
				.Should()
				.Be(2, "porque deve encontrar 2 registros!");

			ValidarRegistroDeUmObjetoQualquer(tabela.Rows[0], 1);
			ValidarRegistroDeUmObjetoQualquer(tabela.Rows[1], 10);
		}

		private void ValidarRegistroDeUmObjetoQualquer(DataRow registro, int multiplicador = 1)
		{
			registro[0]
				.Should()
				.Be(1 * multiplicador);

			registro[1]
				.Should()
				.Be(2 * multiplicador);

			registro[2]
				.Should()
				.Be(3 * multiplicador);
		}

		[TestMethod]
		public void SeConsultarVariosDeveTrazer()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);
			var config = buscador.CriarQuery("abc");

			DefinirScriptQualquer(query);

			var tabela = buscador.Varios(config);

			ValidarTabelaDeUmObjetoQualquer(buscador);
		}

		[TestMethod]
		public void SeConsultarVariosDeUmObjetoQualquerDeveTrazerUmaInstanciaValida()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);
			var config = buscador.CriarQuery("abc");

			DefinirScriptQualquer(query);

			var objetos = buscador.Varios<ObjetoQualquer>(config).ToList();

			objetos
				.Should()
				.NotBeNull()
				.And
				.HaveCount(2, "porque deve encontrar dois registros!");

			var objeto = objetos.FirstOrDefault();

			objeto.Um
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeConsultarUmDeveTrazer()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);
			var config = buscador.CriarQuery("abc");

			DefinirScriptQualquer(query);

			var registro = buscador.Um(config);

			registro
				.Should()
				.NotBeNull();

			ValidarRegistroDeUmObjetoQualquer(registro);
		}

		[TestMethod]
		public void SeConsultarUmDeUmObjetoQualquerDeveTrazerUmaInstanciaValida()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);
			var config = buscador.CriarQuery("abc");

			DefinirScriptQualquer(query);

			var objeto = buscador.Um<ObjetoQualquer>(config);

			objeto
				.Should()
				.NotBeNull();

			objeto.Um
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeConsultarScalarDeUmObjetoQualquerDeveTrazerAPrimeiraColuna()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);
			var config = buscador.CriarQuery("abc");

			DefinirScriptQualquer(query);

			var valor = buscador.Scalar(config);

			valor
				.Should()
				.NotBeNull()
				.And
				.BeOfType<int>();

			((int)valor)
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeConsultarNonQueryDeUmObjetoQualquerDeveTrazerQuantidadeDeRegistrosExistentes()
		{
			var query = new QueryBuilderParaTeste();
			var buscador = new Buscador(new ComandoParaTeste(), query);
			var config = buscador.CriarQuery("abc");

			query.DefinirScriptParaTeste("update ObjetoVirtual set Nome = Nome");

			var valor = buscador.NonQuery(config);

			valor
				.Should()
				.BeGreaterThan(0);
		}

	}
}
