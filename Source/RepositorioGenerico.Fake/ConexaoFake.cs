using System;
using System.Data;
using Moq;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.Fake
{
	public class ConexaoFake : IConexao, ITransacao, IDisposable, IEventoConexao
	{

		private TransacaoFake _transacaoFake;

		protected DataSet BancoDeDadosVirtual { get; private set; }

		public EventoConexaoDelegate AntesIniciarTransacao { get; set; }

		public EventoConexaoDelegate DepoisIniciarTransacao { get; set; }

		public EventoConexaoDelegate AntesConfirmarTransacao { get; set; }

		public EventoConexaoDelegate DepoisConfirmarTransacao { get; set; }

		public EventoConexaoDelegate AntesCancelarTransacao { get; set; }

		public EventoConexaoDelegate DepoisCancelarTransacao { get; set; }

		public bool EmTransacao
		{
			get { return (_transacaoFake != null); }
		}

		public ConexaoFake()
		{
			BancoDeDadosVirtual = new DataSet();
		}

		public IDbConnection CriarConexaoTransacionada()
		{
			IniciarTransacao();
			return _transacaoFake.ConexaoAtual;
		}

		public IDbConnection CriarConexaoSemTransacao()
		{
			var mockConexao = new Mock<IDbConnection>();
			var transacao = new Mock<IDbTransaction>();
			mockConexao.Setup(c => c.BeginTransaction()).Returns(transacao.Object);
			mockConexao.Setup(c => c.State).Returns(ConnectionState.Open);
			var conexao = mockConexao.Object;
			conexao.Open();
			return conexao;
		}

		public void DefinirConexao(IDbCommand comando)
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			comando.Connection = _transacaoFake.ConexaoAtual;
			comando.Transaction = _transacaoFake.TransacaoAtual;
		}

		public void IniciarTransacao()
		{
			if (EmTransacao)
				throw new TransacaoJaIniciadaException();
			ExecutarEventoTransacao(AntesIniciarTransacao);
			_transacaoFake = new TransacaoFake(CriarConexaoSemTransacao());
			_transacaoFake.IniciarTransacao();
			ExecutarEventoTransacao(DepoisIniciarTransacao);
		}

		private void ExecutarEventoTransacao(EventoConexaoDelegate eventoConexao)
		{
			if (eventoConexao == null)
				return;
			eventoConexao();
		}

		public IDbCommand CriarComando()
		{
			return new DbCommandFake(BancoDeDadosVirtual);
		}

		public void ConfirmarTransacao()
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			ExecutarEventoTransacao(AntesConfirmarTransacao);
			_transacaoFake.ConfirmarTransacao();
			ExecutarEventoTransacao(DepoisConfirmarTransacao);
			LimparTransacao();
		}

		private void LimparTransacao()
		{
			_transacaoFake.Dispose();
			_transacaoFake = null;
		}

		public void CancelarTransacao()
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			ExecutarEventoTransacao(AntesCancelarTransacao);
			_transacaoFake.CancelarTransacao();
			ExecutarEventoTransacao(DepoisCancelarTransacao);
			LimparTransacao();
		}

		public void Dispose()
		{
			BancoDeDadosVirtual.Dispose();
			if (EmTransacao)
				CancelarTransacao();
		}

	}
}
