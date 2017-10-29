using System;
using System.Reflection;
using RepositorioGenerico.Dictionary.Helpers;
using RepositorioGenerico.Dictionary.Mapas.Patterns;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Mapas;

namespace RepositorioGenerico.Dictionary.Mapas
{
	public class Mapa<TModel> : IMapa, IMapa<TModel> where TModel : class, IEntidade
	{

		private IConsultadorCamposDaTabela _tabela;

		public Type TipoDaTabela { get; private set; }

		public string NomeDaTabela { get; private set; }

		public PropertyInfo ConsultarPropriedadeDaTabela(PropertyInfo propriedadeDoModel)
		{
			return _tabela.ConsultarPropriedadeDaTabelaRelacionadaComModel(propriedadeDoModel);
		}

		public ITabelaMapaDicionario<TModel, TTabela> Tabela<TTabela>() where TTabela : class, IEntidade
		{
			_tabela = _tabela ?? (_tabela = new TabelaMapaDicionario<TModel, TTabela>());
			TipoDaTabela = typeof(TTabela);
			NomeDaTabela = DataAnnotationHelper.ConsultarNomeDaTabela(TipoDaTabela);
			return (ITabelaMapaDicionario<TModel, TTabela>)_tabela;
		}

	}
}
