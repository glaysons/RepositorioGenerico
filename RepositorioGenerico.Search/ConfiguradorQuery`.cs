using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public class ConfiguradorQuery<TObjeto> : Configurador<TObjeto>, IConfiguracaoQuery<TObjeto>
	{

		private readonly IQueryBuilder _queryBuilder;
		private readonly Dicionario _dicionario;
		private IList<PropertyInfo> _propriedadesAcescentes;
		private IList<PropertyInfo> _propriedadesDescendentes;

		private IList<PropertyInfo> ListaPropriedadesAcescentes
		{
			get { return _propriedadesAcescentes ?? (_propriedadesAcescentes = new List<PropertyInfo>()); }
		}

		private IList<PropertyInfo> ListaPropriedadesDescendentes
		{
			get { return _propriedadesDescendentes ?? (_propriedadesDescendentes = new List<PropertyInfo>()); }
		}

		public bool PossuiPropriedadesCarregadas
		{
			get { return (PropriedadesAcescentes != null) || (PropriedadesDescendentes != null); }
		}

		public IList<PropertyInfo> PropriedadesAcescentes
		{
			get { return _propriedadesAcescentes; }
		}

		public IList<PropertyInfo> PropriedadesDescendentes
		{
			get { return _propriedadesDescendentes; }
		}

		public ConfiguradorQuery(IDbCommand comando, Dicionario dicionario, IQueryBuilder queryBuilder)
			: base(comando, dicionario)
		{
			_dicionario = dicionario;
			_queryBuilder = queryBuilder;
		}

		public IConfiguracaoQuery<TObjeto> CarregarPropriedade<TEstadoObjeto>(Expression<Func<TObjeto, TEstadoObjeto>> propriedadeAscendente) where TEstadoObjeto : class, IEntidade
		{
			ListaPropriedadesAcescentes.Add(ExpressionHelper.PropriedadeDaExpressao(propriedadeAscendente));
			return this;
		}

		public IConfiguracaoQuery<TObjeto> CarregarPropriedade<TEstadoObjeto>(Expression<Func<TObjeto, ICollection<TEstadoObjeto>>> propriedadeDescendente) where TEstadoObjeto : class, IEntidade
		{
			ListaPropriedadesDescendentes.Add(ExpressionHelper.PropriedadeDaExpressao(propriedadeDescendente));
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarResultado(Expression<Func<TObjeto, object>> campo)
		{
			_queryBuilder.AdicionarResultado(ConsultarNomeDaExpressao(campo));
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarResultadoAgregado(Agregadores agregador)
		{
			_queryBuilder.AdicionarResultadoAgregado(agregador, null);
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarResultadoAgregado(Agregadores agregador, Expression<Func<TObjeto, object>> campo)
		{
			_queryBuilder.AdicionarResultadoAgregado(agregador, ConsultarNomeDaExpressao(campo));
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarResultadoPersonalizado(string resultado)
		{
			_queryBuilder.AdicionarResultadoPersonalizado(resultado);
			return this;
		}

		public IConfiguracaoQuery<TObjeto> DefinirTabela()
		{
			_queryBuilder.DefinirTabela(_dicionario.Nome);
			return this;
		}

		public IConfiguracaoQuery<TObjeto> DefinirLimite(int maximo)
		{
			_queryBuilder.DefinirLimite(maximo);
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarRelacionamento(string relacionamento)
		{
			_queryBuilder.AdicionarRelacionamento(relacionamento);
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarCondicaoPersonalizada(string condicao)
		{
			_queryBuilder.AdicionarCondicaoPersonalizada(condicao);
			return this;
		}

		public IConfiguracaoCondicao<TObjeto> AdicionarCondicao(Expression<Func<TObjeto, object>> campo)
		{
			return new ConfiguradorCondicao<TObjeto>(this, ConsultarNomeDaExpressao(campo), _queryBuilder);
		}

		public IConfiguracaoQuery<TObjeto> AdicionarAgrupamento(Expression<Func<TObjeto, object>> campo)
		{
			_queryBuilder.AdicionarAgrupamento(ConsultarNomeDaExpressao(campo));
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarCondicaoAgrupamento(string condicao)
		{
			_queryBuilder.AdicionarCondicaoAgrupamento(condicao);
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarOrdem(Expression<Func<TObjeto, object>> campo)
		{
			_queryBuilder.AdicionarOrdem(ConsultarNomeDaExpressao(campo));
			return this;
		}

		public IConfiguracaoQuery<TObjeto> AdicionarOrdemDescendente(Expression<Func<TObjeto, object>> campo)
		{
			_queryBuilder.AdicionarOrdem(ConsultarNomeDaExpressao(campo) + " desc");
			return this;
		}

		public override void Preparar()
		{
			Comando.CommandType = CommandType.Text;
			Comando.CommandTimeout = 0;
			if (string.IsNullOrEmpty(ScriptPersonalizado))
				Comando.CommandText = _queryBuilder.GerarScript(_dicionario);
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

	}
}
