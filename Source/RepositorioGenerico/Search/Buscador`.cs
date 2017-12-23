using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search.Conversores;

namespace RepositorioGenerico.Search
{
	public class Buscador<TObjeto> : IBuscador<TObjeto>
	{

		private readonly IComando _comando;
		private readonly Dicionario _dicionario;
		private readonly IQueryBuilder _queryBuilder;
		private readonly IRelacionamentoBuilder _relacionamentoBuilder;

		public Buscador(IComando comando, Dicionario dicionario, IQueryBuilder queryBuilder, IRelacionamentoBuilder relacionamentoBuilder)
		{
			_comando = comando;
			_dicionario = dicionario;
			_queryBuilder = queryBuilder;
			_relacionamentoBuilder = relacionamentoBuilder;
		}

		public IConfiguracaoProcedure<TObjeto> CriarProcedure()
		{
			var procedure = new ConfiguradorProcedure<TObjeto>(_comando.CriarComando(), _dicionario);
			procedure.DefinirProcedure();
			return procedure;
		}

		public IConfiguracaoProcedure<TObjeto> CriarProcedure(string nome)
		{
			var procedure = new ConfiguradorProcedure<TObjeto>(_comando.CriarComando(), _dicionario);
			procedure.DefinirProcedure(nome);
			return procedure;
		}

		public IConfiguracaoQuery<TObjeto> CriarQuery()
		{
			var query = new ConfiguradorQuery<TObjeto>(_comando.CriarComando(), _dicionario, _queryBuilder.Criar());
			query.DefinirTabela();
			return query;
		}

		public IEnumerable<TObjeto> Todos()
		{
			return Varios(CriarQuery());
		}

		public IEnumerable<TObjeto> Varios(IConfiguracao<TObjeto> configuracao)
		{
			var loader = new BuscadorLoader<TObjeto>(_comando, _dicionario, _relacionamentoBuilder);
			var dadosVinculados = loader.CarregarPropriedadesVinculadas(configuracao);
			return ConverterRegistrosEmLista(configuracao, loader, dadosVinculados, _comando.ConsultarRegistro(configuracao));
		}

		private IEnumerable<TObjeto> ConverterRegistrosEmLista(IConfiguracao<TObjeto> configuracao, BuscadorLoader<TObjeto> loader, IList<IList<object>> dadosVinculados, IDataReader reader)
		{
			var conversor = Conversor.ConverterDataReaderParaObjeto<TObjeto>(reader);
			foreach (var registro in conversor)
			{
				loader.CarregarPropriedadesVinculadasAoModel(configuracao, registro, dadosVinculados);
				yield return registro;
			}
		}

		public TObjeto Um(IConfiguracao<TObjeto> configuracao)
		{
			var configuracaoQuery = configuracao as IConfiguracaoQuery<TObjeto>;
			DefinirTopUm(configuracaoQuery);
			foreach (var registro in Varios(configuracao))
				return registro;
			return default(TObjeto);
		}

		protected void DefinirTopUm(IConfiguracaoQuery<TObjeto> configuracao)
		{
			if (configuracao == null)
				return;
			configuracao.DefinirLimite(1);
		}

		public bool Existe(IConfiguracao<TObjeto> configuracao)
		{
			return _comando.Existe(configuracao);
		}

		public TEstadoObjeto ConsultarPropriedade<TEstadoObjeto>(TObjeto objeto,
			Expression<Func<TObjeto, TEstadoObjeto>> propriedade) where TEstadoObjeto : class, IEntidade
		{
			return new BuscadorLoader<TObjeto>(_comando, _dicionario, _relacionamentoBuilder).ConsultarPropriedade(this, objeto, propriedade);
		}

		public ICollection<TEstadoObjeto> ConsultarPropriedade<TEstadoObjeto>(TObjeto objeto,
			Expression<Func<TObjeto, ICollection<TEstadoObjeto>>> propriedade) where TEstadoObjeto : class, IEntidade
		{
			return new BuscadorLoader<TObjeto>(_comando, _dicionario, _relacionamentoBuilder).ConsultarPropriedade(this, objeto, propriedade);
		}

		public ICollection ConsultarPropriedade(TObjeto objeto, LambdaExpression propriedade)
		{
			return new BuscadorLoader<TObjeto>(_comando, _dicionario, _relacionamentoBuilder).ConsultarPropriedade(this, objeto, propriedade);
		}

		public object Scalar(IConfiguracao<TObjeto> configuracao)
		{
			return _comando.Scalar(configuracao);
		}

		public int NonQuery(IConfiguracao<TObjeto> configuracao)
		{
			return _comando.NonQuery(configuracao);
		}

	}

}
