using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoQuery<TObjeto> : IConfiguracao<TObjeto>
	{
		IConfiguracaoQuery<TObjeto> CarregarPropriedade<TEstadoObjeto>(Expression<Func<TObjeto, TEstadoObjeto>> propriedade) where TEstadoObjeto : class, IEntidade;
		IConfiguracaoQuery<TObjeto> CarregarPropriedade<TEstadoObjeto>(Expression<Func<TObjeto, ICollection<TEstadoObjeto>>> propriedade) where TEstadoObjeto : class, IEntidade;
		IConfiguracaoQuery<TObjeto> AdicionarResultado(Expression<Func<TObjeto, object>> campo);
		IConfiguracaoQuery<TObjeto> AdicionarResultadoAgregado(Agregadores agregador);
		IConfiguracaoQuery<TObjeto> AdicionarResultadoAgregado(Agregadores agregador, Expression<Func<TObjeto, object>> campo);
		IConfiguracaoQuery<TObjeto> AdicionarResultadoPersonalizado(string resultado);
		IConfiguracaoQuery<TObjeto> DefinirTabela();
		IConfiguracaoQuery<TObjeto> DefinirLimite(int maximo);
		IConfiguracaoQuery<TObjeto> AdicionarRelacionamento(string relacionamento);
		IConfiguracaoCondicao<TObjeto> AdicionarCondicao(Expression<Func<TObjeto, object>> campo);
		IConfiguracaoQuery<TObjeto> AdicionarCondicaoPersonalizada(string condicao);
		IConfiguracaoQuery<TObjeto> AdicionarAgrupamento(Expression<Func<TObjeto, object>> campo);
		IConfiguracaoQuery<TObjeto> AdicionarCondicaoAgrupamento(string condicao);
		IConfiguracaoQuery<TObjeto> AdicionarOrdem(Expression<Func<TObjeto, object>> campo);
		IConfiguracaoQuery<TObjeto> AdicionarOrdemDescendente(Expression<Func<TObjeto, object>> campo);
	}
}
