using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public interface IQueryBuilder
	{

		IQueryBuilder Criar();

		void DefinirLimite(int limite);
		void DefinirTabela(string nome);

		void AdicionarResultado(string campo);
		void AdicionarResultadoAgregado(Agregadores agregador, string campo);
		void AdicionarResultadoPersonalizado(string resultado);
	
		void AdicionarRelacionamento(string relacionamento);
		
		string AdicionarCondicao(string campo, int operador, object valor);
		string[] AdicionarCondicaoEntre(string campo, object inicio, object fim);
		void AdicionarCondicaoApenasCampoNaoNulo(string campo);
		void AdicionarCondicaoPersonalizada(string condicao);

		void AdicionarAgrupamento(string agrupamento);
		void AdicionarCondicaoAgrupamento(string condicao);
		void AdicionarOrdem(string ordem);
		void AdicionarOrdemDescendente(string ordem);

		string GerarScript(Dicionario dicionario);
		string GerarScriptExistencia(Dicionario dicionario);
	}
}