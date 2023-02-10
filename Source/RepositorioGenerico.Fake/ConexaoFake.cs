using System.Data;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.Fake
{
	public class ConexaoFake : IConexao
	{

		private TransacaoFake _transacaoFake;
		private bool _transacaoExterna;

		public bool EmTransacao
		{
			get { return (_transacaoFake != null); }
		}

		public bool RealizaConsultasUtilizandoConexaoTransacionada { get; set; } = true;

		protected DataSet BancoDeDadosVirtual { get; private set; }

		public EventoDelegate AntesIniciarTransacao { get; set; }

		public EventoDelegate DepoisIniciarTransacao { get; set; }

		public ConexaoFake()
		{
			BancoDeDadosVirtual = new DataSet();
		}

		public ConexaoFake(IDbTransaction transacao) : this()
		{
			DefinirUmaTransacaoEspecifica(transacao);
		}

		public void DefinirUmaTransacaoEspecifica(IDbTransaction transacao)
		{
			if (EmTransacao)
				throw new TransacaoJaIniciadaException();
			_transacaoFake = new TransacaoFake(transacao);
			_transacaoExterna = true;
		}

		public IDbConnection CriarConexaoSemTransacao()
		{
			var conexao = new DbConnectionFake();
			conexao.Open();
			return conexao;
		}

		public ITransacao IniciarTransacao()
		{
			if (_transacaoExterna || EmTransacao)
				throw new TransacaoJaIniciadaException();
			try
			{
				ExecutarEventoTransacao(AntesIniciarTransacao);
				_transacaoFake = new TransacaoFake(CriarConexaoSemTransacao());
				_transacaoFake.DepoisLimparTransacao += DepoisLimparTransacao;
				return _transacaoFake;
			}
			finally
			{
				ExecutarEventoTransacao(DepoisIniciarTransacao);
			}
		}

		private void ExecutarEventoTransacao(EventoDelegate eventoConexao)
		{
			if (eventoConexao == null)
				return;
			eventoConexao(this);
		}

		private void DepoisLimparTransacao(object sender)
		{
			_transacaoFake.DepoisLimparTransacao -= DepoisLimparTransacao;
			_transacaoFake.Dispose();
			_transacaoFake = null;
		}

		public void DefinirConexaoTransacionada(IDbCommand comando)
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			comando.Connection = _transacaoFake.ConexaoAtual;
			comando.Transaction = _transacaoFake.TransacaoAtual;
		}

		public IDbCommand CriarComando()
		{
			return new DbCommandFake(BancoDeDadosVirtual);
		}

		public void Dispose()
		{
			if (!_transacaoExterna && EmTransacao)
				_transacaoFake.Dispose();
			BancoDeDadosVirtual.Dispose();
		}

	}
}
