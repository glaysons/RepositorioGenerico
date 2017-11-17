using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.SqlClient.Contextos;

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
		public void SeCriarUmContextoLegadoDeveGerarUmaInstanciaValida()
		{

			var contexto = Fabrica.CriarContextoLegado(ConnectionStringHelper.Consultar());
			contexto
				.Should()
				.NotBeNull()
				.And
				.BeOfType<SqlClient.Contextos.Tables.Contexto>();

		}

	}
}
