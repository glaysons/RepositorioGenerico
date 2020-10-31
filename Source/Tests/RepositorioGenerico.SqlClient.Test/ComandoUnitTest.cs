using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;
using RepositorioGenerico.SqlClient.Builders;
using RepositorioGenerico.Test.Objetos;
using System.Data.SqlClient;
using System.Diagnostics;

namespace RepositorioGenerico.SqlClient.Test
{
	[TestClass]
	public class ComandoUnitTest
	{

		[TestMethod]
		public void SePedirNovoDbCommandDeveCriarSqlCommand()
		{
			var comando = CriarComando();

			comando.CriarComando()
				.Should()
				.NotBeNull()
				.And
				.BeOfType<SqlCommand>();
		}

		private Comando CriarComando()
		{
			return new Comando(CriarConexao());
		}

		private IConexao CriarConexao()
		{
			return new Conexao(ConnectionStringHelper.Consultar());
		}

		[TestMethod]
		public void SeConsultarEstruturaDaTabelaNoBancoDeveGerarTabelaComEstruturaCorreta()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.DefinirTabela(dicionario.Nome);
			config.DefinirLimite(0);

			var tabela = comando.ConsultarTabela(config);

			tabela
				.Should()
				.NotBeNull();

			tabela.Columns.Count
				.Should()
				.Be(dicionario.QuantidadeCampos);

			foreach (var campo in dicionario.Itens)
			{
				tabela.Columns.Contains(campo.Nome)
					.Should()
					.BeTrue("Porque a coluna " + campo.Nome + " deve existir na tabela!");

				var coluna = tabela.Columns[campo.Nome];
				coluna.DataType
					.Should()
					.Be(campo.TipoLocal);
			}
		}

		[TestMethod]
		public void SeConsultarRegistroNoBancoDeveGerarDataReaderComEstruturaCorreta()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.DefinirTabela(dicionario.Nome);
			config.DefinirLimite(0);

			var registros = comando.ConsultarRegistro(config);

			registros
				.Should()
				.NotBeNull();

			registros.FieldCount
				.Should()
				.Be(dicionario.QuantidadeCampos);

			registros.Read()
				.Should()
				.BeFalse();

		}

		[TestMethod]
		public void SeConsultarRegistrosTipadosNoBancoDeveGerarDataReaderComEstruturaCorreta()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery<ObjetoDeTestes>(comando.CriarComando(), dicionario, new QueryBuilder());
			config.DefinirTabela();
			config.DefinirLimite(0);

			var registros = comando.ConsultarRegistro(config);

			registros
				.Should()
				.NotBeNull();

			registros.FieldCount
				.Should()
				.Be(dicionario.QuantidadeCampos);

			registros.Read()
				.Should()
				.BeFalse();

		}

		[TestMethod]
		public void SeConsultarScalarDaQuantidadeDeRegistrosDeveRetornarNumeroValido()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.DefinirTabela(dicionario.Nome);
			config.AdicionarResultado("(Count(*))as[Quantidade]");

			var scalar = comando.Scalar(config);

			scalar
				.Should()
				.BeOfType<int>();

			((int)scalar)
				.Should()
				.BeGreaterThan(0);
		}

		[TestMethod]
		public void SeConsultarScalarTipadoDaQuantidadeDeRegistrosDeveRetornarNumeroValido()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery<ObjetoDeTestes>(comando.CriarComando(), dicionario, new QueryBuilder());
			config.DefinirTabela();
			config.AdicionarResultadoAgregado(Agregadores.Count);

			var scalar = comando.Scalar(config);

			scalar
				.Should()
				.BeOfType<int>();

			((int)scalar)
				.Should()
				.BeGreaterThan(0);
		}

		[TestMethod]
		public void SeExecutarNonQueryDeveRetornarNumeroDeRegistrosAfetadosValido()
		{
			var comando = CriarComando();

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.PersonalizarScript("update[ObjetoVirtual]set[Logico]=[Logico]where[Codigo]=1");

			var afetados = comando.NonQuery(config);

			afetados
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeExecutarNonQueryTipadoDeveRetornarNumeroDeRegistrosAfetadosValido()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery<ObjetoDeTestes>(comando.CriarComando(), dicionario, new QueryBuilder());
			config.PersonalizarScript("update[ObjetoVirtual]set[Logico]=[Logico]where[Codigo]=1");

			var afetados = comando.NonQuery(config);

			afetados
				.Should()
				.Be(1);
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroValidoDeveRetornarVerdadeiro()
		{
			var comando = CriarComando();

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.DefinirTabela("ObjetoVirtual");
			config.AdicionarCondicao("Codigo").Igual(1);

			comando.Existe(config)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroTipadoValidoDeveRetornarVerdadeiro()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery<ObjetoDeTestes>(comando.CriarComando(), dicionario, new QueryBuilder());
			config.DefinirTabela();
			config.AdicionarCondicao(c => c.Codigo).Igual(1);

			comando.Existe(config)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroInvalidoDeveRetornarFalso()
		{
			var comando = CriarComando();

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.DefinirTabela("ObjetoVirtual");
			config.AdicionarCondicao("Codigo").Igual(-123);

			comando.Existe(config)
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeVerificarExistenciaDeUmRegistroTipadoInvalidoDeveRetornarFalso()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery<ObjetoDeTestes>(comando.CriarComando(), dicionario, new QueryBuilder());
			config.DefinirTabela();
			config.AdicionarCondicao(c => c.Codigo).Igual(-123);

			comando.Existe(config)
				.Should()
				.BeFalse();
		}


		[TestMethod]
		public void SeConsultarScalar20MilVezesDeveDurarMenosQueTresSegundos()
		{
			var comando = CriarComando();
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			var config = new ConfiguradorQuery(comando.CriarComando(), new QueryBuilder());
			config.DefinirTabela(dicionario.Nome);
			config.AdicionarResultado("(Count(*))as[Quantidade]");

			var tempo = new Stopwatch();
			tempo.Start();

			for (var i = 0; i < 20000; i++)
			{
				var scalar = comando.Scalar(config);

				scalar
					.Should()
					.BeOfType<int>();

				((int)scalar)
					.Should()
					.BeGreaterThan(0);
			}

			tempo.Stop();
			tempo.ElapsedMilliseconds
				.Should()
				.BeLessOrEqualTo(1000 * 3);
		}

	}
}
