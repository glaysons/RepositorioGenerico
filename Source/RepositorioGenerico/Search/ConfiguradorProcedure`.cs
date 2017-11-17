using System.Data;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public class ConfiguradorProcedure<TObjeto> : Configurador<TObjeto>, IConfiguracaoProcedure<TObjeto>
	{

		private string _nomeProcedure;

		public ConfiguradorProcedure(IDbCommand comando, Dicionario dicionario)
			: base(comando, dicionario)
		{

		}

		public IConfiguracaoProcedure<TObjeto> DefinirProcedure()
		{
			_nomeProcedure = DataAnnotationHelper.ConsultarNomeDaTabela(typeof(TObjeto));
			return this;
		}

		public IConfiguracaoProcedure<TObjeto> DefinirProcedure(string nome)
		{
			_nomeProcedure = nome;
			return this;
		}

		public override void Preparar()
		{
			Comando.CommandType = CommandType.StoredProcedure;
			Comando.CommandText = _nomeProcedure;
			Comando.CommandTimeout = 0;
		}

	}
}
