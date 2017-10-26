using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.SqlClient.Contextos;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.SqlClient.Test.Contextos
{
	[TestClass]
	public class ContextoUnitTest
	{

		[TestMethod]
		public void SeSolicitarUmRepositorioDeveGerarUmRepositorioValido()
		{
			var contexto = CriarContexto();

			contexto.Repositorio<ObjetoDeTestes>()
				.Should()
				.BeOfType<Repositorio<ObjetoDeTestes>>();
		}

		private static Contexto CriarContexto()
		{
			var contexto = new Contexto(ConnectionStringHelper.Consultar());
			return contexto;
		}

		[TestMethod]
		public void SeSolicitarDuasVezesUmRepositorioDeveRetornarAMesmaInstancia()
		{
			var contexto = CriarContexto();

			var repositorioUm = contexto.Repositorio<ObjetoDeTestes>();

			var repositorioDois = contexto.Repositorio<ObjetoDeTestes>();

			repositorioUm
				.Should()
				.BeSameAs(repositorioDois);
		}

	}
}
