using System;
using System.Collections.Generic;
using System.Reflection;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Framework.Helpers;

namespace RepositorioGenerico.Dictionary.Helpers
{
	public static class DataAnnotationHelper
	{

		public static string ConsultarNomeDaTabela(Type tipo)
		{
			var atributo = AttributeHelper.Consultar<TabelaAttribute>(tipo);
			if (atributo == null)
				return tipo.Name;
			return atributo.Nome ?? tipo.Name;
		}

		public static ColunaAttribute ConsultarColuna(PropertyInfo propriedade)
		{
			return AttributeHelper.Consultar<ColunaAttribute>(propriedade);
		}

		public static bool ChavePrimaria(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<ChaveAttribute>(propriedade);
			return (atributo != null);
		}

		public static bool Obrigatorio(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<ObrigatorioAttribute>(propriedade);
			return (atributo != null);
		}

		public static int ConsultarTamanhoMinimo(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<TamanhoMinimoAttribute>(propriedade);
			return (atributo == null) 
				? 0 
				: atributo.Tamanho;
		}

		public static int ConsultarTamanhoMaximo(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<TamanhoMaximoAttribute>(propriedade);
			return (atributo == null) 
				? 0 
				: atributo.Tamanho;
		}

		public static Incremento ConsultarOpcaoGeracao(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<AutoIncrementoAttribute>(propriedade);
			return (atributo == null) 
				? Incremento.Nenhum 
				: atributo.Incremento;
		}

		public static bool Mapeado(PropertyInfo propriedade)
		{
			var getMethod = propriedade.GetGetMethod();
			if (getMethod.IsVirtual && !getMethod.IsFinal)
				return false;
			if (AttributeHelper.Consultar<NaoMapeadoAttribute>(propriedade) != null)
				return false;
			var tipo = propriedade.PropertyType;
			if ((tipo.IsGenericType) && (tipo.GetGenericTypeDefinition() == typeof(ICollection<>)))
				return false;
			if (typeof(Entities.Entidade).IsAssignableFrom(tipo))
				return false;
			return true;
		}

		public static object ConsultarValorPadrao(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<ValorPadraoAttribute>(propriedade);
			return (atributo == null) 
				? null 
				: atributo.Valor;
		}

		public static string ConsultarForeignKey(PropertyInfo propriedade)
		{
			var atributo = AttributeHelper.Consultar<ChaveEstrangeiraAttribute>(propriedade);
			if (atributo == null)
				return null;
			return atributo.Nome;
		}

		public static string ConsultarForeignKeyDaInverseProperty(PropertyInfo propriedade, Type tipo)
		{
			if ((!tipo.IsGenericType) || (tipo.GetGenericTypeDefinition() != typeof(ICollection<>)))
				return null;
			var atributo = AttributeHelper.Consultar<PropriedadeDeLigacaoEstrangeiraAttribute>(propriedade);
			if (atributo == null)
			{
				var dicionario = DicionarioCache.Consultar(propriedade.DeclaringType);
				if (dicionario.OrigemMapa == null)
					return null;
				var item = dicionario.ConsultarPorPropriedade(propriedade.Name);
				if (item.Ligacao != null)
					return string.Join(";", item.Ligacao.ChaveEstrangeira);
			}
			if (atributo == null)
				return null;
			var tipoGenerico = tipo.GetGenericArguments()[0];
			var dicionarioTipoPropriedade = DicionarioCache.Consultar(tipoGenerico);
			if (dicionarioTipoPropriedade.OrigemMapa != null)
				tipoGenerico = dicionarioTipoPropriedade.OrigemMapa;
			var foreign = tipoGenerico.GetProperty(atributo.Nome);
			if (foreign == null)
				return null;
			return ConsultarForeignKey(foreign);
		}

	}
}
