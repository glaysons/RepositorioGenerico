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
			using (var transacao = new Transacao(conexao))
				transacao.ConexaoAtual
					.Should().Be(conexao);
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoNaoDeveGerarErro()
		{
			Action act = () => CriarTransacao();
			act.ShouldNotThrow();
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
		public void AoIniciarUmaTransacaoAPropriedadeTransacaoAtualDeveEstarPreenchida()
		{
			using (var transacao = CriarTransacao())
				transacao.TransacaoAtual
					.Should().NotBeNull();
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoSemTransacaoDeveGerarErroDeTransacaoNaoIniciada()
		{
			using (var transacao = CriarTransacao())
			{
				transacao.CancelarTransacao();
				Action act = () => transacao.ConfirmarTransacao();
				act.ShouldThrow<TransacaoNaoIniciadaException>();
			}
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoNaoDeveGerarErro()
		{
			using (var transacao = CriarTransacao())
			{
				Action act = () => transacao.ConfirmarTransacao();
				act.ShouldNotThrow();
			}
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoATransacaoDeveSerLimpa()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);
			using (var transacao = new Transacao(mockConexao.Object))
			{
				transacao.ConfirmarTransacao();
				transacao.TransacaoAtual
					.Should().BeNull();
				mockTransacao.Verify(t => t.Dispose());
			}
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoAConexaoDeveSerFechada()
		{
			var mockConexao = CriarMockDaConexao();
			using (var transacao = new Transacao(mockConexao.Object))
			{
				transacao.ConfirmarTransacao();
				mockConexao.Verify(c => c.Close());
			}
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoSemTransacaoDeveGerarErroDeTransacaoNaoIniciada()
		{
			using (var transacao = CriarTransacao())
			{
				transacao.CancelarTransacao();
				Action act = () => transacao.CancelarTransacao();
				act.ShouldThrow<TransacaoNaoIniciadaException>();
			}
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoATransacaoDeveSerLimpa()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);
			using (var transacao = new Transacao(mockConexao.Object))
			{
				transacao.CancelarTransacao();
				transacao.TransacaoAtual
					.Should().BeNull();
				mockTransacao.Verify(t => t.Dispose());
			}
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoAConexaoDeveSerFechada()
		{
			var mockConexao = CriarMockDaConexao();
			using (var transacao = new Transacao(mockConexao.Object))
			{
				transacao.CancelarTransacao();
				mockConexao.Verify(c => c.Close());
			}
		}

		[TestMethod]
		public void SeExcluirObjetoTransacaoComUmaTransacaoEmAndamentoAMesmaDeveSerCancelada()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);

			using (var transacao = new Transacao(mockConexao.Object))
			{
			}

			mockTransacao.Verify(t => t.Rollback());
			mockTransacao.Verify(t => t.Dispose());
		}

		[TestMethod]
		public void SeCriarUmaTransacaoUtilizandoUmaTransacaoExistenteNaoPodeIniciarNovaTransacao()
		{
			var cs = ConnectionStringHelper.Consultar();
			using (var conexaoBanco = new SqlConnection(cs))
			{
				conexaoBanco.Open();

				var transacaoBanco = conexaoBanco.BeginTransaction();

				using (var conexao = new Conexao(cs, transacaoBanco))
				{
					Action acao = () => conexao.IniciarTransacao();
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
