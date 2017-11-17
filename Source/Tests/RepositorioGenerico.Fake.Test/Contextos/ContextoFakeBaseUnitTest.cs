using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGenerico.Fake.Contextos;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Fake.Test.Contextos
{
	[TestClass]
	public class ContextoFakeBaseUnitTest
	{
		[TestMethod]
		public void SeConsultarVariasVezesOMesmoBuscadorGenericoDeveRetornarMesmaInstancia()
		{
			var contexto = new ContextoFakeBase();

			var buscadorUm = contexto.Buscar();

			var buscadorDois = contexto.Buscar();

			buscadorUm
				.Should()
				.BeSameAs(buscadorDois);
		}

		[TestMethod]
		public void SeConsultarVariasVezesOMesmoBuscadorEspecificoDeveRetornarMesmaInstancia()
		{
			var contexto = new ContextoFakeBase();

			var buscadorUm = contexto.Buscar<ObjetoDeTestes>();

			var buscadorDois = contexto.Buscar<ObjetoDeTestes>();

			buscadorUm
				.Should()
				.BeSameAs(buscadorDois);
		}

		[TestMethod]
		public void SeSalvarContextoDeveSalvarHistoricoTransacional()
		{
			var transacao = CriarHistoricoTransacional();
			var contexto = new ContextoFakeBase(transacao.Object);

			contexto.Salvar();

			transacao.Verify();
		}

		private static Mock<IHistoricoTransacional> CriarHistoricoTransacional()
		{
			var transacao = new Mock<IHistoricoTransacional>();

			transacao.Setup(t => t.Salvar()).Verifiable();

			return transacao;
		}

		[TestMethod]
		public void SeSalvarContextoComTransacaoIniciadaNaoDeveGerarErro()
		{
			var transacao = CriarHistoricoTransacional();
			var contexto = new ContextoFakeBase(transacao.Object);

			contexto.IniciarTransacao();

			Action salvar = () => contexto.Salvar();

			salvar
				.ShouldNotThrow();
		}

	}
}
