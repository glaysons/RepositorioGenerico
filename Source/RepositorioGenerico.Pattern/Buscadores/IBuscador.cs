using System.Collections.Generic;
using System.Data;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IBuscador
	{

		IConfiguracaoProcedure CriarProcedure(string nome);

		IConfiguracaoQuery CriarQuery(string tabela);

		DataTable Todos(string tabela);

		DataTable Varios(IConfiguracao configuracao);

		DataRow Um(IConfiguracao configuracao);

		IEnumerable<TObjeto> Varios<TObjeto>(IConfiguracao configuracao);

		IEnumerable<IDataRecord> Registros(IConfiguracao configuracao);

		TObjeto Um<TObjeto>(IConfiguracao configuracao);

		object Scalar(IConfiguracao configuracao);

		int NonQuery(IConfiguracao configuracao);

		bool Existe(IConfiguracao configuracao);

	}
}
