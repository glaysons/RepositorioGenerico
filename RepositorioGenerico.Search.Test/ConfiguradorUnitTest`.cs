// ReSharper disable ExpressionIsAlwaysNull

using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.Search.Test
{
	[TestClass]
	public class ConfiguradorUnitTest2
	{
		private Configurador<ObjetoDeTestes> CriarConfigurador()
		{
			return new Configurador<ObjetoDeTestes>(new SqlCommand(), new Mock<IDicionario>().Object);
		}

		[TestMethod]
		public void SeAdicionarUmParametroGenericoDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var outroConfigurador = configurador.DefinirParametro(p => p.Codigo).Tipo(DbType.Int32, 123);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@Codigo", DbType.Int32);

			parametroSql.Value
				.Should().Be(123);
		}

		private static IDbDataParameter TestarParametro(Configurador configurador, IConfiguracao<ObjetoDeTestes> outroConfigurador, string nome, DbType? tipoSql)
		{
			outroConfigurador
				.Should()
				.Be(configurador);

			configurador.Comando.Parameters.Contains(nome)
				.Should().BeTrue();

			var parametro = (IDbDataParameter)configurador.Comando.Parameters[nome];

			parametro
				.Should().BeOfType<SqlParameter>();

			if (tipoSql.HasValue)
				parametro.DbType
					.Should().Be(tipoSql);

			return parametro;
		}

		[TestMethod]
		public void SeAdicionarUmParametroStringDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			const string valor = "abc";

			var outroConfigurador = configurador.DefinirParametro(p => p.Nome).Valor(valor, tamanhoMaximo: 10);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@Nome", DbType.String);

			parametroSql.Value
				.Should().Be("abc");

			parametroSql.Size
				.Should().Be(10);
		}

		[TestMethod]
		public void SeAdicionarUmParametroIntDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			const int valor = 123;

			var outroConfigurador = configurador.DefinirParametro(p => p.Codigo).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@Codigo", DbType.Int32);

			parametroSql.Value
				.Should().Be(123);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableIntDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			int? valor = null;

			var outroConfigurador = configurador.DefinirParametro(p => p.CodigoNulo).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@CodigoNulo", DbType.Int32);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroDoubleDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			const double valor = 123.4;

			var outroConfigurador = configurador.DefinirParametro(p => p.Duplo).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@Duplo", DbType.Double);

			parametroSql.Value
				.Should().Be(123.4);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableDoubleDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			double? valor = null;

			var outroConfigurador = configurador.DefinirParametro(p => p.DuploNulo).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@DuploNulo", DbType.Double);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroDecimalDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			const decimal valor = 123.4M;

			var outroConfigurador = configurador.DefinirParametro(p => p.Decimal).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@Decimal", DbType.Decimal);

			parametroSql.Value
				.Should().Be(123.4M);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableDecimalDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			decimal? valor = null;

			var outroConfigurador = configurador.DefinirParametro(p => p.DecimalNulo).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@DecimalNulo", DbType.Decimal);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroBoolDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			const bool valor = true;

			var outroConfigurador = configurador.DefinirParametro(p => p.Logico).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@Logico", DbType.Boolean);

			parametroSql.Value
				.Should().Be(true);
		}

		[TestMethod]
		public void SeAdicionarUmParametroDateTimeDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var valor = new DateTime(2010, 10, 10);

			var outroConfigurador = configurador.DefinirParametro(p => p.DataHora).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@DataHora", DbType.DateTime);

			parametroSql.Value
				.Should().Be(valor);
		}

		[TestMethod]
		public void SeAdicionarUmParametroNullableDateTimeDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			DateTime? valor = null;

			var outroConfigurador = configurador.DefinirParametro(p => p.DataHoraNulo).Valor(valor);

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@DataHoraNulo", DbType.DateTime);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

		[TestMethod]
		public void SeAdicionarUmParametroSemValorDeveSerCriadoUmParametroNoCommand()
		{
			var configurador = CriarConfigurador();

			var outroConfigurador = configurador.DefinirParametro(p => p.CodigoNulo).Nulo();

			var parametroSql = TestarParametro(configurador, outroConfigurador, "@CodigoNulo", null);

			parametroSql.Value
				.Should().Be(DBNull.Value);
		}

	}
}
