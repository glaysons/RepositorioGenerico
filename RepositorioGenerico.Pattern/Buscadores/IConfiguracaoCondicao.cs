using System;
using System.Data;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoCondicao
	{

		IConfiguracaoQuery Igual(string valor);
		IConfiguracaoQuery Igual(int valor);
		IConfiguracaoQuery Igual(int? valor);
		IConfiguracaoQuery Igual(bool valor);
		IConfiguracaoQuery Igual(double valor);
		IConfiguracaoQuery Igual(double? valor);
		IConfiguracaoQuery Igual(decimal valor);
		IConfiguracaoQuery Igual(decimal? valor);
		IConfiguracaoQuery Igual(DateTime valor);
		IConfiguracaoQuery Igual(DateTime? valor);

		IConfiguracaoQuery InicieCom(string valor);
		IConfiguracaoQuery TermineCom(string valor);
		IConfiguracaoQuery Contenha(string valor);
		IConfiguracaoQuery NaoContenha(string valor);

		IConfiguracaoQuery Seja(Operadores operador, int valor);
		IConfiguracaoQuery Seja(Operadores operador, int? valor);
		IConfiguracaoQuery Seja(Operadores operador, double valor);
		IConfiguracaoQuery Seja(Operadores operador, double? valor);
		IConfiguracaoQuery Seja(Operadores operador, decimal valor);
		IConfiguracaoQuery Seja(Operadores operador, decimal? valor);
		IConfiguracaoQuery Seja(Operadores operador, DateTime valor);
		IConfiguracaoQuery Seja(Operadores operador, DateTime? valor);
		IConfiguracaoQuery Seja(Operadores operador, DbType tipo, object valor);

		IConfiguracaoQuery Entre(int inicio, int fim);
		IConfiguracaoQuery Entre(int? inicio, int? fim);
		IConfiguracaoQuery Entre(double inicio, double fim);
		IConfiguracaoQuery Entre(double? inicio, double? fim);
		IConfiguracaoQuery Entre(decimal inicio, decimal fim);
		IConfiguracaoQuery Entre(decimal? inicio, decimal? fim);
		IConfiguracaoQuery Entre(DateTime inicio, DateTime fim);
		IConfiguracaoQuery Entre(DateTime? inicio, DateTime? fim);

		IConfiguracaoQuery EstejaDentroDaLista(params string[] valor);
		IConfiguracaoQuery EstejaDentroDaLista(params int[] valor);
		IConfiguracaoQuery EstejaDentroDaLista(params double[] valor);
		IConfiguracaoQuery EstejaDentroDaLista(params decimal[] valor);

		IConfiguracaoQuery EstejaForaDaLista(params string[] valor);
		IConfiguracaoQuery EstejaForaDaLista(params int[] valor);
		IConfiguracaoQuery EstejaForaDaLista(params double[] valor);
		IConfiguracaoQuery EstejaForaDaLista(params decimal[] valor);

		IConfiguracaoQuery EhNulo();
		IConfiguracaoQuery NaoEhNulo();

	}

}
