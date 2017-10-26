using System;
using System.Data;
using System.Data.SqlClient;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.SqlClient
{
	public class Conexao : IConexao, ITransacao, IDisposable, IEventoConexao
	{

		private readonly string _stringConexao;
		private Transacao _transacao;

		public EventoConexaoDelegate AntesIniciarTransacao { get; set; }

		public EventoConexaoDelegate DepoisIniciarTransacao { get; set; }

		public EventoConexaoDelegate AntesConfirmarTransacao { get; set; }

		public EventoConexaoDelegate DepoisConfirmarTransacao { get; set; }

		public EventoConexaoDelegate AntesCancelarTransacao { get; set; }

		public EventoConexaoDelegate DepoisCancelarTransacao { get; set; }

		public bool EmTransacao
		{
			get { return (_transacao != null); }
		}

		public Conexao(string stringConexao)
		{
			_stringConexao = stringConexao;
		}

		public IDbConnection CriarConexaoSemTransacao()
		{
			var conexao = new SqlConnection(_stringConexao);
			conexao.Open();
			return conexao;
		}

		public void DefinirConexao(IDbCommand comando)
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			comando.Connection = _transacao.ConexaoAtual;
			comando.Transaction = _transacao.TransacaoAtual;
		}

		public IDbConnection CriarConexaoTransacionada()
		{
			IniciarTransacao();
			return _transacao.ConexaoAtual;
		}

		public void IniciarTransacao()
		{
			if (EmTransacao)
				throw new TransacaoJaIniciadaException();
			ExecutarEventoTransacao(AntesIniciarTransacao);
			_transacao = new Transacao(CriarConexaoSemTransacao());
			_transacao.IniciarTransacao();
			ExecutarEventoTransacao(DepoisIniciarTransacao);
		}

		private void ExecutarEventoTransacao(EventoConexaoDelegate eventoConexao)
		{
			if (eventoConexao != null)
				eventoConexao();
		}

		public IDbCommand CriarComando()
		{
			return new SqlCommand();
		}

		public void ConfirmarTransacao()
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			ExecutarEventoTransacao(AntesConfirmarTransacao);
			_transacao.ConfirmarTransacao();
			LimparTransacao();
			ExecutarEventoTransacao(DepoisConfirmarTransacao);
		}

		private void LimparTransacao()
		{
			_transacao.Dispose();
			_transacao = null;
		}

		public void CancelarTransacao()
		{
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			ExecutarEventoTransacao(AntesCancelarTransacao);
			_transacao.CancelarTransacao();
			LimparTransacao();
			ExecutarEventoTransacao(DepoisCancelarTransacao);
		}

		public void Dispose()
		{
			if (EmTransacao)
				CancelarTransacao();
		}

	}
}
