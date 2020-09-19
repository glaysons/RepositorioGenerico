using System;
using System.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.Fake.Test
{
	[TestClass]
	public class TransacaoFakeUnitTest
	{

		[TestMethod]
		public void SeCriarUmaTransacaoAPropriedadeConexaoAtualDeveSerATransacaoDoConstrutor()
		{
			var conexao = Mock.Of<IDbConnection>();
			using (var transacao = new TransacaoFake(conexao))
				transacao.ConexaoAtual
					.Should().Be(conexao);
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoNaoDeveGerarErro()
		{
			using (var conexao = new ConexaoFake())
			{
				Action act = () => conexao.IniciarTransacao();
				act.Should().NotThrow();
			}
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoAPropriedadeTransacaoAtualDeveEstarPreenchida()
		{
			using (var conexao = new ConexaoFake())
			using (var transacao = conexao.IniciarTransacao() as TransacaoFake)
			{
				transacao.TransacaoAtual
					.Should()
					.NotBeNull();
			}
		}

		[TestMethod]
		public void AoIniciarUmaTransacaoDuasVezesDeveGerarErroDeTransacaoJaIniciada()
		{
			using (var conexao = new ConexaoFake())
			using (var transacao = conexao.IniciarTransacao() as TransacaoFake)
			{
				Action act = () => conexao.IniciarTransacao();
				act.Should().Throw<TransacaoJaIniciadaException>();
			}
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoSemTransacaoDeveGerarErroDeTransacaoNaoIniciada()
		{
			using (var conexao = new ConexaoFake())
			using (var transacao = conexao.IniciarTransacao() as TransacaoFake)
			{
				transacao.ConfirmarTransacao();
				Action act = () => transacao.ConfirmarTransacao();
				act.Should().Throw<TransacaoNaoIniciadaException>();
			}
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoNaoDeveGerarErro()
		{
			using (var conexao = new ConexaoFake())
			using (var transacao = conexao.IniciarTransacao() as TransacaoFake)
			{
				Action act = () => transacao.ConfirmarTransacao();
				act.Should().NotThrow();
			}
		}

		[TestMethod]
		public void AoConfirmarUmaTransacaoATransacaoDeveSerLimpa()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);
			using (var transacao = new TransacaoFake(mockConexao.Object))
			{
				transacao.ConfirmarTransacao();
				transacao.TransacaoAtual
					.Should().BeNull();
				mockTransacao.Verify(t => t.Dispose());
			}
		}

		private static Mock<IDbTransaction> CriarMockDaTransacao()
		{
			var transacao = new Mock<IDbTransaction>();
			transacao.Setup(t => t.Commit()).Verifiable();
			transacao.Setup(t => t.Rollback()).Verifiable();
			transacao.Setup(t => t.Dispose()).Verifiable();
			return transacao;
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

		[TestMethod]
		public void AoConfirmarUmaTransacaoAConexaoDeveEstarFechada()
		{
			var mockConexao = CriarMockDaConexao();
			using (var transacao = new TransacaoFake(mockConexao.Object))
			{
				transacao.ConfirmarTransacao();
				mockConexao.Verify(c => c.Close());
			}
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoSemTransacaoDeveGerarErroDeTransacaoNaoIniciada()
		{
			using (var conexao = new ConexaoFake())
			using (var transacao = conexao.IniciarTransacao() as TransacaoFake)
			{
				transacao.CancelarTransacao();
				Action act = () => transacao.CancelarTransacao();
				act.Should().Throw<TransacaoNaoIniciadaException>();
			}
		}

		[TestMethod]
		public void AoCancelarUmaTransacaoATransacaoDeveSerLimpa()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);
			using (var transacao = new TransacaoFake(mockConexao.Object))
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
			using (var transacao = new TransacaoFake(mockConexao.Object))
			{
				transacao.CancelarTransacao();
				mockConexao.Verify(c => c.Close());
			}
		}

		[TestMethod]
		public void SeExcluirObjetoTransacaoFakeComUmaTransacaoEmAndamentoAMesmaDeveSerCancelada()
		{
			var mockTransacao = CriarMockDaTransacao();
			var mockConexao = CriarMockDaConexao(mockTransacao);

			using (var transacao = new TransacaoFake(mockConexao.Object))
			{
			}

			mockTransacao.Verify(t => t.Rollback());
			mockTransacao.Verify(t => t.Dispose());
		}

	}
}
