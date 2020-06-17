using System;
using System.Data;
using System.Data.SqlClient;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.SqlClient
{
	public class Conexao : IConexao
	{

		private readonly string _stringConexao;
		private IDbConnection _conexao;
		private Transacao _transacao;
		private bool _transacaoExterna;

		public bool EmTransacao
		{
			get { return (_transacao != null); }
		}

		public EventoDelegate AntesIniciarTransacao { get; set; }

		public EventoDelegate DepoisIniciarTransacao { get; set; }

		public Conexao(string stringConexao)
		{
			_stringConexao = stringConexao;
			_transacaoExterna = false;
		}

		public Conexao(string stringConexao, IDbTransaction transacao) : this(stringConexao)
		{
			_transacao = new Transacao(transacao);
			_transacaoExterna = true;
		}

		public IDbConnection CriarConexaoSemTransacao()
		{
			var conexao = new SqlConnection(_stringConexao);
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
				_conexao = CriarConexaoSemTransacao();
				_transacao = new Transacao(_conexao);
				_transacao.DepoisLimparTransacao += DepoisLimparTransacao;
				return _transacao;
			}
			finally
			{
				ExecutarEventoTransacao(DepoisIniciarTransacao);
			}
		}

		private void ExecutarEventoTransacao(EventoDelegate eventoConexao)
		{
			eventoConexao?.Invoke(this);
		}

		private void DepoisLimparTransacao(object sender)
		{
			_transacao.DepoisLimparTransacao -= DepoisLimparTransacao;
			_transacao.Dispose();
			_transacao = null;
			LimparConexaoExistente();
		}

		private void LimparConexaoExistente()
		{
			if (_conexao == null)
				return;
			_conexao.Close();
			_conexao.Dispose();
		}

		public void DefinirConexaoTransacionada(IDbCommand comando)
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			comando.Connection = _transacao.ConexaoAtual;
			comando.Transaction = _transacao.TransacaoAtual;
		}

		public IDbCommand CriarComando()
		{
			return new SqlCommand();
		}

		public void Dispose()
		{
			if (!_transacaoExterna && EmTransacao)
			{
				_transacao.Dispose();
				LimparConexaoExistente();
			}
		}

	}
}
