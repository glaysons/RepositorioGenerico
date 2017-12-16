using System;
using System.Data;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.SqlClient
{
	public class Transacao : ITransacao, IDisposable
	{

		private readonly IDbConnection _conexao;
		private IDbTransaction _transacao;
		private bool _transacaoExterna;

		public IDbConnection ConexaoAtual
		{
			get { return _conexao; }
		}

		public IDbTransaction TransacaoAtual
		{
			get { return _transacao; }
		}

		public bool EmTransacao
		{
			get { return (_transacao != null); }
		}

		public Transacao(IDbConnection conexao)
		{
			_conexao = conexao;
			_transacaoExterna = false;
		}

		public Transacao(IDbTransaction transacao)
		{
			_conexao = transacao.Connection;
			_transacao = transacao;
			_transacaoExterna = true;
		}

		public void IniciarTransacao()
		{
			if (_transacaoExterna || EmTransacao)
				throw new TransacaoJaIniciadaException();
			_transacao = _conexao.BeginTransaction();
		}

		public void ConfirmarTransacao()
		{
			if (_transacaoExterna)
				throw new NaoEhPossivelConfirmarOuCancelarTransacaoExternaException();
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			_transacao.Commit();
			_conexao.Close();
			LimparTransacao();
		}

		private void LimparTransacao()
		{
			_transacao.Dispose();
			_transacao = null;
		}

		public void CancelarTransacao()
		{
			if (_transacaoExterna)
				throw new NaoEhPossivelConfirmarOuCancelarTransacaoExternaException();
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			_transacao.Rollback();
			_conexao.Close();
			LimparTransacao();
		}

		public void Dispose()
		{
			if (!_transacaoExterna && EmTransacao)
				CancelarTransacao();
		}

	}
}
