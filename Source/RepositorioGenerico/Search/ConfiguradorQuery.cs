using System;
using System.Data;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{

	public class ConfiguradorQuery : Configurador, IConfiguracaoQuery, IDisposable
	{

		private readonly IQueryBuilder _queryBuilder;

		public ConfiguradorQuery(IDbCommand comando, IQueryBuilder queryBuilder)
			: base(comando)
		{
			_queryBuilder = queryBuilder;
		}

		public IConfiguracaoQuery DefinirLimite(int maximo)
		{
			_queryBuilder.DefinirLimite(maximo);
			return this;
		}

		public IConfiguracaoCondicao AdicionarCondicao(string campo)
		{
			return new ConfiguradorCondicao(this, campo, _queryBuilder);
		}

		public IConfiguracaoQuery AdicionarAgrupamento(string agrupamento)
		{
			_queryBuilder.AdicionarAgrupamento(agrupamento);
			return this;
		}

		public IConfiguracaoQuery AdicionarCondicaoAgrupamento(string condicao)
		{
			_queryBuilder.AdicionarCondicaoAgrupamento(condicao);
			return this;
		}

		public IConfiguracaoQuery AdicionarCondicaoPersonalizada(string condicao)
		{
			_queryBuilder.AdicionarCondicaoPersonalizada(condicao);
			return this;
		}

		public IConfiguracaoQuery AdicionarOrdem(string ordem)
		{
			_queryBuilder.AdicionarOrdem(ordem);
			return this;
		}

		public IConfiguracaoQuery AdicionarOrdemDescendente(string ordem)
		{
			_queryBuilder.AdicionarOrdem(ordem);
			return this;
		}

		public IConfiguracaoQuery AdicionarRelacionamento(string relacionamento)
		{
			_queryBuilder.AdicionarRelacionamento(relacionamento);
			return this;
		}

		public IConfiguracaoQuery AdicionarResultado(string resultado)
		{
			_queryBuilder.AdicionarResultadoPersonalizado(resultado);
			return this;
		}

		public IConfiguracaoQuery DefinirTabela(string nome)
		{
			_queryBuilder.DefinirTabela(nome);
			return this;
		}

		public override void Preparar()
		{
			Comando.CommandType = CommandType.Text;
			Comando.CommandTimeout = 0;
			if (string.IsNullOrEmpty(ScriptPersonalizado))
				Comando.CommandText = _queryBuilder.GerarScript(null);
			else
			{
				Comando.CommandText = ScriptPersonalizado;
				ScriptPersonalizado = null;
			}
		}

		public override void PrepararExistencia()
		{
			Comando.CommandType = CommandType.Text;
			Comando.CommandTimeout = 0;
			Comando.CommandText = _queryBuilder.GerarScriptExistencia(null);
		}

		public void Dispose()
		{
			Comando.Dispose();
		}
	}
}
