namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoProcedure<TObjeto> : IConfiguracao<TObjeto>
	{

		IConfiguracaoProcedure<TObjeto> DefinirProcedure();

		IConfiguracaoProcedure<TObjeto> DefinirProcedure(string nome);

	}
}
