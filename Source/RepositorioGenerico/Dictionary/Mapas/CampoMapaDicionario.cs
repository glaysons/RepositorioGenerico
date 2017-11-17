using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Mapas;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Framework.Helpers;

namespace RepositorioGenerico.Dictionary.Mapas
{
	public class CampoMapaDicionario<TModel, TTabela> : ICampoMapaDicionario<TModel, TTabela>
		where TModel : class, IEntidade
		where TTabela : class, IEntidade
	{

		private readonly ITabelaMapaDicionario<TModel, TTabela> _pai;

		public PropertyInfo PropriedadeDoCampo { get; private set; }

		public PropertyInfo PropriedadeDoModel { get; private set; }

		public CampoMapaDicionario(ITabelaMapaDicionario<TModel, TTabela> pai, PropertyInfo propriedadeDoCampo)
		{
			PropriedadeDoCampo = propriedadeDoCampo;
			_pai = pai;
			ValidarSeOsTiposDaPropriedadeCampoSaoIguais();
		}

		public ITabelaMapaDicionario<TModel, TTabela> Propriedade(Expression<Func<TModel, object>> propriedade)
		{
			PropriedadeDoModel = ExpressionHelper.PropriedadeDaExpressao(propriedade);
			ValidarSeOsTiposDaPropriedadeCampoSaoIguais();
			return _pai;
		}

		private void ValidarSeOsTiposDaPropriedadeCampoSaoIguais()
		{
			if ((PropriedadeDoCampo == null) || (PropriedadeDoModel == null) || (PropriedadeDoModel.PropertyType.IsEnum))
				return;
			if (AsPropriedadesEstaoRelacionadas())
				return;
			if (ValidarDiferencaDetiposEntreAsPropriedades())
				throw new OsTiposDoCampoEPropriedadeNaoSaoOsMesmosException();
		}

		private bool ValidarDiferencaDetiposEntreAsPropriedades()
		{
			return (PropriedadeDoCampo.PropertyType.IsPrimitive != PropriedadeDoModel.PropertyType.IsPrimitive)
				|| (PropriedadeDoCampo.PropertyType.IsClass != PropriedadeDoModel.PropertyType.IsClass);
		}

		private bool AsPropriedadesEstaoRelacionadas()
		{
			var campo = PropriedadeDoCampo.PropertyType;
			var model = PropriedadeDoModel.PropertyType;
			if (campo.IsGenericType && model.IsGenericType)
			{
				if ((campo.GetGenericTypeDefinition() != typeof (ICollection<>)) ||
					(model.GetGenericTypeDefinition() != typeof (ICollection<>)))
					return false;
				var dicionarioMapeado = DicionarioCache.Consultar(model.GetGenericArguments()[0]);
				return (dicionarioMapeado.OrigemMapa == campo.GetGenericArguments()[0]);
			}
			return (campo == model);
		}
	}
}
