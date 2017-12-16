using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.SqlClient.Contextos;
using System.Data;
using System.Data.SqlClient;

namespace RepositorioGenerico.SqlClient.Test
{
	[TestClass]
	public class FabricaUnitTest
	{

		[TestMethod]
		public void SeCriarUmContextoEspecificoDeveGerarUmaInstanciaValida()
		{

			var contexto = Fabrica.CriarContexto(ConnectionStringHelper.Consultar());
			contexto
				.Should()
				.NotBeNull()
				.And
				.BeOfType<Contexto>();

		}

		[TestMethod]
		public void SeCriarUmContextoEspecificoComTransacaoExistenteAoFinalizarATransacaoDevePermanecerAtivo()
		{
			var cs = ConnectionStringHelper.Consultar();
			using (var conexao = new SqlConnection(cs))
			{
				conexao.Open();

				using (var transacao = conexao.BeginTransaction())
				{

					var contexto = Fabrica.CriarContexto(cs, transacao);
					contexto
						.Should()
						.NotBeNull()
						.And
						.BeOfType<Contexto>();

				}

				conexao.State
					.Should()
					.Be(ConnectionState.Open);
			}
		}

		[TestMethod]
		public void SeCriarUmContextoLegadoDeveGerarUmaInstanciaValida()
		{

			var contexto = Fabrica.CriarContextoLegado(ConnectionStringHelper.Consultar());
			contexto
				.Should()
				.NotBeNull()
				.And
				.BeOfType<SqlClient.Contextos.Tables.Contexto>();

		}

		[TestMethod]
		public void SeCriarUmContextoLegadoComTransacaoExistenteAoFinalizarATransacaoDevePermanecerAtivo()
		{
			var cs = ConnectionStringHelper.Consultar();
			using (var conexao = new SqlConnection(cs))
			{
				conexao.Open();

				using (var transacao = conexao.BeginTransaction())
				{

					var contexto = Fabrica.CriarContextoLegado(cs, transacao);
					contexto
						.Should()
						.NotBeNull()
						.And
						.BeOfType<SqlClient.Contextos.Tables.Contexto>();

				}

				conexao.State
					.Should()
					.Be(ConnectionState.Open);
			}
		}

	}
}
