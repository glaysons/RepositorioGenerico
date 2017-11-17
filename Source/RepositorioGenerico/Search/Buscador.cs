using System.Data;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public class Buscador : IBuscador
	{

		private readonly IComando _comando;
		private readonly IQueryBuilder _queryBuilder;

		public Buscador(IComando comando, IQueryBuilder queryBuilder)
		{
			_comando = comando;
			_queryBuilder = queryBuilder;
		}

		public IConfiguracaoProcedure CriarProcedure(string nome)
		{
			var procedure = new ConfiguradorProcedure(_comando.CriarComando());
			procedure.DefinirProcedure(nome);
			return procedure;
		}

		public IConfiguracaoQuery CriarQuery(string tabela)
		{
			var query = new ConfiguradorQuery(_comando.CriarComando(), _queryBuilder.Criar());
			query.DefinirTabela(tabela);
			return query;
		}

		public DataTable Todos(string tabela)
		{
			return Varios(CriarQuery(tabela));
		}

		public DataTable Varios(IConfiguracao configuracao)
		{
			return _comando.ConsultarTabela(configuracao);
		}

		public DataRow Um(IConfiguracao configuracao)
		{
			var configuracaoQuery = configuracao as IConfiguracaoQuery;
			DefinirTopUm(configuracaoQuery);
			var tabela = _comando.ConsultarTabela(configuracao);
			return (tabela.Rows.Count == 0)
				? null
				: tabela.Rows[0];
		}

		private void DefinirTopUm(IConfiguracaoQuery configuracao)
		{
			if (configuracao == null)
				return;
			configuracao.DefinirLimite(1);
		}

		public object Scalar(IConfiguracao configuracao)
		{
			return _comando.Scalar(configuracao);
		}

		public int NonQuery(IConfiguracao configuracao)
		{
			return _comando.NonQuery(configuracao);
		}

		public bool Existe(IConfiguracao configuracao)
		{
			return _comando.Existe(configuracao);
		}

	}
}
