using System.Data;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Pattern
{
	public interface IComando
	{

		IDbCommand CriarComando();

		DataTable ConsultarTabela(IConfiguracao configuracao);

		IDataReader ConsultarRegistro(IConfiguracao configuracao);
		IDataReader ConsultarRegistro<TObjeto>(IConfiguracao<TObjeto> configuracao);

		object Scalar(IConfiguracao configuracao);
		object Scalar<TObjeto>(IConfiguracao<TObjeto> configuracao);

		int NonQuery(IConfiguracao configuracao);
		int NonQuery<TObjeto>(IConfiguracao<TObjeto> configuracao);

		bool Existe(IConfiguracao configuracao);
		bool Existe<TObjeto>(IConfiguracao<TObjeto> configuracao);

	}
}
