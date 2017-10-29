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

		object Scalar(IConfiguracao configuracao);

		int NonQuery(IConfiguracao configuracao);

		bool Existe(IConfiguracao configuracao);

	}
}
