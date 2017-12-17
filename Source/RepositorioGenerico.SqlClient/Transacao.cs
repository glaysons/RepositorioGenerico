using System.Data;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.SqlClient
{
	public class Transacao : ITransacao
	{

		private readonly IDbConnection _conexao;
		private IDbTransaction _transacao;
		private readonly bool _transacaoExterna;

		public IDbConnection ConexaoAtual
		{
			get { return _conexao; }
		}

		public IDbTransaction TransacaoAtual
		{
			get { return _transacao; }
		}

		private bool EmTransacao
		{
			get { return (_transacao != null); }
		}

		public EventoDelegate AntesConfirmarTransacao { get; set; }

		public EventoDelegate DepoisConfirmarTransacao { get; set; }

		public EventoDelegate AntesCancelarTransacao { get; set; }

		public EventoDelegate DepoisCancelarTransacao { get; set; }

		internal EventoDelegate DepoisLimparTransacao { get; set; }

		public Transacao(IDbConnection conexao)
		{
			_conexao = conexao;
			_transacao = _conexao.BeginTransaction();
			_transacaoExterna = false;
		}

		public Transacao(IDbTransaction transacao)
		{
			_conexao = transacao.Connection;
			_transacao = transacao;
			_transacaoExterna = true;
		}

		public void ConfirmarTransacao()
		{
			if (_transacaoExterna)
				throw new NaoEhPossivelConfirmarOuCancelarTransacaoExternaException();
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			ExecutarEventoTransacao(AntesConfirmarTransacao);
			_transacao.Commit();
			try
			{
				ExecutarEventoTransacao(DepoisConfirmarTransacao);
			}
			finally
			{
				_conexao.Close();
				LimparTransacao();
			}
		}

		private void ExecutarEventoTransacao(EventoDelegate eventoConexao)
		{
			if (eventoConexao != null)
				eventoConexao(this);
		}

		private void LimparTransacao()
		{
			_transacao.Dispose();
			_transacao = null;
			ExecutarEventoTransacao(DepoisLimparTransacao);
		}

		public void CancelarTransacao()
		{
			if (_transacaoExterna)
				throw new NaoEhPossivelConfirmarOuCancelarTransacaoExternaException();
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			ExecutarEventoTransacao(AntesCancelarTransacao);
			_transacao.Rollback();
			try
			{
				ExecutarEventoTransacao(DepoisCancelarTransacao);
			}
			finally
			{
				_conexao.Close();
				LimparTransacao();
			}
		}

		public void Dispose()
		{
			if (!_transacaoExterna && EmTransacao)
				CancelarTransacao();
		}

	}
}
