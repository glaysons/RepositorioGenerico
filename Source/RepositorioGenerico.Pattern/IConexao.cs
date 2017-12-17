using System;
using System.Data;

namespace RepositorioGenerico.Pattern
{

	public interface IConexao : ITransacional, IDisposable
	{

		IDbConnection CriarConexaoSemTransacao();

		IDbCommand CriarComando();

		void DefinirConexaoTransacionada(IDbCommand comando);

	}

}
