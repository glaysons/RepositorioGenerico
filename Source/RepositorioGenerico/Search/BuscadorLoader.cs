using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Relacionamentos;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search.Conversores;

namespace RepositorioGenerico.Search
{
	public class BuscadorLoader<TObjeto> where TObjeto : IEntidade
	{

		private readonly IComando _comando;
		private readonly Dicionario _dicionario;
		private readonly IRelacionamentoBuilder _relacionamentoBuilder;

		public BuscadorLoader(IComando comando, Dicionario dicionario, IRelacionamentoBuilder relacionamentoBuilder)
		{
			_comando = comando;
			_dicionario = dicionario;
			_relacionamentoBuilder = relacionamentoBuilder;
		}

		public IList<IList<object>> CarregarPropriedadesVinculadas(IConfiguracao<TObjeto> configuracao) 
		{
			var config = configuracao as ConfiguradorQuery<TObjeto>;
			if ((config == null) || (!config.PossuiPropriedadesCarregadas))
				return null;
			var vinculos = new List<IList<object>>();
			CarregarPropriedadeVinculada(TiposRelacionamento.Ascendente, configuracao, config.PropriedadesAcescentes, vinculos);
			CarregarPropriedadeVinculada(TiposRelacionamento.Descendente, configuracao, config.PropriedadesDescendentes, vinculos);
			return vinculos;
		}

		private void CarregarPropriedadeVinculada(TiposRelacionamento tipo, IConfiguracao<TObjeto> configuracao, IList<PropertyInfo> propriedades, IList<IList<object>> vinculos)
		{
			var config = (ConfiguradorQuery<TObjeto>)configuracao;
			if (propriedades == null)
				return;
			foreach (var propriedade in propriedades)
				vinculos.Add(CarregarRelacionamentoVinculado(tipo, config, propriedade));
		}

		private IList<object> CarregarRelacionamentoVinculado(TiposRelacionamento tipo, Configurador<TObjeto> configurador, PropertyInfo propriedade)
		{
			var item = _dicionario.ConsultarPorPropriedade(propriedade.Name);
			configurador.Preparar();
			var scriptPadrao = configurador.ConsultarScript();
			if (item.Ligacao == null)
				throw new NaoFoiPossivelEncontrarALigacaoEntreOsCamposException();
			var scriptPersonalizado = (tipo == TiposRelacionamento.Ascendente)
				? _relacionamentoBuilder.CriarScriptConsultaRelacionamentoAscendente(item.Ligacao, scriptPadrao)
				: _relacionamentoBuilder.CriarScriptConsultaRelacionamentoDescendente(item.Ligacao, scriptPadrao, _dicionario.ConsultarCamposChave());
			configurador.PersonalizarScript(scriptPersonalizado);

			IDataReader reader;

			try
			{
				reader = _comando.ConsultarRegistro(configurador);
			}
			catch (Exception ex)
			{
				throw new NaoFoiPossivelConsultarRelacionamentoVinculadoException(_dicionario.AliasOuNome, tipo.ToString(), item.Ligacao.Dicionario.AliasOuNome, ex.Message);
			}

			var tipoModel = (tipo == TiposRelacionamento.Ascendente)
				? propriedade.PropertyType
				: propriedade.PropertyType.GetGenericArguments()[0];

			try
			{
				return Conversor.ConverterDataReaderParaObjeto(tipoModel, reader).Cast<object>().ToList();
			}
			finally
			{
				reader.Close();
			}
		}

		public void CarregarPropriedadesVinculadasAoModel(IConfiguracao<TObjeto> configuracao, TObjeto registro, IList<IList<object>> dadosVinculados)
		{
			if (dadosVinculados == null)
				return;
			var config = configuracao as ConfiguradorQuery<TObjeto>;
			if ((config == null) || (!config.PossuiPropriedadesCarregadas))
				return;
			var n = 0;
			if (config.PropriedadesAcescentes != null)
				foreach (var propriedade in config.PropriedadesAcescentes)
				{
					var ascendente = ConsularValorPropriedadeAscendente(registro, propriedade, dadosVinculados[n]);
					if (ascendente != null)
						propriedade.SetValue(registro, ascendente, null);
					n++;
				}
			if (config.PropriedadesDescendentes != null)
				foreach (var propriedade in config.PropriedadesDescendentes)
				{
					var descendentes = ConsultarPropriedadeDescendente(registro, propriedade, dadosVinculados[n]);
					if (descendentes != null)
						propriedade.SetValue(registro, descendentes, null);
					n++;
				}
		}

