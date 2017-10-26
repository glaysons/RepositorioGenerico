using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using RepositorioGenerico.Dictionary.Mapas.Patterns;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Mapas;
using RepositorioGenerico.Framework.Helpers;

namespace RepositorioGenerico.Dictionary.Mapas
{
	public class TabelaMapaDicionario<TModel, TTabela> : ITabelaMapaDicionario<TModel, TTabela>, IConsultadorCamposDaTabela
		where TModel : class, IEntidade
		where TTabela : class, IEntidade
	{

		private readonly List<CampoMapaDicionario<TModel, TTabela>> _campos;

		public TabelaMapaDicionario()
		{
			_campos = new List<CampoMapaDicionario<TModel, TTabela>>();
		}

		public PropertyInfo ConsultarPropriedadeDaTabelaRelacionadaComModel(PropertyInfo propriedadeDoModel)
		{
			foreach (var campo in _campos)
			{
				var propriedade = campo.PropriedadeDoModel;
				if ((propriedade != null) && (string.Equals(propriedade.Name, propriedadeDoModel.Name)))
					return campo.PropriedadeDoCampo;
			}
			return null;
		}


		public ICampoMapaDicionario<TModel, TTabela> Campo(Expression<Func<TTabela, object>> campo)
		{
			var item = new CampoMapaDicionario<TModel, TTabela>(this, ExpressionHelper.PropriedadeDaExpressao(campo));
			_campos.Add(item);
			return item;
		}

	}
}
