using System;
using System.Data;

namespace RepositorioGenerico.Pattern
{

	public interface IConexao : ITransacional, IDisposable
	{

		bool RealizaConsultasUtilizandoConexaoTransacionada { get; set; }

		IDbConnection CriarConexaoSemTransacao();

		IDbCommand CriarComando();

		void DefinirConexaoTransacionada(IDbCommand comando);

	}

}
