using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.SqlClient.Test
{
	[TestClass]
	public class ConexaoUnitTest
	{
		[TestMethod]
		public void SeCriarUmaConexaoSemTransacaoDeveGerarUmaConexaoAbertaSemTransacaoVinculada()
		{
			var conexao = CriarConexao();

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
			var conexao = CriarConexao();

			using (var connection = conexao.CriarConexaoTransacionada())
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
					.BeTrue();

				conexao.CancelarTransacao();

				conexao.EmTransacao
					.Should()
					.BeFalse();

			}
		}

		[TestMethod]
		public void SeIniciarECancelarTransacaoDeveGerenciarStatusTransacaoCorretamente()
		{
			var conexao = CriarConexao();

			conexao.EmTransacao
				.Should()
				.BeFalse();

			conexao.IniciarTransacao();

			conexao.EmTransacao
				.Should()
				.BeTrue();

			Action repetirIniciar = () => conexao.IniciarTransacao();
			repetirIniciar
				.ShouldThrow<TransacaoJaIniciadaException>();

			conexao.CancelarTransacao();

			conexao.EmTransacao
				.Should()
				.BeFalse();

			Action repetirCancelar = () => conexao.CancelarTransacao();
			repetirCancelar
				.ShouldThrow<TransacaoNaoIniciadaException>();

		}

		[TestMethod]
		public void SeIniciarEConfirmarTransacaoDeveGerenciarStatusTransacaoCorretamente()
		{
			var conexao = CriarConexao();

			conexao.EmTransacao
				.Should()
				.BeFalse();

			conexao.IniciarTransacao();

			conexao.EmTransacao
				.Should()
				.BeTrue();

			conexao.ConfirmarTransacao();

			conexao.EmTransacao
				.Should()
				.BeFalse();

			Action repetirConfirmar = () => conexao.ConfirmarTransacao();
			repetirConfirmar
				.ShouldThrow<TransacaoNaoIniciadaException>();

		}

		[TestMethod]
		public void SeCriarComandoDeveGerarUmaInstanciaDeSqlCommand()
		{
			var conexao = CriarConexao();

			conexao.CriarComando()
				.Should()
				.NotBeNull()
				.And
				.BeOfType<SqlCommand>();

		}

		[TestMethod]
		public void SeExecutarDisposeEmUmaConexaoEmTransacaoAMesmaDeveraSerCancelada()
		{
			var conexao = CriarConexao();

			var executouEventoCancelamento = false;
			conexao.DepoisCancelarTransacao = () => executouEventoCancelamento = true;

			conexao.EmTransacao
				.Should()
				.BeFalse();

			conexao.IniciarTransacao();

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

		[TestMethod]
		public void SeIniciarUmaTransacaoOsEventosDeInicioDevemSerVerificados()
		{
			var conexao = CriarConexao();
			var eventos = new GerenciadorEventosConexao(conexao);

			var connection = conexao.CriarConexaoSemTransacao();

			eventos.Validar(
				executarEventoAntesIniciar: false,
				executarEventoDepoisIniciar: false,
				executarEventoAntesConfirmar: false,
				executarEventoAoConfirmar: false,
				executarEventoAntesCancelar: false,
				executarEventoAoCancelar: false);

			eventos.ReiniciarValidacao();
			connection.Dispose();

			connection = conexao.CriarConexaoTransacionada();

			eventos.Validar(
				executarEventoAntesIniciar: true,
				executarEventoDepoisIniciar: true,
				executarEventoAntesConfirmar: false,
				executarEventoAoConfirmar: false,
				executarEventoAntesCancelar: false,
				executarEventoAoCancelar: false);

			eventos.Validar(
				sequenciaEventoAntesIniciar: 1,
				sequenciaEventoDepoisIniciar: 2,
				sequenciaEventoAntesConfirmar: 0,
				sequenciaEventoAoConfirmar: 0,
				sequenciaEventoAntesCancelar: 0,
				sequenciaEventoAoCancelar: 0);

			conexao.CancelarTransacao();
			connection.Dispose();
			eventos.ReiniciarValidacao();

			conexao.IniciarTransacao();

			eventos.Validar(
				executarEventoAntesIniciar: true,
				executarEventoDepoisIniciar: true,
				executarEventoAntesConfirmar: false,
				executarEventoAoConfirmar: false,
				executarEventoAntesCancelar: false,
				executarEventoAoCancelar: false);

			eventos.Validar(
				sequenciaEventoAntesIniciar: 1,
				sequenciaEventoDepoisIniciar: 2,
				sequenciaEventoAntesConfirmar: 0,
				sequenciaEventoAoConfirmar: 0,
				sequenciaEventoAntesCancelar: 0,
				sequenciaEventoAoCancelar: 0);

			conexao.Dispose();

		}

		[TestMethod]
		public void SeConfirmarUmaTransacaoOsEventosDeConfirmacaoDevemSerVerificados()
		{
			var conexao = CriarConexao();
			var eventos = new GerenciadorEventosConexao(conexao);

			conexao.IniciarTransacao();

			eventos.ReiniciarValidacao();

			conexao.ConfirmarTransacao();

			eventos.Validar(
				executarEventoAntesIniciar: false,
				executarEventoDepoisIniciar: false,
				executarEventoAntesConfirmar: true,
				executarEventoAoConfirmar: true,
				executarEventoAntesCancelar: false,
				executarEventoAoCancelar: false);

			eventos.Validar(
				sequenciaEventoAntesIniciar: 0,
				sequenciaEventoDepoisIniciar: 0,
				sequenciaEventoAntesConfirmar: 1,
				sequenciaEventoAoConfirmar: 2,
				sequenciaEventoAntesCancelar: 0,
				sequenciaEventoAoCancelar: 0);

			conexao.Dispose();
		}

		[TestMethod]
		public void SeCancelarUmaTransacaoOsEventosDeCancelamentoDevemSerVerificados()
		{
			var conexao = CriarConexao();
			var eventos = new GerenciadorEventosConexao(conexao);

			conexao.IniciarTransacao();

			eventos.ReiniciarValidacao();

			conexao.CancelarTransacao();

			eventos.Validar(
				executarEventoAntesIniciar: false,
				executarEventoDepoisIniciar: false,
				executarEventoAntesConfirmar: false,
				executarEventoAoConfirmar: false,
				executarEventoAntesCancelar: true,
				executarEventoAoCancelar: true);

			eventos.Validar(
				sequenciaEventoAntesIniciar: 0,
				sequenciaEventoDepoisIniciar: 0,
				sequenciaEventoAntesConfirmar: 0,
				sequenciaEventoAoConfirmar: 0,
				sequenciaEventoAntesCancelar: 1,
				sequenciaEventoAoCancelar: 2);

		}

		[TestMethod]
		public void SeOcorrerErroAntesDeIniciarUmaTransacaoATransacaoNaoDeveSerIniciada()
		{
			var conexao = CriarConexao();
			conexao.AntesIniciarTransacao = () => { throw new Exception("ErroAntesIniciar"); };

			Action antesIniciarIniciar = () => conexao.IniciarTransacao();

			antesIniciarIniciar
				.ShouldThrow<Exception>()
				.WithMessage("ErroAntesIniciar");

			conexao.EmTransacao
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeOcorrerErroDepoisIniciarUmaTransacaoATransacaoDeveSerIniciadaNormalmente()
		{
			var conexao = CriarConexao();
			conexao.DepoisIniciarTransacao = () => { throw new Exception("ErroDepoisIniciar"); };

			Action depoisIniciar = () => conexao.IniciarTransacao();

			depoisIniciar
				.ShouldThrow<Exception>()
				.WithMessage("ErroDepoisIniciar");

			conexao.EmTransacao
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeOcorrerErroAntesDeConfirmarUmaTransacaoATransacaoDeveContinuarExistindoNormalmente()
		{
			var conexao = CriarConexao();
			conexao.AntesConfirmarTransacao = () => { throw new Exception("ErroAntesConfirmar"); };

			conexao.IniciarTransacao();
			Action antesConfirmar = () => conexao.ConfirmarTransacao();

			antesConfirmar
				.ShouldThrow<Exception>()
				.WithMessage("ErroAntesConfirmar");

			conexao.EmTransacao
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeOcorrerErroAoConfirmarUmaTransacaoATransacaoDeveConfirmada()
		{
			var conexao = CriarConexao();
			conexao.DepoisConfirmarTransacao = () => { throw new Exception("ErroAoConfirmar"); };

			conexao.IniciarTransacao();
			Action aoConfirmar = () => conexao.ConfirmarTransacao();

			aoConfirmar
				.ShouldThrow<Exception>()
				.WithMessage("ErroAoConfirmar");

			conexao.EmTransacao
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeOcorrerErroAntesDeCancelarUmaTransacaoATransacaoDeveContinuarExistindoNormalmente()
		{
			var conexao = CriarConexao();
			conexao.AntesCancelarTransacao = () => { throw new Exception("ErroAntesCancelar"); };

			conexao.IniciarTransacao();
			Action antesCancelar = () => conexao.CancelarTransacao();

			antesCancelar
				.ShouldThrow<Exception>()
				.WithMessage("ErroAntesCancelar");

			conexao.EmTransacao
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeOcorrerErroAoCancelarUmaTransacaoATransacaoDeveCancelada()
		{
			var conexao = CriarConexao();
			conexao.DepoisCancelarTransacao = () => { throw new Exception("ErroAoCancelar"); };

			conexao.IniciarTransacao();
			Action aoCancelar = () => conexao.CancelarTransacao();

			aoCancelar
				.ShouldThrow<Exception>()
				.WithMessage("ErroAoCancelar");

			conexao.EmTransacao
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeDefinirUmaConexaoNumCommandExistenteAMesmaDeveSerPreenchidaCorretamente()
		{
			var conexao = CriarConexao();

			conexao.IniciarTransacao();

			var comando = new SqlCommand();

			conexao.DefinirConexao(comando);

			comando.Connection
				.Should()
				.NotBeNull();

			conexao.Dispose();
		}

		[TestMethod]
		public void SeDefinirUmaConexaoNumCommandSemTransacaoIniciadaDeveGerarErroEspecifico()
		{
			var conexao = CriarConexao();

			var comando = new SqlCommand();

			Action definirConexao = () => conexao.DefinirConexao(comando);

			definirConexao
				.ShouldThrow<TransacaoNaoIniciadaException>();
		}

	}

}