		private object ConsularValorPropriedadeAscendente(TObjeto registro, PropertyInfo propriedade, IList<object> dadosVinculados)
		{
			var item = _dicionario.ConsultarPorPropriedade(propriedade.Name);
			if ((item.Ligacao == null) || (item.Ligacao.Tipo != TiposRelacionamento.Ascendente))
				return null;
			var valor = _dicionario.ConsultarValoresDaChave(registro, item.Ligacao.ChaveEstrangeira);
			foreach (var ascendente in dadosVinculados)
			{
				var chave = item.Ligacao.Dicionario.ConsultarValoresDaChave(ascendente);
				if (valor.SequenceEqual(chave))
					return ascendente;
			}
			return null;
		}

		private IList ConsultarPropriedadeDescendente(TObjeto registro, PropertyInfo propriedade, IList<object> dadosVinculados)
		{
			var item = _dicionario.ConsultarPorPropriedade(propriedade.Name);
			if ((item.Ligacao == null) || (item.Ligacao.Tipo != TiposRelacionamento.Descendente))
				return null;
			var valor = _dicionario.ConsultarValoresDaChave(registro);
			var tipoFilho = typeof(List<>).MakeGenericType(propriedade.PropertyType.GetGenericArguments()[0]);
			var filhos = (IList)Activator.CreateInstance(tipoFilho);
			for (var n = dadosVinculados.Count - 1; n >= 0; n--)
			{
				var ascendente = dadosVinculados[n];
				var chave = item.Ligacao.Dicionario.ConsultarValoresDaChave(ascendente, item.Ligacao.ChaveEstrangeira);
				if (valor.SequenceEqual(chave))
				{
					filhos.Add(ascendente);
					dadosVinculados.RemoveAt(n);
				}
			}
			if (filhos.Count > 0)
				return filhos;
			return null;
		}

		public TEstadoObjeto ConsultarPropriedade<TEstadoObjeto>(Buscador<TObjeto> buscador, TObjeto objeto,
			Expression<Func<TObjeto, TEstadoObjeto>> propriedade) where TEstadoObjeto : class, IEntidade
		{
			var configuracao = CriarConfiguradorDaChave(buscador.CriarQuery(), objeto);
			var vinculos = new List<IList<object>>();
			var propriedades = new List<PropertyInfo>() { ExpressionHelper.PropriedadeDaExpressao(propriedade) };
			CarregarPropriedadeVinculada(TiposRelacionamento.Ascendente, configuracao, propriedades, vinculos);
			return (TEstadoObjeto)ConsularValorPropriedadeAscendente(objeto, propriedades[0], vinculos.Count == 1 ? vinculos[0] : null);
		}

		private IConfiguracao<TObjeto> CriarConfiguradorDaChave(IConfiguracaoQuery<TObjeto> query, TObjeto objeto)
		{
			foreach (var item in _dicionario.ConsultarCamposChave())
			{
				var valor = item.Propriedade.GetValue(objeto, null);
				query.AdicionarCondicaoPersonalizada(string.Concat("([", item.Nome, "]=@c", item.Id, ")"));
				query.DefinirParametro(string.Concat("c", item.Id)).Tipo(item.TipoBanco, valor);
			}
			return query;
		}

		public ICollection<TEstadoObjeto> ConsultarPropriedade<TEstadoObjeto>(Buscador<TObjeto> buscador, TObjeto objeto,
			Expression<Func<TObjeto, ICollection<TEstadoObjeto>>> propriedade) where TEstadoObjeto : class, IEntidade
		{
			return (ICollection<TEstadoObjeto>)ConsultarPropriedade(buscador, objeto, (LambdaExpression)propriedade);
		}

		public ICollection ConsultarPropriedade(Buscador<TObjeto> buscador, TObjeto objeto, LambdaExpression propriedade)
		{
			var configuracao = CriarConfiguradorDaChave(buscador.CriarQuery(), objeto);
			var vinculos = new List<IList<object>>();
			var propriedades = new List<PropertyInfo>() { ExpressionHelper.PropriedadeDaExpressao(propriedade) };
			CarregarPropriedadeVinculada(TiposRelacionamento.Descendente, configuracao, propriedades, vinculos);
			return ConsultarPropriedadeDescendente(objeto, propriedades[0], vinculos.Count == 1 ? vinculos[0] : null);
		}

	}

}
