using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.SqlClient.Test
{
	[TestClass]
	public class ConexaoUnitTest
	{
		[TestMethod]
		public void SeCriarUmaConexaoSemTransacaoDeveGerarUmaConexaoAbertaSemTransacaoVinculada()
		{
			using (var conexao = CriarConexao())
			using (var connection = conexao.CriarConexaoSemTransacao())
			{

				connection
					.Should()
					.NotBeNull()
					.And
					.BeOfType<SqlConnection>();

				connection.State
					.Should()
					.Be(ConnectionState.Open);

				conexao.EmTransacao
					.Should()
					.BeFalse();

			}
		}

		private Conexao CriarConexao()
		{
			return new Conexao(ConnectionStringHelper.Consultar());
		}

		[TestMethod]
		public void SeCriarUmaConexaoTransacionadaDeveGerarUmaConexaoAbertaComTransacao()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{

				transacao
					.Should()
					.NotBeNull()
					.And
					.BeOfType<Transacao>();

				conexao.EmTransacao
					.Should()
					.BeTrue();

				transacao.CancelarTransacao();

				conexao.EmTransacao
					.Should()
					.BeFalse();

			}
		}

		[TestMethod]
		public void SeIniciarECancelarTransacaoDeveGerenciarStatusTransacaoCorretamente()
		{
			using (var conexao = CriarConexao())
			{
				conexao.EmTransacao
					.Should()
					.BeFalse();

				using (var transacao = conexao.IniciarTransacao() as Transacao)
				{
					conexao.EmTransacao
						.Should()
						.BeTrue();

					Action repetirIniciar = () => conexao.IniciarTransacao();
					repetirIniciar
						.Should().Throw<TransacaoJaIniciadaException>();

					transacao.CancelarTransacao();

					conexao.EmTransacao
						.Should()
						.BeFalse();

					Action repetirCancelar = () => transacao.CancelarTransacao();
					repetirCancelar
						.Should().Throw<TransacaoNaoIniciadaException>();
				}
			}

		}

		[TestMethod]
		public void SeIniciarEConfirmarTransacaoDeveGerenciarStatusTransacaoCorretamente()
		{
			using (var conexao = CriarConexao())
			{
				conexao.EmTransacao
					.Should()
					.BeFalse();

				using (var transacao = conexao.IniciarTransacao() as Transacao)
				{

					conexao.EmTransacao
						.Should()
						.BeTrue();

					transacao.ConfirmarTransacao();

					conexao.EmTransacao
						.Should()
						.BeFalse();

					Action repetirConfirmar = () => transacao.ConfirmarTransacao();

					repetirConfirmar
						.Should().Throw<TransacaoNaoIniciadaException>();
				}

			}


		}

		[TestMethod]
		public void SeCriarComandoDeveGerarUmaInstanciaDeSqlCommand()
		{
			using (var conexao = CriarConexao())
				conexao.CriarComando()
					.Should()
					.NotBeNull()
					.And
					.BeOfType<SqlCommand>();

		}

		[TestMethod]
		public void SeExecutarDisposeEmUmaConexaoEmTransacaoAMesmaDeveraSerCancelada()
		{
			using (var conexao = CriarConexao())
			{
				conexao.EmTransacao
					.Should()
					.BeFalse();

				using (var transacao = conexao.IniciarTransacao() as Transacao)
				{
					var executouEventoCancelamento = false;
					transacao.DepoisCancelarTransacao = (sender) => executouEventoCancelamento = true;

					conexao.EmTransacao
						.Should()
						.BeTrue();

					conexao.Dispose();

					conexao.EmTransacao
						.Should()
						.BeFalse();

					executouEventoCancelamento
						.Should()
						.BeTrue();
				}
			}
		}

		[TestMethod]
		public void SeIniciarUmaTransacaoOsEventosDeInicioDevemSerVerificados()
		{
			using (var conexao = CriarConexao())
			{
				var eventosConexao = new GerenciadorEventosConexao(conexao);

				using (var conexaoBanco = conexao.CriarConexaoSemTransacao())
					eventosConexao.Validar(
						executarEventoAntesIniciar: false,
						executarEventoDepoisIniciar: false);

				using (var transacao = conexao.IniciarTransacao() as Transacao)
				{
					eventosConexao.Validar(
						executarEventoAntesIniciar: true,
						executarEventoDepoisIniciar: true);

					eventosConexao.Validar(
						sequenciaEventoAntesIniciar: 1,
						sequenciaEventoDepoisIniciar: 2);

					transacao.CancelarTransacao();
				}

				using (var transacao = conexao.IniciarTransacao() as Transacao)
				{
					var eventosTransacao = new GerenciadorEventosTransacao(transacao as Transacao);

					eventosTransacao.Validar(
						executarEventoAntesConfirmar: false,
						executarEventoAoConfirmar: false,
						executarEventoAntesCancelar: false,
						executarEventoAoCancelar: false);

					eventosTransacao.Validar(
						sequenciaEventoAntesConfirmar: 0,
						sequenciaEventoAoConfirmar: 0,
						sequenciaEventoAntesCancelar: 0,
						sequenciaEventoAoCancelar: 0);
				}
			}
		}

		[TestMethod]
		public void SeConfirmarUmaTransacaoOsEventosDeConfirmacaoDevemSerVerificados()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{
				var eventos = new GerenciadorEventosTransacao(transacao as Transacao);

				transacao.ConfirmarTransacao();

				eventos.Validar(
					executarEventoAntesConfirmar: true,
					executarEventoAoConfirmar: true,
					executarEventoAntesCancelar: false,
					executarEventoAoCancelar: false);

				eventos.Validar(
					sequenciaEventoAntesConfirmar: 1,
					sequenciaEventoAoConfirmar: 2,
					sequenciaEventoAntesCancelar: 0,
					sequenciaEventoAoCancelar: 0);

				conexao.Dispose();
			}
		}

		[TestMethod]
		public void SeCancelarUmaTransacaoOsEventosDeCancelamentoDevemSerVerificados()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{
				var eventos = new GerenciadorEventosTransacao(transacao as Transacao);

				transacao.CancelarTransacao();

				eventos.Validar(
					executarEventoAntesConfirmar: false,
					executarEventoAoConfirmar: false,
					executarEventoAntesCancelar: true,
					executarEventoAoCancelar: true);

				eventos.Validar(
					sequenciaEventoAntesConfirmar: 0,
					sequenciaEventoAoConfirmar: 0,
					sequenciaEventoAntesCancelar: 1,
					sequenciaEventoAoCancelar: 2);
			}

		}

		[TestMethod]
		public void SeOcorrerErroAntesDeIniciarUmaTransacaoATransacaoNaoDeveSerIniciada()
		{
			using (var conexao = CriarConexao())
			{
				conexao.AntesIniciarTransacao = (sender) => { throw new Exception("ErroAntesIniciar"); };

				Action antesIniciarIniciar = () => conexao.IniciarTransacao();

				antesIniciarIniciar
					.Should().Throw<Exception>()
					.WithMessage("ErroAntesIniciar");

				conexao.EmTransacao
					.Should()
					.BeFalse();
			}
		}

		[TestMethod]
		public void SeOcorrerErroDepoisIniciarUmaTransacaoATransacaoDeveSerIniciadaNormalmente()
		{
			using (var conexao = CriarConexao())
			{
				conexao.DepoisIniciarTransacao = (sender) => { throw new Exception("ErroDepoisIniciar"); };

				Action depoisIniciar = () => conexao.IniciarTransacao();

				depoisIniciar
					.Should().Throw<Exception>()
					.WithMessage("ErroDepoisIniciar");

				conexao.EmTransacao
					.Should()
					.BeTrue();
			}
		}

		[TestMethod]
		public void SeOcorrerErroAntesDeConfirmarUmaTransacaoATransacaoDeveContinuarExistindoNormalmente()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{
				transacao.AntesConfirmarTransacao = (sender) => { throw new Exception("ErroAntesConfirmar"); };

				Action antesConfirmar = () => transacao.ConfirmarTransacao();

				antesConfirmar
					.Should().Throw<Exception>()
					.WithMessage("ErroAntesConfirmar");

				conexao.EmTransacao
					.Should()
					.BeTrue();
			}
		}

		[TestMethod]
		public void SeOcorrerErroAoConfirmarUmaTransacaoATransacaoDeveConfirmada()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{
				transacao.DepoisConfirmarTransacao = (sender) => { throw new Exception("ErroAoConfirmar"); };

				Action aoConfirmar = () => transacao.ConfirmarTransacao();

				aoConfirmar
					.Should().Throw<Exception>()
					.WithMessage("ErroAoConfirmar");

				conexao.EmTransacao
					.Should()
					.BeFalse();
			}
		}

		[TestMethod]
		public void SeOcorrerErroAntesDeCancelarUmaTransacaoATransacaoDeveContinuarExistindoNormalmente()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{
				transacao.AntesCancelarTransacao = (sender) => { throw new Exception("ErroAntesCancelar"); };

				Action antesCancelar = () => transacao.CancelarTransacao();

				antesCancelar
					.Should().Throw<Exception>()
					.WithMessage("ErroAntesCancelar");

				conexao.EmTransacao
					.Should()
					.BeTrue();

				transacao.AntesCancelarTransacao = null;
			}
		}

		[TestMethod]
		public void SeOcorrerErroAoCancelarUmaTransacaoATransacaoDeveCancelada()
		{
			using (var conexao = CriarConexao())
			using (var transacao = conexao.IniciarTransacao() as Transacao)
			{
				transacao.DepoisCancelarTransacao = (sender) => { throw new Exception("ErroAoCancelar"); };

				Action aoCancelar = () => transacao.CancelarTransacao();

				aoCancelar
					.Should().Throw<Exception>()
					.WithMessage("ErroAoCancelar");

				conexao.EmTransacao
					.Should()
					.BeFalse();
			}
		}

		[TestMethod]
		public void SeDefinirUmaConexaoTransacionadaNumCommandExistenteAMesmaDeveSerPreenchidaCorretamente()
		{
			using (var conexao = CriarConexao())
			{
				var transacao = conexao.IniciarTransacao() as Transacao;

				var comando = new SqlCommand();

				conexao.DefinirConexaoTransacionada(comando);

				comando.Connection
					.Should()
					.NotBeNull();
			}
		}

		[TestMethod]
		public void SeDefinirUmaConexaoTransacionadaNumCommandSemTransacaoIniciadaDeveGerarErroEspecifico()
		{
			using (var conexao = CriarConexao())
			using (var comando = new SqlCommand())
			{
				Action definirConexao = () => conexao.DefinirConexaoTransacionada(comando);

				definirConexao
					.Should().Throw<TransacaoNaoIniciadaException>();
			}
		}

		[TestMethod]
		public void SeModificarUmRegistroUtilizandoTransacaoExistenteATransacaoDevePermitirCancelar()
		{
			var cs = ConnectionStringHelper.Consultar();
			using (var conexaoBanco = new SqlConnection(cs))
			{
				conexaoBanco.Open();
				var transacaoBanco = conexaoBanco.BeginTransaction();

				var comando = conexaoBanco.CreateCommand();
				comando.Transaction = transacaoBanco;

				ValidarConteudoDoNomeDoRegistroIgual(comando, "Teste A");

				try
				{
					using (var contexto = new RepositorioGenerico.SqlClient.Contextos.Contexto(cs, transacaoBanco))
					{
						var repositorio = contexto.Repositorio<ObjetoDeTestes>();
						var registro = repositorio.Consultar(1);
						registro.Nome = "Nome do Objeto Modificado Em Banco";
						repositorio.Atualizar(registro);
						contexto.Salvar();
					}

					ValidarConteudoDoNomeDoRegistroIgual(comando, "Nome do Objeto Modificado Em Banco");

				}
				finally
				{
					transacaoBanco.Rollback();
				}

				ValidarConteudoDoNomeDoRegistroIgual(comando, "Teste A");
			}
		}

		private static void ValidarConteudoDoNomeDoRegistroIgual(IDbCommand comando, string texto)
		{
			comando.CommandText = "select Nome from ObjetoVirtual where (Codigo = 1)";
			var nome = comando.ExecuteScalar().ToString();
			nome
				.Should()
				.Be(texto);
		}
	}

}
