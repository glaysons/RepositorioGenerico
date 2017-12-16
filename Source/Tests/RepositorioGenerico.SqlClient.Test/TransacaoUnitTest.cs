using System;
using System.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGenerico.Exceptions;
using System.Data.SqlClient;

namespace RepositorioGenerico.SqlClient.Test
{

	[TestClass]
	public class TransacaoUnitTest
	{

		[TestMethod]
		public void SeCriarUmaTransacaoAPropriedadeConexaoAtualDeveSerATransacaoDoConstrutor()
		{

			var conexao = Mock.Of<IDbConnection>();
			var transacao = new Transacao(conexao);
			transacao.ConexaoAtual
				.Should().Be(conexao);
		}

		[TestMethod]
		public void AoCriarUmaTransacaoAPropriedadeEmTransacaoDeveSerFalso()
		{
			var transacao = CriarTransacao();
			transacao.EmTransacao
				.Should().BeFalse();
			transacao.TransacaoAtual
				.Should().BeNull();
		}

		private static Transacao CriarTransacao()
		{
			var mockConexao = CriarMockDaConexao();
			return new Transacao(mockConexao.Object);
		}

		private static Mock<IDbConnection> CriarMockDaConexao(Mock<IDbTransaction> transacao = null)
		{
			var conexao = new Mock<IDbConnection>();
			if (transacao == null)
				transacao = CriarMockDaTransacao();
			conexao.Setup(c => c.BeginTransaction()).Returns(transacao.Object);
			conexao.Setup(c => c.Close()).Verifiable();
			conexao.Setup(t => t.Dispose()).Verifiable();
			return conexao;
		}

		private static Mock<IDbTransaction> CriarMockDaTransacao()
		{
			var transacao = new Mock<IDbTransaction>();
			transacao.Setup(t => t.Commit()).Verifiable();
			transacao.Setup(t => t.Rollback()).Verifiable();
			transacao.Setup(t => t.Dispose()).Verifiable();
			return transacao;
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoNaoDeveGerarErro()
		{
			var transacao = CriarTransacao();
			Action act = () => transacao.IniciarTransacao();
			act.ShouldNotThrow();
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoAPropriedadeTransacaoAtualDeveEstarPreenchida()
		{
			var transacao = CriarTransacao();
			transacao.IniciarTransacao();
			transacao.EmTransacao
				.Should().BeTrue();
			transacao.TransacaoAtual
				.Should().NotBeNull();
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoDuasVezesDeveGerarErroDeTransacaoJaIniciada()
		{
			var transacao = CriarTransacao();
			transacao.IniciarTransacao();
			Action act = () => transacao.IniciarTransacao();
			act.ShouldThrow<TransacaoJaIniciadaException>();
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoSemTransacaoDeveGerarErroDeTransacaoNaoIniciada()
		{
			var transacao = CriarTransacao();
			Action act = () => transacao.ConfirmarTransacao();
			act.ShouldThrow<TransacaoNaoIniciadaException>();
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoNaoDeveGerarErro()
		{
			var transacao = CriarTransacao();
			transacao.IniciarTransacao();
			Action act = () => transacao.ConfirmarTransacao();
			act.ShouldNotThrow();
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoATransacaoDeveSerLimpa()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);
			var transacao = new Transacao(mockConexao.Object);

			transacao.IniciarTransacao();
			transacao.ConfirmarTransacao();
			transacao.TransacaoAtual
				.Should().BeNull();
			transacao.EmTransacao
				.Should().BeFalse();

			mockTransacao.Verify(t => t.Dispose());
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoAConexaoDeveEstarFechada()
		{
			var mockConexao = CriarMockDaConexao();
			var transacao = new Transacao(mockConexao.Object);

			transacao.IniciarTransacao();
			transacao.ConfirmarTransacao();

			mockConexao.Verify(c => c.Close());
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoSemTransacaoDeveGerarErroDeTransacaoNaoIniciada()
		{
			var transacao = CriarTransacao();
			Action act = () => transacao.CancelarTransacao();
			act.ShouldThrow<TransacaoNaoIniciadaException>();
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoATransacaoDeveSerLimpa()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);
			var transacao = new Transacao(mockConexao.Object);

			transacao.IniciarTransacao();
			transacao.CancelarTransacao();
			transacao.TransacaoAtual
				.Should().BeNull();
			transacao.EmTransacao
				.Should().BeFalse();

			mockTransacao.Verify(t => t.Dispose());
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoAConexaoDeveEstarFechada()
		{
			var mockConexao = CriarMockDaConexao();
			var transacao = new Transacao(mockConexao.Object);

			transacao.IniciarTransacao();
			transacao.CancelarTransacao();

			mockConexao.Verify(c => c.Close());
		}

		[TestMethod]
		public void SeExcluirObjetoTransacaoDisposeDeveSerChamado()
		{
			var mockConexao = CriarMockDaConexao();

			using (var transacao = new Transacao(mockConexao.Object))
			{
				transacao.EmTransacao
					.Should().BeFalse();
			}

			mockConexao.Verify(c => c.Dispose());
		}

		[TestMethod]
		public void SeExcluirObjetoTransacaoComUmaTransacaoEmAndamentoAMesmaDeveSerCancelada()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);

			using (var transacao = new Transacao(mockConexao.Object))
			{
				transacao.IniciarTransacao();
			}

			mockTransacao.Verify(t => t.Rollback());
			mockTransacao.Verify(t => t.Dispose());
		}

		[TestMethod]
		public void SeCriarUmaTransacaoUtilizandoUmaTransacaoExistenteNaoPodeIniciarNovaTransacao()
		{
			using (var conexao = new SqlConnection(ConnectionStringHelper.Consultar()))
			{
				conexao.Open();

				var transacaoBanco = conexao.BeginTransaction();

				using (var transacao = new Transacao(transacaoBanco))
				{
					Action acao = () => transacao.IniciarTransacao();
					acao
						.ShouldThrow<TransacaoJaIniciadaException>();
				}

				transacaoBanco.Rollback();
			}
		}

		[TestMethod]
		public void SeCriarUmaTransacaoUtilizandoUmaTransacaoExistenteNaoPodeConfirmarTransacao()
		{
			using (var conexao = new SqlConnection(ConnectionStringHelper.Consultar()))
			{
				conexao.Open();

				var transacaoBanco = conexao.BeginTransaction();

				using (var transacao = new Transacao(transacaoBanco))
				{
					Action acao = () => transacao.ConfirmarTransacao();
					acao
						.ShouldThrow<NaoEhPossivelConfirmarOuCancelarTransacaoExternaException>();
				}

				transacaoBanco.Rollback();
			}
		}

		[TestMethod]
		public void SeCriarUmaTransacaoUtilizandoUmaTransacaoExistenteNaoPodeCancelarTransacao()
		{
			using (var conexao = new SqlConnection(ConnectionStringHelper.Consultar()))
			{
				conexao.Open();

				var transacaoBanco = conexao.BeginTransaction();

				using (var transacao = new Transacao(transacaoBanco))
				{
					Action acao = () => transacao.CancelarTransacao();
					acao
						.ShouldThrow<NaoEhPossivelConfirmarOuCancelarTransacaoExternaException>();
				}

				transacaoBanco.Rollback();
			}
		}

	}

}
