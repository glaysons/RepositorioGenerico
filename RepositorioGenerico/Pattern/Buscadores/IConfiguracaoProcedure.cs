namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoProcedure : IConfiguracao
	{

		IConfiguracaoProcedure DefinirProcedure(string nome);

	}
}
