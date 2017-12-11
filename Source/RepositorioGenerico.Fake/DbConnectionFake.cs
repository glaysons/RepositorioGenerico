using System;
using System.Data;

namespace RepositorioGenerico.Fake
{
	internal class DbConnectionFake : IDbConnection
	{

		public string ConnectionString { get; set; }

		public int ConnectionTimeout { get; private set; }

		public string Database { get; private set; }

		public ConnectionState State { get; private set; }

		public DbConnectionFake()
		{
			State = ConnectionState.Closed;
		}

		public IDbTransaction BeginTransaction()
		{
			if (State != ConnectionState.Open)
				throw new InvalidOperationException();
			return BeginTransaction(IsolationLevel.Unspecified);
		}

		public IDbTransaction BeginTransaction(IsolationLevel il)
		{
			return new DbTransactionFake(this, il);
		}

		public void ChangeDatabase(string databaseName)
		{
			Database = databaseName;
		}

		public void Close()
		{
			if (State != ConnectionState.Open)
				throw new InvalidOperationException();
			State = ConnectionState.Closed;
		}

		public IDbCommand CreateCommand()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}

		public void Open()
		{
			if (State == ConnectionState.Open)
				throw new InvalidOperationException();
			State = ConnectionState.Open;
		}
	}
}
