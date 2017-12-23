using RepositorioGenerico.Search;
using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Test.Objetos
{
	public class QueryBuilderParaTeste : IQueryBuilder
	{

		private string _script;

		public void DefinirScriptParaTeste(string script)
		{
			_script = script;
		}

		public void AdicionarAgrupamento(string agrupamento)
		{
		}

		public string AdicionarCondicao(string campo, int operador, object valor)
		{
			return string.Empty;
		}

		public void AdicionarCondicaoAgrupamento(string condicao)
		{
		}

		public void AdicionarCondicaoApenasCampoNaoNulo(string campo)
		{
		}

		public string[] AdicionarCondicaoEntre(string campo, object inicio, object fim)
		{
			return null;
		}

		public void AdicionarCondicaoPersonalizada(string condicao)
		{
		}

		public void AdicionarOrdem(string ordem)
		{
		}

		public void AdicionarOrdemDescendente(string ordem)
		{
		}

		public void AdicionarRelacionamento(string relacionamento)
		{
		}

		public void AdicionarResultado(string campo)
		{
		}

		public void AdicionarResultadoAgregado(Agregadores agregador, string campo)
		{
		}

		public void AdicionarResultadoPersonalizado(string resultado)
		{
		}

		public IQueryBuilder Criar()
		{
			return this;
		}

		public void DefinirLimite(int limite)
		{
		}

		public void DefinirTabela(string nome)
		{
		}

		public string GerarScript(Dicionario dicionario)
		{
			return _script;
		}

		public string GerarScriptExistencia(Dicionario dicionario)
		{
			return _script;
		}

	}
}
