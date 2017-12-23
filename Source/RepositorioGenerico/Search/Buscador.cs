using System.Data;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;
using System.Collections.Generic;
using RepositorioGenerico.Search.Conversores;

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

		public IEnumerable<TObjeto> Varios<TObjeto>(IConfiguracao configuracao)
		{
			var reader = _comando.ConsultarRegistro(configuracao);
			var conversor = Conversor.ConverterDataReaderParaObjeto<TObjeto>(reader);
			foreach (var registro in conversor)
				yield return registro;
		}

		public DataRow Um(IConfiguracao configuracao)
		{
			DefinirTopUm(configuracao as IConfiguracaoQuery);
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

		public TObjeto Um<TObjeto>(IConfiguracao configuracao)
		{
			DefinirTopUm(configuracao as IConfiguracaoQuery);
			foreach (var registro in Varios<TObjeto>(configuracao))
				return registro;
			return default(TObjeto);
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
