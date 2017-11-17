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
		}

		public void IniciarTransacao()
		{
			if (EmTransacao)
				throw new TransacaoJaIniciadaException();
			_transacao = _conexao.BeginTransaction();
		}

		public void ConfirmarTransacao()
		{
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
			if (!EmTransacao)
				throw new TransacaoNaoIniciadaException();
			_transacao.Rollback();
			_conexao.Close();
			LimparTransacao();
		}

		public void Dispose()
		{
			if (EmTransacao)
				CancelarTransacao();
			_conexao.Dispose();
		}

	}
}
