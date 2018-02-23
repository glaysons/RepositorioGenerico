using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.SqlClient.Builders
{
	public static class CommandBuilder
	{

		public static void DefinirParametrosParaTodosOsCampos(IDicionario dicionario, IDbCommand comando)
		{
			var configurador = new Configurador(comando);
			foreach (var campo in dicionario.Itens)
				configurador.DefinirParametro(string.Concat("p", campo.Id)).Tipo(campo.TipoBanco, null);
		}

		public static void DefinirParametrosParaTodosOsCamposDaChave(IDicionario dicionario, IDbCommand comando)
		{
			var configurador = new Configurador(comando);
			foreach (var campo in dicionario.ConsultarCamposChave())
				configurador.DefinirParametro(string.Concat("p", campo.Id)).Tipo(campo.TipoBanco, null);
		}

		public static void DefinirParametrosParaCamposDaChaveQueNaoSaoAutoIncremento(IDicionario dicionario, IDbCommand comando)
		{
			var configurador = new Configurador(comando);
			foreach (var campo in dicionario.ConsultarCamposChave())
				if (campo.OpcaoGeracao == Incremento.Nenhum)
					configurador.DefinirParametro(string.Concat("p", campo.Id)).Tipo(campo.TipoBanco, null);
		}

		public static void SincronizarParametrosDosCamposChaveQueNaoSaoAutoIncremento<TObjeto>(IDicionario dicionario, IDbCommand comando, TObjeto model)
			where TObjeto : IEntidade
		{
			foreach (var campo in dicionario.ConsultarCamposChave())
				if (campo.OpcaoGeracao == Incremento.Nenhum)
					SincronizarParametro(comando, campo, ConsultarValorDaPropriedadeDoObjeto(model, campo));
		}

		private static object ConsultarValorDaPropriedadeDoObjeto<TObjeto>(TObjeto model, ItemDicionario campo) where TObjeto : IEntidade
		{
			return (model == null) 
				? null 
				: campo.Propriedade.GetValue(model, null);
		}

		private static void SincronizarParametro(IDbCommand comando, ItemDicionario itemDicionario, object valor)
		{
			var nomeParametro = string.Concat("@p", itemDicionario.Id);
			var parametro = (SqlParameter)comando.Parameters[nomeParametro];
			var valorParametro = (valor ?? DBNull.Value);
			if ((itemDicionario.TipoLocal.IsEnum) && (valor != null) && (valorParametro != DBNull.Value))
				valorParametro = (itemDicionario.TipoBanco == DbType.String) 
					? ConverterEnumEmString(itemDicionario.Propriedade, valorParametro) 
					: valor.GetHashCode();
			parametro.Value = valorParametro;
		}

		public static void SincronizarParametrosDeTodosOsCampos<TObjeto>(IDicionario dicionario, IDbCommand comando, TObjeto model)
			where TObjeto : IEntidade
		{
			foreach (var campo in dicionario.Itens)
				SincronizarParametro(comando, campo, ConsultarValorDaPropriedadeDoObjeto(model, campo));
		}

		private static object ConverterEnumEmString(PropertyInfo propriedade, object valor)
		{
			var tipoEnum = propriedade.PropertyType;
			if (tipoEnum.IsGenericType)
				tipoEnum = tipoEnum.GetGenericArguments()[0];
			foreach (var opcaoEnum in Enum.GetValues(tipoEnum))
				if (string.Equals(opcaoEnum.ToString(), valor.ToString()))
				{
					var field = tipoEnum.GetField(opcaoEnum.ToString());
					var valorPadrao = AttributeHelper.Consultar<ValorPadraoAttribute>(field);
					if (valorPadrao != null)
						return valorPadrao.Valor;
					break;
				}
			return valor.ToString();
		}

		public static void SincronizarParametrosDosCamposChave<TObjeto>(IDicionario dicionario, IDbCommand comando, TObjeto model)
			where TObjeto : IEntidade
		{
			foreach (var campo in dicionario.ConsultarCamposChave())
				SincronizarParametro(comando, campo, ConsultarValorDaPropriedadeDoObjeto(model, campo));
		}

		public static void SincronizarParametrosDosCamposChaveQueNaoSaoAutoIncremento(IDicionario dicionario, IDbCommand comando, DataRow registro)
		{
			foreach (var campo in dicionario.ConsultarCamposChave())
				if (campo.OpcaoGeracao == Incremento.Nenhum)
					SincronizarParametro(comando, campo, registro[campo.Nome]);
		}

		public static void SincronizarParametrosDeTodosOsCampos(IDicionario dicionario, IDbCommand comando, DataRow registro)
		{
			foreach (var campo in dicionario.Itens)
				SincronizarParametro(comando, campo, registro[campo.Nome]);
		}

		public static void SincronizarParametrosDosCamposChave(IDicionario dicionario, IDbCommand comando, DataRow registro)
		{
			foreach (var campo in dicionario.ConsultarCamposChave())
				SincronizarParametro(comando, campo, registro[campo.Nome]);
		}

	}
}
