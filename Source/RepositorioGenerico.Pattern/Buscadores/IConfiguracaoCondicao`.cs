using System;
using System.Data;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoCondicao<TObjeto>
	{

		IConfiguracaoQuery<TObjeto> Igual(string valor);
		IConfiguracaoQuery<TObjeto> Igual(int valor);
		IConfiguracaoQuery<TObjeto> Igual(int? valor);
		IConfiguracaoQuery<TObjeto> Igual(bool valor);
		IConfiguracaoQuery<TObjeto> Igual(double valor);
		IConfiguracaoQuery<TObjeto> Igual(double? valor);
		IConfiguracaoQuery<TObjeto> Igual(decimal valor);
		IConfiguracaoQuery<TObjeto> Igual(decimal? valor);
		IConfiguracaoQuery<TObjeto> Igual(DateTime valor);
		IConfiguracaoQuery<TObjeto> Igual(DateTime? valor);

		IConfiguracaoQuery<TObjeto> InicieCom(string valor);
		IConfiguracaoQuery<TObjeto> TermineCom(string valor);
		IConfiguracaoQuery<TObjeto> Contenha(string valor);
		IConfiguracaoQuery<TObjeto> NaoContenha(string valor);

		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, int valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, int? valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, double valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, double? valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, decimal valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, decimal? valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, DateTime valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, DateTime? valor);
		IConfiguracaoQuery<TObjeto> Seja(Operadores operador, DbType tipo, object valor);

		IConfiguracaoQuery<TObjeto> Entre(int inicio, int fim);
		IConfiguracaoQuery<TObjeto> Entre(int? inicio, int? fim);
		IConfiguracaoQuery<TObjeto> Entre(double inicio, double fim);
		IConfiguracaoQuery<TObjeto> Entre(double? inicio, double? fim);
		IConfiguracaoQuery<TObjeto> Entre(decimal inicio, decimal fim);
		IConfiguracaoQuery<TObjeto> Entre(decimal? inicio, decimal? fim);
		IConfiguracaoQuery<TObjeto> Entre(DateTime inicio, DateTime fim);
		IConfiguracaoQuery<TObjeto> Entre(DateTime? inicio, DateTime? fim);

		IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params string[] valor);
		IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params int[] valor);
		IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params double[] valor);
		IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params decimal[] valor);

		IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params string[] valor);
		IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params int[] valor);
		IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params double[] valor);
		IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params decimal[] valor);

		IConfiguracaoQuery<TObjeto> EhNulo();
		IConfiguracaoQuery<TObjeto> NaoEhNulo();

	}

}
