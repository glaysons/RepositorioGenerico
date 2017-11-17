using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.Test.Search
{
	[TestClass]
	public class ConfiguradorUnitTest
	{

		private Configurador CriarConfigurador()
		{
			return new Configurador(new SqlCommand());
		}

		[TestMethod]
		public void SeAdicionarUmParametroSemValorDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var outroConfigurador = configurador.DefinirParametro("p").Nulo();

			outroConfigurador
				.Should()
				.Be(configurador);

			configurador.Comando.Parameters.Contains("@p")
				.Should().BeTrue();

			var parametro = configurador.Comando.Parameters["@p"];

			parametro
				.Should().BeOfType<SqlParameter>();

			((SqlParameter)parametro).Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroGenericoDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var outroConfigurador = configurador.DefinirParametro("p").Tipo(DbType.Int32, 123);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.Int32);

			parametroSql.Value
				.Should().Be(123);
		}

		private static IDbDataParameter TestarParametro(Configurador configurador, IConfiguracao outroConfigurador, DbType tipoSql)
		{
			outroConfigurador
				.Should()
				.Be(configurador);

			configurador.Comando.Parameters.Contains("@p")
				.Should().BeTrue();

			var parametro = (IDbDataParameter) configurador.Comando.Parameters["@p"];

			parametro
				.Should().BeOfType<SqlParameter>();

			parametro.DbType
				.Should().Be(tipoSql);

			return parametro;
		}

		[TestMethod]
		public void SeAdicionarUmParametroDecimalDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			const decimal valor = 123.4M;

			var outroConfigurador = configurador.DefinirParametro("p").Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.Decimal);

			parametroSql.Value
				.Should().Be(123.4M);
		}

		[TestMethod]
		public void SeAdicionarUmParametroBoolDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var outroConfigurador = configurador.DefinirParametro("p").Valor(true);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.Boolean);

			parametroSql.Value
				.Should().Be(true);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableDateTimeDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			DateTime? valor = null;

			var outroConfigurador = configurador.DefinirParametro("p").Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.DateTime);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroDateTimeDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var valor = new DateTime(2010, 10, 10);

			var outroConfigurador = configurador.DefinirParametro("p").Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.DateTime);

			parametroSql.Value
				.Should().Be(valor);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableDoubleDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			double? valor = null;

			var outroConfigurador = configurador.DefinirParametro("p").Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.Double);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableDecimalDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			decimal? valor = null;

			var outroConfigurador = configurador.DefinirParametro("p").Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.Decimal);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableIntDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			int? valor = null;

			var outroConfigurador = configurador.DefinirParametro("p").Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.Int32);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroStringDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var outroConfigurador = configurador.DefinirParametro("p").Valor("abc", tamanhoMaximo: 10);

			var parametroSql = TestarParametro(configurador, outroConfigurador, DbType.String);

			parametroSql.Value
				.Should().Be("abc");

			parametroSql.Size
				.Should().Be(10);
		}

		[TestMethod]
		public void SeExecutarOMetodoPrepararDeveGerarErroNaoImplementado()
		{
			var configurador = CriarConfigurador();

			Action teste = () => configurador.Preparar();

			teste
				.ShouldThrow<NotImplementedException>();
		}

		[TestMethod]
		public void SeExecutarConsultarScriptDeveRetornarConteudoCommand()
		{
			var configurador = CriarConfigurador();

			configurador.Comando.CommandText = "select 1";

			configurador.ConsultarScript()
				.Should().Be("select 1");
		}

	}
}
