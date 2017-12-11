using System;
using System.Data;

namespace RepositorioGenerico.Fake
{
	internal class DbTransactionFake : IDbTransaction
	{

		private bool _emTransacao;

		public IDbConnection Connection { get; private set; }

		public IsolationLevel IsolationLevel { get; private set; }

		public DbTransactionFake(IDbConnection conexao, IsolationLevel isolamento)
		{
			Connection = conexao;
			IsolationLevel = isolamento;
			_emTransacao = true;
		}

		public void Commit()
		{
			Validar();
			_emTransacao = false;
		}

		private void Validar()
		{
			if (Connection.State != ConnectionState.Open || !_emTransacao)
				throw new InvalidOperationException();
		}

		public void Rollback()
		{
			Validar();
			_emTransacao = false;
		}

		public void Dispose()
		{
		}

	}
}
