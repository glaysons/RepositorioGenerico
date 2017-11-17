namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoQuery : IConfiguracao
	{

		IConfiguracaoQuery AdicionarResultado(string resultado);
		IConfiguracaoQuery DefinirTabela(string nome);
		IConfiguracaoQuery DefinirLimite(int maximo);
		IConfiguracaoQuery AdicionarRelacionamento(string relacionamento);
		IConfiguracaoCondicao AdicionarCondicao(string campo);
		IConfiguracaoQuery AdicionarCondicaoPersonalizada(string condicao);
		IConfiguracaoQuery AdicionarAgrupamento(string agrupamento);
		IConfiguracaoQuery AdicionarCondicaoAgrupamento(string condicao);
		IConfiguracaoQuery AdicionarOrdem(string ordem);
		IConfiguracaoQuery AdicionarOrdemDescendente(string campo);

	}
}
